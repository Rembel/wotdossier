namespace WotDossier.Domain.Tank
{
    public interface ITankFilterable
    {
        int CountryId { get; set; }
        int Type { get; set; }
        string Tank { get; set; }
        double Tier { get; set; }
        bool IsPremium { get; set; }
        bool IsFavorite { get; set; }
    }

    public class FragsJson : ITankFilterable
    {
        public int CountryId { get; set; }

        public int TankId { get; set; }

        public TankIcon Icon { get; set; }

        public int TankUniqueId { get; set; }

        public int KilledByTankUniqueId { get; set; }

        public int Count { get; set; }

        public int Type { get; set; }

        public string Tank { get; set; }

        public double Tier { get; set; }

        public bool IsPremium { get; set; }
        
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", Tank, Count);
        }
    }
}
