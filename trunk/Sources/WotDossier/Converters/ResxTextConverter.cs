using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    public class ResxTextConverter : IValueConverter
    {
        private static readonly ResxTextConverter _defaultInstance = new ResxTextConverter();

        public static ResxTextConverter Default { get { return _defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return Resources.Resources.ResourceManager.GetString(value.ToString()) ?? value;
            }
            return "-res not found-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
