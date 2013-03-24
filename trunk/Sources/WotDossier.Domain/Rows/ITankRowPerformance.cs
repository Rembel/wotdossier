namespace WotDossier.Domain.Rows
{
    public interface ITankRowPerformance
    {
        int Shots { get; set; }
        int Hits { get; set; }
        double HitRatio { get; set; }
        int CapturePoints { get; set; }
        int DefencePoints { get; set; }
        int TanksSpotted { get; set; }
    }
}