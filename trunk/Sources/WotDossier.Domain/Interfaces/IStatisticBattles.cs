namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticBattles
    {
        int BattlesCount { get; set; }
        int Wins { get; set; }
        double WinsPercent { get; }
        int Losses { get; set; }
        double LossesPercent { get; }
        int Draws { get; }
        double DrawsPercent { get; }
        int SurvivedBattles { get; set; }
        double SurvivedBattlesPercent { get; }
        int SurvivedAndWon { get; set; }
        double SurvivedAndWonPercent { get; }
    }
}