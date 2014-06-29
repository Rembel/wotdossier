using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Mark of mastery to description tooltip converter
    /// </summary>
    public class DamageRatingToTooltipConverter : IValueConverter
    {
        private static readonly DamageRatingToTooltipConverter _default = new DamageRatingToTooltipConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static DamageRatingToTooltipConverter Default
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
            int mark = (int)value;
            return string.Format("{0:0.00}%", mark/100.0);
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
