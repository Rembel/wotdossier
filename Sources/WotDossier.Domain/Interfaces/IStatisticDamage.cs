namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticDamage
    {
        int DamageDealt { get; set; }
        int DamageTaken { get; set; }
        int MaxDamage { get; set; }
    }
}