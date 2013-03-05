using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        private static readonly DateTimeConverter _defaultInstance = new DateTimeConverter();

        public static DateTimeConverter Default { get { return _defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedDate = value as DateTime?;

            if (selectedDate != null)
            {
                return selectedDate.Value.ToString(parameter.ToString());
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    return DateTime.Parse(value.ToString());
                }

                return null;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }

}
