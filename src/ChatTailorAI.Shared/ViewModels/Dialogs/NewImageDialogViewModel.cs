using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Image.OpenAI;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Image;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class NewImageDialogViewModel : Observable
    {
        private readonly IAppNotificationService _appNotificationService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IImageService _imageService;
        private readonly IEventAggregator _eventAggregator;

        private bool _isGenerating;
        private string _selectedModel;
        private string _size;
        private string _prompt;
        private string _imageQuality;

        private ObservableCollection<string> _models;

        public NewImageDialogViewModel(
            IAppNotificationService appNotificationService,
            IUserSettingsService userSettingsService,
            IImageService imageService,
            IEventAggregator eventAggregator)
        {
            _appNotificationService = appNotificationService;
            _userSettingsService = userSettingsService;
            _imageService = imageService;
            _eventAggregator = eventAggregator;

            IsGenerating = false;

            Prompt = "";

            GenerateImageCommand = new AsyncRelayCommand(GenerateImageAsync);
            
            Models = new ObservableCollection<string> { "dall-e-3", "dall-e-2" };
            SelectedModel = _userSettingsService.Get<string>(UserSettings.ImageModel) ?? Models[0];
        }

        public ICommand GenerateImageCommand { get; set; }

        public bool IsGenerating
        {
            get => _isGenerating;
            set => SetProperty(ref _isGenerating, value);
        }

        public string SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public string Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public string Prompt
        {
            get => _prompt;
            set => SetProperty(ref _prompt, value);
        }

        public string ImageQuality
        {
            get => _imageQuality;
            set => SetProperty(ref _imageQuality, value);
        }

        public ObservableCollection<string> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        private async Task GenerateImageAsync()
        {
            if (string.IsNullOrEmpty(Prompt))
            {
                _appNotificationService.Display("Please enter a prompt to generate an image.");
                IsGenerating = false;
                return;
            }

            IsGenerating = true;

            var imagePromptDto = new PromptDto
            {
                Content = Prompt,
                PromptType = PromptType.ImageGeneration
            };

            var imageModel = SelectedModel;
            var numberOfImages = imageModel.Equals("dall-e-3")
                ? 1 // Dall-e-3 can only generate 1 image at a time at the moment
                : int.Parse(_userSettingsService.Get<string>(UserSettings.ImageCount));
            var imageQuality = imageModel.Equals("dall-e-3")
                ? _userSettingsService.Get<string>(UserSettings.ImageQuality)
                : null;

            var openAIImageRequest = new OpenAIImageGenerationRequest
            {
                Model = imageModel,
                Prompt = imagePromptDto.Content,
                Settings = new OpenAIImageGenerationSettings
                {
                    Model = imageModel,
                    Prompt = imagePromptDto.Content,
                    N = numberOfImages,
                    // The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2
                    // Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models
                    // TODO: Update this to be configurable, but for now, just use 1024x1024
                    Size = "1024x1024",
                    ImageQuality = imageQuality
                }
            };

            try
            {
                var imageGenerationResponse = await _imageService.GenerateImagesAsync(imagePromptDto);

                // TODO: Publish here or save to dob here? 
                _eventAggregator.PublishImageGenerated(new ImageGeneratedEvent { 
                    ImageGenerationResponse = imageGenerationResponse,
                    PromptDto = imagePromptDto
                });
            }
            catch (Exception ex)
            {
                _appNotificationService.Display($"Error generating image: {ex.Message}");
            }
            finally
            {
                IsGenerating = false;
            }
        }
    }
}