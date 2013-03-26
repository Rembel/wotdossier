using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRow : TankRowBase, ITankRowBattleAwards, ITankRowBattles, ITankRowDamage, ITankRowEpic, 
                           ITankRowFrags, ITankRowMasterTanker, ITankRowMedals, ITankRowPerformance, ITankRowRatings, ITankRowSeries, 
                           ITankRowSpecialAwards, ITankRowTime, ITankRowXP
    {

        #region [ ITankRowBattleAwards ]
        private int _battleHero;
        private int _topGun;
        private int _invader;
        private int _sniper;
        private int _defender;
        private int _steelWall;
        private int _confederate;
        private int _scout;
        private int _patrolDuty;
        private int _brothersInArms;
        private int _crucialContribution;
        private int _coolHeaded;
        private int _luckyDevil;
        private int _spartan;

        public int BattleHero
        {
            get { return _battleHero; }
            set { _battleHero = value; }
        }

        public int TopGun
        {
            get { return _topGun; }
            set { _topGun = value; }
        }

        public int Invader
        {
            get { return _invader; }
            set { _invader = value; }
        }

        public int Sniper
        {
            get { return _sniper; }
            set { _sniper = value; }
        }

        public int Defender
        {
            get { return _defender; }
            set { _defender = value; }
        }

        public int SteelWall
        {
            get { return _steelWall; }
            set { _steelWall = value; }
        }

        public int Confederate
        {
            get { return _confederate; }
            set { _confederate = value; }
        }

        public int Scout
        {
            get { return _scout; }
            set { _scout = value; }
        }

        public int PatrolDuty
        {
            get { return _patrolDuty; }
            set { _patrolDuty = value; }
        }

        public int BrothersInArms
        {
            get { return _brothersInArms; }
            set { _brothersInArms = value; }
        }

        public int CrucialContribution
        {
            get { return _crucialContribution; }
            set { _crucialContribution = value; }
        }

        public int CoolHeaded
        {
            get { return _coolHeaded; }
            set { _coolHeaded = value; }
        }

        public int LuckyDevil
        {
            get { return _luckyDevil; }
            set { _luckyDevil = value; }
        }

        public int Spartan
        {
            get { return _spartan; }
            set { _spartan = value; }
        }
        #endregion

        #region [ ITankRowBattles ]
        private int _battles;
        private int _won;
        private double _wonPercent;
        private int _lost;
        private double _lostPercent;
        private int _draws;
        private double _drawsPercent;
        private int _survived;
        private double _survivedPercent;
        private int _survivedAndWon;
        private double _survivedAndWonPercent;

        public int Battles
        {
            get { return _battles; }
            set { _battles = value; }
        }

        public int Won
        {
            get { return _won; }
            set { _won = value; }
        }

        public double WonPercent
        {
            get { return _wonPercent; }
            set { _wonPercent = value; }
        }

        public int Lost
        {
            get { return _lost; }
            set { _lost = value; }
        }

        public double LostPercent
        {
            get { return _lostPercent; }
            set { _lostPercent = value; }
        }

        public int Draws
        {
            get { return _draws; }
            set { _draws = value; }
        }

        public double DrawsPercent
        {
            get { return _drawsPercent; }
            set { _drawsPercent = value; }
        }

        public int Survived
        {
            get { return _survived; }
            set { _survived = value; }
        }

        public double SurvivedPercent
        {
            get { return _survivedPercent; }
            set { _survivedPercent = value; }
        }

        public int SurvivedAndWon
        {
            get { return _survivedAndWon; }
            set { _survivedAndWon = value; }
        }

        public double SurvivedAndWonPercent
        {
            get { return _survivedAndWonPercent; }
            set { _survivedAndWonPercent = value; }
        }
        #endregion

        #region [ ITankRowDamage ]
        public int DamageDealt
        {
            get { return _damageDealt; }
            set { _damageDealt = value; }
        }

        public int DamageTaken
        {
            get { return _damageTaken; }
            set { _damageTaken = value; }
        }

        public double DamageRatio
        {
            get { return _damageRatio; }
            set { _damageRatio = value; }
        }

        public int AverageDamageDealt
        {
            get { return _averageDamageDealt; }
            set { _averageDamageDealt = value; }
        }

        public int DamagePerHit
        {
            get { return _damagePerHit; }
            set { _damagePerHit = value; }
        }

        private int _damageDealt;
        private int _damageTaken;
        private double _damageRatio;
        private int _averageDamageDealt;
        private int _damagePerHit;
        #endregion

        #region [ ITankRowEpic ]
        private int _boelter;
        private int _radleyWalters;
        private int _lafayettePool;
        private int _orlik;
        private int _oskin;
        private int _lehvaslaiho;
        private int _nikolas;
        private int _halonen;
        private int _burda;
        private int _pascucci;
        private int _dumitru;
        private int _tamadaYoshio;
        private int _billotte;
        private int _brunoPietro;
        private int _tarczay;
        private int _kolobanov;
        private int _fadin;
        private int _heroesOfRaseiniai;
        private int _deLanglade;

        public int Boelter
        {
            get { return _boelter; }
            set { _boelter = value; }
        }

        public int RadleyWalters
        {
            get { return _radleyWalters; }
            set { _radleyWalters = value; }
        }

        public int LafayettePool
        {
            get { return _lafayettePool; }
            set { _lafayettePool = value; }
        }

        public int Orlik
        {
            get { return _orlik; }
            set { _orlik = value; }
        }

        public int Oskin
        {
            get { return _oskin; }
            set { _oskin = value; }
        }

        public int Lehvaslaiho
        {
            get { return _lehvaslaiho; }
            set { _lehvaslaiho = value; }
        }

        public int Nikolas
        {
            get { return _nikolas; }
            set { _nikolas = value; }
        }

        public int Halonen
        {
            get { return _halonen; }
            set { _halonen = value; }
        }

        public int Burda
        {
            get { return _burda; }
            set { _burda = value; }
        }

        public int Pascucci
        {
            get { return _pascucci; }
            set { _pascucci = value; }
        }

        public int Dumitru
        {
            get { return _dumitru; }
            set { _dumitru = value; }
        }

        public int TamadaYoshio
        {
            get { return _tamadaYoshio; }
            set { _tamadaYoshio = value; }
        }

        public int Billotte
        {
            get { return _billotte; }
            set { _billotte = value; }
        }

        public int BrunoPietro
        {
            get { return _brunoPietro; }
            set { _brunoPietro = value; }
        }

        public int Tarczay
        {
            get { return _tarczay; }
            set { _tarczay = value; }
        }

        public int Kolobanov
        {
            get { return _kolobanov; }
            set { _kolobanov = value; }
        }

        public int Fadin
        {
            get { return _fadin; }
            set { _fadin = value; }
        }

        public int HeroesOfRaseiniai
        {
            get { return _heroesOfRaseiniai; }
            set { _heroesOfRaseiniai = value; }
        }

        public int DeLanglade
        {
            get { return _deLanglade; }
            set { _deLanglade = value; }
        }
        #endregion

        #region [ ITankRowFrags ]
        private int _frags;
        private int _maxFrags;
        private double _fragsPerBattle;
        private double _killDeathRatio;
        private int _tier8Frags;
        private int _beastFrags;
        private int _sinaiFrags;

        public int Frags
        {
            get { return _frags; }
            set { _frags = value; }
        }

        public int MaxFrags
        {
            get { return _maxFrags; }
            set { _maxFrags = value; }
        }

        public double FragsPerBattle
        {
            get { return _fragsPerBattle; }
            set { _fragsPerBattle = value; }
        }

        public double KillDeathRatio
        {
            get { return _killDeathRatio; }
            set { _killDeathRatio = value; }
        }

        public int Tier8Frags
        {
            get { return _tier8Frags; }
            set { _tier8Frags = value; }
        }

        public int BeastFrags
        {
            get { return _beastFrags; }
            set { _beastFrags = value; }
        }

        public int SinaiFrags
        {
            get { return _sinaiFrags; }
            set { _sinaiFrags = value; }
        }
        #endregion

        #region [ ITankRowMasterTanker ]
        private bool _isPremium;

        public bool IsPremium
        {
            get { return _isPremium; }
            set { _isPremium = value; }
        }
        #endregion

        #region [ ITankRowMedals]
        private int _kay;
        private int _carius;
        private int _knispel;
        private int _poppel;
        private int _abrams;
        private int _leclerk;
        private int _lavrinenko;
        private int _ekins;

        public int Kay
        {
            get { return _kay; }
            set { _kay = value; }
        }

        public int Carius
        {
            get { return _carius; }
            set { _carius = value; }
        }

        public int Knispel
        {
            get { return _knispel; }
            set { _knispel = value; }
        }

        public int Poppel
        {
            get { return _poppel; }
            set { _poppel = value; }
        }

        public int Abrams
        {
            get { return _abrams; }
            set { _abrams = value; }
        }

        public int Leclerk
        {
            get { return _leclerk; }
            set { _leclerk = value; }
        }

        public int Lavrinenko
        {
            get { return _lavrinenko; }
            set { _lavrinenko = value; }
        }

        public int Ekins
        {
            get { return _ekins; }
            set { _ekins = value; }
        }
        #endregion

        #region [ ITankRowPerformance ]
        private int _shots;
        private int _hits;
        private double _hitRatio;
        private int _capturePoints;
        private int _defencePoints;
        private int _tanksSpotted;

        public int Shots
        {
            get { return _shots; }
            set { _shots = value; }
        }

        public int Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }

        public double HitRatio
        {
            get { return _hitRatio; }
            set { _hitRatio = value; }
        }

        public int CapturePoints
        {
            get { return _capturePoints; }
            set { _capturePoints = value; }
        }

        public int DefencePoints
        {
            get { return _defencePoints; }
            set { _defencePoints = value; }
        }

        public int TanksSpotted
        {
            get { return _tanksSpotted; }
            set { _tanksSpotted = value; }
        }
        #endregion

        #region [ ITankRowRatings ]
        private double _winrate;
        private int _averageDamage;
        private int _newEffRating;
        private int _wn6;
        private int _damageRatingRev1;
        private int _kievArmorRating;
        private int _markOfMastery;

        public double Winrate
        {
            get { return _winrate; }
            set { _winrate = value; }
        }

        public int AverageDamage
        {
            get { return _averageDamage; }
            set { _averageDamage = value; }
        }

        public int NewEffRating
        {
            get { return _newEffRating; }
            set { _newEffRating = value; }
        }

        public int WN6
        {
            get { return _wn6; }
            set { _wn6 = value; }
        }

        public int DamageRatingRev1
        {
            get { return _damageRatingRev1; }
            set { _damageRatingRev1 = value; }
        }

        public int KievArmorRating
        {
            get { return _kievArmorRating; }
            set { _kievArmorRating = value; }
        }

        public int MarkOfMastery
        {
            get { return _markOfMastery; }
            set { _markOfMastery = value; }
        }
        #endregion

        #region [ ITankRowSeries ]
        private int _reaperLongest;
        private int _reaperProgress;
        private int _sharpshooterLongest;
        private int _sharpshooterProgress;
        private int _masterGunnerLongest;
        private int _masterGunnerProgress;
        private int _invincibleLongest;
        private int _invincibleProgress;
        private int _survivorLongest;
        private int _survivorProgress;

        public int ReaperLongest
        {
            get { return _reaperLongest; }
            set { _reaperLongest = value; }
        }

        public int ReaperProgress
        {
            get { return _reaperProgress; }
            set { _reaperProgress = value; }
        }

        public int SharpshooterLongest
        {
            get { return _sharpshooterLongest; }
            set { _sharpshooterLongest = value; }
        }

        public int SharpshooterProgress
        {
            get { return _sharpshooterProgress; }
            set { _sharpshooterProgress = value; }
        }

        public int MasterGunnerLongest
        {
            get { return _masterGunnerLongest; }
            set { _masterGunnerLongest = value; }
        }

        public int MasterGunnerProgress
        {
            get { return _masterGunnerProgress; }
            set { _masterGunnerProgress = value; }
        }

        public int InvincibleLongest
        {
            get { return _invincibleLongest; }
            set { _invincibleLongest = value; }
        }

        public int InvincibleProgress
        {
            get { return _invincibleProgress; }
            set { _invincibleProgress = value; }
        }

        public int SurvivorLongest
        {
            get { return _survivorLongest; }
            set { _survivorLongest = value; }
        }

        public int SurvivorProgress
        {
            get { return _survivorProgress; }
            set { _survivorProgress = value; }
        }
        #endregion

        #region [ ITankRowSpecialAwards ]
        private int _kamikaze;
        private int _raider;
        private int _bombardier;
        private int _reaper;
        private int _sharpshooter;
        private int _invincible;
        private int _survivor;
        private int _mouseTrap;
        private int _hunter;
        private int _sinai;
        private int _pattonValley;
        private int _ranger;

        public int Kamikaze
        {
            get { return _kamikaze; }
            set { _kamikaze = value; }
        }

        public int Raider
        {
            get { return _raider; }
            set { _raider = value; }
        }

        public int Bombardier
        {
            get { return _bombardier; }
            set { _bombardier = value; }
        }

        public int Reaper
        {
            get { return _reaper; }
            set { _reaper = value; }
        }

        public int Sharpshooter
        {
            get { return _sharpshooter; }
            set { _sharpshooter = value; }
        }

        public int Invincible
        {
            get { return _invincible; }
            set { _invincible = value; }
        }

        public int Survivor
        {
            get { return _survivor; }
            set { _survivor = value; }
        }

        public int MouseTrap
        {
            get { return _mouseTrap; }
            set { _mouseTrap = value; }
        }

        public int Hunter
        {
            get { return _hunter; }
            set { _hunter = value; }
        }

        public int Sinai
        {
            get { return _sinai; }
            set { _sinai = value; }
        }

        public int PattonValley
        {
            get { return _pattonValley; }
            set { _pattonValley = value; }
        }

        public int Ranger
        {
            get { return _ranger; }
            set { _ranger = value; }
        }
        #endregion

        #region [ ITankRowTime ]
        public DateTime LastBattle { get; set; }
        public TimeSpan PlayTime { get; set; }
        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        #region [ ITankRowXP ]
        private int _totalXP;
        private int _maximumXp;
        private int _averageXp;

        public int TotalXP
        {
            get { return _totalXP; }
            set { _totalXP = value; }
        }

        public int MaximumXP
        {
            get { return _maximumXp; }
            set { _maximumXp = value; }
        }

        public int AverageXP
        {
            get { return _averageXp; }
            set { _averageXp = value; }
        }
        #endregion

        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public TankRow(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankRow(TankJson tank, IEnumerable<TankJson> list)
        {
            Tier = tank.Common.tier;
            TankType = tank.Common.type;
            Tank = tank.Name;
            Icon = tank.TankContour;
            CountryId = tank.Common.countryid;

            #region [ ITankRowBattleAwards ]
            _battleHero = tank.Battle.battleHeroes;
            _topGun = tank.Battle.warrior;
            _invader = tank.Battle.invader;
            _sniper = tank.Battle.sniper;
            _defender = tank.Battle.sniper;
            _steelWall = tank.Battle.steelwall;
            _confederate = tank.Battle.supporter;
            _scout = tank.Battle.scout;
            _patrolDuty = tank.Battle.evileye;
            _brothersInArms = tank.Epic.BrothersInArms;
            _crucialContribution = tank.Epic.CrucialContribution;
            _coolHeaded = tank.Special.alaric;
            _luckyDevil = tank.Special.luckyDevil;
            _spartan = tank.Special.sturdy;
            #endregion

            #region [ ITankRowBattles ]
            _battles = tank.Tankdata.battlesCount;
            _won = tank.Tankdata.wins;
            _wonPercent = _won / (double)_battles * 100.0;
            _lost = tank.Tankdata.losses;
            _lostPercent = _lost / (double)_battles * 100.0;
            _draws = _battles - _won - _lost;
            _drawsPercent = _draws / (double)_battles * 100.0;
            _survived = tank.Tankdata.survivedBattles;
            _survivedPercent = _survived / (double)_battles * 100.0;
            _survivedAndWon = tank.Tankdata.winAndSurvived;
            _survivedAndWonPercent = _survivedAndWon / (double)_battles * 100.0;
            #endregion

            #region [ ITankRowDamage ]
            _damageDealt = tank.Tankdata.damageDealt;
            _damageTaken = tank.Tankdata.damageReceived;
            _damageRatio = DamageDealt / (double)DamageTaken;
            _averageDamageDealt = DamageDealt / tank.Tankdata.battlesCount;
            _damagePerHit = DamageDealt / tank.Tankdata.hits;
            #endregion

            #region [ ITankRowEpic ]
            _boelter = tank.Epic.Boelter;
            _radleyWalters = tank.Epic.RadleyWalters;
            _lafayettePool = tank.Epic.LafayettePool;
            _orlik = tank.Epic.Orlik;
            _oskin = tank.Epic.Oskin;
            _lehvaslaiho = tank.Epic.Lehvaslaiho;
            _nikolas = tank.Epic.Nikolas;
            _halonen = tank.Epic.Halonen;
            _burda = tank.Epic.Burda;
            _pascucci = tank.Epic.Pascucci;
            _dumitru = tank.Epic.Dumitru;
            _tamadaYoshio = tank.Epic.TamadaYoshio;
            _billotte = tank.Epic.Billotte;
            _brunoPietro = tank.Epic.BrunoPietro;
            _tarczay = tank.Epic.Tarczay;
            _kolobanov = tank.Epic.Kolobanov;
            _fadin = tank.Epic.Fadin;
            _heroesOfRaseiniai = tank.Special.heroesOfRassenay;
            _deLanglade = tank.Epic.DeLanglade;
            #endregion

            #region [ ITankRowFrags ]
            _battles = tank.Tankdata.battlesCount;
            _frags = tank.Tankdata.frags;
            _maxFrags = tank.Tankdata.maxFrags;
            _fragsPerBattle = _frags / (double)_battles;
            _killDeathRatio = _frags / (double)(Battles - tank.Tankdata.survivedBattles);
            _tier8Frags = tank.Tankdata.frags8p;
            _beastFrags = tank.Tankdata.fragsBeast;
            _sinaiFrags = tank.Battle.fragsSinai;
            #endregion

            #region [ ITankRowMasterTanker ]

            #endregion

            #region [ ITankRowMedals]
            _kay = tank.Major.Kay;
            _carius = tank.Major.Carius;
            _knispel = tank.Major.Knispel;
            _poppel = tank.Major.Poppel;
            _abrams = tank.Major.Abrams;
            _leclerk = tank.Major.LeClerc;
            _lavrinenko = tank.Major.Lavrinenko;
            _ekins = tank.Major.Ekins;
            #endregion

            #region [ ITankRowPerformance ]
            _shots = tank.Tankdata.shots;
            _hits = tank.Tankdata.hits;
            _hitRatio = _hits / (double)_shots * 100.0;
            _capturePoints = tank.Tankdata.capturePoints;
            _defencePoints = tank.Tankdata.droppedCapturePoints;
            _tanksSpotted = tank.Tankdata.spotted;
            #endregion

            #region [ ITankRowRatings ]
            _battles = tank.Tankdata.battlesCount;
            _winrate = tank.Tankdata.wins / (double)tank.Tankdata.battlesCount * 100.0;
            _averageDamage = tank.Tankdata.damageDealt / tank.Tankdata.battlesCount;
            _killDeathRatio = tank.Tankdata.frags / (double)(_battles - tank.Tankdata.survivedBattles);
            double avgFrags = tank.Tankdata.frags / (double)_battles;
            double avgSpot = tank.Tankdata.spotted / (double)_battles;
            double avgCap = tank.Tankdata.capturePoints / (double)_battles;
            double avgDef = tank.Tankdata.droppedCapturePoints / (double)_battles;
            double avgXP = tank.Tankdata.xp / (double)_battles;

            double value = RatingHelper.CalcER(_averageDamage, Tier, avgFrags, avgSpot, avgCap, avgDef);

            _newEffRating = (int)value;
            value = RatingHelper.CalcWN6(_averageDamage, Tier, avgFrags, avgSpot, avgDef, _winrate);
            _wn6 = (int)value;
            _damageRatingRev1 = (int)(tank.Tankdata.damageDealt / (double)tank.Tankdata.damageReceived * 100);
            value = RatingHelper.CalcKievArmorRating(_battles, avgXP, _averageDamage, _winrate / 100, avgFrags, avgSpot, avgCap, avgDef);
            _kievArmorRating = (int)value;
            _markOfMastery = tank.Special.markOfMastery;
            #endregion

            #region [ ITankRowSeries ]
            _reaperLongest = tank.Series.maxKillingSeries;
            _reaperProgress = tank.Series.killingSeries;
            _sharpshooterLongest = tank.Series.maxSniperSeries;
            _sharpshooterProgress = tank.Series.sniperSeries;
            _masterGunnerLongest = tank.Series.maxPiercingSeries;
            _masterGunnerProgress = tank.Series.piercingSeries;
            _invincibleLongest = tank.Series.maxInvincibleSeries;
            _invincibleProgress = tank.Series.invincibleSeries;
            _survivorLongest = tank.Series.maxDiehardSeries;
            _survivorProgress = tank.Series.diehardSeries;
            #endregion

            #region [ ITankRowSpecialAwards ]
            _kamikaze = tank.Special.kamikaze;
            _raider = tank.Special.raider;
            _bombardier = tank.Special.bombardier;
            _reaper = tank.Series.maxKillingSeries;
            _sharpshooter = tank.Series.maxSniperSeries;
            _invincible = tank.Series.maxInvincibleSeries;
            _survivor = tank.Series.maxDiehardSeries;
            _mouseTrap = tank.Special.mousebane;
            _hunter = tank.Special.beasthunter;
            _sinai = tank.Special.sinai;
            _pattonValley = tank.Special.pattonValley;
            _ranger = tank.Special.lumberjack;
            #endregion

            #region [ ITankRowTime ]
            LastBattle = Utils.UnixDateToDateTime(tank.Tankdata.lastBattleTime);
            PlayTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime);
            AverageBattleTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime / tank.Tankdata.battlesCount);
            #endregion

            #region [ ITankRowXP ]
            _totalXP = tank.Tankdata.xp;
            _maximumXp = tank.Tankdata.maxXP;
            _averageXp = _totalXP / tank.Tankdata.battlesCount;
            #endregion

            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
            _list = list.Select(x => new TankRow(x));
            _prevPlayerStatistic = _list.Where(x => x.Updated <= Updated).OrderByDescending(x => x.Updated).FirstOrDefault() ?? this;
            _previousDate = Updated;
        }

        private readonly IEnumerable<TankRow> _list;
        private DateTime _previousDate;
        private TankRow _prevPlayerStatistic;

        public void SetPreviousDate(DateTime date)
        {
            _previousDate = date;
            _prevPlayerStatistic = _list.OrderBy(x => x.Updated).FirstOrDefault(x => x.Updated <= date) ?? this;
        }
    }
}