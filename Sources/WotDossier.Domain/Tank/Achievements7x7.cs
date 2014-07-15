using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    public class Achievements7x7 : ITeamBattlesAchievements
    {
        public int WolfAmongSheep { get; set; }
        public int WolfAmongSheepMedal { get; set; }
        public int GeniusForWar { get; set; }
        public int GeniusForWarMedal { get; set; }
        public int KingOfTheHill { get; set; }
        public int TacticalBreakthroughSeries { get; set; }
        public int MaxTacticalBreakthroughSeries { get; set; }
        public int ArmoredFist { get; set; }
        public int TacticalBreakthrough { get; set; }

        public int GodOfWar { get; set; }
        public int FightingReconnaissance { get; set; }
        public int FightingReconnaissanceMedal { get; set; }
        public int WillToWinSpirit { get; set; }
        public int CrucialShot { get; set; }
        public int CrucialShotMedal { get; set; }
        public int ForTacticalOperations { get; set; }

        #region 0.9.1

        public int PromisingFighter { get; set; }
        public int PromisingFighterMedal { get; set; }
        public int HeavyFire { get; set; }
        public int HeavyFireMedal { get; set; }
        public int Ranger { get; set; }
        public int RangerMedal { get; set; }
        public int FireAndSteel { get; set; }
        public int FireAndSteelMedal { get; set; }
        public int Pyromaniac { get; set; }
        public int PyromaniacMedal { get; set; }
        public int NoMansLand { get; set; }

        #endregion

        #region 0.9.2

        public int Guerrilla { get; set; }
        public int GuerrillaMedal { get; set; }
        public int Infiltrator { get; set; }
        public int InfiltratorMedal { get; set; }
        public int Sentinel { get; set; }
        public int SentinelMedal { get; set; }
        public int PrematureDetonation { get; set; }
        public int PrematureDetonationMedal { get; set; }
        public int BruteForce { get; set; }
        public int BruteForceMedal { get; set; }
        public int AwardCount { get; set; }
        public int BattleTested { get; set; }

        #endregion
    }
}