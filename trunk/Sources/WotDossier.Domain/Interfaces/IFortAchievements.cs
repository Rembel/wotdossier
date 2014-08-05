namespace WotDossier.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFortAchievements
    {
        /// <summary>
        /// Gets or sets the conqueror achievement count.
        /// </summary>
        int Conqueror { get; set; }

        /// <summary>
        /// Gets or sets the fire and sword achievement count.
        /// </summary>
        int FireAndSword { get; set; }

        /// <summary>
        /// Gets or sets the crusher achievement count.
        /// </summary>
        int Crusher { get; set; }

        /// <summary>
        /// Gets or sets the counterblow achievement count.
        /// </summary>
        int CounterBlow { get; set; }

        /// <summary>
        /// Gets or sets the soldier of fortune achievement count.
        /// </summary>
        int SoldierOfFortune { get; set; }

        /// <summary>
        /// Gets or sets the kampfer achievement count.
        /// </summary>
        int Kampfer { get; set; }
    }
}