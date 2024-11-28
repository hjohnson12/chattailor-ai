using System;
using System.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Converters
{
    public class InverseCollectionEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var collection = value as ICollection;
            return (collection == null || collection.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Converting from Visibility to Collection is not supported.");
        }
    }
}
