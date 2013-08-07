using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Converters
{
    public class WN6ToColorConverter : IValueConverter
    {
        private static readonly WN6ToColorConverter defaultInstance = new WN6ToColorConverter();

        public static WN6ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IRating eff = (IRating)value;
            if (eff != null)
            {
                if (eff.WN6Rating >= 1885)
                    return Brushes.Purple;
                if (eff.WN6Rating >= 1570)
                    return Brushes.CornflowerBlue;
                if (eff.WN6Rating >= 1175)
                    return Brushes.Lime;
                if (eff.WN6Rating >= 795)
                    return Brushes.Yellow;
                if (eff.WN6Rating >= 425)
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
