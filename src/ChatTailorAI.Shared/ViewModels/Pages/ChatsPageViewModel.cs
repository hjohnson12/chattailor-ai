using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Common.Navigation;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class ChatsPageViewModel : Observable, IDisposable
    {
        private readonly IDialogService _dialogService;
        private readonly IConversationDataService _conversationDataService;
        private readonly IOpenAIThreadService _openAIThreadService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly IChatPageNavigationService _chatNavigationService;
        private readonly IAssistantDataService _assistantDataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDataService _messageDataService;
        private readonly ILoggerService _loggerService;

        private ObservableCollection<ConversationViewModel> _chats;
        private bool _isChatsPaneOpen;

        private ConversationViewModel _selectedChat;

        public ChatsPageViewModel(
            IDialogService dialogService,
            IConversationDataService conversationDataService,
            IOpenAIThreadService openAIThreadService,
            IAppNotificationService appNotificationService,
            IChatPageNavigationService chatNavigationService,
            IAssistantDataService assistantDataService,
            IEventAggregator eventAggregator,
            IMessageDataService messageDataService,
            ILoggerService loggerService)
        {
            _dialogService = dialogService;
            _conversationDataService = conversationDataService;
            _openAIThreadService = openAIThreadService;
            _appNotificationService = appNotificationService;
            _chatNavigationService = chatNavigationService;
            _assistantDataService = assistantDataService;
            _eventAggregator = eventAggregator;
            _messageDataService = messageDataService;
            _loggerService = loggerService;

            _eventAggregator.ChatUpdated += OnChatUpdated;

            _chats = new ObservableCollection<ConversationViewModel>();

            _isChatsPaneOpen = true;

            CreateChatCommand = new AsyncRelayCommand(CreateChat);
            LoadedCommand = new AsyncRelayCommand(LoadChats);
            EditChatCommand = new AsyncRelayCommand<ConversationViewModel>(EditChat);
            DeleteChatCommand = new AsyncRelayCommand<ConversationViewModel>(DeleteChat);
            DeleteSelectedChatCommand = new AsyncRelayCommand(DeleteSelectedChat);
            ShowChatHelpCommand = new AsyncRelayCommand(ShowChatHelp);
        }

        ~ChatsPageViewModel()
        {
            Dispose();
        }

        public void Dispose()
        {
            _eventAggregator.ChatUpdated -= OnChatUpdated;
        }

        public ICommand CreateChatCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand EditChatCommand { get; private set; }
        public ICommand DeleteChatCommand { get; private set; }
        public ICommand DeleteSelectedChatCommand { get; private set; }
        public ICommand ShowChatHelpCommand { get; private set; }

        public ObservableCollection<ConversationViewModel> Chats
        {
            get => _chats;
            set => SetProperty(ref _chats, value);
        }

        public bool IsChatsPaneOpen
        {
            get => _isChatsPaneOpen;
            set => SetProperty(ref _isChatsPaneOpen, value);
        }

        public ConversationViewModel SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                OnPropertyChanged(nameof(IsChatSelected));
            }
        }

        public bool IsChatsEmpty => Chats.Count < 1;
        public bool IsChatSelected => SelectedChat != null;

        private async Task CreateChat()
        {
            await _dialogService.ShowNewChatDialogAsync();
        }

        private void OnChatUpdated(object sender, ChatUpdatedEvent e)
        {
            // User may have updated chat name or other details from within chat page,
            // so we need to update the chat list here to reflect those changes

            // Need to look at why SelectedChat is updated multiple times in the bindings
            if (SelectedChat != null)
            {
                var index = Chats.IndexOf(SelectedChat);

                var conversationViewModel = ConversationViewModel.FromDto(e.Conversation);
                if (SelectedChat != conversationViewModel)
                {
                    Chats[index] = conversationViewModel;
                    SelectedChat = conversationViewModel;
                }
            }
        }

        private async Task EditChat(ConversationViewModel conversation)
        {
            // TODO: Handle editing chat while its open, model doesnt update usage until clicked
            // again from the side
            var conversationDto = conversation.ToDto();

            bool result = await _dialogService.ShowEditChatDialogAsync(conversationDto);
            if (result != true)
            {
                return;
            }

            try
            {
                // OpenAI Assistants don't support vision, make sure to avoid updating if
                // its an assistant conversation and a vision model is selected
                if (conversationDto.Model == "gpt-4-vision-preview" && conversationDto.ConversationType == "OpenAI Assistant")
                {
                    _appNotificationService.Display("Unable to update, OpenAI Assistants don't support vision models");
                    return;
                }

                // no need to update anything in OpenAI here
                // Conversations are stored in db and OpenAI is queried on demand using threadId if needed
                await _conversationDataService.UpdateConversationAsync(conversationDto);
                _eventAggregator.PublishChatUpdated(new ChatUpdatedEvent { Conversation = conversationDto });

                var index = Chats.IndexOf(conversation);
                conversation = ConversationViewModel.FromDto(conversationDto, conversation.AssistantName);
                Chats[index] = conversation;
                // TODO: Figure out why selected chat gets set to null / multiple calls, this is a quick hack
                SelectedChat = conversation;

                _appNotificationService.Display("Chat updated successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error updating chat");
                _appNotificationService.Display(ex.Message);
            }
        }

        private async Task DeleteChat(ConversationViewModel conversation)
        {
            if (Chats.Count < 1 || conversation == null)
                return;

            try
            {
                var result = await _dialogService.ShowDeleteDialogAsync();
                if (result == true)
                {
                    await _conversationDataService.DeleteConversationAsync(conversation.ToDto());
                    
                    var messages = await _messageDataService.GetMessagesAsync(conversation.Id);
                    foreach (var message in messages)
                    {
                        await _messageDataService.DeleteMessageAsync(message);
                    }

                    if (conversation.ConversationType == "OpenAI Assistant")
                    {
                        await _openAIThreadService.DeleteThreadAsync(conversation.ThreadId);
                    }

                    var index = Chats.IndexOf(conversation);
                    SelectedChat = SelectedChat != null ? Chats.ElementAtOrDefault(index - 1) : null;
                    Chats.RemoveAt(index);

                    OnPropertyChanged(nameof(IsChatsEmpty));
                    if (!IsChatsEmpty && SelectedChat != null)
                    {
                        _chatNavigationService.NavigateTo(NavigationPageKeys.ChatPage, SelectedChat.ToDto());
                    }

                    _appNotificationService.Display("Chat deleted successfully");
                }
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error deleting chat");

                // TODO: May need to handle rollback if errors occur too deep in the calls above
                // and if EF still wont auto rollback on failure
                _appNotificationService.Display(ex.Message);
            }
        }

        public async Task DeleteSelectedChat()
        {
            if (SelectedChat == null)
            {
                _appNotificationService.Display("No chat selected to delete");
                return;
            }

            await DeleteChat(SelectedChat);
        }

        private async Task LoadChats()
        {
            try
            {
                var chatDtos = await _conversationDataService.GetConversationsAsync();
                var assistantIds = chatDtos
                    .OfType<AssistantConversationDto>()
                    .Select(dto => dto.AssistantId)
                    .Distinct()
                    .ToList();

                var assistantNames = assistantIds.Count > 0
                    ? await GetAssistantNamesAsync(assistantIds)
                    : new Dictionary<string, string>();

                var chatViewModels = chatDtos
                    .Select(dto =>
                    {
                        var viewModel = ConversationViewModel.FromDto(dto);

                        if (dto is AssistantConversationDto assistantDto)
                        {
                            if (assistantDto.AssistantId != null && assistantNames.TryGetValue(assistantDto.AssistantId, out var name))
                            {
                                viewModel.AssistantName = name;
                            }
                            else
                            {
                                // TODO: Eventually update to show assistant name even if it was deleted
                                // This will utilize the Archived chat data ideally, until the specific
                                // chat tied to the assistant is deleted itself
                                viewModel.AssistantName = "N/A - Assistant Deleted";
                            }
                        }

                        return viewModel;
                    })
                    .OrderByDescending(vm => vm.CreatedAt)
                    .ToList();

                Chats = new ObservableCollection<ConversationViewModel>(chatViewModels);
                OnPropertyChanged(nameof(IsChatsEmpty));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error loading chats, starting with empty collection...");
                Chats = new ObservableCollection<ConversationViewModel>();
            }
        }

        private async Task<Dictionary<string, string>> GetAssistantNamesAsync(IEnumerable<string> assistantIds)
        {
            return await _assistantDataService.GetAssistantNamesByIdsAsync(assistantIds);
        }

        private async Task ShowChatHelp()
        {
            await _dialogService.ShowChatHelpDialogAsync();
        }
    }
}