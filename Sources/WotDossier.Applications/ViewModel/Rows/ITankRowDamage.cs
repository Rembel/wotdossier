namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowDamage
    {
        int DamageDealt { get; set; }
        int DamageTaken { get; set; }
        double DamageRatio { get; set; }
        int AverageDamageDealt { get; set; }
        int DamagePerHit { get; set; }
    }
}