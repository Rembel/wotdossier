using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
{
    public class WN6ToColorConverter : IValueConverter
    {
        private static readonly WN6ToColorConverter defaultInstance = new WN6ToColorConverter();

        public static WN6ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1925)
                    return EffRangeBrushes.Purple;
                if (eff >= 1585)
                    return EffRangeBrushes.Blue;
                if (eff >= 1185)
                    return EffRangeBrushes.Green;
                if (eff >= 795)
                    return EffRangeBrushes.Yellow;
                if (eff >= 425)
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
