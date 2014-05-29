using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerAchievementsEntity"/>.
    /// </summary>
    public class PlayerAchievementsMapping : ClassMapBase<PlayerAchievementsEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAchievementsMapping"/> class.
        /// </summary>
        public PlayerAchievementsMapping()
        {
			Map(v => v.Warrior);
			Map(v => v.Sniper);
			Map(v => v.Sniper2);
			Map(v => v.MainGun);
			Map(v => v.Invader);
			Map(v => v.Defender);
			Map(v => v.SteelWall);
			Map(v => v.Confederate);
			Map(v => v.Scout);
			Map(v => v.PatrolDuty);
            Map(v => v.HeroesOfRassenay);
			Map(v => v.LafayettePool);
			Map(v => v.RadleyWalters);
			Map(v => v.CrucialContribution);
			Map(v => v.BrothersInArms);
			Map(v => v.Kolobanov);
			Map(v => v.Nikolas);
			Map(v => v.Orlik);
			Map(v => v.Oskin);
			Map(v => v.Halonen);
			Map(v => v.Lehvaslaiho);
			Map(v => v.DeLanglade);
			Map(v => v.Burda);
			Map(v => v.Dumitru);
			Map(v => v.Pascucci);
			Map(v => v.TamadaYoshio);
			Map(v => v.Boelter);
			Map(v => v.Fadin);
			Map(v => v.Tarczay);
			Map(v => v.BrunoPietro);
			Map(v => v.Billotte);
			Map(v => v.Survivor);
			Map(v => v.Kamikaze);
			Map(v => v.Invincible);
			Map(v => v.Raider);
			Map(v => v.Bombardier);
			Map(v => v.Reaper);
			Map(v => v.MouseTrap);
			Map(v => v.PattonValley);
			Map(v => v.Hunter);
			Map(v => v.Sinai);
			Map(v => v.MasterGunnerLongest);
			Map(v => v.SharpshooterLongest);
            Map(v => v.Huntsman, "Ranger");
			Map(v => v.IronMan, "CoolHeaded");
			Map(v => v.Sturdy, "Spartan");
			Map(v => v.LuckyDevil);
            Map(v => v.Kay);
            Map(v => v.Carius);
            Map(v => v.Knispel);
            Map(v => v.Poppel);
            Map(v => v.Abrams);
            Map(v => v.Leclerk);
            Map(v => v.Lavrinenko);
            Map(v => v.Ekins);
            Map(v => v.MarksOnGun);
            Map(v => v.MovingAvgDamage);
            Map(v => v.MedalMonolith);
            Map(v => v.MedalAntiSpgFire);
            Map(v => v.MedalGore);
            Map(v => v.MedalCoolBlood);
            Map(v => v.MedalStark);
        }
    }
}
