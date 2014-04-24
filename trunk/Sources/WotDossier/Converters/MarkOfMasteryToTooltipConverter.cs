using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Mark of mastery to description tooltip converter
    /// </summary>
    public class MarkOfMasteryToTooltipConverter : IValueConverter
    {
        private static readonly MarkOfMasteryToTooltipConverter _default = new MarkOfMasteryToTooltipConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static MarkOfMasteryToTooltipConverter Default
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
            switch (mark)
            {
                case 1:
                    return Resources.Resources.Tooltip_MarkOfMastery_1;
                case 2:
                    return Resources.Resources.Tooltip_MarkOfMastery_2;
                case 3:
                    return Resources.Resources.Tooltip_MarkOfMastery_3;
                case 4:
                    return Resources.Resources.Tooltip_MarkOfMastery_4;
                default:
                    return string.Empty;
            }
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
