
using System;

namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticExtended
    {
        double AvgXp { get; }

        int BattlesPerDay { get; set; }

        double WinsPercent { get; }

        double LossesPercent { get; }

        int Draws { get; }

        double DrawsPercent { get; }

        double SurvivedBattlesPercent { get; }

        int SurvivedAndWon { get; set; }

        double SurvivedAndWonPercent { get; }

        #region Damage

        double DamageRatio { get; }

        double AvgDamageDealt { get; }

        int DamagePerHit { get; }

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

        #endregion

        #region Time

        DateTime LastBattle { get; set; }
        TimeSpan PlayTime { get; set; }
        TimeSpan AverageBattleTime { get; set; }

        #endregion


    }
}