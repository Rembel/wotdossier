namespace WotDossier.Domain.Rows
{
    public interface ITankRowBattles
    {
        int Battles { get; set; }
        int Won { get; set; }
        double WonPercent { get; set; }
        int Lost { get; set; }
        double LostPercent { get; set; }
        int Draws { get; set; }
        double DrawsPercent { get; set; }
        int Survived { get; set; }
        double SurvivedPercent { get; set; }
        int SurvivedAndWon { get; set; }
        double SurvivedAndWonPercent { get; set; }
    }
}