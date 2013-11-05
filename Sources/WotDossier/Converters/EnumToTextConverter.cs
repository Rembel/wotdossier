using System;
using System.Globalization;
using System.Windows.Data;
using WotDossier.Common;

namespace WotDossier.Converters
{
    public class EnumToTextConverter : IValueConverter
    {
        private static readonly EnumToTextConverter defaultInstance = new EnumToTextConverter();

        public static EnumToTextConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool b = value is Enum;
                if (b)
                {
                    return Resources.Resources.ResourceManager.GetEnumResource((Enum)value);
                }
            }
            return "-res not found-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
