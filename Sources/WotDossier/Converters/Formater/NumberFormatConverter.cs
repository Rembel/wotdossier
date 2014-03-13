using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class NumberFormatConverter : IValueConverter
    {
        private static NumberFormatConverter _default = new NumberFormatConverter();

        public static NumberFormatConverter Default
        {
            get { return _default; }
        }

        private static NumberFormatInfo _formatProvider;

        public static NumberFormatInfo FormatProvider
        {
            get
            {
                if (_formatProvider == null)
                {
                    _formatProvider = ((CultureInfo)CultureInfo.CurrentCulture.Clone()).NumberFormat;
                    FormatProvider.NumberGroupSeparator = " ";
                }
                return _formatProvider;
            }
        }

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

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
