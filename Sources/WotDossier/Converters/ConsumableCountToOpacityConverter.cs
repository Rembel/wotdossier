using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Consumable count to opacity converter
    /// </summary>
    public class ConsumableCountToOpacityConverter : IValueConverter
    {
        private static readonly ConsumableCountToOpacityConverter _default = new ConsumableCountToOpacityConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static ConsumableCountToOpacityConverter Default
        {
            get { return _default; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }

            int val = (int)value;
            return val > 0 ? (double)1 : 0.1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
