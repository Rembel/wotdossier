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
                if (eff >= 1735)
                    return EffRangeBrushes.Purple;
                if (eff >= 1460)
                    return EffRangeBrushes.Blue;
                if (eff >= 1140)
                    return EffRangeBrushes.Green;
                if (eff >= 860)
                    return EffRangeBrushes.Yellow;
                if (eff >= 630)
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
