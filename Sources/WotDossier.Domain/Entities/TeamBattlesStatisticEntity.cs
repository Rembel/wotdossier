namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'TeamBattlesStatistic'.
    /// </summary>
    
    public class TeamBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesStatisticEntity"/> class.
        /// </summary>
        public TeamBattlesStatisticEntity()
        {
        }

        /// <summary>
        /// Gets/Sets the <see cref="RandomBattlesAchievementsEntity"/> object.
        /// </summary>
        public virtual TeamBattlesAchievementsEntity AchievementsIdObject { get; set; } = new TeamBattlesAchievementsEntity();
    }
}

