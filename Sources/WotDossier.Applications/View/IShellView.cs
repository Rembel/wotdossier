using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface IShellView : IView
    {
        Window Owner { set; get; }

        ChartPlotter ChartWinPercent { get; }
        ChartPlotter ChartAvgDamage { get; }
        ChartPlotter ChartLastUsedTanks { get; }
    }
}
