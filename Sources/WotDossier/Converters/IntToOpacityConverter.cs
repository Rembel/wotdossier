using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class IntToOpacityConverter : IValueConverter
    {
        private static readonly IntToOpacityConverter defaultInstance = new IntToOpacityConverter();

        public static IntToOpacityConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int?)value > 0 ? (double)1 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
