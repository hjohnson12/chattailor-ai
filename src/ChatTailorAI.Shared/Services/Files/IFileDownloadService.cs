using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Files
{
    /// <summary>
    /// Interface for downloading files from a web url.
    /// </summary>
    public interface IFileDownloadService
    {
        /// <summary>
        /// Writes a file to a folder chosen from the Folder Picker.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        Task DownloadFileToFolderAsync(string imageUrl);

        Task<byte[]> DownloadFileAsync(string url);
        Task<IEnumerable<byte[]>> DownloadFilesAsync(string[] urls);

        /// <summary>
        /// Downloads all files to a folder specified from a folder picker shown
        /// to the user.
        /// </summary>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        Task DownloadFilesToFolderAsync(string[] imageUrls);
    }
}