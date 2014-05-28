using System.Collections.Generic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankStatisticRow : IStatisticBase, IStatisticBattles, IStatisticDamage, IStatisticFrags, IStatisticPerformance, IStatisticRatings, IStatisticTime, ITankFilterable
    {
        TankIcon Icon { get; set; }
        int TankId { get; set; }
        int TankUniqueId { get; set; }
        double WinsPercentForPeriod { get; }
        int BattlesCountDelta { get; }
        int DamageDealtDelta { get; }
        int SpottedDelta { get; }
        int DroppedCapturePointsDelta { get; }
        int WinsDelta { get; }
        int FragsDelta { get; }
        TankDescription Description { get; set; }
        int OriginalXP { get; set; }
        int DamageAssistedTrack { get; set; }
        int DamageAssistedRadio { get; set; }
        double Mileage { get; set; }
        int ShotsReceived { get; set; }
        int NoDamageShotsReceived { get; set; }
        int PiercedReceived { get; set; }
        int HeHitsReceived { get; set; }
        int HeHits { get; set; }
        int Pierced { get; set; }
        int XpBefore88 { get; set; }
        int BattlesCountBefore88 { get; set; }
        int BattlesCount88 { get; set; }
        int MaxDamage { get; set; }
        double AvgOriginalXP { get; }
        double AvgDamageAssisted { get; }
        double AvgDamageAssistedRadio { get; }
        double AvgDamageAssistedTrack { get; }
        int DamageAssisted { get; }
        IEnumerable<ITankStatisticRow> GetAll();
        void SetPreviousStatistic(ITankStatisticRow model);
        ITankStatisticRow GetPreviousStatistic();
    }
}