using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
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
                    return EffRangeBrushes.Purple;
                if (eff >= 5570)
                    return EffRangeBrushes.Blue;
                if (eff >= 3850)
                    return EffRangeBrushes.Green;
                if (eff >= 2730)
                    return EffRangeBrushes.Yellow;
                if (eff >= 2080)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
