using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
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
                    return EffRangeBrushes.Purple;
                if (eff >= 1685)
                    return EffRangeBrushes.Blue;
                if (eff >= 1445)
                    return EffRangeBrushes.Green;
                if (eff >= 1215)
                    return EffRangeBrushes.Yellow;
                if (eff >= 1000)
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
