﻿namespace WotDossier.Domain.Tank
{
    public interface ITankFilterable
    {
        int CountryId { get; set; }
        int Type { get; set; }
        string Tank { get; set; }
        double Tier { get; set; }
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
    }
}