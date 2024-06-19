using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    /// <summary>
    /// Interface for a service for picking and creating folders
    /// </summary>
    public interface IFolderService
    {
        /// <summary>
        /// Opens the folder picker and returns the chosen folder.
        /// </summary>
        /// <returns>Chosen storage folder.</returns>
        Task<StorageFolder> OpenFolderPickerAsync();

        /// <summary>
        /// Creates a new folder with its name passed as an argument inside a specified folder.
        /// </summary>
        /// <param name="folder">Folder to create a new folder in.</param>
        /// <param name="folderName">New folders name.</param>
        /// <returns></returns>
        Task<StorageFolder> CreateFolderByNameAsync(StorageFolder folder, string folderName);
    }
}
