using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OfficeOpenXml;
using TournamentStat.Applications.Annotations;
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

    public class TournamentPlayer : INotifyPropertyChanged
    {
        public int PlayerId { get; set; }
        public int AccountId { get; set; }

        public string PlayerName { get; set; }
        private string _mods;

        public string Mods
        {
            get { return _mods; }
            set
            {
                _mods = value;
                OnPropertyChanged(nameof(Mods));
            }
        }

        private string _twitchUrl;

        public string TwitchUrl
        {
            get { return _twitchUrl; }
            set
            {
                _twitchUrl = value;
                OnPropertyChanged(nameof(TwitchUrl));
            }
        }

        public List<TournamentTank> Tanks { get; set; }

        public string TanksList
        {
            get { return string.Join(", ", Tanks.Select(x => x.Tank)); }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Modes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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

        public string Dossier { get; set; }
        public string ReplaysUrlOwner { get; set; }
        public string ReplaysUrl { get; set; }
        public int BattlesCount { get; set; }
    }
}