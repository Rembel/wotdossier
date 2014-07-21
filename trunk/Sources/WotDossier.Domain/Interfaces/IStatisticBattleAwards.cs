namespace WotDossier.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStatisticBattleAwards
    {
        /// <summary>
        /// Gets or sets the battle hero.
        /// </summary>
        int BattleHero { get; set; }

        /// <summary>
        /// Gets or sets the warrior.
        /// </summary>
        int Warrior { get; set; }

        /// <summary>
        /// Gets or sets the invader.
        /// </summary>
        int Invader { get; set; }

        /// <summary>
        /// Gets or sets the sniper.
        /// </summary>
        int Sniper { get; set; }

        /// <summary>
        /// Gets or sets the sniper2.
        /// </summary>
        int Sniper2 { get; set; }

        /// <summary>
        /// Gets or sets the main gun.
        /// </summary>
        int MainGun { get; set; }

        /// <summary>
        /// Gets or sets the defender.
        /// </summary>
        int Defender { get; set; }

        /// <summary>
        /// Gets or sets the steel wall.
        /// </summary>
        int SteelWall { get; set; }

        /// <summary>
        /// Gets or sets the confederate.
        /// </summary>
        int Confederate { get; set; }

        /// <summary>
        /// Gets or sets the scout.
        /// </summary>
        int Scout { get; set; }

        /// <summary>
        /// Gets or sets the patrol duty.
        /// </summary>
        int PatrolDuty { get; set; }

        /// <summary>
        /// Gets or sets the brothers in arms.
        /// </summary>
        int BrothersInArms { get; set; }

        /// <summary>
        /// Gets or sets the crucial contribution.
        /// </summary>
        int CrucialContribution { get; set; }

        /// <summary>
        /// Gets or sets the iron man.
        /// </summary>
        int IronMan { get; set; }

        /// <summary>
        /// Gets or sets the lucky devil.
        /// </summary>
        int LuckyDevil { get; set; }

        /// <summary>
        /// Gets or sets the sturdy.
        /// </summary>
        int Sturdy { get; set; }
    }
}