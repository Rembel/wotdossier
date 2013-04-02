namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowXP
    {
        int Xp { get; set; }
        int MaxXp { get; set; }
        double AvgXp { get; }
    }
}