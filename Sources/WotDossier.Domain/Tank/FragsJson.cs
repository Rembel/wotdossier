namespace WotDossier.Domain.Tank
{
    public class FragsJson
    {
        public int CountryId { get; set; }

        public int TankId { get; set; }

        public TankIcon Icon { get; set; }

        public int TankUniqueId { get; set; }

        public int KilledByTankUniqueId { get; set; }

        public int Count { get; set; }

        public int Type { get; set; }

        public string Tank { get; set; }

        public int Tier { get; set; }
    }
}
