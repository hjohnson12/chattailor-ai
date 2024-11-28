using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Files
{
    /// <summary>
    /// Interface for a service for picking and creating folders
    /// </summary>
    /// <typeparam name="TFolder">The type of folder used by the service.</typeparam>
    public interface IFolderService<TFolder>
    {
        /// <summary>
        /// Opens the folder picker and returns the chosen folder.
        /// </summary>
        /// <returns>Chosen folder.</returns>
        Task<TFolder> OpenFolderPickerAsync();

        /// <summary>
        /// Creates a new folder with its name passed as an argument inside a specified folder.
        /// </summary>
        /// <param name="folder">Folder to create a new folder in.</param>
        /// <param name="folderName">New folder's name.</param>
        /// <returns>The created folder.</returns>
        Task<TFolder> CreateFolderByNameAsync(TFolder folder, string folderName);
    }
}