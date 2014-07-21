using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract class StatisticViewModelBase : INotifyPropertyChanged, IStatisticBase
    {
        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }

        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int MaxFrags { get; set; }
        public int Spotted { get; set; }

        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public int MaxDamage { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }

        public int MarkOfMastery { get; set; }

        public TimeSpan PlayTime { get; set; }

        public double Tier { get; set; }

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
                    return Draws / (double)BattlesCount * 100.0;
                }
                return 0;
            }
        }

        public double KillDeathRatio
        {
            get
            {
                if (BattlesCount - SurvivedBattles > 0)
                {
                    return Frags / (double)(BattlesCount - SurvivedBattles);
                }
                return 0;
            }
        }

        public double DamageRatio
        {
            get
            {
                if (DamageTaken > 0)
                {
                    return DamageDealt / (double)DamageTaken;
                }
                return 0;
            }
        }

        #region Percents

        public double HitsPercents { get; set; }

        public double WinsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Wins/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }
        public double LossesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Losses/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }
        public double SurvivedBattlesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedBattles/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region Average values

        public double AvgXp
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Xp/(double) BattlesCount;
                }
                return 0;
            }
        }

        public double AvgFrags
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

        public double AvgSpotted
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Spotted / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDamageTaken
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DamageTaken / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDamageDealt
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DamageDealt / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return CapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DroppedCapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        #endregion

        #region Unofficial ratings
        
        public double WN7Rating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.Wn7(BattlesCount, AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);
                }
                return 0;
            }
        }

        public double NoobRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.WotNoobsRating(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgCapturePoints, AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double EffRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.EffectivityRating(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgCapturePoints, AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double KievArmorRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.KievArmorRating(BattlesCount, AvgXp, AvgDamageDealt, WinsPercent/100.0,
                                                            AvgFrags, AvgSpotted, AvgCapturePoints,
                                                            AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double XEFF
        {
            get { return RatingHelper.Xwn8(WN8Rating); }
        }

        public double XWN
        {
            get { return RatingHelper.Xwn6(WN7Rating); }
        }

        private double _performanceRating;
        private double _rbr;

        public virtual double PerformanceRating
        {
            get { return _performanceRating; }
            set
            {
                _performanceRating = value;
                OnPropertyChanged("PerformanceRating");
            }
        }

        private double _wn8Rating;
        public virtual double WN8Rating
        {
            get { return _wn8Rating; }
            set
            {
                _wn8Rating = value;
                OnPropertyChanged("Wn8Rating");
            }
        }

        public virtual double RBR
        {
            get { return _rbr; }
            set
            {
                _rbr = value;
                OnPropertyChanged("RBR");
            }
        }

        #endregion

        #region Achievments

        #region Random

        public int BattleHero { get; set; }

        public int Warrior { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Sniper2 { get; set; }

        public int MainGun { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int IronMan { get; set; }

        public int LuckyDevil { get; set; }

        public int Sturdy { get; set; }

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

        public int HeroesOfRassenay { get; set; }

        public int DeLanglade { get; set; }

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

        public int Huntsman { get; set; }

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

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

        public int MedalMonolith { get; set; }

        public int MedalAntiSpgFire { get; set; }

        public int MedalGore { get; set; }

        public int MedalCoolBlood { get; set; }

        public int MedalStark { get; set; }

        public int DamageRating { get; set; }

        /// <summary>
        /// Gets/Sets the field "MarksOnGun".
        /// </summary>
        public int MarksOnGun { get; set; }

        /// <summary>
        /// Gets/Sets the field "MovingAvgDamage".
        /// </summary>
        public int MovingAvgDamage { get; set; }

        public int Lumberjack { get; set; }

        #endregion

        #region 7x7

        public int KingOfTheHill { get; set; }
        public int ArmoredFist { get; set; }
        public int CrucialShot { get; set; }
        public int CrucialShotMedal { get; set; }
        public int FightingReconnaissance { get; set; }
        public int FightingReconnaissanceMedal { get; set; }
        public int ForTacticalOperations { get; set; }
        public int GeniusForWar { get; set; }
        public int GeniusForWarMedal { get; set; }
        public int GodOfWar { get; set; }
        public int MaxTacticalBreakthroughSeries { get; set; }
        public int TacticalBreakthrough { get; set; }
        public int TacticalBreakthroughSeries { get; set; }
        public int WillToWinSpirit { get; set; }
        public int WolfAmongSheep { get; set; }
        public int WolfAmongSheepMedal { get; set; }

        public int NoMansLand { get; set; }
        public int PyromaniacMedal { get; set; }
        public int Pyromaniac { get; set; }
        public int FireAndSteel { get; set; }
        public int FireAndSteelMedal { get; set; }
        public int RangerMedal { get; set; }
        public int Ranger { get; set; }
        public int HeavyFireMedal { get; set; }
        public int HeavyFire { get; set; }
        public int PromisingFighterMedal { get; set; }
        public int PromisingFighter { get; set; }

        public int Guerrilla { get; set; }
        public int GuerrillaMedal { get; set; }
        public int Infiltrator { get; set; }
        public int InfiltratorMedal { get; set; }
        public int Sentinel { get; set; }
        public int SentinelMedal { get; set; }
        public int PrematureDetonation { get; set; }
        public int PrematureDetonationMedal { get; set; }
        public int BruteForce { get; set; }
        public int BruteForceMedal { get; set; }
        public int AwardCount { get; set; }
        public int BattleTested { get; set; }
        public int Alaric { get; set; }
        public int MasterGunner { get; set; }

        #endregion
        
        #region Historical

        public int GuardsMan { get; set; }
        public int MakerOfHistory { get; set; }
        public int BothSidesWins { get; set; }
        public int WeakVehiclesWins { get; set; }

        #endregion
        
        #endregion

        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public int BattlesPerDay { get; set; }

        /// <summary>
        /// Gets all statistic slices.
        /// </summary>
        /// <returns></returns>
        public abstract List<StatisticViewModelBase> GetAllSlices();

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}