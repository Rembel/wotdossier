namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticFrags
    {
        int Frags { get; set; }
        int MaxFrags { get; set; }
        double AvgFrags { get; }
        double KillDeathRatio { get; }
        int Tier8Frags { get; set; }
        int BeastFrags { get; set; }
        int SinaiFrags { get; set; }
        int PattonFrags { get; set; }
        int MouseFrags { get; set; }
    }
}