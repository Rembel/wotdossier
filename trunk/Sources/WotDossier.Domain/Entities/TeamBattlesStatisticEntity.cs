using System;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'TeamBattlesStatistic'.
    /// </summary>
    [Serializable]
    public class TeamBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesStatisticEntity"/> class.
        /// </summary>
        public TeamBattlesStatisticEntity()
        {
            AchievementsIdObject = new TeamBattlesAchievementsEntity();
        }

        /// <summary>
        /// Gets/Sets the field "AchievementsId".
        /// </summary>
        public virtual int? AchievementsId { get; set; }

        /// <summary>
        /// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
        /// </summary>
        public virtual TeamBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}

