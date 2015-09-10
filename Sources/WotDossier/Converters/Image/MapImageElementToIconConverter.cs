using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Resources;

namespace WotDossier.Converters
{
    public class MapImageElementToIconConverter : IValueConverter
    {
        private static readonly MapImageElementToIconConverter _default = new MapImageElementToIconConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static MapImageElementToIconConverter Default
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
            MapImageElement element = (MapImageElement)value;
            BitmapImage bitmapImage = null;
            if (element != null)
            {
                string file;
                if (element.Type == "base")
                {
                    file = string.Format(@"{0}_{1}.png", element.Owner, element.Type);
                }
                else
                {
                    file = string.Format(@"{0}_{1}{2}.png", element.Owner, element.Type, element.Position);
                }
                Uri uriSource = new Uri(string.Format(@"pack://application:,,,/WotDossier.Resources;component/Images/Replays/Viewer/elements/{0}", file));
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