using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Domain.Rows;

namespace WotDossier.Converters
{
    public class EffectivityToColorConverter : IValueConverter
    {
        private static readonly EffectivityToColorConverter defaultInstance = new EffectivityToColorConverter();

        public static EffectivityToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TankRowRatings eff = (TankRowRatings)value;
            if (eff.NewEffRating >= 1725) 
                return Brushes.Purple;
            if (eff.NewEffRating >= 1465)
                return Brushes.CornflowerBlue;
            if (eff.NewEffRating >= 1150)
                return Brushes.Lime;
            if (eff.NewEffRating >= 870)
                return Brushes.Yellow;
            if (eff.NewEffRating >= 645)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
