using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Image;

namespace ChatTailorAI.Shared.Services.Files
{
    public interface IImageFileService
    {
        /// <summary>
        /// Allows the user to choose images and returns the tempoary file paths of the selected images
        /// </summary>
        /// <returns>Temporary file paths of the selected images</returns>
        Task<IEnumerable<string>> ChooseImagesAsync();

        /// <summary>
        /// Saves an image from a byte array to a file and returns the relative path of the saved file
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="fileName"></param>
        /// <returns>Relative path of the saved file</returns>
        Task<string> SaveImageAsync(byte[] imageBytes, string fileName = null);

        Task<string> SaveImageToTemporaryAsync(byte[] imageBytes);

        /// <summary>
        /// Saves multiple images from byte arrays and returns the relative paths of the saved files
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns>Relative paths of the saved files</returns>
        Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<byte[]> imageBytes);

        /// <summary>
        ///  Saves multiple images from URLs and returns the results including the relative paths of the saved files
        /// </summary>
        /// <param name="imageUrls"></param>
        /// <returns>Relative paths of the saved files</returns>
        Task<IEnumerable<ImageResult>> SaveImagesAsync(IEnumerable<string> imageUrls);

        /// <summary>
        ///  // Copies images from temporary storage to permanent storage and returns the relative paths of the copied files
        /// </summary>
        /// <param name="temporaryImageUris"></param>
        /// <returns>Relative paths of the copied files</returns>
        Task<List<string>> CopyImagesToPermanentStorageAsync(IEnumerable<string> temporaryImageUris);

        Task<string> ConvertImageToBase64(string localUri);
    }
}
