using System.Windows.Media;
using WotDossier.Applications.Logic;

namespace WotDossier.Converters.Ratings
{
    public class RatingsManager
    {
        public static IRatingStrategy Get(Rating rating)
        {
            switch (rating)
            {
                    case Rating.EFF:
                        return new EFFRatingStrategy();
                    case Rating.BS:
                        return new BSRatingStrategy();
                    case Rating.WN6:
                        return new WN6RatingStrategy();
                    case Rating.WN7:
                        return new WN7RatingStrategy();
                    case Rating.WN8:
                        return new WN8RatingStrategy();
                    case Rating.PR:
                        return new PRRatingStrategy();
                    case Rating.WGR:
                        return new WGRRatingStrategy();
                    case Rating.XVM:
                        return new XVMRatingStrategy();
                    case Rating.WR:
                        return new WRRatingStrategy();
            }
            return null;
        }
    }

    public class WRRatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WR_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.WR_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.WR_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.WR_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.WR_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class XVMRatingStrategy : IRatingStrategy
    {
        public virtual SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.XVM_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.XVM_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.XVM_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.XVM_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.XVM_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class WGRRatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WGR_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.WGR_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.WGR_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.WGR_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.WGR_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class PRRatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.PR_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.PR_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.PR_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.PR_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.PR_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class WN8RatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WN8_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.WN8_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.WN8_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.WN8_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.WN8_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class WN7RatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WN7_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.WN7_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.WN7_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.WN7_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.WN7_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class WN6RatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WN6_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.WN6_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.WN6_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.WN6_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.WN6_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class BSRatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.BS_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.BS_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.BS_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.BS_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.BS_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public class EFFRatingStrategy : IRatingStrategy
    {
        public SolidColorBrush GetBrush(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.EFF_P5)
                    return EffRangeBrushes.Purple;
                if (value >= Constants.Rating.EFF_P4)
                    return EffRangeBrushes.Blue;
                if (value >= Constants.Rating.EFF_P3)
                    return EffRangeBrushes.Green;
                if (value >= Constants.Rating.EFF_P2)
                    return EffRangeBrushes.Yellow;
                if (value >= Constants.Rating.EFF_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }
    }

    public interface IRatingStrategy
    {
        /// <summary>
        /// Gets the brush.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        SolidColorBrush GetBrush(double? value);
    }
}
