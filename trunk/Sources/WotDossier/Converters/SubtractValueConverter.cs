using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Substract param from value
    /// </summary>
    public class SubtractValueConverter : IValueConverter
    {
        private static readonly SubtractValueConverter _defaultInstance = new SubtractValueConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static SubtractValueConverter Default { get { return _defaultInstance; } }
        
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
