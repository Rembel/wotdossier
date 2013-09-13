using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class NoobRatingToColorConverter : IValueConverter
    {
        private static readonly NoobRatingToColorConverter defaultInstance = new NoobRatingToColorConverter();

        public static NoobRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 190)
                    return Brushes.Purple;
                if (eff >= 110)
                    return Brushes.Lime;
                if (eff >= 80)
                    return Brushes.Yellow;
                if (eff >= 60)
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
