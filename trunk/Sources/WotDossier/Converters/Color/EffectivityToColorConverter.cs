using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
{
    public class EffectivityToColorConverter : IValueConverter
    {
        private static readonly EffectivityToColorConverter defaultInstance = new EffectivityToColorConverter();

        public static EffectivityToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1775)
                    return EffRangeBrushes.Purple;
                if (eff >= 1475)
                    return EffRangeBrushes.Blue;
                if (eff >= 1145)
                    return EffRangeBrushes.Green;
                if (eff >= 850)
                    return EffRangeBrushes.Yellow;
                if (eff >= 610)
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
