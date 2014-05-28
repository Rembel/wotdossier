using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerAchievementsEntity"/>.
    /// </summary>
    public class TeamBattlesAchievementsEntityMapping : ClassMapBase<TeamBattlesAchievementsEntity>
    {
        public TeamBattlesAchievementsEntityMapping()
        {
			Map(v => v.WolfAmongSheep, "WolfAmongSheep");
			Map(v => v.WolfAmongSheepMedal, "WolfAmongSheepMedal");
			Map(v => v.GeniusForWar, "GeniusForWar");
			Map(v => v.GeniusForWarMedal, "GeniusForWarMedal");
			Map(v => v.KingOfTheHill, "KingOfTheHill");
			Map(v => v.TacticalBreakthroughSeries, "TacticalBreakthroughSeries");
			Map(v => v.MaxTacticalBreakthroughSeries, "MaxTacticalBreakthroughSeries");
			Map(v => v.ArmoredFist, "ArmoredFist");
			Map(v => v.TacticalBreakthrough, "TacticalBreakthrough");
			Map(v => v.GodOfWar, "GodOfWar");
			Map(v => v.FightingReconnaissance, "FightingReconnaissance");
			Map(v => v.FightingReconnaissanceMedal, "FightingReconnaissanceMedal");
			Map(v => v.WillToWinSpirit, "WillToWinSpirit");
			Map(v => v.CrucialShot, "CrucialShot");
			Map(v => v.CrucialShotMedal, "CrucialShotMedal");
			Map(v => v.ForTacticalOperations, "ForTacticalOperations");

            Map(v => v.PromisingFighter, "PromisingFighter");
            Map(v => v.PromisingFighterMedal, "PromisingFighterMedal");
            Map(v => v.HeavyFire, "HeavyFire");
            Map(v => v.HeavyFireMedal, "HeavyFireMedal");
            Map(v => v.Ranger, "Ranger");
            Map(v => v.RangerMedal, "RangerMedal");
            Map(v => v.FireAndSteel, "FireAndSteel");
            Map(v => v.FireAndSteelMedal, "FireAndSteelMedal");
            Map(v => v.Pyromaniac, "Pyromaniac");
            Map(v => v.PyromaniacMedal, "PyromaniacMedal");
            Map(v => v.NoMansLand, "NoMansLand");
		
			//HasMany(v => v.PlayerStatisticEntities).KeyColumn(Column<PlayerStatisticEntity>(v => v.AchievementsId));
        }
    }
}
