using System;
using System.Collections.Generic;
using WotDossier.Common;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerStatisticViewModel : PeriodStatisticViewModel<PlayerStatisticViewModel>
    {
        //GR-->
            //Global Rating
            public static readonly string PropRating_IntegratedValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_IntegratedValue);
            public static readonly string PropRating_IntegratedPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_IntegratedPlace);
            //W/B-->
            //Victories/Battles
            public static readonly string PropRating_BattleAvgPerformanceValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgPerformanceValue);
            public static readonly string PropRating_BattleAvgPerformancePlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgPerformancePlace);
            //E/B-->
            //Average Experience per Battle
            public static readonly string PropRating_BattleAvgXpValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgXpValue);
            public static readonly string PropRating_BattleAvgXpPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgXpPlace);
            //WIN-->
            //Victories
            public static readonly string PropRating_BattleWinsValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleWinsValue);
            public static readonly string PropRating_BattleWinsPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleWinsPlace);
            //GPL-->
            //Battles Participated
            public static readonly string PropRating_BattlesValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattlesValue);
            public static readonly string PropRating_BattlesPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattlesPlace);
            //CPT-->
            //Capture Points
            public static readonly string PropRating_CapturedPointsValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_CapturedPointsValue);
            public static readonly string PropRating_CapturedPointsPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_CapturedPointsPlace);
            //DMG-->
            //Damage Caused
            public static readonly string PropRating_DamageDealtValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DamageDealtValue);
            public static readonly string PropRating_DamageDealtPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DamageDealtPlace);
            //DPT-->
            //Defense Points
            public static readonly string PropRating_DroppedPointsValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DroppedPointsValue);
            public static readonly string PropRating_DroppedPointsPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DroppedPointsPlace);
            //FRG-->
            //Targets Destroyed
            public static readonly string PropRating_FragsValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_FragsValue);
            public static readonly string PropRating_FragsPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_FragsPlace);
            //SPT-->
            //Targets Detected
            public static readonly string PropRating_SpottedValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_SpottedValue);
            public static readonly string PropRating_SpottedPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_SpottedPlace);
            //EXP-->
            //Total Experience
            public static readonly string PropRating_XpValue = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_XpValue);
            public static readonly string PropRating_XpPlace = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_XpPlace);

            //GR-->
            //Global Rating
            public static readonly string PropRating_IntegratedValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_IntegratedValueDelta);
            public static readonly string PropRating_IntegratedPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_IntegratedPlaceDelta);
            //W/B-->
            //Victories/Battles
            public static readonly string PropRating_BattleAvgPerformanceValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgPerformanceValueDelta);
            public static readonly string PropRating_BattleAvgPerformancePlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgPerformancePlaceDelta);
            //E/B-->
            //Average Experience per Battle
            public static readonly string PropRating_BattleAvgXpValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgXpValueDelta);
            public static readonly string PropRating_BattleAvgXpPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleAvgXpPlaceDelta);
            //WIN-->
            //Victories
            public static readonly string PropRating_BattleWinsValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleWinsValueDelta);
            public static readonly string PropRating_BattleWinsPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattleWinsPlaceDelta);
            //GPL-->
            //Battles Participated
            public static readonly string PropRating_BattlesValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattlesValueDelta);
            public static readonly string PropRating_BattlesPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_BattlesPlaceDelta);
            //CPT-->
            //Capture Points
            public static readonly string PropRating_CapturedPointsValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_CapturedPointsValueDelta);
            public static readonly string PropRating_CapturedPointsPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_CapturedPointsPlaceDelta);
            //DMG-->
            //Damage Caused
            public static readonly string PropRating_DamageDealtValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DamageDealtValueDelta);
            public static readonly string PropRating_DamageDealtPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DamageDealtPlaceDelta);
            //DPT-->
            //Defense Points
            public static readonly string PropRating_DroppedPointsValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DroppedPointsValueDelta);
            public static readonly string PropRating_DroppedPointsPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_DroppedPointsPlaceDelta);
            //FRG-->
            //Targets Destroyed
            public static readonly string PropRating_FragsValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_FragsValueDelta);
            public static readonly string PropRating_FragsPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_FragsPlaceDelta);
            //SPT-->
            //Targets Detected
            public static readonly string PropRating_SpottedValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_SpottedValueDelta);
            public static readonly string PropRating_SpottedPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_SpottedPlaceDelta);
            //EXP-->
            //Total Experience
            public static readonly string PropRating_XpValueDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_XpValueDelta);
            public static readonly string PropRating_XpPlaceDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.Rating_XpPlaceDelta);

        #region Common

        public string Name { get; set; }

        /// <summary>
        ///     Player account created
        /// </summary>
        public DateTime Created { get; set; }

        #endregion

        #region Rating

        //GR-->
        //Global Rating
        public int Rating_IntegratedValue { get; set; }

        public int Rating_IntegratedPlace { get; set; }

        //W/B-->
        //Victories/Battles
        public int Rating_BattleAvgPerformanceValue { get; set; }

        public int Rating_BattleAvgPerformancePlace { get; set; }

        //E/B-->
        //Average Experience per Battle
        public int Rating_BattleAvgXpValue { get; set; }

        public int Rating_BattleAvgXpPlace { get; set; }

        //WIN-->
        //Victories
        public int Rating_BattleWinsValue { get; set; }

        public int Rating_BattleWinsPlace { get; set; }

        //GPL-->
        //Battles Participated
        public int Rating_BattlesValue { get; set; }

        public int Rating_BattlesPlace { get; set; }

        //CPT-->
        //Capture Points
        public int Rating_CapturedPointsValue { get; set; }

        public int Rating_CapturedPointsPlace { get; set; }

        //DMG-->
        //Damage Caused
        public int Rating_DamageDealtValue { get; set; }

        public int Rating_DamageDealtPlace { get; set; }

        //DPT-->
        //Defense Points
        public int Rating_DroppedPointsValue { get; set; }

        public int Rating_DroppedPointsPlace { get; set; }

        //FRG-->
        //Targets Destroyed
        public int Rating_FragsValue { get; set; }

        public int Rating_FragsPlace { get; set; }

        //SPT-->
        //Targets Detected
        public int Rating_SpottedValue { get; set; }

        public int Rating_SpottedPlace { get; set; }

        //EXP-->
        //Total Experience
        public int Rating_XpValue { get; set; }

        public int Rating_XpPlace { get; set; }

        #endregion

        #region Rating delta

        public int Rating_IntegratedValueDelta
        {
            get { return Rating_IntegratedValue - PrevStatistic.Rating_IntegratedValue; }
        }

        public int Rating_IntegratedPlaceDelta
        {
            get { return Rating_IntegratedPlace - PrevStatistic.Rating_IntegratedPlace; }
        }

        public int Rating_BattleAvgPerformanceValueDelta
        {
            get { return Rating_BattleAvgPerformanceValue - PrevStatistic.Rating_BattleAvgPerformanceValue; }
        }

        public int Rating_BattleAvgPerformancePlaceDelta
        {
            get { return Rating_BattleAvgPerformancePlace - PrevStatistic.Rating_BattleAvgPerformancePlace; }
        }

        public int Rating_BattleAvgXpValueDelta
        {
            get { return Rating_BattleAvgXpValue - PrevStatistic.Rating_BattleAvgXpValue; }
        }

        public int Rating_BattleAvgXpPlaceDelta
        {
            get { return Rating_BattleAvgXpPlace - PrevStatistic.Rating_BattleAvgXpPlace; }
        }

        public int Rating_BattleWinsValueDelta
        {
            get { return Rating_BattleWinsValue - PrevStatistic.Rating_BattleWinsValue; }
        }

        public int Rating_BattleWinsPlaceDelta
        {
            get { return Rating_BattleWinsPlace - PrevStatistic.Rating_BattleWinsPlace; }
        }

        public int Rating_BattlesValueDelta
        {
            get { return Rating_BattlesValue - PrevStatistic.Rating_BattlesValue; }
        }

        public int Rating_BattlesPlaceDelta
        {
            get { return Rating_BattlesPlace - PrevStatistic.Rating_BattlesPlace; }
        }

        public int Rating_CapturedPointsValueDelta
        {
            get { return Rating_CapturedPointsValue - PrevStatistic.Rating_CapturedPointsValue; }
        }

        public int Rating_CapturedPointsPlaceDelta
        {
            get { return Rating_CapturedPointsPlace - PrevStatistic.Rating_CapturedPointsPlace; }
        }

        public int Rating_DamageDealtValueDelta
        {
            get { return Rating_DamageDealtValue - PrevStatistic.Rating_DamageDealtValue; }
        }

        public int Rating_DamageDealtPlaceDelta
        {
            get { return Rating_DamageDealtPlace - PrevStatistic.Rating_DamageDealtPlace; }
        }

        public int Rating_DroppedPointsValueDelta
        {
            get { return Rating_DroppedPointsValue - PrevStatistic.Rating_DroppedPointsValue; }
        }

        public int Rating_DroppedPointsPlaceDelta
        {
            get { return Rating_DroppedPointsPlace - PrevStatistic.Rating_DroppedPointsPlace; }
        }

        public int Rating_FragsValueDelta
        {
            get { return Rating_FragsValue - PrevStatistic.Rating_FragsValue; }
        }

        public int Rating_FragsPlaceDelta
        {
            get { return Rating_FragsPlace - PrevStatistic.Rating_FragsPlace; }
        }

        public int Rating_SpottedValueDelta
        {
            get { return Rating_SpottedValue - PrevStatistic.Rating_SpottedValue; }
        }

        public int Rating_SpottedPlaceDelta
        {
            get { return Rating_SpottedPlace - PrevStatistic.Rating_SpottedPlace; }
        }

        public int Rating_XpValueDelta
        {
            get { return Rating_XpValue - PrevStatistic.Rating_XpValue; }
        }

        public int Rating_XpPlaceDelta
        {
            get { return Rating_XpPlace - PrevStatistic.Rating_XpPlace; }
        }

        #endregion

        public PlayerStatisticViewModel(PlayerStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public PlayerStatisticViewModel(PlayerStatisticEntity stat, List<PlayerStatisticViewModel> list)
            : base(stat.Updated, list)
        {
            #region Common init

            BattlesCount = stat.BattlesCount;
            Wins = stat.Wins;
            Losses = stat.Losses;
            SurvivedBattles = stat.SurvivedBattles;
            Xp = stat.Xp;
            MaxXp = stat.MaxXp;
            Frags = stat.Frags;
            Spotted = stat.Spotted;
            HitsPercents = stat.HitsPercents;
            DamageDealt = stat.DamageDealt;
            CapturePoints = stat.CapturePoints;
            DroppedCapturePoints = stat.DroppedCapturePoints;
            //Created = stat.PlayerIdObject.Creaded);
            Updated = stat.Updated;
            Tier = stat.AvgLevel;

            #endregion

            #region Ratings init

            //GR-->
            //Global Rating
            Rating_IntegratedValue = stat.RatingIntegratedValue;
            Rating_IntegratedPlace = stat.RatingIntegratedPlace;
            //W/B-->
            //Victories/Battles
            Rating_BattleAvgPerformanceValue = stat.RatingBattleAvgPerformanceValue;
            Rating_BattleAvgPerformancePlace = stat.RatingBattleAvgPerformancePlace;
            //E/B-->
            //Average Experience per Battle
            Rating_BattleAvgXpValue = stat.RatingBattleAvgXpValue;
            Rating_BattleAvgXpPlace = stat.RatingBattleAvgXpPlace;
            //WIN-->
            //Victories
            Rating_BattleWinsValue = stat.RatingBattleWinsValue;
            Rating_BattleWinsPlace = stat.RatingBattleWinsPlace;
            //GPL-->
            //Battles Participated
            Rating_BattlesValue = stat.RatingBattlesValue;
            Rating_BattlesPlace = stat.RatingBattlesPlace;
            //CPT-->
            //Capture Points
            Rating_CapturedPointsValue = stat.RatingCapturedPointsValue;
            Rating_CapturedPointsPlace = stat.RatingCapturedPointsPlace;
            //DMG-->
            //Damage Caused
            Rating_DamageDealtValue = stat.RatingDamageDealtValue;
            Rating_DamageDealtPlace = stat.RatingDamageDealtPlace;
            //DPT-->
            //Defense Points
            Rating_DroppedPointsValue = stat.RatingDroppedPointsValue;
            Rating_DroppedPointsPlace = stat.RatingDroppedPointsPlace;
            //FRG-->
            //Targets Destroyed
            Rating_FragsValue = stat.RatingFragsValue;
            Rating_FragsPlace = stat.RatingFragsPlace;
            //SPT-->
            //Targets Detected
            Rating_SpottedValue = stat.RatingSpottedValue;
            Rating_SpottedPlace = stat.RatingSpottedPlace;
            //EXP-->
            //Total Experience
            Rating_XpValue = stat.RatingXpValue;
            Rating_XpPlace = stat.RatingXpPlace;

            #endregion

            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                #region [ IRowBattleAwards ]

                //BattleHero = stat.AchievementsIdObject.battleHeroes;
                TopGun = stat.AchievementsIdObject.Warrior;
                Invader = stat.AchievementsIdObject.Invader;
                Sniper = stat.AchievementsIdObject.Sniper;
                Defender = stat.AchievementsIdObject.Defender;
                SteelWall = stat.AchievementsIdObject.SteelWall;
                Confederate = stat.AchievementsIdObject.Confederate;
                Scout = stat.AchievementsIdObject.Scout;
                PatrolDuty = stat.AchievementsIdObject.PatrolDuty;
                BrothersInArms = stat.AchievementsIdObject.BrothersInArms;
                CrucialContribution = stat.AchievementsIdObject.CrucialContribution;
                CoolHeaded = stat.AchievementsIdObject.CoolHeaded;
                LuckyDevil = stat.AchievementsIdObject.LuckyDevil;
                Spartan = stat.AchievementsIdObject.Spartan;

                #endregion

                #region [ IRowEpic ]

                Boelter = stat.AchievementsIdObject.Boelter;
                RadleyWalters = stat.AchievementsIdObject.RadleyWalters;
                LafayettePool = stat.AchievementsIdObject.LafayettePool;
                Orlik = stat.AchievementsIdObject.Orlik;
                Oskin = stat.AchievementsIdObject.Oskin;
                Lehvaslaiho = stat.AchievementsIdObject.Lehvaslaiho;
                Nikolas = stat.AchievementsIdObject.Nikolas;
                Halonen = stat.AchievementsIdObject.Halonen;
                Burda = stat.AchievementsIdObject.Burda;
                Pascucci = stat.AchievementsIdObject.Pascucci;
                Dumitru = stat.AchievementsIdObject.Dumitru;
                TamadaYoshio = stat.AchievementsIdObject.TamadaYoshio;
                Billotte = stat.AchievementsIdObject.Billotte;
                BrunoPietro = stat.AchievementsIdObject.BrunoPietro;
                Tarczay = stat.AchievementsIdObject.Tarczay;
                Kolobanov = stat.AchievementsIdObject.Kolobanov;
                Fadin = stat.AchievementsIdObject.Fadin;
                HeroesOfRassenay = stat.AchievementsIdObject.HeroesOfRassenay;
                DeLanglade = stat.AchievementsIdObject.DeLanglade;

                #endregion

                #region [ IRowMedals]

                //Kay = stat.AchievementsIdObject.Major.Kay;
                //Carius = stat.AchievementsIdObject.Major.Carius;
                //Knispel = stat.AchievementsIdObject.Major.Knispel;
                //Poppel = stat.AchievementsIdObject.Major.Poppel;
                //Abrams = stat.AchievementsIdObject.Major.Abrams;
                //Leclerk = stat.AchievementsIdObject.Major.LeClerc;
                //Lavrinenko = stat.AchievementsIdObject.Major.Lavrinenko;
                //Ekins = stat.AchievementsIdObject.Major.Ekins;

                #endregion

                #region [ IRowSeries ]

                ReaperLongest = stat.AchievementsIdObject.Reaper;
                SharpshooterLongest = stat.AchievementsIdObject.SharpshooterLongest;
                MasterGunnerLongest = stat.AchievementsIdObject.MasterGunnerLongest;
                InvincibleLongest = stat.AchievementsIdObject.Invincible;
                SurvivorLongest = stat.AchievementsIdObject.Survivor;

                #endregion

                #region [ IRowSpecialAwards ]

                Kamikaze = stat.AchievementsIdObject.Kamikaze;
                Raider = stat.AchievementsIdObject.Raider;
                Bombardier = stat.AchievementsIdObject.Bombardier;
                Reaper = stat.AchievementsIdObject.Reaper;
                Sharpshooter = stat.AchievementsIdObject.SharpshooterLongest;
                Invincible = stat.AchievementsIdObject.Invincible;
                Survivor = stat.AchievementsIdObject.Survivor;
                MouseTrap = stat.AchievementsIdObject.MouseTrap;
                Hunter = stat.AchievementsIdObject.Hunter;
                Sinai = stat.AchievementsIdObject.Sinai;
                PattonValley = stat.AchievementsIdObject.PattonValley;
                Ranger = stat.AchievementsIdObject.Ranger;

                #endregion
            }
        }

        public PlayerStatisticClanViewModel Clan { get; set; }

        protected override void SetPreviousStatistic(PlayerStatisticViewModel prevPlayerStatistic)
        {
            base.SetPreviousStatistic(prevPlayerStatistic);

            OnPropertyChanged(PropBattlesCountDelta);
            //GR-->
            //Global Rating
            OnPropertyChanged(PropRating_IntegratedValue);
            OnPropertyChanged(PropRating_IntegratedPlace);
            //W/B-->
            //Victories/Battles
            OnPropertyChanged(PropRating_BattleAvgPerformanceValue);
            OnPropertyChanged(PropRating_BattleAvgPerformancePlace);
            //E/B-->
            //Average Experience per Battle
            OnPropertyChanged(PropRating_BattleAvgXpValue);
            OnPropertyChanged(PropRating_BattleAvgXpPlace);
            //WIN-->
            //Victories
            OnPropertyChanged(PropRating_BattleWinsValue);
            OnPropertyChanged(PropRating_BattleWinsPlace);
            //GPL-->
            //Battles Participated
            OnPropertyChanged(PropRating_BattlesValue);
            OnPropertyChanged(PropRating_BattlesPlace);
            //CPT-->
            //Capture Points
            OnPropertyChanged(PropRating_CapturedPointsValue);
            OnPropertyChanged(PropRating_CapturedPointsPlace);
            //DMG-->
            //Damage Caused
            OnPropertyChanged(PropRating_DamageDealtValue);
            OnPropertyChanged(PropRating_DamageDealtPlace);
            //DPT-->
            //Defense Points
            OnPropertyChanged(PropRating_DroppedPointsValue);
            OnPropertyChanged(PropRating_DroppedPointsPlace);
            //FRG-->
            //Targets Destroyed
            OnPropertyChanged(PropRating_FragsValue);
            OnPropertyChanged(PropRating_FragsPlace);
            //SPT-->
            //Targets Detected
            OnPropertyChanged(PropRating_SpottedValue);
            OnPropertyChanged(PropRating_SpottedPlace);
            //EXP-->
            //Total Experience
            OnPropertyChanged(PropRating_XpValue);
            OnPropertyChanged(PropRating_XpPlace);

            //GR-->
            //Global Rating
            OnPropertyChanged(PropRating_IntegratedValueDelta);
            OnPropertyChanged(PropRating_IntegratedPlaceDelta);
            //W/B-->
            //Victories/Battles
            OnPropertyChanged(PropRating_BattleAvgPerformanceValueDelta);
            OnPropertyChanged(PropRating_BattleAvgPerformancePlaceDelta);
            //E/B-->
            //Average Experience per Battle
            OnPropertyChanged(PropRating_BattleAvgXpValueDelta);
            OnPropertyChanged(PropRating_BattleAvgXpPlaceDelta);
            //WIN-->
            //Victories
            OnPropertyChanged(PropRating_BattleWinsValueDelta);
            OnPropertyChanged(PropRating_BattleWinsPlaceDelta);
            //GPL-->
            //Battles Participated
            OnPropertyChanged(PropRating_BattlesValueDelta);
            OnPropertyChanged(PropRating_BattlesPlaceDelta);
            //CPT-->
            //Capture Points
            OnPropertyChanged(PropRating_CapturedPointsValueDelta);
            OnPropertyChanged(PropRating_CapturedPointsPlaceDelta);
            //DMG-->
            //Damage Caused
            OnPropertyChanged(PropRating_DamageDealtValueDelta);
            OnPropertyChanged(PropRating_DamageDealtPlaceDelta);
            //DPT-->
            //Defense Points
            OnPropertyChanged(PropRating_DroppedPointsValueDelta);
            OnPropertyChanged(PropRating_DroppedPointsPlaceDelta);
            //FRG-->
            //Targets Destroyed
            OnPropertyChanged(PropRating_FragsValueDelta);
            OnPropertyChanged(PropRating_FragsPlaceDelta);
            //SPT-->
            //Targets Detected
            OnPropertyChanged(PropRating_SpottedValueDelta);
            OnPropertyChanged(PropRating_SpottedPlaceDelta);
            //EXP-->
            //Total Experience
            OnPropertyChanged(PropRating_XpValueDelta);
            OnPropertyChanged(PropRating_XpPlaceDelta);
        }

        #endregion
    }
}