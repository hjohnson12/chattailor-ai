using System;
using ChatTailorAI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ChatTailorAI.Shared.Services.Files;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    /// <summary>
    /// Class for downloading image files from web urls
    /// </summary>
    public class FileDownloadService : IFileDownloadService
    {
        private HttpClient _httpClient;
        private IFolderService _folderService;

        public FileDownloadService(HttpClient client, IFolderService folderService)
        {
            _folderService = folderService;
            _httpClient = client;
        }

        /// <summary>
        /// Writes an image to a folder chosen from the Folder Picker.
        /// </summary>
        /// <param name="url">Url of image file</param>
        /// <returns></returns>
        public async Task DownloadFileToFolderAsync(string imageUrl)
        {
            var pickedFolder = await _folderService.OpenFolderPickerAsync();
            //var photoFolder = await _folderService.CreateFolderByNameAsync(pickedFolder, "");

            if (pickedFolder != null)
            {
                // Get byte array buffer of file to save
                byte[] buffer = await _httpClient.GetByteArrayAsync(imageUrl);
                await WriteToFile(pickedFolder, buffer);
            }
        }

        /// <summary>
        /// Downloads all files to a folder specified from a folder picker shown
        /// to the user.
        /// </summary>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        public async Task DownloadFilesToFolderAsync(string[] imageUrls)
        {
            var pickedFolder = await _folderService.OpenFolderPickerAsync();
            //var photoFolder = await _folderService.CreateFolderByNameAsync(pickedFolder, "");

            if (pickedFolder != null)
            {
                foreach (var url in imageUrls)
                {
                    await WriteToFile(pickedFolder, url);
                }
            }
        }

        private async Task WriteToFile(StorageFolder photoFolder, string url)
        {
            if (url.StartsWith("http") || url.StartsWith("https"))
            {
                await WriteToFile(photoFolder, url);
            }
            else
            {
                // Must be a local file from msapps data
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(url));
                var buffer = await FileIO.ReadBufferAsync(file);
                await WriteToFile(photoFolder, buffer.ToArray());
            }
        }

        /// <summary>
        /// Writes a file in the given url to a specified folder
        /// </summary>
        /// <param name="photoFolder">Folder to save file in</param>
        /// <param name="url">Url of file</param>
        /// <returns></returns>
        private async Task WriteToFile(StorageFolder photoFolder, byte[] buffer)
        {
            int randomNum = new Random().Next(0, 15000);

            StorageFile photoFile = await photoFolder.CreateFileAsync(
                $"{randomNum}.png",
                CreationCollisionOption.ReplaceExisting);

            using (Stream stream = await photoFile.OpenStreamForWriteAsync())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }


        public async Task<byte[]> DownloadFileAsync(string url)
        {
            return await _httpClient.GetByteArrayAsync(url);
        }

        public async Task<IEnumerable<byte[]>> DownloadFilesAsync(string[] urls)
        {
            return await Task.WhenAll(urls.Select(DownloadFileAsync));
        }
    }
}
