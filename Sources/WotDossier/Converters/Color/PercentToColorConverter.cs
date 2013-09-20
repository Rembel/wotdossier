using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class PercentToColorConverter : IValueConverter
    {
        private static readonly PercentToColorConverter defaultInstance = new PercentToColorConverter();

        public static PercentToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double eff = (double)value;
            if (eff >= 64) 
                return Brushes.Purple;
            if (eff >= 57)
                return Brushes.CornflowerBlue;
            if (eff >= 52)
                return Brushes.Lime;
            if (eff >= 49)
                return Brushes.Yellow;
            if (eff >= 47)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
