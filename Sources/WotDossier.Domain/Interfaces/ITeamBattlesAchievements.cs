namespace WotDossier.Domain.Interfaces
{
    public interface ITeamBattlesAchievements
    {
        int KingOfTheHill { get; set; }
        int ArmoredFist { get; set; }
        int CrucialShot { get; set; }
        int CrucialShotMedal { get; set; }
        int FightingReconnaissance { get; set; }
        int FightingReconnaissanceMedal { get; set; }
        int ForTacticalOperations { get; set; }
        int GeniusForWar { get; set; }
        int GeniusForWarMedal { get; set; }
        int GodOfWar { get; set; }
        int MaxTacticalBreakthroughSeries { get; set; }
        int TacticalBreakthrough { get; set; }
        int TacticalBreakthroughSeries { get; set; }
        int WillToWinSpirit { get; set; }
        int WolfAmongSheep { get; set; }
        int WolfAmongSheepMedal { get; set; }

        int PromisingFighter { get; set; }
        int PromisingFighterMedal { get; set; }
        int HeavyFire { get; set; }
        int HeavyFireMedal { get; set; }
        int Ranger { get; set; }
        int RangerMedal { get; set; }
        int FireAndSteel { get; set; }
        int FireAndSteelMedal { get; set; }
        int Pyromaniac { get; set; }
        int PyromaniacMedal { get; set; }
        int NoMansLand { get; set; }
    }
}