namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowFrags
    {
        int Frags { get; set; }
        int MaxFrags { get; set; }
        double FragsPerBattle { get; }
        double KillDeathRatio { get; }
        int Tier8Frags { get; set; }
        int BeastFrags { get; set; }
        int SinaiFrags { get; set; }
    }
}