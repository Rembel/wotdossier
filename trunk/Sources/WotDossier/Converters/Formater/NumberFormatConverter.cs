using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class NumberFormatConverter : IValueConverter
    {
        private static readonly NumberFormatConverter _default = new NumberFormatConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static NumberFormatConverter Default
        {
            get { return _default; }
        }

        private static NumberFormatInfo _formatProvider;

        /// <summary>
        /// Gets the format provider.
        /// </summary>
        /// <value>
        /// The format provider.
        /// </value>
        public static NumberFormatInfo FormatProvider
        {
            get
            {
                if (_formatProvider == null)
                {
                    _formatProvider = ((CultureInfo)CultureInfo.CurrentCulture.Clone()).NumberFormat;
                    FormatProvider.NumberGroupSeparator = " ";
                    FormatProvider.NumberDecimalSeparator = ".";
                }
                return _formatProvider;
            }
        }

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
            if (value == null) return DependencyProperty.UnsetValue;
            var type = value.GetType();
            var stringFormat = parameter as string;
            if (IsNumeric(type))
            {
                if (stringFormat == null)
                {
                    return value.ToString();
                }
                var formattible = (IFormattable)value;
                // Gets a NumberFormatInfo associated with the en-US culture.
                return formattible.ToString(stringFormat, FormatProvider);
            }
            return DependencyProperty.UnsetValue;
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
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Determines whether the specified type is numeric.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsNumeric(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var elementType = new NullableConverter(type).UnderlyingType;
                return IsNumeric(elementType);
            }
            return
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(double);
        }
    }
}
