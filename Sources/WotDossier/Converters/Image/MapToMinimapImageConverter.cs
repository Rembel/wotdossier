using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;
using WotDossier.Resources;

namespace WotDossier.Converters
{
    public class MapToMinimapImageConverter : IValueConverter
    {
        private static readonly MapToMinimapImageConverter _default = new MapToMinimapImageConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static MapToMinimapImageConverter Default
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
            IReplayMap description = value as IReplayMap;
            int layer = 0;
            if (parameter != null)
            {
                layer = int.Parse(parameter.ToString());
            }

            if (description != null)
            {
                Uri uriSource;
                if (layer == 0)
                {
                    uriSource = new Uri(
                        string.Format(
                            @"pack://application:,,,/WotDossier.Resources;component/Images/Maps/Minimap/{0}.png",
                            description.MapNameId));
                }
                else
                {
                    uriSource =
                        new Uri(
                            string.Format(
                                @"pack://application:,,,/WotDossier.Resources;component/Images/Maps/Minimap/{0}_{1}_{2}.png",
                                description.MapNameId, description.Gameplay == Gameplay.nations ? Gameplay.ctf : description.Gameplay, description.Team));
                }

                BitmapImage bitmapImage = ImageCache.GetBitmapImage(uriSource);

                return bitmapImage;
            }
            return null;
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
