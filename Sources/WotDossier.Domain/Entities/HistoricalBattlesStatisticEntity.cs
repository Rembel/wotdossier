namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'HistoricalBattlesStatistic'.
    /// </summary>
    public class HistoricalBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalBattlesStatisticEntity"/> class.
        /// </summary>
        public HistoricalBattlesStatisticEntity()
        {
        }
        
        /// <summary>
        /// Gets/Sets the <see cref="RandomBattlesAchievementsEntity"/> object.
        /// </summary>
        public virtual HistoricalBattlesAchievementsEntity AchievementsIdObject { get; set; } = new HistoricalBattlesAchievementsEntity();
    }
}

