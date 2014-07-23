using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.UI
{
    public class TankTypeLabelProvider : LabelProviderBase<int>
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

            return ticksInfo.Ticks.Select(num => new TextBlock
            {
                Text = GetTankTypeName(num)
            }).ToArray();
        }

        public string GetTankTypeName(int type)
        {
            return Resources.Resources.ResourceManager.GetEnumResource((TankType)type);
        }
    }
}