using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Converters
{
    public class PercentToColorConverter : IValueConverter
    {
        private static readonly PercentToColorConverter defaultInstance = new PercentToColorConverter();

        public static PercentToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ITankRowRatings eff = (ITankRowRatings)value;
            if (eff.WN6Rating >= 64) 
                return Brushes.Purple;
            if (eff.WN6Rating >= 57)
                return Brushes.CornflowerBlue;
            if (eff.WN6Rating >= 52)
                return Brushes.Lime;
            if (eff.WN6Rating >= 49)
                return Brushes.Yellow;
            if (eff.WN6Rating >= 47)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
