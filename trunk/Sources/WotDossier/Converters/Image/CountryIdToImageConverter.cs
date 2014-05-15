using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WotDossier.Converters
{
    public class CountryIdToImageConverter : IValueConverter
    {
        private static readonly CountryIdToImageConverter _default = new CountryIdToImageConverter();
        private static readonly Dictionary<Uri, BitmapImage> _cache = new Dictionary<Uri, BitmapImage>();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static CountryIdToImageConverter Default
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
            int countryId = (int)value;
            if (countryId > -1)
            {
                Uri uriSource = new Uri(string.Format(@"pack://application:,,,/WotDossier.Resources;component/Images/Countries/slot_bright_{0}.png", countryId));
                var bitmapImage = GetBitmapImage(uriSource);
                return bitmapImage;
            }
            return null;
        }

        private static BitmapImage GetBitmapImage(Uri uriSource)
        {
            if (!_cache.ContainsKey(uriSource))
            {
                _cache.Add(uriSource, new BitmapImage(uriSource));
            }
            return _cache[uriSource];
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
