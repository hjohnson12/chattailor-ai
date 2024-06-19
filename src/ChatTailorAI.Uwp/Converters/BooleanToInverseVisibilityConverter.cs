using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace ChatTailorAI.Uwp.Converters
{
    public class BooleanToInverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = (Visibility)value;
            return visibility != Visibility.Visible;
        }
    }

    
}
