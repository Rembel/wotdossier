namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowPerformance
    {
        int Shots { get; set; }
        int Hits { get; set; }
        double HitsPercents { get; set; }
        int CapturePoints { get; set; }
        int DefencePoints { get; set; }
        int Spotted { get; set; }
    }
}