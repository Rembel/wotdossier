using System.ComponentModel;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface ITankStatisticView : IView
    {
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        Window Owner { set; get; }

        /// <summary>
        /// Gets the chart rating.
        /// </summary>
        /// <value>
        /// The chart rating.
        /// </value>
        ChartPlotter ChartRating { get; }

        /// <summary>
        /// Gets the chart win percent.
        /// </summary>
        /// <value>
        /// The chart win percent.
        /// </value>
        ChartPlotter ChartWinPercent { get; }

        /// <summary>
        /// Gets the chart average damage.
        /// </summary>
        /// <value>
        /// The chart average damage.
        /// </value>
        ChartPlotter ChartAvgDamage { get; }
    }
}
