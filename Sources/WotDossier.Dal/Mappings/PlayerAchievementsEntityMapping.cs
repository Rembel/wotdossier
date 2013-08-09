using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerAchievementsEntity"/>.
    /// </summary>
    public class PlayerAchievementsMapping : ClassMapBase<PlayerAchievementsEntity>
    {
        public PlayerAchievementsMapping()
        {
			Map(v => v.Warrior, "Warrior");
			Map(v => v.Sniper, "Sniper");
			Map(v => v.Invader, "Invader");
			Map(v => v.Defender, "Defender");
			Map(v => v.SteelWall, "SteelWall");
			Map(v => v.Confederate, "Confederate");
			Map(v => v.Scout, "Scout");
			Map(v => v.PatrolDuty, "PatrolDuty");
            Map(v => v.HeroesOfRassenay, "HeroesOfRassenay");
			Map(v => v.LafayettePool, "LafayettePool");
			Map(v => v.RadleyWalters, "RadleyWalters");
			Map(v => v.CrucialContribution, "CrucialContribution");
			Map(v => v.BrothersInArms, "BrothersInArms");
			Map(v => v.Kolobanov, "Kolobanov");
			Map(v => v.Nikolas, "Nikolas");
			Map(v => v.Orlik, "Orlik");
			Map(v => v.Oskin, "Oskin");
			Map(v => v.Halonen, "Halonen");
			Map(v => v.Lehvaslaiho, "Lehvaslaiho");
			Map(v => v.DeLanglade, "DeLanglade");
			Map(v => v.Burda, "Burda");
			Map(v => v.Dumitru, "Dumitru");
			Map(v => v.Pascucci, "Pascucci");
			Map(v => v.TamadaYoshio, "TamadaYoshio");
			Map(v => v.Boelter, "Boelter");
			Map(v => v.Fadin, "Fadin");
			Map(v => v.Tarczay, "Tarczay");
			Map(v => v.BrunoPietro, "BrunoPietro");
			Map(v => v.Billotte, "Billotte");
			Map(v => v.Survivor, "Survivor");
			Map(v => v.Kamikaze, "Kamikaze");
			Map(v => v.Invincible, "Invincible");
			Map(v => v.Raider, "Raider");
			Map(v => v.Bombardier, "Bombardier");
			Map(v => v.Reaper, "Reaper");
			Map(v => v.MouseTrap, "MouseTrap");
			Map(v => v.PattonValley, "PattonValley");
			Map(v => v.Hunter, "Hunter");
			Map(v => v.Sinai, "Sinai");
			Map(v => v.MasterGunnerLongest, "MasterGunnerLongest");
			Map(v => v.SharpshooterLongest, "SharpshooterLongest");
            Map(v => v.Ranger, "Ranger");
			Map(v => v.CoolHeaded, "CoolHeaded");
			Map(v => v.Spartan, "Spartan");
			Map(v => v.LuckyDevil, "LuckyDevil");
            Map(v => v.Kay, "Kay");
            Map(v => v.Carius, "Carius");
            Map(v => v.Knispel, "Knispel");
            Map(v => v.Poppel, "Poppel");
            Map(v => v.Abrams, "Abrams");
            Map(v => v.Leclerk, "Leclerk");
            Map(v => v.Lavrinenko, "Lavrinenko");
            Map(v => v.Ekins, "Ekins");
		
			//HasMany(v => v.PlayerStatisticEntities).KeyColumn(Column<PlayerStatisticEntity>(v => v.AchievementsId));
        }
    }
}
