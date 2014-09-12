using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TotalTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        private double _wn8RatingValue;
        private double _performanceRatingValue;

        public override double WN8Rating
        {
            get { return _wn8RatingValue; }
            set { _wn8RatingValue = value; }
        }

        public override double PerformanceRating
        {
            get { return _performanceRatingValue; }
            set { _performanceRatingValue = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TotalTankStatisticRowViewModel(List<ITankStatisticRow> list)
        {
            Type = -1;
            Tank = Resources.Resources.Total;
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
            IronMan = list.Sum(x => x.IronMan);
            LuckyDevil = list.Sum(x => x.LuckyDevil);
            Sturdy = list.Sum(x => x.Sturdy);
            Huntsman = list.Sum(x => x.Huntsman);
            Sniper2 = list.Sum(x => x.Sniper2);
            MainGun = list.Sum(x => x.MainGun);
            #endregion

            #region [ IStatisticBattles ]
            BattlesCount = list.Sum(x => x.BattlesCount);
            Wins = list.Sum(x => ((IStatisticBattles) x).Wins);
            Losses = list.Sum(x => x.Losses);
            SurvivedBattles = list.Sum(x => x.SurvivedBattles);
            SurvivedAndWon = list.Sum(x => x.SurvivedAndWon);
            #endregion

            Tier = list.Sum(x => x.Tier * x.BattlesCount) / BattlesCount;

            #region [ IStatisticDamage ]
            DamageDealt = list.Sum(x => x.DamageDealt);
            DamageTaken = list.Sum(x => x.DamageTaken);
            MaxDamage = list.Max(x => x.MaxDamage);
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

            #region [ IStatisticFrags ]
            Frags = list.Sum(x => x.Frags);
            MaxFrags = list.Max(x => x.MaxFrags);
            Tier8Frags = list.Sum(x => x.Tier8Frags);
            BeastFrags = list.Sum(x => x.BeastFrags);
            SinaiFrags = list.Sum(x => x.SinaiFrags);
            MouseFrags = list.Sum(x => x.MouseFrags);
            PattonFrags = list.Sum(x => x.PattonFrags);
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

            #region [ IStatisticPerformance ]
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
            Sharpshooter = list.Max(x => x.Sharpshooter);
            Invincible = list.Max(x => x.Invincible);
            Survivor = list.Max(x => x.Survivor);
            MouseTrap = list.Sum(x => x.MouseFrags) / 10;
            Hunter = list.Sum(x => x.BeastFrags) / 100;
            Sinai = list.Sum(x => x.SinaiFrags) / 100;
            PattonValley = list.Sum(x => x.PattonFrags) / 100;
            #endregion

            #region [ IStatisticTime ]
            LastBattle = list.Max(x => x.LastBattle);
            double totalSeconds = list.Sum(x => x.PlayTime.TotalSeconds);
            PlayTime = new TimeSpan(0, 0, 0, (int) totalSeconds);
            if (BattlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, (int) (totalSeconds / BattlesCount));
            }
            #endregion

            #region [ ITankRowXP ]
            Xp = list.Sum(x => x.Xp);
            MaxXp = list.Max(x => x.MaxXp);
            #endregion

            WN8Rating = RatingHelper.Wn8(list);
            PerformanceRating = RatingHelper.PerformanceRating(list);
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { throw new NotImplementedException(); }
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