using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace ChatTailorAI.Uwp.Converters
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, string language)
        //{
        //    var imagePath = value as string;
        //    if (string.IsNullOrWhiteSpace(imagePath))
        //        return null;

        //    BitmapImage bitmapImage = new BitmapImage();

        //    StorageFile.GetFileFromPathAsync(imagePath).AsTask().ContinueWith(async t =>
        //    {
        //        var file = t.Result;
        //        using (IRandomAccessStream stream = await file.OpenReadAsync())
        //        {
        //            await bitmapImage.SetSourceAsync(stream);
        //        }
        //    }, TaskScheduler.FromCurrentSynchronizationContext()).Wait(); // Ensure continuation runs on UI thread

        //    return bitmapImage;
        //}

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var imagePath = value as string;
            if (string.IsNullOrWhiteSpace(imagePath))
                return null;

            var bitmapImage = new BitmapImage();

            // Initiate the asynchronous loading process without blocking
            LoadImageAsync(bitmapImage, imagePath);

            return bitmapImage;
        }

        private async void LoadImageAsync(BitmapImage bitmapImage, string imagePath)
        {
            try
            {
                // Obtain the StorageFile from the path and set it as the image source
                var storageFile = await StorageFile.GetFileFromPathAsync(imagePath);
                using (var stream = await storageFile.OpenReadAsync())
                {
                    await bitmapImage.SetSourceAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, access denied).
                // Depending on your needs, you might want to log the error
                // or set a default image in case of failure.
                Debug.WriteLine($"Error loading image: {ex.Message}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
