using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Files;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class ImagesPageViewModel : Observable, IDisposable
    {
        private readonly IImageDataService _imageDataService;
        private readonly IImageFileService _imageFileService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPromptDataService _promptDataService;
        private readonly IFileDownloadService _fileDownloadService;
        private readonly ILoggerService _loggerService;

        private ObservableCollection<ChatImageDto> _images;
        private ObservableCollection<ChatImageDto> _selectedImages;
        private bool _isImageDetailsEnabled;

        public ImagesPageViewModel(
            IImageDataService imageDataService,
            IImageFileService imageFileService,
            IAppNotificationService appNotificationService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IPromptDataService promptDataService,
            IFileDownloadService fileDownloadService,
            ILoggerService loggerService)
        {
            _imageDataService = imageDataService;
            _imageFileService = imageFileService;
            _appNotificationService = appNotificationService;
            _dialogService = dialogService;
            _promptDataService = promptDataService;
            _eventAggregator = eventAggregator;
            _fileDownloadService = fileDownloadService;

            _images = new ObservableCollection<ChatImageDto>();
            _eventAggregator.ImageGenerated += OnImageGenerated;

            SelectedImages = new ObservableCollection<ChatImageDto>();

            ShowGenerateImageDialogCommand = new AsyncRelayCommand(ShowGenerateImageDialog);
            DeleteImagesCommand = new AsyncRelayCommand(DeleteSelectedImages);
            SaveSelectedPhotosCommand = new AsyncRelayCommand(SaveSelectedPhotos);
            _loggerService = loggerService;
        }


        ~ImagesPageViewModel()
        {
            Dispose();
        }

        public void Dispose()
        {
            _eventAggregator.ImageGenerated -= OnImageGenerated;
        }

        public ICommand SelectImageCommand { get; set; }
        public ICommand ShowGenerateImageDialogCommand { get; set; }
        public ICommand DeleteImagesCommand { get; set; }
        public ICommand SaveSelectedPhotosCommand { get; set; }

        public ObservableCollection<ChatImageDto> Images
        {
            get => _images;
            set
            {
                SetProperty(ref _images, value);
                OnPropertyChanged(nameof(IsImagesEmpty));
            }
        }

        public bool IsImageDetailsEnabled
        {
            get => _isImageDetailsEnabled;
            set => SetProperty(ref _isImageDetailsEnabled, value);
        }

        public ObservableCollection<ChatImageDto> SelectedImages
        {
            get => _selectedImages;
            set => SetProperty(ref _selectedImages, value);
        }

        public bool IsImagesEmpty => Images.Count == 0;

        private async void OnImageGenerated(object sender, ImageGeneratedEvent e)
        {
            try
            {
                var response = e.ImageGenerationResponse;
                var promptDto = e.PromptDto;
                
                var savedImages = await _imageFileService.SaveImagesAsync(response.ImageUrls);
                var imageDtos = savedImages.Select(image => new ChatImageDto
                {
                    Url = image.RelativePath,
                    ModelIdentifier = response.Settings.Model,
                    MessageId = null, // Not necessarily tied to a message in this case
                    PromptId = promptDto.Id,
                    Size = response.Settings.Size,
                    Prompt = promptDto.Content
                }).ToList();

                await _promptDataService.AddPromptAsync(promptDto);
                await _imageDataService.SaveImagesAsync(imageDtos);

                foreach (var image in imageDtos)
                {
                    var index = Images
                        .TakeWhile(existingImage => existingImage.CreatedAt >= image.CreatedAt)
                        .Count();

                    Images.Insert(index, image);
                }

                OnPropertyChanged(nameof(IsImagesEmpty));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error saving generated images");
                _appNotificationService.Display($"Error saving generated images: {ex.Message}");
                Console.Error.WriteLine(ex);
            }
        }

        public void LoadImages(IEnumerable<ChatImageDto> images)
        {
            Images = new ObservableCollection<ChatImageDto>(images);
        }

        public async Task LoadImages()
        {
            var imageDtos = await _imageDataService.GetImagesAsync();

            // TODO: Optimize image loading, possibly do in a repository method
            // and maybe loading symbols before image is loaded

            var orderedImageDtos = imageDtos
                .Where(i => !i.IsUserImage)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

            Images = new ObservableCollection<ChatImageDto>(orderedImageDtos);
        }

        private async Task ShowGenerateImageDialog()
        {
            await _dialogService.ShowGenerateImageDialogAsync();
        }

        private async Task DeleteSelectedImages()
        {
            try
            {
                if (SelectedImages == null || SelectedImages.Count == 0)
                {
                    _appNotificationService.Display("No images selected to delete");
                    return;
                }

                var result = await _dialogService.ShowDeleteDialogAsync();
                if (result == false)
                {
                    return;
                }

                foreach (var image in SelectedImages.ToList())
                {
                    await _imageDataService.DeleteImageAsync(image);
                    Images.Remove(image);
                }

                SelectedImages = new ObservableCollection<ChatImageDto>();
                OnPropertyChanged(nameof(IsImagesEmpty));

                _appNotificationService.Display("Images deleted successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error deleting image");
                _appNotificationService.Display($"Error deleting image: {ex.Message}");
                Console.Error.WriteLine(ex);
            }
        }

        private async Task SaveSelectedPhotos()
        {
            try
            {
                if (SelectedImages == null || SelectedImages.Count == 0)
                {
                    _appNotificationService.Display("Please select an image to save");
                    return;
                }

                string[] imageUrls = SelectedImages
                    .Select(x => x.LocalUri.ToString())
                    .ToArray();

                await _fileDownloadService.DownloadFilesToFolderAsync(imageUrls);
                SelectedImages = new ObservableCollection<ChatImageDto>();

                _appNotificationService.Display("Images saved successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Error saving images");
                _appNotificationService.Display($"Error saving images: {ex.Message}");
                Console.Error.WriteLine(ex);
            }
        }
    }
}