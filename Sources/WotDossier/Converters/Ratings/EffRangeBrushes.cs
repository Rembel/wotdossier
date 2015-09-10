using System.Windows.Media;

namespace WotDossier.Converters.Ratings
{
    public static class EffRangeColors
    {
        public static System.Windows.Media.Color Black = System.Windows.Media.Color.FromRgb(0, 0, 0);
        public static System.Windows.Media.Color BlackRed = System.Windows.Media.Color.FromRgb(94, 0, 0);
        public static System.Windows.Media.Color Red = System.Windows.Media.Color.FromRgb(254, 14, 0);
        public static System.Windows.Media.Color Purple = System.Windows.Media.Color.FromRgb(208, 66, 243);
        public static System.Windows.Media.Color Blue = System.Windows.Media.Color.FromRgb(2, 201, 179);
        public static System.Windows.Media.Color Green = System.Windows.Media.Color.FromRgb(96, 255, 0);
        public static System.Windows.Media.Color Yellow = System.Windows.Media.Color.FromRgb(248, 244, 3);
        public static System.Windows.Media.Color Orange = System.Windows.Media.Color.FromRgb(254, 121, 3);
    }

    public static class EffRangeBrushes
    {
        public static SolidColorBrush Black = new SolidColorBrush(EffRangeColors.Black);
        public static SolidColorBrush BlackRed = new SolidColorBrush(EffRangeColors.BlackRed);
        public static SolidColorBrush Red = new SolidColorBrush(EffRangeColors.Red);
        public static SolidColorBrush Purple = new SolidColorBrush(EffRangeColors.Purple);
        public static SolidColorBrush Blue = new SolidColorBrush(EffRangeColors.Blue);
        public static SolidColorBrush Green = new SolidColorBrush(EffRangeColors.Green);
        public static SolidColorBrush Yellow = new SolidColorBrush(EffRangeColors.Yellow);
        public static SolidColorBrush Orange = new SolidColorBrush(EffRangeColors.Orange);
    }
}