using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Domain;
using WotDossier.Resources;

namespace WotDossier.Converters
{
    public class ReplayIconConverter : IValueConverter
    {
        private static readonly ReplayIconConverter _default = new ReplayIconConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ReplayIconConverter Default
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
            MapVehicle member = (MapVehicle)value;
            BitmapImage bitmapImage = null;
            if (member != null)
            {
                var teamMate = member.TeamMate;
                string file;
                if (!member.Recorder)
                {
                    file = string.Format(@"{0}_{1}.png", (TankType) member.TankDescription.Type, teamMate ? "g" : "r");
                }
                else
                {
                    file = "recorder.png";
                }
                Uri uriSource = new Uri(string.Format(@"pack://application:,,,/WotDossier.Resources;component/Images/Replays/Viewer/{0}", file));
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