namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticPerformance
    {
        int Shots { get; set; }
        int Hits { get; set; }
        double HitsPercents { get; set; }
        
        int CapturePoints { get; set; }
        double AvgCapturePoints { get; }

        int DroppedCapturePoints { get; set; }
        double AvgDroppedCapturePoints { get; }
        
        int Spotted { get; set; }
        double AvgSpotted { get; }
    }
}