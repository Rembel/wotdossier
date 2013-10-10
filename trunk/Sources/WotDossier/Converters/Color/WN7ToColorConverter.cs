﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
{
    public class WN7ToColorConverter : IValueConverter
    {
        private static readonly WN7ToColorConverter defaultInstance = new WN7ToColorConverter();

        public static WN7ToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 1890)
                    return EffRangeBrushes.Purple;
                if (eff >= 1570)
                    return EffRangeBrushes.Blue;
                if (eff >= 1180)
                    return EffRangeBrushes.Green;
                if (eff >= 815)
                    return EffRangeBrushes.Yellow;
                if (eff >= 455)
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
