using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class SubtractValueConverter : IValueConverter
    {
        private static readonly SubtractValueConverter _defaultInstance = new SubtractValueConverter();

        public static SubtractValueConverter Default { get { return _defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value;
            double parameterValue;

            if (value != null && targetType == typeof(Double) && double.TryParse((string)parameter, NumberStyles.Float, culture, out parameterValue))
            {
                result = (double)value - parameterValue;
            }

            return result; 

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
