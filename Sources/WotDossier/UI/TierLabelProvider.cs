using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;

namespace WotDossier.UI
{
    public class TierLabelProvider : LabelProviderBase<int>
    {
        /// <summary>
        /// Creates labels by given ticks info.
        ///             Is not intended to be called from your code.
        /// </summary>
        /// <param name="ticksInfo">The ticks info.</param>
        /// <returns>
        /// Array of <see cref="T:System.Windows.UIElement"/>s, which are axis labels for specified axis ticks.
        /// </returns>
        public override UIElement[] CreateLabels(ITicksInfo<int> ticksInfo)
        {
            if (ticksInfo == null)
                throw new ArgumentNullException("ticks");

            return ticksInfo.Ticks.Select(num => new TextBlock { Text = GetCountryName(num) }).ToArray();
        }

        public string GetCountryName(int tier)
        {
            if (tier > 0 && tier < 11)
            {
                return tier.ToString();
            }
            return string.Empty;
        }
    }
}