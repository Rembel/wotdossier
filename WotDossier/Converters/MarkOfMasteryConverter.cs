using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    [ValueConversion(typeof(int), typeof(Int32Rect))]
    public class MarkOfMasteryConverter : IValueConverter
    {
        private static MarkOfMasteryConverter _default = new MarkOfMasteryConverter();

        public static MarkOfMasteryConverter Default
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
                    return new Int32Rect(0, 0, 20, 20);
                case 2:
                    return new Int32Rect(0, 0, 20, 20);
                case 3:
                    return new Int32Rect(0, 0, 20, 20);
                case 4:
                    return new Int32Rect(0, 0, 20, 20);
            }
            return new Int32Rect(0, 0, 20, 20);
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
