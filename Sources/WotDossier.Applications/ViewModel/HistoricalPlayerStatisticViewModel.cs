using System.Collections.Generic;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel
{
    public class HistoricalPlayerStatisticViewModel : PlayerStatisticViewModel
    {
        public HistoricalPlayerStatisticViewModel(HistoricalBattlesStatisticEntity stat)
            : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public HistoricalPlayerStatisticViewModel(HistoricalBattlesStatisticEntity stat, List<PlayerStatisticViewModel> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                GuardsMan = stat.AchievementsIdObject.GuardsMan;
                MakerOfHistory = stat.AchievementsIdObject.MakerOfHistory;
                WeakVehiclesWins = stat.AchievementsIdObject.WeakVehiclesWins;
                BothSidesWins = stat.AchievementsIdObject.BothSidesWins;
            }

            #endregion
        }
    }
}