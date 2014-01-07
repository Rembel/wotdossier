using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModel : PeriodStatisticViewModel<TankStatisticRowViewModel>, 
        ITankRowBattles, ITankRowDamage, ITankRowFrags, ITankRowPerformance, ITankRowRatings, ITankRowTime, ITankRowXP, ITankFilterable, 
        ITankRowBase, ITankRowBattleAwards, ITankRowEpic, ITankRowSpecialAwards, ITankRowMedals, ITankRowSeries
    {
        private DateTime _lastBattle;
        private IEnumerable<FragsJson> _tankFrags;
        private bool _isFavorite;

        #region Common

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }

        public int Type { get; set; }

        public int CountryId { get; set; }

        public int TankId { get; set; }

        public int TankUniqueId { get; set; }

        public bool IsPremium { get; set; }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        #endregion

        #region [ ITankRowBattles ]

        public int Draws
        {
            get { return BattlesCount - Wins - Losses; }
        }

        public double DrawsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Draws/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        public int SurvivedAndWon { get; set; }

        public double SurvivedAndWonPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedAndWon/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region [ ITankRowDamage ]

        public int DamagePerHit
        {
            get
            {
                if (Hits > 0)
                {
                    return DamageDealt/Hits;
                }
                return 0;
            }
        }

        public double DamageRatioDelta
        {
            get { return DamageRatio - PrevStatistic.DamageRatio; }
        }

        #endregion
       
        #region [ ITankRowFrags ]

        public int MaxFrags { get; set; }

        public double FragsPerBattle
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Frags/(double) BattlesCount;
                }
                return 0;
            }
        }

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        #endregion

        #region [ ITankRowPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ ITankRowRatings ]

        public int DamageRatingRev1
        {
            get
            {
                if (DamageTaken > 0)
                {
                    return (int) (DamageDealt/(double) DamageTaken*100);
                }
                return 0;
            }
        }

        public int MarkOfMastery { get; set; }

        #endregion

        #region [ ITankRowTime ]
        public DateTime LastBattle
        {
            get { return _lastBattle; }
            set { _lastBattle = value; }
        }

        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        public int OriginalXP { get; set; }
        public int DamageAssistedTrack { get; set; }
        public int DamageAssistedRadio { get; set; }
        public double Mileage { get; set; }
        public int ShotsReceived { get; set; }
        public int NoDamageShotsReceived { get; set; }
        public int PiercedReceived { get; set; }
        public int HeHitsReceived { get; set; }
        public int HeHits { get; set; }
        public int Pierced { get; set; }
        public int XpBefore88 { get; set; }
        public int BattlesCountBefore88 { get; set; }
        public int BattlesCount88 { get; set; }
        public int MaxDamage { get; set; }

        public int DamageAssisted
        {
            get
            {
                return DamageAssistedTrack + DamageAssistedRadio;
            }
        }

        public double AvgDamageAssistedTrack
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedTrack / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssistedRadio
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedRadio / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssisted
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssisted / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgOriginalXP
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return OriginalXP / (double)BattlesCount88;
                }
                return 0;
            }
        }

        protected TankStatisticRowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public TankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list) : base(Utils.UnixDateToDateTime(tank.Common.updated), list.Select(x => new TankStatisticRowViewModel(x)).ToList())
        {
            Tier = tank.Common.tier;
            Type = tank.Common.type;
            Tank = tank.Common.tanktitle;
            Icon = tank.Description.Icon;
            CountryId = tank.Common.countryid;
            TankId = tank.Common.tankid;
            TankUniqueId = tank.UniqueId();
            TankFrags = tank.Frags;
            OriginalXP = tank.A15x15.originalXP;
            DamageAssistedTrack = tank.A15x15.damageAssistedTrack;
            DamageAssistedRadio = tank.A15x15.damageAssistedRadio;
            Mileage = tank.Common.mileage / 1000;
            ShotsReceived = tank.A15x15.shotsReceived;
            NoDamageShotsReceived = tank.A15x15.noDamageShotsReceived;
            PiercedReceived = tank.A15x15.piercedReceived;
            HeHitsReceived = tank.A15x15.heHitsReceived;
            HeHits = tank.A15x15.he_hits;
            Pierced = tank.A15x15.pierced;
            XpBefore88 = tank.A15x15.xpBefore8_8;
            BattlesCountBefore88 = tank.A15x15.battlesCountBefore8_8;
            BattlesCount88 = tank.A15x15.battlesCount - BattlesCountBefore88;
            IsPremium = tank.Common.premium == 1;

            #region [ ITankRowBattleAwards ]
            BattleHero = tank.Achievements.battleHeroes;
            Warrior = tank.Achievements.warrior;
            Invader = tank.Achievements.invader;
            Sniper = tank.Achievements.sniper;
            Defender = tank.Achievements.defender;
            SteelWall = tank.Achievements.steelwall;
            Confederate = tank.Achievements.supporter;
            Scout = tank.Achievements.scout;
            PatrolDuty = tank.Achievements.evileye;
            BrothersInArms = tank.Achievements.medalBrothersInArms;
            CrucialContribution = tank.Achievements.medalCrucialContribution;
            CoolHeaded = tank.Achievements.ironMan;
            LuckyDevil = tank.Achievements.luckyDevil;
            Spartan = tank.Achievements.sturdy;
            Ranger = tank.Achievements.huntsman;
            #endregion

            #region [ ITankRowBattles ]
            BattlesCount = tank.A15x15.battlesCount;
            Wins = tank.A15x15.wins;
            Losses = tank.A15x15.losses;
            SurvivedBattles = tank.A15x15.survivedBattles;
            SurvivedAndWon = tank.A15x15.winAndSurvived;
            #endregion

            #region [ ITankRowDamage ]
            DamageDealt = tank.A15x15.damageDealt;
            DamageTaken = tank.A15x15.damageReceived;
            MaxDamage = tank.A15x15.maxDamage;
            #endregion

            #region [ ITankRowEpic ]
            Boelter = tank.Achievements.medalWittmann;
            RadleyWalters = tank.Achievements.medalRadleyWalters;
            LafayettePool = tank.Achievements.medalLafayettePool;
            Orlik = tank.Achievements.medalOrlik;
            Oskin = tank.Achievements.medalOskin;
            Lehvaslaiho = tank.Achievements.medalLehvaslaiho;
            Nikolas = tank.Achievements.medalNikolas;
            Halonen = tank.Achievements.medalHalonen;
            Burda = tank.Achievements.medalBurda;
            Pascucci = tank.Achievements.medalPascucci;
            Dumitru = tank.Achievements.medalDumitru;
            TamadaYoshio = tank.Achievements.medalTamadaYoshio;
            Billotte = tank.Achievements.medalBillotte;
            BrunoPietro = tank.Achievements.medalBrunoPietro;
            Tarczay = tank.Achievements.medalTarczay;
            Kolobanov = tank.Achievements.medalKolobanov;
            Fadin = tank.Achievements.medalFadin;
            HeroesOfRassenay = tank.Achievements.heroesOfRassenay;
            DeLanglade = tank.Achievements.medalDeLanglade;
            #endregion

            #region [ ITankRowFrags ]
            Frags = tank.A15x15.frags;
            MaxFrags = tank.A15x15.maxFrags;
            Tier8Frags = tank.A15x15.frags8p;
            BeastFrags = tank.Achievements.fragsBeast;
            SinaiFrags = tank.Achievements.fragsSinai;
            #endregion

            #region [ ITankRowMasterTanker ]

            #endregion

            #region [ ITankRowMedals]
            Kay = tank.Achievements.medalKay;
            Carius = tank.Achievements.medalCarius;
            Knispel = tank.Achievements.medalKnispel;
            Poppel = tank.Achievements.medalPoppel;
            Abrams = tank.Achievements.medalAbrams;
            Leclerk = tank.Achievements.medalLeClerc;
            Lavrinenko = tank.Achievements.medalLavrinenko;
            Ekins = tank.Achievements.medalEkins;
            #endregion

            #region [ ITankRowPerformance ]
            Shots = tank.A15x15.shots;
            Hits = tank.A15x15.hits;
            if (Shots > 0)
            {
                HitsPercents = Hits/(double) Shots*100.0;
            }
            CapturePoints = tank.A15x15.capturePoints;
            DroppedCapturePoints = tank.A15x15.droppedCapturePoints;
            Spotted = tank.A15x15.spotted;
            #endregion

            #region [ ITankRowSeries ]
            ReaperLongest = tank.Achievements.maxKillingSeries;
            ReaperProgress = tank.Achievements.killingSeries;
            SharpshooterLongest = tank.Achievements.maxSniperSeries;
            SharpshooterProgress = tank.Achievements.sniperSeries;
            MasterGunnerLongest = tank.Achievements.maxPiercingSeries;
            MasterGunnerProgress = tank.Achievements.piercingSeries;
            InvincibleLongest = tank.Achievements.maxInvincibleSeries;
            InvincibleProgress = tank.Achievements.invincibleSeries;
            SurvivorLongest = tank.Achievements.maxDiehardSeries;
            SurvivorProgress = tank.Achievements.diehardSeries;
            #endregion

            #region [ ITankRowSpecialAwards ]
            Kamikaze = tank.Achievements.kamikaze;
            Raider = tank.Achievements.raider;
            Bombardier = tank.Achievements.bombardier;
            Reaper = tank.Achievements.maxKillingSeries;
            Sharpshooter = tank.Achievements.maxSniperSeries;
            Invincible = tank.Achievements.maxInvincibleSeries;
            Survivor = tank.Achievements.maxDiehardSeries;
            MouseTrap = tank.Achievements.mousebane;
            Hunter = tank.Achievements.beasthunter;
            Sinai = tank.Achievements.sinai;
            PattonValley = tank.Achievements.pattonValley;
            #endregion

            #region [ ITankRowTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (tank.A15x15.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime/tank.A15x15.battlesCount);
            }
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.A15x15.xp;
            MaxXp = tank.A15x15.maxXP;
            #endregion

            #region [ ITankRowRatings ]
            MarkOfMastery = tank.Achievements.markOfMastery;

            #endregion

            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
        }

        public IEnumerable<FragsJson> TankFrags
        {
            get { return _tankFrags; }
            set { _tankFrags = value; }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Tank;
        }
    }
}