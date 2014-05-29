using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TeamBattlesAchievementsEntityMapping"/>.
    /// </summary>
    public class TeamBattlesAchievementsEntityMapping : ClassMapBase<TeamBattlesAchievementsEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesAchievementsEntityMapping"/> class.
        /// </summary>
        public TeamBattlesAchievementsEntityMapping()
        {
			Map(v => v.WolfAmongSheep);
			Map(v => v.WolfAmongSheepMedal);
			Map(v => v.GeniusForWar);
			Map(v => v.GeniusForWarMedal);
			Map(v => v.KingOfTheHill);
			Map(v => v.TacticalBreakthroughSeries);
			Map(v => v.MaxTacticalBreakthroughSeries);
			Map(v => v.ArmoredFist);
			Map(v => v.TacticalBreakthrough);
			Map(v => v.GodOfWar);
			Map(v => v.FightingReconnaissance);
			Map(v => v.FightingReconnaissanceMedal);
			Map(v => v.WillToWinSpirit);
			Map(v => v.CrucialShot);
			Map(v => v.CrucialShotMedal);
			Map(v => v.ForTacticalOperations);

            Map(v => v.PromisingFighter);
            Map(v => v.PromisingFighterMedal);
            Map(v => v.HeavyFire);
            Map(v => v.HeavyFireMedal);
            Map(v => v.Ranger);
            Map(v => v.RangerMedal);
            Map(v => v.FireAndSteel);
            Map(v => v.FireAndSteelMedal);
            Map(v => v.Pyromaniac);
            Map(v => v.PyromaniacMedal);
            Map(v => v.NoMansLand);
        }
    }
}
