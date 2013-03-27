namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowBattles
    {
        int BattlesCount { get; set; }
        int Wins { get; set; }
        double WinsPercent { get; set; }
        int Losses { get; set; }
        double LossesPercent { get; set; }
        int Draws { get; set; }
        double DrawsPercent { get; set; }
        int SurvivedBattles { get; set; }
        double SurvivedBattlesPercent { get; set; }
        int SurvivedAndWon { get; set; }
        double SurvivedAndWonPercent { get; set; }
    }
}