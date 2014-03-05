namespace WotDossier.Applications.ViewModel.Rows
{
    public interface IStatisticXp
    {
        int Xp { get; set; }
        int MaxXp { get; set; }
        double AvgXp { get; }
    }
}