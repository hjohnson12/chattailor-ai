using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ChatTailorAI.Uwp.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = (value as string) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(path))
            {
                return "ms-appx:///Assets/image-not-found-icon.png";
            }
            else
            {
                return path;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
