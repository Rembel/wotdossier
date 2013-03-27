using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Converters
{
    public class EffectivityToColorConverter : IValueConverter
    {
        private static readonly EffectivityToColorConverter defaultInstance = new EffectivityToColorConverter();

        public static EffectivityToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ITankRowRatings eff = (ITankRowRatings)value;
            if (eff.EffRating >= 1725) 
                return Brushes.Purple;
            if (eff.EffRating >= 1465)
                return Brushes.CornflowerBlue;
            if (eff.EffRating >= 1150)
                return Brushes.Lime;
            if (eff.EffRating >= 870)
                return Brushes.Yellow;
            if (eff.EffRating >= 645)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
