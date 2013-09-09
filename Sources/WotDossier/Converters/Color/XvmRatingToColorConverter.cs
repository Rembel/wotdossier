using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class XvmRatingToColorConverter : IValueConverter
    {
        private static readonly XvmRatingToColorConverter defaultInstance = new XvmRatingToColorConverter();

        public static XvmRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 93)
                    return Brushes.Purple;
                if (eff >= 76)
                    return Brushes.CornflowerBlue;
                if (eff >= 53)
                    return Brushes.Lime;
                if (eff >= 34)
                    return Brushes.Yellow;
                if (eff >= 17)
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
