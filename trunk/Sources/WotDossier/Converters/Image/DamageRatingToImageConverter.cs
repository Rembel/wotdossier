using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Domain;
using WotDossier.Resources;

namespace WotDossier.Converters.Image
{
    public class DamageRatingToImageConverter : IMultiValueConverter
    {
        private static readonly DamageRatingToImageConverter _default = new DamageRatingToImageConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static DamageRatingToImageConverter Default
        {
            get { return _default; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int percentValue = (int)values[0]/100;
            Country nation = (Country)values[1];
            int mark = GetMark(percentValue);
            if (mark > 0)
            {
                var uriSource = new Uri(string.Format(@"pack://application:,,,/WotDossier.Resources;component/Images/marksOnGun/95x85/{0}_{1}_marks.png", nation.ToString().ToLower(), mark));
                BitmapImage bitmapImage = ImageCache.GetBitmapImage(uriSource);
                return bitmapImage;
            }
            return null;
        }

        private int GetMark(int percentValue)
        {
            if (percentValue > 95)
            {
                return 3;
            }
            if (percentValue > 85)
            {
                return 2;
            }
            if (percentValue > 65)
            {
                return 1;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
