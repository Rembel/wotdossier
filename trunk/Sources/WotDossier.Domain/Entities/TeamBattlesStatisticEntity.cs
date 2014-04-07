using System;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'TeamBattlesStatisticEntity'.
    /// </summary>
    [Serializable]
    public class TeamBattlesStatisticEntity : StatisticEntity
    {
        #region Property names

		public static readonly string PropAchievementsId = TypeHelper<TeamBattlesStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

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

