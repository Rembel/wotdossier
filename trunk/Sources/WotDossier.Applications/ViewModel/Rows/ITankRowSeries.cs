namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowSeries
    {
        int ReaperLongest { get; set; }
        int ReaperProgress { get; set; }
        int SharpshooterLongest { get; set; }
        int SharpshooterProgress { get; set; }
        int MasterGunnerLongest { get; set; }
        int MasterGunnerProgress { get; set; }
        int InvincibleLongest { get; set; }
        int InvincibleProgress { get; set; }
        int SurvivorLongest { get; set; }
        int SurvivorProgress { get; set; }
    }
}