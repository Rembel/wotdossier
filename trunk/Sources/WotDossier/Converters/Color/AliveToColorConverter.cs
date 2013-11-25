using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WotDossier.Converters.Color
{
    public class AliveToColorConverter : IValueConverter
    {
        private static readonly AliveToColorConverter defaultInstance = new AliveToColorConverter();

        public static AliveToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            bool delta = (bool)value;

            if (delta)
                return new SolidColorBrush(Colors.Transparent);
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.Fill;
            imageBrush.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\Images\tile.png", UriKind.Relative));
            return imageBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
