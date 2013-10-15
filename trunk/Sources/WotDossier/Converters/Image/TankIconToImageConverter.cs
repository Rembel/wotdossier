using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Domain.Tank;

namespace WotDossier.Converters
{
    public class TankIconToImageConverter : IValueConverter
    {
        private static readonly TankIconToImageConverter _default = new TankIconToImageConverter();

        public static TankIconToImageConverter Default
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
            TankIcon icon = (TankIcon)value;
            BitmapImage bitmapImage = null;
            if (icon != null)
            {
                bitmapImage = new BitmapImage(new Uri(string.Format(@"\Resources\Images\Tanks\{0}.png", icon.IconId), UriKind.Relative));
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
