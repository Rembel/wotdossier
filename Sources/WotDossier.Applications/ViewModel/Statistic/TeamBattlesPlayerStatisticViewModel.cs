using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class TeamBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, ITeamBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public TeamBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        /// <param name="list">The list.</param>
        public TeamBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<ITeamBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}