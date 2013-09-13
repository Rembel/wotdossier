using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class WN7ToColorConverter : IValueConverter
    {
        private static readonly WN7ToColorConverter defaultInstance = new WN7ToColorConverter();

        public static WN7ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1890)
                    return Brushes.Purple;
                if (eff >= 1570)
                    return Brushes.CornflowerBlue;
                if (eff >= 1180)
                    return Brushes.Lime;
                if (eff >= 815)
                    return Brushes.Yellow;
                if (eff >= 455)
                    return Brushes.DarkOrange;
            }
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
