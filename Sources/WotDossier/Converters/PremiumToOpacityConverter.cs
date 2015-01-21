using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Premium flag to opacity converter
    /// </summary>
    public class PremiumToOpacityConverter : IValueConverter
    {
        private static readonly PremiumToOpacityConverter _default = new PremiumToOpacityConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static PremiumToOpacityConverter Default
        {
            get { return _default; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }

            bool val = (bool)value;
            return val ? (double)1 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
