namespace WotDossier.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRandomBattlesAchievements : IStatisticBattleAwards, IStatisticEpic, IStatisticSpecialAwards, IStatisticMedals, IStatisticSeries
    {
        /// <summary>
        /// Gets or sets the medal monolith.
        /// </summary>
        int MedalMonolith { get; set; }

        /// <summary>
        /// Gets or sets the medal anti SPG fire.
        /// </summary>
        int MedalAntiSpgFire { get; set; }

        /// <summary>
        /// Gets or sets the medal gore.
        /// </summary>
        int MedalGore { get; set; }

        /// <summary>
        /// Gets or sets the medal cool blood.
        /// </summary>
        int MedalCoolBlood { get; set; }

        /// <summary>
        /// Gets or sets the medal stark.
        /// </summary>
        int MedalStark { get; set; }

        /// <summary>
        /// Gets or sets the damage rating.
        /// </summary>
        int DamageRating { get; set; }

        /// <summary>
        /// Gets/Sets the field "MarksOnGun".
        /// </summary>
        int MarksOnGun { get; set; }

        /// <summary>
        /// Gets/Sets the field "MovingAvgDamage".
        /// </summary>
        int MovingAvgDamage { get; set; }

        /// <summary>
        /// Gets or sets the lumberjack.
        /// </summary>
        int Lumberjack { get; set; }

        /// <summary>
        /// Gets or sets the master gunner.
        /// </summary>
        int MasterGunner { get; set; }

        /// <summary>
        /// Gets or sets the alaric.
        /// </summary>
        int Alaric { get; set; }

        /// <summary>
        /// Gets or sets the mark of mastery.
        /// </summary>
        int MarkOfMastery { get; set; }
    }
}