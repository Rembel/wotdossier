using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Converters
{
    public class XvmRatingToColorConverter : IValueConverter
    {
        private static readonly XvmRatingToColorConverter defaultInstance = new XvmRatingToColorConverter();

        public static XvmRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IRating eff = (IRating)value;
            if (eff != null)
            {
                if (eff.XEFF >= 93)
                    return Brushes.Purple;
                if (eff.XEFF >= 76)
                    return Brushes.CornflowerBlue;
                if (eff.XEFF >= 53)
                    return Brushes.Lime;
                if (eff.XEFF >= 34)
                    return Brushes.Yellow;
                if (eff.XEFF >= 17)
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
