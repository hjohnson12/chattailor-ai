using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Services.Speech;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Chat.LMStudio;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Models.Shared;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class SettingsPageViewModel : Observable
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IFileService _fileService;
        private readonly INavigationService _navigationService;
        private readonly IAzureSpeechService _azureSpeechService;
        private readonly IOpenAISpeechService _openAISpeechService;
        private readonly IElevenLabsSpeechService _elevenLabsSpeechService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPromptDataService _promptDataService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly ILoggerService _loggerService;
        private readonly ILMStudioChatService _lmStudioChatService;
        private readonly IModelManagerService _modelManagerService;

        private ObservableCollection<string> _models;
        private ObservableCollection<string> _imageModels;
        private ObservableCollection<string> _voiceNames;
        private ObservableCollection<string> _speechModels;
        private ObservableCollection<string> _speechProviders;
        private ObservableCollection<string> _imageCountOptions;
        private ObservableCollection<string> _imageSizeOptions;
        private ObservableCollection<string> _imageQualityOptions;

        public SettingsPageViewModel(
            IUserSettingsService userSettingsService,
            IFileService fileService,
            INavigationService navigationService,
            IAzureSpeechService azureSpeechService,
            IOpenAISpeechService openAISpeechService,
            IElevenLabsSpeechService elevenLabsSpeechService,
            IEventAggregator eventAggregator,
            IPromptDataService promptDataService,
            ILoggerService loggerService,
            ILMStudioChatService lmStudioChatService,
            IModelManagerService modelManagerService)
        {
            _userSettingsService = userSettingsService;
            _navigationService = navigationService;
            _azureSpeechService = azureSpeechService;
            _openAISpeechService = openAISpeechService;
            _elevenLabsSpeechService = elevenLabsSpeechService;
            _eventAggregator = eventAggregator;
            _promptDataService = promptDataService;
            _loggerService = loggerService;
            _lmStudioChatService = lmStudioChatService;
            _modelManagerService = modelManagerService;

            _fileService = fileService;

            _modelManagerService = modelManagerService;
            Models = new ObservableCollection<string>(_modelManagerService.GetAllModels());

            SpeechProviders = new ObservableCollection<string>
            {
                "azure",
                "openai",
                "elevenlabs"
            };

            ImageModels = new ObservableCollection<string>
            {
                "dall-e-3",
                "dall-3-2"
            };

            ImageCountOptions = new ObservableCollection<string>
            {
                "1","2","3","4","5","6","7","8","9","10"
            };

            ImageSizeOptions = new ObservableCollection<string>
            {
                "1024x1024", "512x512", "256x256"
            };

            ImageQualityOptions = new ObservableCollection<string>
            {
                "standard", "hd"
            };

            // TODO: Change to use selectionChanged or a behavior instead
            PropertyChanged += OnSelectedSpeechProviderChanged;

            string[] selectedFunctions = _userSettingsService
                .Get<string>(UserSettings.FunctionsSelected)
                .Split(',');

            GroupInfoCollection = new ObservableCollection<FunctionListGroupInfo>
            {
                new FunctionListGroupInfo
                {
                    Header = "Spotify",
                    Items = new List<FunctionListItem>
                    {
                        new FunctionListItem
                        {
                            Key = "Get playlist names",
                            Value = "get_my_playlist_names",
                            IsSelected = selectedFunctions.Contains("get_my_playlist_names")
                        },
                        new FunctionListItem
                        {
                            Key = "Get songs from playlist",
                            Value = "get_playlist_songs",
                            IsSelected = selectedFunctions.Contains("get_playlist_songs")
                        },
                        new FunctionListItem
                        {
                            Key = "Add song to playlist",
                            Value = "add_song_to_playlist",
                            IsSelected = selectedFunctions.Contains("add_song_to_playlist")
                        },
                        new FunctionListItem
                        {
                            Key = "Play a song",
                            Value = "search_and_play_song",
                            IsSelected = selectedFunctions.Contains("search_and_play_song")
                        },
                    }
                }
            };


            Prompts = new ObservableCollection<PromptDto>();

            LoadVoicesCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await LoadVoicesAsync());
            TextLostFocusCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(OnLostFocus);
            SelectedItemsCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(GetSelectedItems);
            SavePromptCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await SavePrompt());
            DeletePromptCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await DeletePrompt());
            DeletePromptsCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await DeletePrompts());
            ClearPromptCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => ClearPrompt());
            SetSelectedPromptCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SetSelectedPrompt());
        }

        public ICommand TextLostFocusCommand { get; }
        public ICommand LoadVoicesCommand { get; }
        public ICommand SelectedItemsCommand { get; }
        public ICommand SavePromptCommand { get; }
        public ICommand DeletePromptCommand { get; }
        public ICommand DeletePromptsCommand { get; }
        public ICommand ClearPromptCommand { get; }
        public ICommand SetSelectedPromptCommand { get; }

        private async void OnSelectedSpeechProviderChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedSpeechProvider))
            {
                await LoadVoicesAsync();
                await LoadSpeechModelsAsync();
            }
        }

        private void SetSelectedPrompt()
        {
            if (SelectedPrompt == null) return;

            DefaultPromptId = SelectedPrompt.Id;
            SystemMessage = SelectedPrompt.Content;
            PromptTitle = SelectedPrompt.Title;

            // Doesn't use SetProperty in the setter currently
            OnPropertyChanged(nameof(SystemMessage));
            OnPropertyChanged(nameof(PromptTitle));
        }

        private void ClearPrompt()
        {
            _userSettingsService.Set<string>(UserSettings.DefaultPromptId, null);
            SystemMessage = null;
            PromptTitle = null;

            // Doesn't use SetProperty in the setter currently
            OnPropertyChanged(nameof(SystemMessage));
            OnPropertyChanged(nameof(PromptTitle));
        }

        public async Task DeletePrompts()
        {
            if (Prompts == null) return;

            await _fileService.DeletePromptsAsync();
            Prompts.Clear();
        }

        public async Task DeletePrompt()
        {
            IsFlyoutOpen = false;

            if (SelectedPrompt == null) return;

            var prompts = Prompts.ToList();
            int indexToDelete = prompts.FindIndex(p => Convert.ToInt32(p.Id) == Convert.ToInt32(SelectedPrompt.Id));

            if (indexToDelete != -1)
            {
                prompts.RemoveAt(indexToDelete);

                for (int i = indexToDelete; i < prompts.Count; i++)
                {
                    var id = Convert.ToInt32(prompts[i].Id);
                    prompts[i].Id = id--.ToString();
                }

                await _fileService.UpdatePromptsAsync(prompts);
                Prompts = new ObservableCollection<PromptDto>(prompts);
            }
        }

        public async Task LoadPromptsAsync()
        {
            try
            {
                var promptsFromDb = await _promptDataService.GetAllAsync();

                List<PromptDto> prompts = promptsFromDb
                    .Where(p => p.PromptType == PromptType.Standard)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList();
                
                Prompts = new ObservableCollection<PromptDto>(prompts);

                // Load default prompt info
                var defaultPromptId = _userSettingsService.Get<string>(UserSettings.DefaultPromptId);
                if (!string.IsNullOrEmpty(defaultPromptId))
                {
                    var defaultPrompt = prompts.FirstOrDefault(p => p.Id == defaultPromptId);
                    if (defaultPrompt != null)
                    {
                        SystemMessage = defaultPrompt.Content;
                        PromptTitle = defaultPrompt.Title;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error loading prompts");
                _appNotificationService.Display($"Error loading prompts: {ex.Message}");
            }
        }

        public async Task SavePrompt()
        {
            if (SystemMessage != string.Empty &&
                SystemMessage != null)
            {
                var newPrompt = new PromptDto
                {
                    Content = SystemMessage,
                    Id = Guid.NewGuid().ToString(),
                    Title = PromptTitle,
                    PromptType = PromptType.Standard,
                    IsActive = false // TODO
                };
                Prompts.Add(newPrompt);

                await _fileService.AppendPromptToFileAsync(newPrompt);
                //var promptsList = Prompts.ToList();
                //await _fileService.UpdatePromptsFlieAsync(promptsList);
            }
        }

        private void GetSelectedItems()
        {
            throw new NotImplementedException();
        }

        public async Task LoadVoicesAsync()
        {
            try
            {
                List<string> voiceNames = new List<string>();
                var selectedSpeechProvider = _userSettingsService.Get<string>(UserSettings.SpeechProvider);
                switch (selectedSpeechProvider)
                {
                    case "azure":
                        if (AzureSpeechApiKey != null && AzureSpeechApiKey != string.Empty && SpeechServiceRegion != string.Empty)
                        {
                            break;
                        }
                        voiceNames = await _azureSpeechService.GetVoicesListAsync();
                        break;
                    case "openai":
                        voiceNames = await _openAISpeechService.GetVoicesListAsync();
                        break;
                    case "elevenlabs":
                        voiceNames = await _elevenLabsSpeechService.GetVoiceNamesAsync();
                        break;
                    default:
                        break;
                }

                VoiceNames = new ObservableCollection<string>(voiceNames);
                OnPropertyChanged(nameof(SelectedVoiceName));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error loading voices, starting with empty collection...");
                VoiceNames = new ObservableCollection<string>();
            }

        }

        public async Task LoadModelsAsync()
        {
            // TODO: Eventually update how this is done
            // SelectedModel gets set to null when the models are refreshed
            try
            {
                string previousSelectedModel = SelectedModel;

                await _modelManagerService.RefreshDynamicModelsAsync();

                var newModels = new ObservableCollection<string>();
                foreach (var model in _modelManagerService.GetAllModels())
                {
                    newModels.Add(model);
                }

                Models = new ObservableCollection<string>(newModels);

                // Attempt to restore the previous selection
                if (newModels.Contains(previousSelectedModel))
                {
                    SelectedModel = previousSelectedModel;
                }
                else if (Models.Any())
                {
                    SelectedModel = "gpt-4o";
                }

                OnPropertyChanged(nameof(SelectedModel));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error loading models, starting with default collection...");
                Models = new ObservableCollection<string>();
            }
        }

        public async Task LoadSpeechModelsAsync()
        {
            try
            {
                List<string> speechModels = new List<string>();
                var selectedSpeechProvider = _userSettingsService.Get<string>(UserSettings.SpeechProvider);
                switch (selectedSpeechProvider)
                {
                    case "azure":
                        if (AzureSpeechApiKey != null && AzureSpeechApiKey != string.Empty && SpeechServiceRegion != string.Empty)
                        {
                            break;
                        }
                        speechModels = await _azureSpeechService.GetModelsListAsync();
                        break;
                    case "openai":
                        speechModels = await _openAISpeechService.GetModelsListAsync();
                        break;
                    case "elevenlabs":
                        speechModels = await _elevenLabsSpeechService.GetModelsListAsync();
                        break;
                    default:
                        break;
                }

                SpeechModels = new ObservableCollection<string>(speechModels);
                OnPropertyChanged(nameof(SelectedSpeechModel));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error loading speech models, starting with empty collection...");
                SpeechModels = new ObservableCollection<string>();
            }
        }

        private ObservableCollection<FunctionListGroupInfo> _groupInfoCollection;
        public ObservableCollection<FunctionListGroupInfo> GroupInfoCollection
        {
            get => _groupInfoCollection;
            set => SetProperty(ref _groupInfoCollection, value);
        }

        private ObservableCollection<PromptDto> _prompts;
        public ObservableCollection<PromptDto> Prompts
        {
            get => _prompts;
            set => SetProperty(ref _prompts, value);
        }

        private string _password;

        public string Password
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);
            }
            set
            {
                // TODO: Try to avoid updating per character changed
                _userSettingsService.Set(UserSettings.OpenAiApiKey, value);
            }
        }

        public string AzureSpeechApiKey
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.AzureSpeechServicesKey);
            }
            set
            {
                _userSettingsService.Set(UserSettings.AzureSpeechServicesKey, value);
                _eventAggregator.PublishApiKeyChange(new ApiKeyChangedEventArgs { ApiKey = value, KeyType = Events.ApiKeyType.AzureSpeech });
            }
        }

        public string ElevenLabsApiKey
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.ElevenLabsApiKey);
            }
            set
            {
                _userSettingsService.Set(UserSettings.ElevenLabsApiKey, value);
                _eventAggregator.PublishApiKeyChange(new ApiKeyChangedEventArgs { ApiKey = value, KeyType = Events.ApiKeyType.ElevenLabs });
            }
        }

        public string AnthropicApiKey
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.AnthropicApiKey);
            }
            set
            {
                _userSettingsService.Set(UserSettings.AnthropicApiKey, value);
                _eventAggregator.PublishApiKeyChange(new ApiKeyChangedEventArgs { ApiKey = value, KeyType = Events.ApiKeyType.Anthropic });
            }
        }

        public string GoogleAIApiKey
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.GoogleAIApiKey);
            }
            set
            {
                _userSettingsService.Set(UserSettings.GoogleAIApiKey, value);
                _eventAggregator.PublishApiKeyChange(new ApiKeyChangedEventArgs { ApiKey = value, KeyType = Events.ApiKeyType.Google });
            }
        }

        public string LMStudioServerUrl
        {
            get
            {
                return _userSettingsService.Get<string>(UserSettings.LMStudioServerUrl);
            }
            set
            {
                _userSettingsService.Set(UserSettings.LMStudioServerUrl, value);
            }
        }

        public ObservableCollection<string> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public ObservableCollection<string> ImageModels
        {
            get => _imageModels;
            set => SetProperty(ref _imageModels, value);
        }

        public ObservableCollection<string> VoiceNames
        {
            get => _voiceNames;
            set => SetProperty(ref _voiceNames, value);
        }

        public ObservableCollection<string> SpeechModels
        {
            get => _speechModels;
            set => SetProperty(ref _speechModels, value);
        }

        public ObservableCollection<string> SpeechProviders
        {
            get => _speechProviders;
            set => SetProperty(ref _speechProviders, value);
        }

        public ObservableCollection<string> ImageCountOptions
        {
            get => _imageCountOptions;
            set => SetProperty(ref _imageCountOptions, value);
        }

        public ObservableCollection<string> ImageSizeOptions
        {
            get => _imageSizeOptions;
            set => SetProperty(ref _imageSizeOptions, value);
        }

        public ObservableCollection<string> ImageQualityOptions
        {
            get => _imageQualityOptions;
            set => SetProperty(ref _imageQualityOptions, value);
        }

        private IList<FunctionListItem> _selectedFunctions;

        public IList<FunctionListItem> SelectedFunctions
        {
            get => _selectedFunctions;
            set
            {
                SetProperty(ref _selectedFunctions, value);

                var schemaNames = _selectedFunctions.Select(x => x.Value);
                var combinedSchemaNames = string.Join(",", schemaNames);
                _userSettingsService.Set(UserSettings.FunctionsSelected, combinedSchemaNames);
            }
        }

        public bool IsStreamingEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.StreamReply);
            set => _userSettingsService.Set(UserSettings.StreamReply, value);
        }

        public bool IsContinuous
        {
            get => _userSettingsService.Get<bool>(UserSettings.StreamReply);
            set => _userSettingsService.Set(UserSettings.StreamReply, value);
        }

        public bool IsSpeechEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.SpeechEnabled);
            set => _userSettingsService.Set(UserSettings.SpeechEnabled, value);
        }

        public bool IsSpeechToTextEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.SpeechToTextEnabled);
            set => _userSettingsService.Set(UserSettings.SpeechToTextEnabled, value);
        }

        public bool FunctionsEnabled
        {
            get => _userSettingsService.Get<bool>(UserSettings.FunctionsEnabled);
            set => _userSettingsService.Set(UserSettings.FunctionsEnabled, value);
        }

        public string SelectedModel
        {
            get => _userSettingsService.Get<string>(UserSettings.ChatModel);
            set => _userSettingsService.Set(UserSettings.ChatModel, value);
        }

        public string SelectedImageModel
        {
            get => _userSettingsService.Get<string>(UserSettings.ImageModel);
            set => _userSettingsService.Set(UserSettings.ImageModel, value);
        }

        public string SelectedNumberOfImages
        {
            get => _userSettingsService.Get<string>(UserSettings.ImageCount);
            set => _userSettingsService.Set(UserSettings.ImageCount, value);
        }

        public string SelectedImageQuality
        {
            get => _userSettingsService.Get<string>(UserSettings.ImageQuality);
            set => _userSettingsService.Set(UserSettings.ImageQuality, value);
        }

        public string SelectedImageSize
        {
            get => _userSettingsService.Get<string>(UserSettings.ImageSize);
            set => _userSettingsService.Set(UserSettings.ImageSize, value);
        }

        public string SelectedSpeechModel
        {
            get => _userSettingsService.Get<string>(UserSettings.SpeechModel);
            set => _userSettingsService.Set(UserSettings.SpeechModel, value);
        }

        public string SelectedVoiceName
        {
            get => _userSettingsService.Get<string>(UserSettings.VoiceName);
            set => _userSettingsService.Set(UserSettings.VoiceName, value);
        }

        public string SelectedSpeechProvider
        {
            get => _userSettingsService.Get<string>(UserSettings.SpeechProvider);
            set
            {
                _userSettingsService.Set(UserSettings.SpeechProvider, value);
                OnPropertyChanged();
            }
        }

        public int MaxTokens
        {
            get => _userSettingsService.Get<int>(UserSettings.MaxTokens);
            set => _userSettingsService.Set(UserSettings.MaxTokens, value);
        }

        public double Temperature
        {
            get => _userSettingsService.Get<double>(UserSettings.Temperature);
            set => _userSettingsService.Set(UserSettings.Temperature, value);
        }

        public double FrequencyPenalty
        {
            get => _userSettingsService.Get<double>(UserSettings.FrequencyPenalty);
            set => _userSettingsService.Set(UserSettings.FrequencyPenalty, value);
        }

        public double PresencePenalty
        {
            get => _userSettingsService.Get<double>(UserSettings.PresencePenalty);
            set => _userSettingsService.Set(UserSettings.PresencePenalty, value);
        }

        private string _systemMessage;
        public string SystemMessage
        {
            get => _systemMessage;
            set => SetProperty(ref _systemMessage, value);
        }

        private string _defaultPromptId;
        public string DefaultPromptId
        {
            get => _userSettingsService.Get<string>(UserSettings.DefaultPromptId);
            set => _userSettingsService.Set(UserSettings.DefaultPromptId, value);
        }

        private string _promptTitle;
        public string PromptTitle
        {
            get => _promptTitle;
            set
            {
                if (value != null && value.Equals(string.Empty))
                {
                    SetProperty(ref _promptTitle, null);
                    return;
                }

                SetProperty(ref _promptTitle, value);
            }
        }

        private PromptDto _selectedPrompt;
        public PromptDto SelectedPrompt
        {
            get => _selectedPrompt;
            set => SetProperty(ref _selectedPrompt, value);
        }

        public string SpeechServiceRegion
        {
            get => _userSettingsService.Get<string>(UserSettings.SpeechServiceRegion);
            set => _userSettingsService.Set(UserSettings.SpeechServiceRegion, value);
        }

        private bool _isFlyoutOpen;
        public bool IsFlyoutOpen
        {
            get { return _isFlyoutOpen; }
            set => SetProperty(ref _isFlyoutOpen, value);
        }

        public void NavigateBack()
        {
            // Certain services lsiten for API key changes, since currently it only initializes 
            // the API key for a specific service on instantiation
            // Eventually may just grab it each time before the request intead of an event
            _eventAggregator.PublishApiKeyChange(new ApiKeyChangedEventArgs { ApiKey = Password, KeyType = Events.ApiKeyType.OpenAI });

            _navigationService.NavigateBack();
        }

        public async void OnLostFocus()
        {
            await LoadVoicesAsync();
        }
    }
}