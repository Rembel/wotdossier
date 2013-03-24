namespace WotDossier.Domain.Rows
{
    public interface ITankRowXP
    {
        int TotalXP { get; set; }
        int MaximumXP { get; set; }
        int AverageXP { get; set; }
    }
}