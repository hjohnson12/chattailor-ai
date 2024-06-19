using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class NewChatDialogViewModel : Observable
    {
        private readonly IConversationDataService _conversationDataService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly INavigationService _navigationService;
        private readonly IAssistantDataService _assistantDataService;
        private readonly IOpenAIAssistantManagerService _openAIAssistantManagerService;
        private readonly IModelManagerService _modelManagerService;
        private string _title;
        private string _model;
        private string _instructions;
        private string _conversationType;
        private bool _isStandardConversation;
        private ObservableCollection<string> _models;
        private string _selectedModel;

        private ObservableCollection<AssistantViewModel> _assistants;
        private AssistantViewModel _selectedAssistant;

        public NewChatDialogViewModel(
            IConversationDataService conversationDataService,
            IAppNotificationService appNotificationService,
            INavigationService navigationService,
            IAssistantDataService assistantDataService,
            IOpenAIAssistantManagerService openAIAssistantManagerService,
            IModelManagerService modelManagerService)
        {
            _conversationDataService = conversationDataService;
            _appNotificationService = appNotificationService;
            _navigationService = navigationService;
            _assistantDataService = assistantDataService;
            _openAIAssistantManagerService = openAIAssistantManagerService;
            _modelManagerService = modelManagerService;

            _assistants = new ObservableCollection<AssistantViewModel>();

            ConversationType = "Standard";
            Models = new ObservableCollection<string>(ModelConstants.DefaultChatModels);

            // TODO: If assistant mode, dont set model
            SelectedModel = Models[0];

            CreateChatCommand = new AsyncRelayCommand(CreateChat);
            LoadedCommand = new AsyncRelayCommand(LoadAssistants);
            _modelManagerService = modelManagerService;
        }

        public ICommand CreateChatCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);
        }

        public string ConversationType
        {
            get => _conversationType;
            set
            {
                SetProperty(ref _conversationType, value);
                OnPropertyChanged(nameof(IsStandardConversation));
                if (IsStandardConversation)
                {
                    SelectedAssistant = null;
                }
                else
                {
                    SelectedModel = null;
                }
            }
        }

        public bool IsStandardConversation
        {
            get => _conversationType == "Standard";
        }

        public string SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public AssistantViewModel SelectedAssistant
        {
            get => _selectedAssistant;
            set => SetProperty(ref _selectedAssistant, value);
        }

        public ObservableCollection<string> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public ObservableCollection<AssistantViewModel> Assistants
        {
            get => _assistants;
            set => SetProperty(ref _assistants, value);
        }

        public async Task LoadAssistants()
        {
            var assistantDtos = await _assistantDataService.GetAssistantsAsync();
            var assistantViewModels = assistantDtos.Select(dto => new AssistantViewModel(dto)).ToList();
            Assistants = new ObservableCollection<AssistantViewModel>(assistantViewModels);
        }

        public async Task CreateChat()
        {
            try
            {
                ValidateInput();

                var newId = Guid.NewGuid().ToString();
                ConversationDto conversationDto = null;
                switch (ConversationType)
                {
                    case "Standard":
                        conversationDto = new ConversationDto
                        {
                            Title = Title,
                            Model = SelectedModel ?? "gpt-4-0125-preview",
                            Instructions = Instructions
                        };
                        break;
                    case "Assistant":
                        // TODO: Create AssistantConversation or OpenAIAssistantConversation depending on the assistant type
                        // TODO: Decide completely whether to create thread up front rather than on first message
                        var thread = await _openAIAssistantManagerService.CreateThreadAsync();
                        var threadId = thread.Id;

                        // Null if not overriding assistant model and model is not gpt-4
                        // Assistants can only use gpt models currently
                        var model = (SelectedModel != null && SelectedModel.StartsWith("gpt")) ? SelectedModel : null;

                        conversationDto = new OpenAIAssistantConversationDto
                        {
                            Title = Title,
                            Model = model,
                            Instructions = Instructions,
                            AssistantId = SelectedAssistant != null ? SelectedAssistant.Id : null,
                            ThreadId = threadId
                        };

                        break;
                    default:
                        throw new Exception("Invalid conversation type");
                }

                await _conversationDataService.SaveConversationAsync(conversationDto);
                _appNotificationService.Display($"Created chat successfully");

                _navigationService.NavigateTo(NavigationPageKeys.ChatPage, conversationDto);
            }
            catch (Exception ex)
            {
                _appNotificationService.Display($"Failed to create new chat: {ex.Message}");
            }
        }

        private void ValidateInput()
        {
            if (string.IsNullOrEmpty(Title))
            {
                throw new Exception("Please enter a chat title");
            }

            if (string.IsNullOrEmpty(SelectedModel) && IsStandardConversation)
            {
                throw new Exception("Please select a model for the chat");
            }

            if (ConversationType == "Assistant" && SelectedAssistant == null)
            {
                throw new Exception("Please select an assistant for chat type 'Assistant'");
            }

            if (ConversationType == "Assistant" && SelectedModel == "gpt-4-vision-preview")
            {
                throw new Exception("Vision model not supported in OpenAI assistant conversations");
            }
        }

        public async Task RefreshModelsAsync()
        {
            string currentChatModel = SelectedModel;
            await _modelManagerService.RefreshDynamicModelsAsync();
            ObservableCollection<string> newModels = new ObservableCollection<string>(_modelManagerService.GetAllModels());
            Models.Clear();
            foreach (string model in newModels)
            {
                Models.Add(model);
            }
            if (Models.Contains(currentChatModel))
            {
                SelectedModel = currentChatModel;
            }
            else
            {
                SelectedModel = null;
            }

            OnPropertyChanged(nameof(SelectedModel));
        }
    }
}