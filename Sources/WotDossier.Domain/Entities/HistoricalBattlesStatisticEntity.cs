using System;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'HistoricalBattlesStatisticEntity'.
    /// </summary>
    [Serializable]
    public class HistoricalBattlesStatisticEntity : StatisticEntity
    {
        #region Property names

        public static readonly string PropAchievementsId = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalBattlesStatisticEntity"/> class.
        /// </summary>
        public HistoricalBattlesStatisticEntity()
        {
            AchievementsIdObject = new HistoricalBattlesAchievementsEntity();
        }
        /// <summary>
        /// Gets/Sets the field "AchievementsId".
        /// </summary>
        public virtual int? AchievementsId { get; set; }

        /// <summary>
        /// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
        /// </summary>
        public virtual HistoricalBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}

