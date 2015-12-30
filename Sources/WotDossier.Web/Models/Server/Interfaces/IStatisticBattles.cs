namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticBattles
    {
        int BattlesCount { get; set; }

        int Wins { get; set; }

        int Losses { get; set; }

        int SurvivedBattles { get; set; }
    }
}