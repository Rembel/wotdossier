using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class HistoricalBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IHistoricalBattlesAchievements
    {
        public HistoricalBattlesPlayerStatisticViewModel(HistoricalBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        public HistoricalBattlesPlayerStatisticViewModel(HistoricalBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IHistoricalBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}