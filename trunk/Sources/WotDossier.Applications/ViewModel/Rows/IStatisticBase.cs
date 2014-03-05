using System;

namespace WotDossier.Applications.ViewModel.Rows
{
    public interface IStatisticBase : IStatisticXp, IStatisticBattleAwards, IStatisticEpic, IStatisticSpecialAwards, IStatisticMedals, IStatisticSeries
    {
        double AvgFrags { get; }
        double AvgSpotted { get; }
        double AvgCapturePoints { get; }
        double AvgDroppedCapturePoints { get; }
        double NoobRating { get; }
        double XEFF { get; }
        double XWN { get; }
        double PerformanceRating { get; set; }
        double WN8Rating { get; set; }
        double? RBR { get; set; }
        int KingOfTheHill { get; set; }
        int ArmoredFist { get; set; }
        int TacticalBreakthrough { get; set; }

        /// <summary>
        /// Stat updated
        /// </summary>
        DateTime Updated { get; set; }

        int BattlesPerDay { get; set; }
    }
}