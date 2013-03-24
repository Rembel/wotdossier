namespace WotDossier.Domain.Rows
{
    public interface ITankRowFrags
    {
        int Battles { get; set; }
        int Frags { get; set; }
        int MaxFrags { get; set; }
        double FragsPerBattle { get; set; }
        double KillDeathRatio { get; set; }
        int Tier8Frags { get; set; }
        int BeastFrags { get; set; }
        int SinaiFrags { get; set; }
    }
}