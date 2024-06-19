using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Diagnostics;
using ChatTailorAI.Shared.Models.Image;
using ChatTailorAI.Shared.Services.Files;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    public class ImageFileService : IImageFileService
    {
        private readonly IFileService _fileService;
        private readonly IFileDownloadService _fileDownloadService;

        public ImageFileService(
            IFileService fileService,
            IFileDownloadService fileDownloadService)
        {
            _fileService = fileService;
            _fileDownloadService = fileDownloadService;
        }

        public async Task<IEnumerable<string>> ChooseImagesAsync()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var files = await openPicker.PickMultipleFilesAsync();

            if (files.Count > 0)
            {
                var tempFolder = ApplicationData.Current.TemporaryFolder;
                List<string> imageUris = new List<string>();

                foreach (var file in files)
                {
                    var copiedFile = await file.CopyAsync(tempFolder, file.Name, NameCollisionOption.GenerateUniqueName);
                    string uri = $"ms-appdata:///temp/{copiedFile.Name}";
                    imageUris.Add(uri);
                }

                return imageUris;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public async Task<string> SaveImageAsync(byte[] imageBytes, string fileName = null)
        {
            if (fileName == null)
            {
                fileName = $"{Guid.NewGuid()}.png";
            }

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder imagesFolder = await localFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);
            StorageFile imageFile = await imagesFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await imageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteBytes(imageBytes);
                    await writer.StoreAsync();
                }
            }

            // Return the relative path to the file within the local folder
            return $"Images/{fileName}";
        }

        public async Task<string> SaveImageToTemporaryAsync(byte[] imageBytes)
        {
            var fileName = $"{Guid.NewGuid()}.png";
            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile imageFile = await tempFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await imageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteBytes(imageBytes);
                    await writer.StoreAsync();
                }
            }

            return $"ms-appdata:///temp/{fileName}";
        }

        public async Task<StorageFile> GetImageAsync(string fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return await localFolder.GetFileAsync(fileName);
        }

        public async Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<byte[]> imageBytes)
        {
            var imageNames = new List<string>();

            foreach (var image in imageBytes)
            {
                var imageName = await SaveImageAsync(image, $"{Guid.NewGuid()}.png");
                imageNames.Add(imageName);
            }

            return imageNames;
        }

        /// <summary>
        /// Copies image files from their temporary URIs to the "Images" subfolder in permanent storage and returns the new relative paths.
        /// </summary>
        /// <param name="temporaryImageUris">List of URIs pointing to the temporary image files.</param>
        /// <returns>A task representing the operation, which, upon completion, returns a list of relative paths pointing to the images in permanent storage.</returns>
        public async Task<List<string>> CopyImagesToPermanentStorageAsync(IEnumerable<string> temporaryImageUris)
        {
            var permanentImagePaths = new List<string>();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder imagesFolder = await localFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);

            foreach (var tempUri in temporaryImageUris)
            {
                try
                {
                    StorageFile temporaryFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(tempUri));
                    var fileName = $"{Guid.NewGuid()}{temporaryFile.FileType}";
                    StorageFile permanentFile = await temporaryFile.CopyAsync(imagesFolder, fileName, NameCollisionOption.GenerateUniqueName);

                    // Return the relative path to the file within the "Images" subfolder
                    string permanentPath = $"Images/{permanentFile.Name}";
                    permanentImagePaths.Add(permanentPath);

                    await temporaryFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error copying image to permanent storage: {ex.Message}");
                    throw new Exception($"Error copying image to permanent storage: {ex.Message}");
                }
            }

            return permanentImagePaths;
        }

        private async void RemoveTemporaryFile(string tempFilePath)
        {
            if (tempFilePath.StartsWith("ms-appdata:///temp/"))
            {
                string fileName = tempFilePath.Split('/').Last();

                StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
                StorageFile fileToDelete = await tempFolder.GetFileAsync(fileName);
                await fileToDelete.DeleteAsync();
            }
        }

        public async Task<IEnumerable<ImageResult>> SaveImagesAsync(IEnumerable<string> imageUrls)
        {
            var downloadTasks = imageUrls.Select(async url =>
            {
                var imageBytes = await _fileDownloadService.DownloadFileAsync(url);
                return new { Url = url, Bytes = imageBytes };
            });

            var downloadedImages = await Task.WhenAll(downloadTasks);
            var saveTasks = downloadedImages.Select(async downloadedImage =>
            {
                var filename = $"{Guid.NewGuid()}.png";
                var relativePath = await SaveImageAsync(downloadedImage.Bytes, filename);
                return new ImageResult { Url = downloadedImage.Url, RelativePath = relativePath, Bytes = downloadedImage.Bytes };
            });

            return await Task.WhenAll(saveTasks);
        }

        public async Task<string> ConvertImageToBase64(string localUri)
        {
            var filename = localUri.Replace("ms-appdata:///local/Images/", "");
            StorageFolder imagesFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Images");
            StorageFile file = await imagesFolder.GetFileAsync(filename);
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                var reader = new DataReader(stream.GetInputStreamAt(0));
                var bytes = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}