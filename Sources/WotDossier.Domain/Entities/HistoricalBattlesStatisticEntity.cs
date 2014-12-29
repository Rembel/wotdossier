using System;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'HistoricalBattlesStatistic'.
    /// </summary>
    [Serializable]
    public class HistoricalBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalBattlesStatisticEntity"/> class.
        /// </summary>
        public HistoricalBattlesStatisticEntity()
        {
            AchievementsIdObject = new HistoricalBattlesAchievementsEntity();
        }
        
        /// <summary>
        /// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
        /// </summary>
        public virtual HistoricalBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}

