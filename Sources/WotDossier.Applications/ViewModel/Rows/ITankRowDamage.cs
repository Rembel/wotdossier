namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowDamage
    {
        int DamageDealt { get; set; }
        int DamageTaken { get; set; }
        double DamageRatio { get; }
        double AvgDamageDealt { get; }
        int DamagePerHit { get; }
    }
}