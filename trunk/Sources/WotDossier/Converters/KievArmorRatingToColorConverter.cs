using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Converters
{
    public class KievArmorRatingToColorConverter : IValueConverter
    {
        private static readonly KievArmorRatingToColorConverter defaultInstance = new KievArmorRatingToColorConverter();

        public static KievArmorRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IRating eff = (IRating)value;
            if (eff.KievArmorRating >= 7300) 
                return Brushes.Purple;
            if (eff.KievArmorRating >= 5570)
                return Brushes.CornflowerBlue;
            if (eff.KievArmorRating >= 3850)
                return Brushes.Lime;
            if (eff.KievArmorRating >= 2730)
                return Brushes.Yellow;
            if (eff.KievArmorRating >= 2080)
                return Brushes.DarkOrange;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
