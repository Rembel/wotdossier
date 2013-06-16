using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        private static readonly IntToVisibilityConverter defaultInstance = new IntToVisibilityConverter();

        public static IntToVisibilityConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int?)value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
