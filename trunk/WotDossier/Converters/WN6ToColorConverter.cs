using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Domain.Rows;

namespace WotDossier.Converters
{
    public class WN6ToColorConverter : IValueConverter
    {
        private static readonly WN6ToColorConverter defaultInstance = new WN6ToColorConverter();

        public static WN6ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TankRowRatings eff = (TankRowRatings)value;
            if (eff.WN6 >= 1880) 
                return Brushes.Purple;
            if (eff.WN6 >= 1585)
                return Brushes.CornflowerBlue;
            if (eff.WN6 >= 1195)
                return Brushes.Lime;
            if (eff.WN6 >= 800)
                return Brushes.Yellow;
            if (eff.WN6 >= 435)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
