using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class RandomBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IRandomBattlesAchievements
    {
        public RandomBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat) : this(stat, new List<StatisticSlice>())
        {
        }

        public RandomBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IRandomBattlesAchievements>(stat.AchievementsIdObject, this);

                //TODO: rename Reaper -> ReaperLongest in BD
                ReaperLongest = stat.AchievementsIdObject.Reaper;
                //TODO: rename Invincible -> InvincibleLongest in BD
                InvincibleLongest = stat.AchievementsIdObject.Invincible;
                //TODO: rename Survivor -> SurvivorLongest in BD
                SurvivorLongest = stat.AchievementsIdObject.Survivor;
            }

            #endregion
        }
    }
}