namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticDamage
    {
        int DamageDealt { get; set; }
        int DamageTaken { get; set; }
        double DamageRatio { get; }
        double AvgDamageDealt { get; }
        int DamagePerHit { get; }
    }
}