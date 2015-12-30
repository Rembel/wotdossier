using System;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'RandomBattlesStatistic'.
    /// </summary>
    [Serializable]
    [DataContract]
    public class RandomBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        ///     Gets/Sets the <see cref="RandomBattlesAchievementsEntity" /> object.
        /// </summary>
        [DataMember(Name = "Achievements")]
        public virtual RandomBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}