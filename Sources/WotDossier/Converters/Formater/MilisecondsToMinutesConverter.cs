using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class MilisecondsToMinutesConverter : IValueConverter
    {
        private static readonly MilisecondsToMinutesConverter _defaultInstance = new MilisecondsToMinutesConverter();

        public static MilisecondsToMinutesConverter Default { get { return _defaultInstance; } }

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
                var seconds = (int)value % 60;
                var minutes = (int)value / 60;
                return String.Format("{0:00}:{1:00}", minutes, seconds);
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