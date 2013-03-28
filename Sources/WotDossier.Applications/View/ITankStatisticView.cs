using System.ComponentModel;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.View
{
    public interface ITankStatisticView : IView
    {
        event CancelEventHandler Closing;

        void Show();

        bool? ShowDialog();

        void Close();

        Window Owner { set; get; }

        ChartPlotter ChartRating { get; }
        ChartPlotter ChartWinPercent { get; }
        ChartPlotter ChartAvgDamage { get; }
    }
}
