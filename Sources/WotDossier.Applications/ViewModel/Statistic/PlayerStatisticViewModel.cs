using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Model;
using WotDossier.Dal;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract class PlayerStatisticViewModel : StatisticViewModelBase
    {
        private ClanModel _clan;

        #region Common

        public string Name { get; set; }

        public long AccountId { get; set; }

        /// <summary>
        ///     Player account created
        /// </summary>
        public DateTime Created { get; set; }

        #endregion

        private PlayerStatisticViewModel TypedPrevStatistic
        {
            get { return (PlayerStatisticViewModel)PrevStatisticSlice.Statistic; }
        }

        public string PerformanceRatingLink
        {
            get { return string.Format(RatingHelper.NOOBMETER_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, Name); }
        }

        public string KievArmorRatingLink
        {
            get { return string.Format(RatingHelper.ARNORKIEV_STATISTIC_LINK_FORMAT, Name); }
        }

        public string EffRatingLink
        {
            get { return string.Format(RatingHelper.WOTNEWS_STATISTIC_LINK_FORMAT, Name); }
        }

        public string NameLink
        {
            get { return string.Format(RatingHelper.WG_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, AccountId, Name); }
        }

        public ClanModel Clan
        {
            get { return _clan; }
            set
            {
                _clan = value;
                OnPropertyChanged("Clan");
            }
        }

        protected PlayerStatisticViewModel(StatisticEntity stat, List<StatisticSlice> list) : base(stat.Updated, list)
        {
            BattlesCount = stat.BattlesCount;
            Wins = stat.Wins;
            Losses = stat.Losses;
            SurvivedBattles = stat.SurvivedBattles;
            Xp = stat.Xp;
            MaxXp = stat.MaxXp;
            Frags = stat.Frags;
            //TODO: field MaxFrags
            MaxFrags = stat.MaxFrags;
            Spotted = stat.Spotted;
            HitsPercents = stat.HitsPercents;
            DamageDealt = stat.DamageDealt;
            DamageTaken = stat.DamageTaken;
            MaxDamage = stat.MaxDamage;
            CapturePoints = stat.CapturePoints;
            DroppedCapturePoints = stat.DroppedCapturePoints;
            //Created = stat.PlayerIdObject.Creaded);
            Updated = stat.Updated;
            Tier = stat.AvgLevel;
            MarkOfMastery = stat.MarkOfMastery;

            RBR = stat.RBR;
            PerformanceRating = stat.PerformanceRating;
            WN8Rating = stat.WN8Rating;
        }
    }
}