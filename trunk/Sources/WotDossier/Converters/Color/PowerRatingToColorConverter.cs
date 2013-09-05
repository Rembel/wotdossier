using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class PowerRatingToColorConverter : IValueConverter
    {
        private static readonly PowerRatingToColorConverter defaultInstance = new PowerRatingToColorConverter();

        public static PowerRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1990)
                    return Brushes.Purple;
                if (eff >= 1685)
                    return Brushes.CornflowerBlue;
                if (eff >= 1445)
                    return Brushes.Lime;
                if (eff >= 1215)
                    return Brushes.Yellow;
                if (eff >= 1000)
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
