namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticPerformance
    {
        double HitsPercents { get; set; }
        
        int CapturePoints { get; set; }
        
        int DroppedCapturePoints { get; set; }
        
        int Spotted { get; set; }

        int MarkOfMastery { get; set; }
    }
}