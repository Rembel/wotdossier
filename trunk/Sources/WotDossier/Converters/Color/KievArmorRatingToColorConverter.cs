using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters
{
    public class KievArmorRatingToColorConverter : IValueConverter
    {
        private static readonly KievArmorRatingToColorConverter defaultInstance = new KievArmorRatingToColorConverter();

        public static KievArmorRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff !=null)
            {
                if (eff >= 7300)
                    return Brushes.Purple;
                if (eff >= 5570)
                    return Brushes.CornflowerBlue;
                if (eff >= 3850)
                    return Brushes.Lime;
                if (eff >= 2730)
                    return Brushes.Yellow;
                if (eff >= 2080)
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
