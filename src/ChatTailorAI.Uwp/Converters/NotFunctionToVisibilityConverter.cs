using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Converters
{
    public class NotFunctionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            if (str == null)
            {
                return Visibility.Collapsed;
            }
            return str != "function" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
