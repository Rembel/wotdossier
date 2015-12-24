using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class ClanBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IClanBattlesAchievements
    {
        public ClanBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat) : this(stat, new List<StatisticSlice>())
        {
        }

        public ClanBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IClanBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}