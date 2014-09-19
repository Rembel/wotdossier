
using System;

namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticExtended
    {
        #region XP

        double AvgXp { get; }
        int OriginalXP { get; set; }
        double AvgOriginalXP { get; }
        int XpBefore88 { get; set; }

        #endregion

        #region Battles

        double SurvivedBattlesPercent { get; }
        int BattlesPerDay { get; set; }

        double WinsPercent { get; }

        double LossesPercent { get; }

        int Draws { get; }

        double DrawsPercent { get; }

        int SurvivedAndWon { get; set; }

        double SurvivedAndWonPercent { get; }
        int BattlesCountBefore88 { get; set; }
        int BattlesCount88 { get; set; }
        int BattlesCount90 { get; set; }

        #endregion

        #region Damage

        double DamageRatio { get; }

        double AvgDamageDealt { get; }

        double AvgDamageTaken { get; }
        int DamagePerHit { get; }

        int DamageAssistedTrack { get; set; }
        int DamageAssistedRadio { get; set; }

        double AvgDamageAssisted { get; }
        double AvgDamageAssistedRadio { get; }
        double AvgDamageAssistedTrack { get; }
        int DamageAssisted { get; }

        #endregion
        
        #region Frags

        double AvgFrags { get; }
        double KillDeathRatio { get; }
        int Tier8Frags { get; set; }
        int BeastFrags { get; set; }
        int SinaiFrags { get; set; }
        int PattonFrags { get; set; }
        int MouseFrags { get; set; }

        #endregion

        #region Performance

        int Shots { get; set; }
        int Hits { get; set; }
        double AvgSpotted { get; }
        double AvgCapturePoints { get; }
        double AvgDroppedCapturePoints { get; }
        double Mileage { get; set; }
        int ShotsReceived { get; set; }
        int NoDamageShotsReceived { get; set; }
        int Pierced { get; set; }
        int PiercedReceived { get; set; }
        int HeHitsReceived { get; set; }
        int HeHits { get; set; }
        int PotentialDamageReceived { get; set; }
        int DamageBlockedByArmor { get; set; }

        #endregion

        #region Time

        DateTime LastBattle { get; set; }
        TimeSpan PlayTime { get; set; }
        TimeSpan AverageBattleTime { get; set; }

        #endregion

        int DamageDealtDelta { get; }
        int BattlesCountDelta { get; }
        int WinsDelta { get; }
        int SpottedDelta { get; }
        int DroppedCapturePointsDelta { get; }
        int FragsDelta { get; }
        double WinsPercentForPeriod { get; }
    }
}