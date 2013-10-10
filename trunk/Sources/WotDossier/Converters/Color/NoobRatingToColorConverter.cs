using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
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
                    return EffRangeBrushes.Purple;
                if (eff >= 110)
                    return EffRangeBrushes.Green;
                if (eff >= 80)
                    return EffRangeBrushes.Yellow;
                if (eff >= 60)
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
