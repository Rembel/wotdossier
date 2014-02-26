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
                if (eff >= 2540)
                    return EffRangeBrushes.Purple;
                if (eff >= 1965)
                    return EffRangeBrushes.Blue;
                if (eff >= 1310)
                    return EffRangeBrushes.Green;
                if (eff >= 750)
                    return EffRangeBrushes.Yellow;
                if (eff >= 310)
                    return EffRangeBrushes.Orange;
                if (eff >= 0)
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
