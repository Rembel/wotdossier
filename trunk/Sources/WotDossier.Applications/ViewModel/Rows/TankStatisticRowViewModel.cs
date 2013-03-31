using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModel : PeriodStatisticViewModel<TankStatisticRowViewModel>, ITankRowBattleAwards, ITankRowBattles, ITankRowDamage, ITankRowEpic, 
                           ITankRowFrags, ITankRowMasterTanker, ITankRowMedals, ITankRowPerformance, ITankRowRatings, ITankRowSeries, 
                           ITankRowSpecialAwards, ITankRowTime, ITankRowXP
    {
        #region Common

        public TankContour Icon { get; set; }

        public string Tank { get; set; }

        public int TankType { get; set; }

        public int CountryId { get; set; }

        #endregion

        #region [ ITankRowBattleAwards ]

        public int BattleHero { get; set; }

        public int TopGun { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int CoolHeaded { get; set; }

        public int LuckyDevil { get; set; }

        public int Spartan { get; set; }

        #endregion

        #region [ ITankRowBattles ]

        public int Draws { get; set; }

        public double DrawsPercent { get; set; }

        public int SurvivedAndWon { get; set; }

        public double SurvivedAndWonPercent { get; set; }

        #endregion

        #region [ ITankRowDamage ]

        public int DamageTaken { get; set; }

        public double DamageRatio { get; set; }

        public int AverageDamageDealt { get; set; }

        public int DamagePerHit { get; set; }

        #endregion

        #region [ ITankRowEpic ]

        public int Boelter { get; set; }

        public int RadleyWalters { get; set; }

        public int LafayettePool { get; set; }

        public int Orlik { get; set; }

        public int Oskin { get; set; }

        public int Lehvaslaiho { get; set; }

        public int Nikolas { get; set; }

        public int Halonen { get; set; }

        public int Burda { get; set; }

        public int Pascucci { get; set; }

        public int Dumitru { get; set; }

        public int TamadaYoshio { get; set; }

        public int Billotte { get; set; }

        public int BrunoPietro { get; set; }

        public int Tarczay { get; set; }

        public int Kolobanov { get; set; }

        public int Fadin { get; set; }

        public int HeroesOfRaseiniai { get; set; }

        public int DeLanglade { get; set; }

        #endregion

        #region [ ITankRowFrags ]

        public int MaxFrags { get; set; }

        public double FragsPerBattle { get; set; }

        public double KillDeathRatio { get; set; }

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        #endregion

        #region [ ITankRowMasterTanker ]

        public bool IsPremium { get; set; }

        #endregion

        #region [ ITankRowMedals]

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

        #endregion

        #region [ ITankRowPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ ITankRowRatings ]

        public double Winrate { get; set; }

        public int AverageDamage { get; set; }

        public int DamageRatingRev1 { get; set; }

        public int MarkOfMastery { get; set; }

        #endregion

        #region [ ITankRowSeries ]

        public int ReaperLongest { get; set; }

        public int ReaperProgress { get; set; }

        public int SharpshooterLongest { get; set; }

        public int SharpshooterProgress { get; set; }

        public int MasterGunnerLongest { get; set; }

        public int MasterGunnerProgress { get; set; }

        public int InvincibleLongest { get; set; }

        public int InvincibleProgress { get; set; }

        public int SurvivorLongest { get; set; }

        public int SurvivorProgress { get; set; }

        #endregion

        #region [ ITankRowSpecialAwards ]

        public int Kamikaze { get; set; }

        public int Raider { get; set; }

        public int Bombardier { get; set; }

        public int Reaper { get; set; }

        public int Sharpshooter { get; set; }

        public int Invincible { get; set; }

        public int Survivor { get; set; }

        public int MouseTrap { get; set; }

        public int Hunter { get; set; }

        public int Sinai { get; set; }

        public int PattonValley { get; set; }

        public int Ranger { get; set; }

        #endregion

        #region [ ITankRowTime ]
        public DateTime LastBattle { get; set; }
        public TimeSpan PlayTime { get; set; }
        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        public TankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list) : base(Utils.UnixDateToDateTime(tank.Common.updated), list.Select(x => new TankStatisticRowViewModel(x)))
        {
            Tier = tank.Common.tier;
            TankType = tank.Common.type;
            Tank = tank.Common.tanktitle;
            Icon = tank.TankContour;
            CountryId = tank.Common.countryid;

            #region [ ITankRowBattleAwards ]
            BattleHero = tank.Battle.battleHeroes;
            TopGun = tank.Battle.warrior;
            Invader = tank.Battle.invader;
            Sniper = tank.Battle.sniper;
            Defender = tank.Battle.sniper;
            SteelWall = tank.Battle.steelwall;
            Confederate = tank.Battle.supporter;
            Scout = tank.Battle.scout;
            PatrolDuty = tank.Battle.evileye;
            BrothersInArms = tank.Epic.BrothersInArms;
            CrucialContribution = tank.Epic.CrucialContribution;
            CoolHeaded = tank.Special.alaric;
            LuckyDevil = tank.Special.luckyDevil;
            Spartan = tank.Special.sturdy;
            #endregion

            #region [ ITankRowBattles ]
            BattlesCount = tank.Tankdata.battlesCount;
            Wins = tank.Tankdata.wins;
            WinsPercent = Wins / (double)BattlesCount * 100.0;
            Losses = tank.Tankdata.losses;
            LossesPercent = Losses / (double)BattlesCount * 100.0;
            Draws = BattlesCount - Wins - Losses;
            DrawsPercent = Draws / (double)BattlesCount * 100.0;
            SurvivedBattles = tank.Tankdata.survivedBattles;
            SurvivedBattlesPercent = SurvivedBattles / (double)BattlesCount * 100.0;
            SurvivedAndWon = tank.Tankdata.winAndSurvived;
            SurvivedAndWonPercent = SurvivedAndWon / (double)BattlesCount * 100.0;
            #endregion

            #region [ ITankRowDamage ]
            DamageDealt = tank.Tankdata.damageDealt;
            DamageTaken = tank.Tankdata.damageReceived;
            if (DamageTaken > 0)
            {
                DamageRatio = DamageDealt/(double) DamageTaken;
            }
            if (tank.Tankdata.battlesCount > 0)
            {
                AverageDamageDealt = DamageDealt/tank.Tankdata.battlesCount;
            }
            if (tank.Tankdata.hits > 0)
            {
                DamagePerHit = DamageDealt/tank.Tankdata.hits;
            }
            #endregion

            #region [ ITankRowEpic ]
            Boelter = tank.Epic.Boelter;
            RadleyWalters = tank.Epic.RadleyWalters;
            LafayettePool = tank.Epic.LafayettePool;
            Orlik = tank.Epic.Orlik;
            Oskin = tank.Epic.Oskin;
            Lehvaslaiho = tank.Epic.Lehvaslaiho;
            Nikolas = tank.Epic.Nikolas;
            Halonen = tank.Epic.Halonen;
            Burda = tank.Epic.Burda;
            Pascucci = tank.Epic.Pascucci;
            Dumitru = tank.Epic.Dumitru;
            TamadaYoshio = tank.Epic.TamadaYoshio;
            Billotte = tank.Epic.Billotte;
            BrunoPietro = tank.Epic.BrunoPietro;
            Tarczay = tank.Epic.Tarczay;
            Kolobanov = tank.Epic.Kolobanov;
            Fadin = tank.Epic.Fadin;
            HeroesOfRaseiniai = tank.Special.heroesOfRassenay;
            DeLanglade = tank.Epic.DeLanglade;
            #endregion

            #region [ ITankRowFrags ]
            BattlesCount = tank.Tankdata.battlesCount;
            Frags = tank.Tankdata.frags;
            MaxFrags = tank.Tankdata.maxFrags;
            FragsPerBattle = Frags / (double)BattlesCount;
            KillDeathRatio = Frags / (double)(BattlesCount - tank.Tankdata.survivedBattles);
            Tier8Frags = tank.Tankdata.frags8p;
            BeastFrags = tank.Tankdata.fragsBeast;
            SinaiFrags = tank.Battle.fragsSinai;
            #endregion

            #region [ ITankRowMasterTanker ]

            #endregion

            #region [ ITankRowMedals]
            Kay = tank.Major.Kay;
            Carius = tank.Major.Carius;
            Knispel = tank.Major.Knispel;
            Poppel = tank.Major.Poppel;
            Abrams = tank.Major.Abrams;
            Leclerk = tank.Major.LeClerc;
            Lavrinenko = tank.Major.Lavrinenko;
            Ekins = tank.Major.Ekins;
            #endregion

            #region [ ITankRowPerformance ]
            Shots = tank.Tankdata.shots;
            Hits = tank.Tankdata.hits;
            HitsPercents = Hits / (double)Shots * 100.0;
            CapturePoints = tank.Tankdata.capturePoints;
            DroppedCapturePoints = tank.Tankdata.droppedCapturePoints;
            Spotted = tank.Tankdata.spotted;
            #endregion

            if (tank.Tankdata.battlesCount > 0)
            {
                #region [ ITankRowRatings ]

                BattlesCount = tank.Tankdata.battlesCount;
                Winrate = tank.Tankdata.wins/(double) tank.Tankdata.battlesCount*100.0;

                AverageDamage = tank.Tankdata.damageDealt/tank.Tankdata.battlesCount;

                KillDeathRatio = tank.Tankdata.frags/(double) (BattlesCount - tank.Tankdata.survivedBattles);
                double avgFrags = tank.Tankdata.frags/(double) BattlesCount;
                double avgSpot = tank.Tankdata.spotted/(double) BattlesCount;
                double avgCap = tank.Tankdata.capturePoints/(double) BattlesCount;
                double avgDef = tank.Tankdata.droppedCapturePoints/(double) BattlesCount;
                double avgXP = tank.Tankdata.xp/(double) BattlesCount;

                double value = RatingHelper.CalcER(AverageDamage, Tier, avgFrags, avgSpot, avgCap, avgDef);

                EffRating = (int) value;
                value = RatingHelper.CalcWN6(AverageDamage, Tier, avgFrags, avgSpot, avgDef, Winrate);
                WN6Rating = (int) value;
                DamageRatingRev1 = (int) (tank.Tankdata.damageDealt/(double) tank.Tankdata.damageReceived*100);
                value = RatingHelper.CalcKievArmorRating(BattlesCount, avgXP, AverageDamage, Winrate/100, avgFrags,
                                                         avgSpot, avgCap, avgDef);
                KievArmorRating = (int) value;
                MarkOfMastery = tank.Special.markOfMastery;

                #endregion
            }
            #region [ ITankRowSeries ]
            ReaperLongest = tank.Series.maxKillingSeries;
            ReaperProgress = tank.Series.killingSeries;
            SharpshooterLongest = tank.Series.maxSniperSeries;
            SharpshooterProgress = tank.Series.sniperSeries;
            MasterGunnerLongest = tank.Series.maxPiercingSeries;
            MasterGunnerProgress = tank.Series.piercingSeries;
            InvincibleLongest = tank.Series.maxInvincibleSeries;
            InvincibleProgress = tank.Series.invincibleSeries;
            SurvivorLongest = tank.Series.maxDiehardSeries;
            SurvivorProgress = tank.Series.diehardSeries;
            #endregion

            #region [ ITankRowSpecialAwards ]
            Kamikaze = tank.Special.kamikaze;
            Raider = tank.Special.raider;
            Bombardier = tank.Special.bombardier;
            Reaper = tank.Series.maxKillingSeries;
            Sharpshooter = tank.Series.maxSniperSeries;
            Invincible = tank.Series.maxInvincibleSeries;
            Survivor = tank.Series.maxDiehardSeries;
            MouseTrap = tank.Special.mousebane;
            Hunter = tank.Special.beasthunter;
            Sinai = tank.Special.sinai;
            PattonValley = tank.Special.pattonValley;
            Ranger = tank.Special.lumberjack;
            #endregion

            #region [ ITankRowTime ]
            LastBattle = Utils.UnixDateToDateTime(tank.Tankdata.lastBattleTime);
            PlayTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime);
            if (tank.Tankdata.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime/tank.Tankdata.battlesCount);
            }
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.Tankdata.xp;
            MaxXp = tank.Tankdata.maxXP;
            if (tank.Tankdata.battlesCount > 0)
            {
                BattleAvgXp = Xp/tank.Tankdata.battlesCount;
            }
            #endregion

            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
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