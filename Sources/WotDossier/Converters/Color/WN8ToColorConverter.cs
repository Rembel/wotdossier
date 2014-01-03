using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
{
    public class WN8ToColorConverter : IValueConverter
    {
        private static readonly WN8ToColorConverter defaultInstance = new WN8ToColorConverter();

        public static WN8ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 2350)
                    return EffRangeBrushes.Purple;
                if (eff >= 1900)
                    return EffRangeBrushes.Blue;
                if (eff >= 1250)
                    return EffRangeBrushes.Green;
                if (eff >= 900)
                    return EffRangeBrushes.Yellow;
                if (eff >= 600)
                    return EffRangeBrushes.Orange;
                if (eff >= 300)
                    return EffRangeBrushes.Red;
            }
            return EffRangeBrushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
