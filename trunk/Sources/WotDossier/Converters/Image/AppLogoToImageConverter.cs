using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Resources;

namespace WotDossier.Converters
{
    public class AppLogoToImageConverter : IValueConverter
    {
        private static readonly AppLogoToImageConverter _default = new AppLogoToImageConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static AppLogoToImageConverter Default
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
            string logo = (string)parameter;
            BitmapImage bitmapImage = null;
            if (logo != null)
            {
                Uri uriSource = new Uri(string.Format(@"pack://application:,,,/{0};component/{1}", Assembly.GetEntryAssembly().GetName().Name, logo));
                bitmapImage = ImageCache.GetBitmapImage(uriSource);
            }
            return bitmapImage;
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
