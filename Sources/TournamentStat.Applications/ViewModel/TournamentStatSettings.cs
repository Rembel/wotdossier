using System;
using System.Collections.Generic;
using OfficeOpenXml;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Settings;
using WotDossier.Domain.Tank;

namespace TournamentStat.Applications.ViewModel
{
    public class TournamentStatSettings : AppSettingsBase
    {
        public TournamentStatSettings()
        {
            Players = new List<TournamentPlayer>();
            TournamentNominations = new List<TournamentNomination>();
        }

        public string TournamentName { get; set; }
        public List<TournamentPlayer> Players { get; set; }
        public List<TournamentNomination> TournamentNominations { get; set; }
    }

    public class TournamentNomination
    {
        public string Nomination { get; set; }

        public int FirstPlacePrize { get; set; }

        public int SecondPlacePrize { get; set; }

        public int ThirdPlacePrize { get; set; }

        public List<TournamentTank> TournamentTanks { get; set; }

        public TournamentCriterion Criterion { get; set; }
    }

    public enum TournamentCriterion
    {
        Damage,
        DamageWithAssist,
        DamageWithArmor,
        Frags,
        WinPercent
    }

    public class TournamentPlayer
    {
        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public string TwitchUrl { get; set; }

        public List<TournamentTank> Tanks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Modes { get; set; }
    }

    public class TournamentTank : ITankFilterable
    {
        public TournamentTank()
        {
        }

        public TournamentTank(TankDescription tankDescription, bool selected = false)
        {
            Tier = tankDescription.Tier;
            Tank = tankDescription.Title;
            TankId = tankDescription.TankId;
            TankUniqueId = tankDescription.UniqueId();
            CountryId = tankDescription.CountryId;
            Type = tankDescription.Type;
            IsPremium = tankDescription.Premium == 1;
            TankIcon = tankDescription.Icon;
            IsSelected = selected;
        }

        public double Tier { get; set; }
        public string Tank { get; set; }
        public int TankId { get; set; }
        public int TankUniqueId { get; set; }
        public int CountryId { get; set; }
        public int Type { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSelected { get; set; }
        public TankIcon TankIcon { get; set; }
    }
}