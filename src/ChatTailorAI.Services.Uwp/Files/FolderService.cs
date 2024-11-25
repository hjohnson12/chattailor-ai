using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    /// <summary>
    /// A class for picking and creating folders
    /// </summary>
    public class FolderService : IFolderService
    {
        /// <summary>
        /// Opens the folder picker and returns the chosen folder.
        /// </summary>
        /// <returns>Chosen storage folder, or null if none chosen.</returns>
        public async Task<StorageFolder> OpenFolderPickerAsync()
        {
            FolderPicker picker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            return await picker.PickSingleFolderAsync();
        }

        /// <summary>
        /// Creates a new folder with its name passed as an argument inside a specified folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<StorageFolder> CreateFolderByNameAsync(StorageFolder folder, string folderName)
        {
            if (folder != null)
            {
                string todaysDate = $"{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Today.Year}";

                StorageFolder rootPhotoFolder =
                    await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                StorageFolder photoFolderByDate =
                    await rootPhotoFolder.CreateFolderAsync(todaysDate, CreationCollisionOption.OpenIfExists);

                return photoFolderByDate;
            }
            return folder;
        }
    }
}