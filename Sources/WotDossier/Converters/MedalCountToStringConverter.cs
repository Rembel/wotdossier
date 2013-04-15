using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class MedalCountToStringConverter : IValueConverter
    {
        private static readonly MedalCountToStringConverter _default = new MedalCountToStringConverter();

        public static MedalCountToStringConverter Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int degree = (int) value;
            if (degree == 1)
            {
                return "I";
            }
            if (degree == 2)
            {
                return "II";
            }
            if (degree == 3)
            {
                return "III";
            }
            if (degree == 4)
            {
                return "IV";
            }
            if (degree == 5)
            {
                return "V";
            }
            return "-";
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
