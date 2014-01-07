using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TotalTankStatisticRowViewModel : TankStatisticRowViewModel, 
        ITankRowBattles, ITankRowDamage, ITankRowFrags, ITankRowPerformance, ITankRowRatings, ITankRowTime 
    {
        private DateTime _lastBattle;
        private IEnumerable<FragsJson> _tankFrags;
        private bool _isFavorite;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TotalTankStatisticRowViewModel(IEnumerable<TankStatisticRowViewModel> list)
        {
            Tier = -1;
            Type = -1;
            Tank = "Total";
            Icon = null;
            CountryId = -1;
            TankId = -1;
            TankUniqueId = -1;
            OriginalXP = list.Sum(x => x.OriginalXP);
            DamageAssistedTrack = list.Sum(x => x.DamageAssistedTrack);
            DamageAssistedRadio = list.Sum(x => x.DamageAssistedRadio);
            Mileage = list.Sum(x => x.Mileage);
            ShotsReceived = list.Sum(x => x.ShotsReceived);
            NoDamageShotsReceived = list.Sum(x => x.NoDamageShotsReceived);
            PiercedReceived = list.Sum(x => x.PiercedReceived);
            HeHitsReceived = list.Sum(x => x.HeHitsReceived);
            HeHits = list.Sum(x => x.HeHits);
            Pierced = list.Sum(x => x.Pierced);
            XpBefore88 = list.Sum(x => x.XpBefore88);
            BattlesCountBefore88 = list.Sum(x => x.BattlesCountBefore88);
            BattlesCount88 = list.Sum(x => x.BattlesCount88);
            IsPremium = false;

            #region [ ITankRowBattleAwards ]
            BattleHero = list.Sum(x => x.BattleHero);
            Warrior = list.Sum(x => x.Warrior);
            Invader = list.Sum(x => x.Invader);
            Sniper = list.Sum(x => x.Sniper);
            Defender = list.Sum(x => x.Defender);
            SteelWall = list.Sum(x => x.SteelWall);
            Confederate = list.Sum(x => x.Confederate);
            Scout = list.Sum(x => x.Scout);
            PatrolDuty = list.Sum(x => x.PatrolDuty);
            BrothersInArms = list.Sum(x => x.BrothersInArms);
            CrucialContribution = list.Sum(x => x.CrucialContribution);
            CoolHeaded = list.Sum(x => x.CoolHeaded);
            LuckyDevil = list.Sum(x => x.LuckyDevil);
            Spartan = list.Sum(x => x.Spartan);
            Ranger = list.Sum(x => x.Ranger);
            #endregion

            #region [ ITankRowBattles ]
            BattlesCount = list.Sum(x => x.BattlesCount);
            Wins = list.Sum(x => x.Wins);
            Losses = list.Sum(x => x.Losses);
            SurvivedBattles = list.Sum(x => x.SurvivedBattles);
            SurvivedAndWon = list.Sum(x => x.SurvivedAndWon);
            #endregion

            #region [ ITankRowDamage ]
            DamageDealt = list.Sum(x => x.DamageDealt);
            DamageTaken = list.Sum(x => x.DamageTaken);
            MaxDamage = list.Sum(x => x.MaxDamage);
            #endregion

            #region [ ITankRowEpic ]
            Boelter = list.Sum(x => x.Boelter);
            RadleyWalters = list.Sum(x => x.RadleyWalters);
            LafayettePool = list.Sum(x => x.LafayettePool);
            Orlik = list.Sum(x => x.Orlik);
            Oskin = list.Sum(x => x.Oskin);
            Lehvaslaiho = list.Sum(x => x.Lehvaslaiho);
            Nikolas = list.Sum(x => x.Nikolas);
            Halonen = list.Sum(x => x.Halonen);
            Burda = list.Sum(x => x.Burda);
            Pascucci = list.Sum(x => x.Pascucci);
            Dumitru = list.Sum(x => x.Dumitru);
            TamadaYoshio = list.Sum(x => x.TamadaYoshio);
            Billotte = list.Sum(x => x.Billotte);
            BrunoPietro = list.Sum(x => x.BrunoPietro);
            Tarczay = list.Sum(x => x.Tarczay);
            Kolobanov = list.Sum(x => x.Kolobanov);
            Fadin = list.Sum(x => x.Fadin);
            HeroesOfRassenay = list.Sum(x => x.HeroesOfRassenay);
            DeLanglade = list.Sum(x => x.DeLanglade);
            #endregion

            #region [ ITankRowFrags ]
            Frags = list.Sum(x => x.Frags);
            MaxFrags = list.Max(x => x.MaxFrags);
            Tier8Frags = list.Sum(x => x.Tier8Frags);
            BeastFrags = list.Sum(x => x.BeastFrags);
            SinaiFrags = list.Sum(x => x.SinaiFrags);
            #endregion

            #region [ ITankRowMasterTanker ]

            #endregion

            #region [ ITankRowMedals]
            Kay = list.Sum(x => x.Kay);
            Carius = list.Sum(x => x.Carius);
            Knispel = list.Sum(x => x.Knispel);
            Poppel = list.Sum(x => x.Poppel);
            Abrams = list.Sum(x => x.Abrams);
            Leclerk = list.Sum(x => x.Leclerk);
            Lavrinenko = list.Sum(x => x.Lavrinenko);
            Ekins = list.Sum(x => x.Ekins);
            #endregion

            #region [ ITankRowPerformance ]
            Shots = list.Sum(x => x.Shots);
            Hits = list.Sum(x => x.Hits);
            if (Shots > 0)
            {
                HitsPercents = Hits/(double) Shots*100.0;
            }
            CapturePoints = list.Sum(x => x.CapturePoints);
            DroppedCapturePoints = list.Sum(x => x.DroppedCapturePoints);
            Spotted = list.Sum(x => x.Spotted);
            #endregion

            #region [ ITankRowSeries ]
            ReaperLongest = list.Max(x => x.ReaperLongest);
            ReaperProgress = list.Max(x => x.ReaperProgress);
            SharpshooterLongest = list.Max(x => x.SharpshooterLongest);
            SharpshooterProgress = list.Max(x => x.SharpshooterProgress);
            MasterGunnerLongest = list.Max(x => x.MasterGunnerLongest);
            MasterGunnerProgress = list.Max(x => x.MasterGunnerProgress);
            InvincibleLongest = list.Max(x => x.InvincibleLongest);
            InvincibleProgress = list.Max(x => x.InvincibleProgress);
            SurvivorLongest = list.Max(x => x.SurvivorLongest);
            SurvivorProgress = list.Max(x => x.SurvivorProgress);
            #endregion

            #region [ ITankRowSpecialAwards ]
            Kamikaze = list.Sum(x => x.Kamikaze);
            Raider = list.Sum(x => x.Raider);
            Bombardier = list.Sum(x => x.Bombardier);
            Reaper = list.Max(x => x.Reaper);
            Sharpshooter = list.Sum(x => x.Sharpshooter);
            Invincible = list.Sum(x => x.Invincible);
            Survivor = list.Sum(x => x.Survivor);
            MouseTrap = list.Sum(x => x.MouseTrap);
            Hunter = list.Sum(x => x.Hunter);
            Sinai = list.Sum(x => x.Sinai);
            PattonValley = list.Sum(x => x.PattonValley);
            #endregion

            #region [ ITankRowTime ]
            LastBattle = list.Max(x => x.LastBattle);
            PlayTime = list.Max(x => x.PlayTime);
            if (BattlesCount > 0)
            {
                //AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime/tank.A15x15.battlesCount);
            }
            #endregion

            TankFrags = new List<FragsJson>();
            
            #region [ ITankRowXP ]
            Xp = list.Sum(x => x.Xp);
            MaxXp = list.Max(x => x.MaxXp);
            #endregion

            #region [ ITankRowRatings ]
            //MarkOfMastery = list.Sum(x => x.markOfMastery;

            #endregion

            //Updated = Utils.UnixDateToDateTime(tank.Common.updated);
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