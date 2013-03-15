using System;
using System.Globalization;
using System.Windows.Data;
using WotDossier.Common;

namespace WotDossier.Converters
{
    public class DateTimeToTimeConverter : IValueConverter
    {
        private static readonly DateTimeToTimeConverter _defaultInstance = new DateTimeToTimeConverter();

        public static DateTimeToTimeConverter Default { get { return _defaultInstance; } }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return DataFormatter.FormatTimeOfDay(DateTime.SpecifyKind((DateTime) value, DateTimeKind.Utc).ToLocalTime(), false);
            }
            return value;
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
            throw new NotImplementedException();
        }
    }
}