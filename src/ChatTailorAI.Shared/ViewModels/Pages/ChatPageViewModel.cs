using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Services.Chat;
using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Speech;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Image;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Authentication;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.Factories;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Services.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Google;
using ChatTailorAI.Shared.Services.Chat.Google;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.LMStudio;
using ChatTailorAI.Shared.Services.Chat.LMStudio;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class ChatPageViewModel : Observable, IDisposable
    {
        private readonly IOpenAIChatService _openAIChatService;
        private readonly IDispatcherService _dispatcherService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IFileDownloadService _fileDownloadService;
        private readonly IDialogService _dialogService;
        private readonly IWindowsClipboardService _windowsService;
        private readonly IChatFileService _chatFileService;
        private readonly IAudioRecorderService _audioService;
        private readonly IWhisperService _whisperService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly IOpenAIAssistantManagerService _openAIAssistantManagerService;
        private readonly IConversationDataService _conversationDataService;
        private readonly IAssistantDataService _assistantDataService;
        private readonly IMessageDataService _messageDataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IImageDataService _imageDataService;
        private readonly IImageFileService _imageFileService;
        private readonly IPromptDataService _promptDataService;
        private readonly IChatMessageViewModelFactory _chatMessageViewModelFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISpeechService _speechService;
        private readonly IImageService _imageService;
        private readonly IChatMessageTransformerFactory _chatMessageTransformerFactory;
        private readonly IChatMessageViewModelMapper _chatMessageViewModelMapper;
        private readonly IChatRequestBuilderFactory _chatRequestBuilderFactory;
        private readonly IChatSettingsFactory _chatSettingsFactory;
        private readonly IFileService _fileService;
        private readonly IAnthropicChatService _claudeChatService;
        private readonly IGoogleChatService _googleChatService;
        private readonly ILoggerService _loggerService;
        private readonly ILMStudioChatService _lmStudioChatService;

        private bool _isActiveViewModel;
        private string _userInput;
        private bool _isTyping;
        private bool _isAuthWindowVisible;
        private bool _isRecording;
        private int newMessageIndex;
        private int previousContentLength = 0;
        private string defaultMode = "Chat";
        private string _selectedMode;
        private ObservableCollection<string> _modes;
        private ObservableCollection<ChatMessageViewModel> _messages;
        private ObservableCollection<ChatImageDto> _selectedImages;
        private ObservableCollection<string> _attachedImages;
        public event Action<string> AuthenticateRequested;
        public event Action<bool> AuthenticationCompleted;

        public event EventHandler MessageAdded;

        public ICommand SavePhotosCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public ICommand DeleteMessagesCommand { get; set; }
        public ICommand DeleteMessageCommand { get; set; }
        public ICommand StopGeneratingResponseCommand { get; set; }
        public ICommand CopyMessageCommand { get; set; }
        public ICommand CopyToPromptCommand { get; set; }
        public ICommand RecordCommand { get; set; }
        public ICommand AttachImageCommand { get; set; }
        public ICommand AttachImageFromClipboardCommand { get; set; }
        public ICommand EditChatCommand { get; set; }

        public ChatPageViewModel(
            IOpenAIChatService chatGptService,
            IDispatcherService dispatcherService,
            IUserSettingsService userSettingsService,
            IFileDownloadService fileDownloadService,
            IImageFileService imageFileService,
            IDialogService dialogService,
            IWindowsClipboardService windowsService,
            IChatFileService chatFileService,
            IAudioRecorderService audioService,
            IWhisperService whisperService,
            IAppNotificationService appNotificationService,
            IOpenAIAssistantManagerService openAIAssistantManagerService,
            IConversationDataService conversationDataService,
            IAssistantDataService assistantDataService,
            IMessageDataService messageDataService,
            IEventAggregator eventAggregator,
            IImageDataService imageDataService,
            IPromptDataService promptDataService,
            IChatMessageViewModelFactory chatMessageViewModelFactory,
            IAuthenticationService authenticationService,
            ISpeechService SpeechService,
            IImageService imageService,
            IChatMessageTransformerFactory chatMessageTransformerFactory,
            IChatMessageViewModelMapper chatMessageViewModelMapper,
            IChatRequestBuilderFactory chatRequestBuilderFactory,
            IChatSettingsFactory chatSettingsFactory,
            IFileService fileService,
            IAnthropicChatService claudeChatService,
            IGoogleChatService googleChatService,
            ILoggerService loggerService,
            ILMStudioChatService lmStudioChatService
            )
        {
            _openAIChatService = chatGptService;
            _dispatcherService = dispatcherService;
            _userSettingsService = userSettingsService;
            _fileDownloadService = fileDownloadService;
            _dialogService = dialogService;
            _windowsService = windowsService;
            _chatFileService = chatFileService;
            _audioService = audioService;
            _whisperService = whisperService;
            _appNotificationService = appNotificationService;
            _openAIAssistantManagerService = openAIAssistantManagerService;
            _conversationDataService = conversationDataService;
            _assistantDataService = assistantDataService;
            _eventAggregator = eventAggregator;
            _messageDataService = messageDataService;
            _imageDataService = imageDataService;
            _imageFileService = imageFileService;
            _promptDataService = promptDataService;
            _chatMessageViewModelFactory = chatMessageViewModelFactory;
            _authenticationService = authenticationService;
            _speechService = SpeechService;
            _imageService = imageService;
            _chatMessageTransformerFactory = chatMessageTransformerFactory;
            _chatMessageViewModelMapper = chatMessageViewModelMapper;
            _chatRequestBuilderFactory = chatRequestBuilderFactory;
            _chatSettingsFactory = chatSettingsFactory;
            _fileService = fileService;
            _claudeChatService = claudeChatService;
            _googleChatService = googleChatService;
            _loggerService = loggerService;
            _lmStudioChatService = lmStudioChatService;

            _eventAggregator.ChatMessageReceived += OnChatMessageReceived;
            _eventAggregator.ModeChanged += OnChatModeChanged;
            _eventAggregator.ChatUpdated += OnChatUpdated;
            _userSettingsService.SettingChanged += OnUserSettingChanged;

            Modes = new ObservableCollection<string>
            {
                "Chat",
                "Image"
            };
            SelectedMode = defaultMode;
            SelectedImages = new ObservableCollection<ChatImageDto>();
            Messages = new ObservableCollection<ChatMessageViewModel>();
            AttachedImages = new ObservableCollection<string>();
            IsTyping = false;
            previousContentLength = 0;
            IsRecording = false;

            SavePhotosCommand =
                new AsyncRelayCommand(SaveSelectedPhotos, () => true);
            DeleteMessagesCommand =
                new AsyncRelayCommand(DeleteMessages, () => true);
            DeleteMessageCommand =
                new CommunityToolkit.Mvvm.Input.RelayCommand<ChatMessageViewModel>(DeleteMessage);
            StopGeneratingResponseCommand =
                new CommunityToolkit.Mvvm.Input.RelayCommand(StopGeneratingResponse, () => true);
            SendMessageCommand =
                new AsyncRelayCommand(SendMessage, () => true);
            CopyMessageCommand =
                new CommunityToolkit.Mvvm.Input.RelayCommand<ChatMessageViewModel>(CopyMessage);
            CopyToPromptCommand =
                new CommunityToolkit.Mvvm.Input.RelayCommand<ChatMessageViewModel>(CopyMessageToPrompt);
            RecordCommand =
                new AsyncRelayCommand(RecordAudio, () => true);
            AttachImageCommand =
                new AsyncRelayCommand(AttachImage, () => true);
            AttachImageFromClipboardCommand =
                new AsyncRelayCommand(AttachImageFromClipboard, () => true);
            EditChatCommand =
                new AsyncRelayCommand(EditChat, () => true);
        }

        ~ChatPageViewModel()
        {
            Dispose();
        }

        public void Dispose()
        {
            // TODO: Check into hwo to handle going back to chat page and chat view model being disposed
            // but need event aggregator to still be active
            _eventAggregator.ChatMessageReceived -= OnChatMessageReceived;
            _eventAggregator.ModeChanged -= OnChatModeChanged;
            _userSettingsService.SettingChanged -= OnUserSettingChanged;
            _eventAggregator.ChatUpdated -= OnChatUpdated;

            IsActiveViewModel = false;

            // Cancels any ongoing chat generation when leaving the page
            // This is to prevent the chat from continuing to generate when the user leaves the page
            // and to prevent the chat from generating when the user returns to the page
            // May change when we subscribe to the event for receiving messages
            StopGeneratingResponse();
        }


        public string CurrentIcon
        {
            get { return IsTyping ? "\uE71A" : "\uE724"; }
        }

        public string CurrentRecordIcon
        {
            get { return IsRecording ? "\uF12E" : "\uE1D6"; }
        }

        public string CurrentUploadIcon
        {
            get { return "\uEB9F"; }
        }

        public ICommand CurrentCommand
        {
            get { return IsTyping ? StopGeneratingResponseCommand : SendMessageCommand; }

        }

        public string SelectedMode
        {
            get => _selectedMode;
            set => SetProperty(ref _selectedMode, value);
        }

        public string SelectedModel
        {
            get => _userSettingsService.Get<string>(UserSettings.ChatModel);
            set => _userSettingsService.Set(UserSettings.ChatModel, value);
        }

        public bool IsAssistantChat
        {
            get => CurrentConversation?.ConversationType.Contains("Assistant") ?? false;
        }

        public bool CurrentModelSupportsVision
        {
            get
            {
                var model = CurrentConversation?.Model ?? "";
                return ModelConstants.VisionModels.Contains(model);
            }
        }

        public bool IsActiveViewModel
        {
            get => _isActiveViewModel;
            set => SetProperty(ref _isActiveViewModel, value);
        }

        public ConversationViewModel CurrentConversation { get; set; }


        public ObservableCollection<ChatMessageViewModel> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        public ObservableCollection<ChatImageDto> SelectedImages
        {
            get => _selectedImages;
            set => SetProperty(ref _selectedImages, value);
        }
        public string UserInput
        {
            get => _userInput;
            set => SetProperty(ref _userInput, value);
        }

        public bool IsTyping
        {
            get => _isTyping;
            set
            {
                SetProperty(ref _isTyping, value);
                OnPropertyChanged(nameof(CurrentIcon));
                OnPropertyChanged(nameof(CurrentCommand));
            }
        }

        public bool IsStreamingEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.StreamReply);
            set => _userSettingsService.Set(UserSettings.StreamReply, value);
        }

        public bool IsSpeechToTextEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.SpeechToTextEnabled);
        }

        public bool IsAuthWindowVisible
        {
            get => _isAuthWindowVisible;
            set => SetProperty(ref _isAuthWindowVisible, value);
        }

        public ObservableCollection<string> Modes
        {
            get => _modes;
            set
            {
                SetProperty(ref _modes, value);
            }
        }

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                SetProperty(ref _isRecording, value);
                OnPropertyChanged(nameof(CurrentRecordIcon));
            }
        }

        public ObservableCollection<string> AttachedImages
        {
            get => _attachedImages;
            set => SetProperty(ref _attachedImages, value);
        }

        public async Task InitializeMediaCapture()
        {
            await _audioService.InitializeMediaCapture();
        }

        private async Task RecordAudio()
        {
            try
            {
                if (!IsRecording && IsSpeechToTextEnabled)
                {
                    await _audioService.RecordAudio();
                    IsRecording = true;
                    return;
                }

                IsRecording = false;
                var stream = await _audioService.StopRecordingAudio();
                var buffer = await _whisperService.StreamToBuffer(stream);
                var text = await _whisperService.Translate("test.mp3", buffer);
                await SendChatRequest(text);
            }
            catch (Exception ex)
            {
                _appNotificationService.Display(ex.Message);
            }
        }

        private void CopyMessageToPrompt(ChatMessageViewModel message)
        {
            if (message.Content != null)
            {
                UserInput = message.Content;
            }
        }

        public async Task<string> RequestAccessToken(string authCode)
        {
            return await _authenticationService.RequestSpotifyAccessToken(authCode);
        }

        public void InitializeChat(ConversationDto conversation)
        {
            // If the conversation has an assistant, then we need to create a new chat with that assistant
            // If the conversation has no assistant, then we need to create a new chat with no assistant
            CurrentConversation = ConversationViewModel.FromDto(conversation);

            if (conversation is AssistantConversationDto assistantConversationDto)
            {
                SelectedMode = "Assistant";
            }
            else if (conversation is OpenAIAssistantConversationDto openAIAssistantConversationDto)
            {
                SelectedMode = "Assistant";
            }
            else
            {
                SelectedMode = "Chat";
            }

            OnPropertyChanged(nameof(IsAssistantChat));
        }

        public async Task InitializeInstantChat(PromptDto promptDto = null)
        {
            var instructions = (promptDto != null && !string.IsNullOrEmpty(promptDto.Content)) 
                ? promptDto.Content 
                : null;

            var model = _userSettingsService.Get<string>(UserSettings.ChatModel) ?? "gpt-4-0125-preview";


            CurrentConversation = new ConversationViewModel
            {
                Title = "New Chat",
                Model = model,
                Id = null,
                ConversationType = "Standard",
                Instructions = instructions
            };

            OnPropertyChanged(nameof(IsAssistantChat));
            OnPropertyChanged(nameof(CurrentModelSupportsVision));
        }

        public void InitializeChat(AssistantDto assistant)
        {
            if (assistant.AssistantType == AssistantType.OpenAI)
            {
                CurrentConversation = new ConversationViewModel
                {
                    ConversationType = "OpenAI Assistant",
                    Title = "New Chat",
                    Model = assistant.Model,
                    AssistantId = assistant.Id,
                    AssistantType = assistant.AssistantType,
                    CreatedAt = DateTime.Now,
                    ThreadId = null,
                    Id = null
                };
            }
            else
            {
                // Custom assistant type (similar to chat mode)
                CurrentConversation = new ConversationViewModel
                {
                    ConversationType = "Assistant",
                    Title = "New Chat",
                    Model = assistant.Model,
                    AssistantId = assistant.Id,
                    AssistantType = assistant.AssistantType,
                    CreatedAt = DateTime.Now,
                    Id = null
                };
            }

            SelectedMode = "Assistant";
            OnPropertyChanged(nameof(IsAssistantChat));
        }

        public async Task LoadMessages()
        {
            var messageDtos = await _messageDataService.GetMessagesAsync(CurrentConversation.Id);
            List<ChatMessageViewModel> messages = messageDtos
                .Select(dto => _chatMessageViewModelFactory.CreateViewModelFromDto(dto))
                .ToList();

            Messages = new ObservableCollection<ChatMessageViewModel>(messages);
        }

        private async Task SaveSelectedPhotos()
        {
            if (SelectedImages != null && SelectedImages.Count > 0)
            {
                string[] urls = SelectedImages.Select(image => image.LocalUri.ToString()).ToArray();
                await _fileDownloadService.DownloadFilesToFolderAsync(urls);
                //await _fileDownloadService.DownloadFilesAsync(urls);
                SelectedImages.Clear();
            }
        }

        private async Task DeleteMessages()
        {
            if (Messages.Count > 0)
            {
                var result = await _dialogService.ShowDeleteDialogAsync();
                if (result == true)
                {
                    Messages.Clear();
                }
            }
        }

        private void StopGeneratingResponse()
        {
            if (IsTyping == false) { return; }

            _openAIChatService.CancelStream();

            if (IsAuthWindowVisible)
            {
                IsAuthWindowVisible = false;
                _authenticationService.CancelAuthentication();
            }
        }

        private void OnChatModeChanged(object sender, ModeChangedEventArgs e)
        {
            SelectedMode = e.Mode;
        }

        private void OnUserSettingChanged(object sender, string e)
        {
            if (e == UserSettings.StreamReply)
            {
                OnPropertyChanged(nameof(IsStreamingEnabled));
            }
        }

        private async void OnChatMessageReceived(object sender, MessageReceivedEvent message)
        {
            // Due to event publishing, when we leave the page after sending a chat and come back
            // before the response is received, the response will be received here and we need to
            // handle it properly. In this case, we ignore the response. Eventually update VM
            // to send a cancellation request to the chat service API when leaving the page
            if (IsActiveViewModel == false || CurrentConversation.Id == null) return;

            // TODO: Handle previousContentLength keeping old value when switching chats in chats page

            string newContent = "";
            if (message.Message is OpenAIChatResponseDto openAIChatResponseDto)
            {
                newContent = openAIChatResponseDto.Content.Substring(previousContentLength);
                previousContentLength = openAIChatResponseDto.Content.Length;
            }
            else if (message.Message is AnthropicChatResponseDto anthropicChatResponseDto)
            {
                newContent = anthropicChatResponseDto.Content.Substring(previousContentLength);
                previousContentLength = anthropicChatResponseDto.Content.Length;
            }
            else if (message.Message is GoogleChatResponseDto googleChatResponseDto)
            {
                newContent = googleChatResponseDto.Content.Substring(previousContentLength);
                previousContentLength = googleChatResponseDto.Content.Length;
            }
            else if (message.Message is LMStudioChatResponseDto lmStudioChatResponseDto)
            {
                newContent = lmStudioChatResponseDto.Content.Substring(previousContentLength);
                previousContentLength = lmStudioChatResponseDto.Content.Length;
            }

            await _dispatcherService.RunOnUIThreadAsync(async () =>
            {
                Messages[newMessageIndex].Content += newContent;
            });
        }

        private void ChatViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MessageAdded?.Invoke(this, EventArgs.Empty);
        }

        public async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            var message = UserInput;
            UserInput = string.Empty;

            await CheckIfNewChat(message);

            var attachedImageUris = AttachedImages.ToList();
            AttachedImages = new ObservableCollection<string>();

            switch (SelectedMode)
            {
                case "Assistant":
                    await SendAssistantChatRequest(message);
                    break;
                case "Chat":
                    await SendChatRequest(message, attachedImageUris);
                    break;
                case "Image":
                    await SendImageRequest(message);
                    break;
            }
        }

        private async Task CheckIfNewChat(string message = null)
        {
            if (CurrentConversation == null)
            {
                _appNotificationService.Display("No conversation set");
                return;
            }
            else if (CurrentConversation.Id != null)
            {
                return;
            }

            CurrentConversation.Id = Guid.NewGuid().ToString();
            CurrentConversation.CreatedAt = DateTime.UtcNow;
            CurrentConversation.Title = message.Substring(0, Math.Min(message.Length, 30));

            if (CurrentConversation.ConversationType == "OpenAI Assistant" && CurrentConversation.ThreadId == null)
            {
                // Create new thread with OpenAI Assistants API to tie to this conversation
                var thread = await _openAIAssistantManagerService.CreateThreadAsync();
                CurrentConversation.ThreadId = thread.Id;
            }

            // TODO: Issue with the type of DTO it converts to
            await _conversationDataService.SaveConversationAsync(CurrentConversation.ToDto());
        }

        private async Task SendAssistantChatRequest(string message)
        {
            IsTyping = true;

            var userMessageViewModel = await CreateNewUserMessage(message);
            Messages.Add(userMessageViewModel);

            var assistantMessageDto = new ChatMessageDto
            {
                Role = "assistant",
                Content = "Assistant is typing...", // OpenAI Assistants dont support streaming
                ConversationId = CurrentConversation.Id
            };
            var assistantMessageViewModel = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(assistantMessageDto);
            Messages.Add(assistantMessageViewModel);
            newMessageIndex = Messages.IndexOf(assistantMessageViewModel);

            List<string> botTextMessages = new List<string>();

            try
            {
                var apiMessages = await MapMessagesToApiMessagesAsync();
                var assistantId = CurrentConversation.AssistantId;
                var threadId = CurrentConversation.ThreadId;

                if (CurrentConversation.AssistantType == AssistantType.OpenAI)
                {
                    var assistant = await _assistantDataService.GetAssistantByIdAsync(assistantId);
                    if (assistant == null)
                    {
                        // Conversations leftover after assistant deletion will have no assistant
                        // tied to them, since we avoid deleting conversations when deleting assistants
                        // we need to handle this case
                        _appNotificationService.Display("Unable to message assistant, it may have been deleted. Please create a new chat.");
                        Messages.Remove(assistantMessageViewModel);

                        return;
                    }

                    assistantId = assistant.ExternalAssistantId;
                }

                var botResponse =
                    await Task.Run(async () => await _openAIAssistantManagerService.SendMessage(assistantId, threadId, message));

                if (botResponse == null || botResponse.Count == 0)
                {
                    Messages[newMessageIndex] = new ChatMessageViewModel
                    {
                        Role = "assistant",
                        Content = "Sorry, an error occurred. Please try again."
                    };
                }
                else
                {
                    botTextMessages = botResponse
                        .SelectMany(botMessage => botMessage.Content)    // Flatten the Content lists from all messages
                        .Where(content => content.Type == "text")  // Filter for content with Type "text"
                        .Select(content => content.Text.Value)     // Project each TextContent to its Value
                        .ToList();

                    Messages.RemoveAt(newMessageIndex);

                    var userMessageDto = _chatMessageViewModelMapper.MapToDto(userMessageViewModel);
                    await _messageDataService.SaveMessageAsync(userMessageDto);

                    foreach (var textMessage in botTextMessages)
                    {
                        var assistantTextMessageDto = new ChatMessageDto
                        {
                            Role = "assistant",
                            Content = textMessage,
                            ConversationId = CurrentConversation.Id
                        };
                        await _messageDataService.SaveMessageAsync(assistantTextMessageDto);

                        var assistantTextMessageViewModel = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(assistantTextMessageDto);
                        Messages.Add(assistantTextMessageViewModel);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                var errMessage = "Operation timed out / cancelled. Please try again.";
                await UpdateErrorMessageAsync(errMessage, ex.Message);
            }
            catch (Exception ex)
            {
                await UpdateErrorMessageAsync("Sorry, an error occurred.", ex.Message);
            }
            finally
            {
                await _dispatcherService.RunOnUIThreadAsync(async () =>
                {
                    IsTyping = false;
                    previousContentLength = 0;

                    await SynthesizeSpeech(string.Join("\n", botTextMessages));
                });
            }
        }

        private string GetChatServiceTypeFromModel(string model)
        {
            string serviceType;
            bool isApiKeyValid;

            // TODO: Replace how we get the service type after refactor is finished
            // This was a workaround to get the app to pass in the MS App Store submission
            // since on invalid key the app would crash and rejected the submission, instead
            // just displays the error message for now
            if (model.StartsWith("gpt"))
            {
                // CHeck if API key is filled, if not display message
                isApiKeyValid = _openAIChatService.ValidateApiKey();
                serviceType = "openai";
            }
            else if (model.StartsWith("claude"))
            {
                isApiKeyValid = _claudeChatService.ValidateApiKey();
                serviceType = "anthropic";
            }
            else if (model.StartsWith("gemini"))
            {
                isApiKeyValid = _googleChatService.ValidateApiKey();
                serviceType = "google";
            }
            else if (model.StartsWith("@lmstudio/"))
            {
                // No API key required for LMStudio
                return "lmstudio";
            }
            else
            {
                isApiKeyValid = _openAIChatService.ValidateApiKey();
                serviceType = "openai";
            }

            if (!isApiKeyValid)
            {
                throw new Exception($"Please enter an API key for {serviceType} in Settings to chat.");
            }

            return serviceType;
        }

        private async Task SendChatRequest(string message, List<string> imageUris = null)
        {
            IsTyping = true;

            var userMessageViewModel = await CreateNewUserMessage(message, imageUris);
            Messages.Add(userMessageViewModel);
            var model = CurrentConversation.Model ?? SelectedModel;

            var assistantMessageViewModel = CreateTemporaryAssistantMessage(model);
            Messages.Add(assistantMessageViewModel);
            newMessageIndex = Messages.IndexOf(assistantMessageViewModel);


            try
            {
                var chatServiceType = GetChatServiceTypeFromModel(model);
                var defaultPromptId = _userSettingsService.Get<string>(UserSettings.DefaultPromptId);
                if (defaultPromptId != null && string.IsNullOrEmpty(CurrentConversation.Instructions))
                {
                    var prompt = await _promptDataService.GetAsync(defaultPromptId);
                    CurrentConversation.Instructions = prompt?.Content;
                }

                var modelInstructions = CurrentConversation.Instructions ?? "";
                var apiMessages = await MapMessagesToApiMessagesAsync(chatServiceType);
                var chatSettings = _chatSettingsFactory.CreateChatSettings(chatServiceType);
                var chatRequestBuilder = _chatRequestBuilderFactory.GetBuilder(chatServiceType);
                var chatRequest = chatRequestBuilder.BuildChatRequest(model, modelInstructions, apiMessages, chatSettings);

                ChatResponseDto botChatResponseDto = await GenerateChatResponseAsync(chatServiceType, chatRequest);
                if (botChatResponseDto.Content != null)
                {
                    var userMessageDto = _chatMessageViewModelMapper.MapToDto(userMessageViewModel);
                    await _messageDataService.SaveMessageAsync(userMessageDto);

                    var botChatMessageDto = new ChatMessageDto
                    {
                        Role = botChatResponseDto.Role,
                        Content = botChatResponseDto.Content,
                        ConversationId = CurrentConversation.Id
                    };

                    await _messageDataService.SaveMessageAsync(botChatMessageDto);

                    // If streaming is enabled, the message is already set in Messages due to the 
                    // OnChatMessageReceived event
                    if (!IsStreamingEnabled || ModelConstants.ModelsWithoutStreamingSupport.Contains(model))
                    {
                        // TODO: Look into updating the message instead of replacing it
                        // May have render issues like this
                        var botMessageViewModel = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(botChatMessageDto);
                        Messages[newMessageIndex] = botMessageViewModel;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                await UpdateErrorMessageAsync("Operation timed out / cancelled. Please try again.", ex.Message);
            }
            catch (Exception ex)
            {
                await UpdateErrorMessageAsync("Sorry, an error occurred.", ex.Message);
            }
            finally
            {
                await _dispatcherService.RunOnUIThreadAsync(async () =>
                {
                    IsTyping = false;
                    previousContentLength = 0;

                    try
                    {
                        await SynthesizeSpeech(Messages[newMessageIndex].Content);
                    }
                    catch (Exception ex)
                    {
                        var errMsesage = ex.Message;
                        Messages[newMessageIndex] = new ChatMessageViewModel
                        {
                            Role = "assistant",
                            Content = $"Sorry, an error occurred. {ex.Message}",
                            ErrorMessage = ex.Message
                        };
                    }
                });
            }
        }

        private async Task<ChatResponseDto> GenerateChatResponseAsync(string chatServiceType, object chatRequest)
        {
            switch (chatServiceType)
            {
                case "openai":
                    return await Task.Run(async () => await _openAIChatService.GenerateChatResponseAsync((OpenAIChatRequest)chatRequest));
                case "anthropic":
                    return await Task.Run(async () => await _claudeChatService.GenerateChatResponseAsync((AnthropicChatRequest)chatRequest));
                case "google":
                    return await Task.Run(async () => await _googleChatService.GenerateChatResponseAsync((GoogleChatRequest)chatRequest));
                case "lmstudio":
                    return await Task.Run(async () => await _lmStudioChatService.GenerateChatResponseAsync((LMStudioChatRequest)chatRequest));
                default:
                    return null;
            }
        }

        public async Task<List<IChatModelMessage>> MapMessagesToApiMessagesAsync(string chatServiceType = "openai")
        {
            var chatMessageTransformer = _chatMessageTransformerFactory.Create(chatServiceType);

            // TODO: Work on removing image messages when needed but keeping context
            IEnumerable<Task<IChatModelMessage>> tasks;
            if (CurrentConversation.Model != null && ModelSupportsVision(CurrentConversation.Model))
            {
                tasks = Messages.Take(Messages.Count - 1).Select(async message =>
                {
                    var messageDto = _chatMessageViewModelMapper.MapToDto(message);
                    var transformedMessage = await chatMessageTransformer.Transform(messageDto);
                    return transformedMessage;
                });
            }
            else
            {
                tasks = Messages
                    .Take(Messages.Count - 1)
                    .Where(msg => msg.GetType() != typeof(ChatImageMessageViewModel))
                    .Select(async message =>
                    {
                        var messageDto = _chatMessageViewModelMapper.MapToDto(message);
                        var transformedMessage = await chatMessageTransformer.Transform(messageDto);
                        return transformedMessage;
                    });
            }

            var apiMessages = await Task.WhenAll(tasks);
            return apiMessages.ToList();
        }

        private bool ModelSupportsVision(string model)
        {
            return ModelConstants.VisionModels.Contains(model);
        }

        private async Task UpdateErrorMessageAsync(string message, string errorMessage)
        {
            _loggerService.Error(errorMessage);

            await _dispatcherService.RunOnUIThreadAsync(() =>
            {
                var errorDto = new ChatMessageDto
                {
                    Role = "assistant",
                    Content = $"{message} {errorMessage}",
                    ErrorMessage = $"{message} {errorMessage}",
                    ConversationId = CurrentConversation.Id // Assuming this is relevant??
                };

                var errorMessageViewModel = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(errorDto);
                Messages[newMessageIndex] = errorMessageViewModel;
            });
        }

        private async Task<ChatMessageViewModel> CreateNewUserMessage(string message, List<string> imageUris = null)
        {
            ChatMessageDto dto;

            if (imageUris != null && imageUris.Count > 0)
            {
                var localImageUrls = await _imageFileService.CopyImagesToPermanentStorageAsync(imageUris);
                dto = new ChatImageMessageDto
                {
                    Role = "user",
                    Content = message,
                    ConversationId = CurrentConversation.Id,
                    Images = localImageUrls.Select(uri => new ChatImageDto { Url = uri }).ToList()
                };
            }
            else
            {
                dto = new ChatMessageDto
                {
                    Role = "user",
                    Content = message,
                    ConversationId = CurrentConversation.Id
                };
            }

            return _chatMessageViewModelFactory.CreateViewModelFromDto(dto);
        }

        private ChatMessageViewModel CreateTemporaryAssistantMessage(string model)
        {
            var tempMessage = (IsStreamingEnabled && !ModelConstants.ModelsWithoutStreamingSupport.Contains(model)) 
                ? "" 
                : "Assistant is typing...";

            var assistantMessageDto = new ChatMessageDto
            {
                Role = "assistant",
                Content = tempMessage,
                ConversationId = CurrentConversation.Id
            };

            return _chatMessageViewModelFactory.CreateViewModelFromDto(assistantMessageDto);
        }

        private async Task SynthesizeSpeech(string text)
        {
            var speechEnabled = _userSettingsService.Get<bool>(UserSettings.SpeechEnabled);
            if (!speechEnabled) return;

            var speechProvider = _userSettingsService.Get<string>(UserSettings.SpeechProvider);
            await _speechService.ProcessTextToSpeech(speechProvider, text);
        }

        public void AuthenticationComplete(string token)
        {
            _authenticationService.CompleteAuthentication(true);
        }

        private async Task SendImageRequest(string message)
        {
            IsTyping = true;

            var userMessageDto = new ChatMessageDto
            {
                Role = "user",
                Content = message,
                ConversationId = CurrentConversation.Id
            };
            var userMessage = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(userMessageDto);
            Messages.Add(userMessage);

            var imageMessageDto = new ChatImageMessageDto
            {
                Role = "assistant",
                Content = "Assistant is generating image...",
                ConversationId = CurrentConversation.Id,
                Images = new List<ChatImageDto>() // Assuming empty or relevant image data here
            };
            var assistantMessage = _chatMessageViewModelFactory.CreateViewModel<ChatImageMessageViewModel>(imageMessageDto);
            Messages.Add(assistantMessage);
            newMessageIndex = Messages.IndexOf(assistantMessage);

            var imagePromptDto = new PromptDto
            {
                Content = message,
                PromptType = PromptType.ImageGeneration
            };

            // TODO: Fix switching back to chats after this mode, ignore image messages or 
            // if images exist in the list of messages default to a vision model
            try
            {
                var imageGenerationResponse = await _imageService.GenerateImagesAsync(imagePromptDto);
                var savedImages = await _imageFileService.SaveImagesAsync(imageGenerationResponse.ImageUrls);
                var imageDtos = savedImages.Select(image => new ChatImageDto
                {
                    Url = image.RelativePath,
                    ModelIdentifier = imageGenerationResponse.Settings.Model,
                    MessageId = Messages[newMessageIndex].Id,
                    PromptId = imagePromptDto.Id,
                    Size = imageGenerationResponse.Settings.Size
                }).ToList();

                // TODO: Fix issue where URL is now a relative path and not a URL to an image (doesnt load in gridview)

                assistantMessage.Images = imageDtos;
                assistantMessage.Content = null;
                assistantMessage.MessageType = MessageType.Image;

                await _promptDataService.AddPromptAsync(imagePromptDto);
                await _messageDataService.SaveMessageAsync(userMessageDto);
                var assistantMessageDto = _chatMessageViewModelMapper.MapToDto(assistantMessage);
                await _messageDataService.SaveMessageAsync(assistantMessageDto);
                // No need to save imageDtos since they are saved with the message by EF Core

                await _dispatcherService.RunOnUIThreadAsync(() =>
                {
                    Messages[newMessageIndex] = assistantMessage;
                });
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Error sending image request: {ex.Message}");

                await _dispatcherService.RunOnUIThreadAsync(() =>
                {
                    var errorDto = new ChatMessageDto
                    {
                        Role = "assistant",
                        Content = $"Sorry, an error occurred. {ex.Message}",
                        ErrorMessage = ex.Message,
                        ConversationId = CurrentConversation.Id // assuming this is relevant here??
                    };

                    var errorMessageViewModel = _chatMessageViewModelFactory.CreateViewModel<ChatMessageViewModel>(errorDto);
                    Messages[newMessageIndex] = errorMessageViewModel;
                });
            }
            finally
            {
                await _dispatcherService.RunOnUIThreadAsync(() =>
                {
                    IsTyping = false;
                });
            }
        }

        public async Task ExportChatToFile()
        {
            var msgs = Messages
                .Select(m => _chatMessageViewModelMapper.MapToDto(m))
                .ToList();

            await _chatFileService.SaveMessagesToFileAsync($"messages-export.txt", msgs);
        }

        public async Task ImportChatFromFile()
        {
            _appNotificationService.Display("Feature temporarily disabled until a future release");
            return;

            // TODO: Need to create conversation and messages in db when imported
            //var messages = await _chatFileService.LoadMessagesFromFileAsync();
        }

        public void DeleteMessage(ChatMessageViewModel message)
        {
            // OpenAI Assistants manage their own context, so we can't delete messages
            if (IsAssistantChat)
            {
                _appNotificationService.Display("Cannot delete messages in an OpenAI assistant chat");
                return;
            }

            // TODO: When custom assistants are implemented, it will manage context and we can delete messages
            // and will need to specify only OpenAI above to not delete messages

            Messages.Remove(message);
        }

        public void CopyMessage(ChatMessageViewModel message)
        {
            var messageDto = _chatMessageViewModelMapper.MapToDto(message);
            _windowsService.CopyToClipboard(messageDto);
        }

        private async Task AttachImage()
        {
            IEnumerable<string> chosenImagePaths = await _imageFileService.ChooseImagesAsync();
            List<string> imagesToAttach = chosenImagePaths.ToList();
            AttachedImages = new ObservableCollection<string>(imagesToAttach);
            _appNotificationService.Display($"Attached {imagesToAttach.Count} images");
        }

        private async Task AttachImageFromClipboard()
        {
            var imageBuffer = await _windowsService.GetImageFromClipboardAsPngAsync();
            if (imageBuffer != null)
            {
                var tempImagePath = await _imageFileService.SaveImageToTemporaryAsync(imageBuffer);
                var currentAttachedImages = AttachedImages.ToList();
                currentAttachedImages.Add(tempImagePath);
                AttachedImages = new ObservableCollection<string>(currentAttachedImages);
            }
        }

        private void OnChatUpdated(object sender, ChatUpdatedEvent e)
        {
            if (CurrentConversation != null)
            {
                CurrentConversation = ConversationViewModel.FromDto(e.Conversation);
                OnPropertyChanged(nameof(CurrentModelSupportsVision));
            }
        }


        public async Task EditChat()
        {
            var conversationDto = CurrentConversation.ToDto();
            bool result = await _dialogService.ShowEditChatDialogAsync(conversationDto);
            if (result != true)
            {
                return;
            }

            if (conversationDto.Model == "gpt-4-vision-preview" && conversationDto.ConversationType == "OpenAI Assistant")
            {
                _appNotificationService.Display("Unable to update, OpenAI Assistants don't support vision models");
                return;
            }

            await _conversationDataService.UpdateConversationAsync(conversationDto);

            CurrentConversation = ConversationViewModel.FromDto(conversationDto);
            _eventAggregator.PublishChatUpdated(new Events.ChatUpdatedEvent { Conversation = conversationDto });
            _appNotificationService.Display("Chat updated successfully");
        }
    }
}