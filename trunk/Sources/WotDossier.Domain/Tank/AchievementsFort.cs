using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    public class AchievementsFort : IFortAchievements
    {
        /// <summary>
        /// Gets or sets the conqueror achievement count.
        /// </summary>
        public int Conqueror { get; set; }
        
        /// <summary>
        /// Gets or sets the fire and sword achievement count.
        /// </summary>
        public int FireAndSword { get; set; }
        
        /// <summary>
        /// Gets or sets the crusher achievement count.
        /// </summary>
        public int Crusher { get; set; }
        
        /// <summary>
        /// Gets or sets the counterblow achievement count.
        /// </summary>
        public int CounterBlow { get; set; }
        
        /// <summary>
        /// Gets or sets the soldier of fortune achievement count.
        /// </summary>
        public int SoldierOfFortune { get; set; }

        /// <summary>
        /// Gets or sets the kampfer achievement count.
        /// </summary>
        public int Kampfer { get; set; }
    }
}