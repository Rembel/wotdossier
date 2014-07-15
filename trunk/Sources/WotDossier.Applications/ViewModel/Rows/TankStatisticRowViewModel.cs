using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModel : TankStatisticRowViewModelBase<TankStatisticRowViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
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
        public TankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list)
            : base(tank, list.Select(x => new TankStatisticRowViewModel(x)).ToList())
        {
            OriginalXP = tank.A15x15.originalXP;
            DamageAssistedTrack = tank.A15x15.damageAssistedTrack;
            DamageAssistedRadio = tank.A15x15.damageAssistedRadio;
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

            #region [ IStatisticBattles ]
            BattlesCount = tank.A15x15.battlesCount;
            Wins = tank.A15x15.wins;
            Losses = tank.A15x15.losses;
            SurvivedBattles = tank.A15x15.survivedBattles;
            SurvivedAndWon = tank.A15x15.winAndSurvived;
            #endregion

            #region [ IStatisticDamage ]
            DamageDealt = tank.A15x15.damageDealt;
            DamageTaken = tank.A15x15.damageReceived;
            MaxDamage = tank.A15x15.maxDamage;
            #endregion

            #region [ IStatisticPerformance ]
            Shots = tank.A15x15.shots;
            Hits = tank.A15x15.hits;
            if (Shots > 0)
            {
                HitsPercents = Hits / (double)Shots * 100.0;
            }
            CapturePoints = tank.A15x15.capturePoints;
            DroppedCapturePoints = tank.A15x15.droppedCapturePoints;
            Spotted = tank.A15x15.spotted;
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.A15x15.xp;
            MaxXp = tank.A15x15.maxXP;
            #endregion

            #region [ IStatisticFrags ]
            Frags = tank.A15x15.frags;
            MaxFrags = tank.A15x15.maxFrags;
            Tier8Frags = tank.A15x15.frags8p;
            #endregion

            #region [ IStatisticTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (tank.A15x15.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime / tank.A15x15.battlesCount);
            }
            #endregion

            #region [ IStatisticFrags ]
            BeastFrags = tank.Achievements.FragsBeast;
            SinaiFrags = tank.Achievements.FragsSinai;
            PattonFrags = tank.Achievements.FragsPatton;
            MouseFrags = tank.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count);
            #endregion

            #region Achievements

            BattleHero = tank.Achievements.BattleHeroes;
            Warrior = tank.Achievements.Warrior;
            Invader = tank.Achievements.Invader;
            Sniper = tank.Achievements.Sniper;
            Defender = tank.Achievements.Defender;
            SteelWall = tank.Achievements.Steelwall;
            Confederate = tank.Achievements.Supporter;
            Scout = tank.Achievements.Scout;
            PatrolDuty = tank.Achievements.Evileye;
            BrothersInArms = tank.Achievements.MedalBrothersInArms;
            CrucialContribution = tank.Achievements.MedalCrucialContribution;
            IronMan = tank.Achievements.IronMan;
            LuckyDevil = tank.Achievements.LuckyDevil;
            Sturdy = tank.Achievements.Sturdy;
            Huntsman = tank.Achievements.Huntsman;
            MainGun = tank.Achievements.MainGun;
            Sniper2 = tank.Achievements.Sniper2;

            Boelter = tank.Achievements.MedalWittmann;
            RadleyWalters = tank.Achievements.MedalRadleyWalters;
            LafayettePool = tank.Achievements.MedalLafayettePool;
            Orlik = tank.Achievements.MedalOrlik;
            Oskin = tank.Achievements.MedalOskin;
            Lehvaslaiho = tank.Achievements.MedalLehvaslaiho;
            Nikolas = tank.Achievements.MedalNikolas;
            Halonen = tank.Achievements.MedalHalonen;
            Burda = tank.Achievements.MedalBurda;
            Pascucci = tank.Achievements.MedalPascucci;
            Dumitru = tank.Achievements.MedalDumitru;
            TamadaYoshio = tank.Achievements.MedalTamadaYoshio;
            Billotte = tank.Achievements.MedalBillotte;
            BrunoPietro = tank.Achievements.MedalBrunoPietro;
            Tarczay = tank.Achievements.MedalTarczay;
            Kolobanov = tank.Achievements.MedalKolobanov;
            Fadin = tank.Achievements.MedalFadin;
            HeroesOfRassenay = tank.Achievements.HeroesOfRassenay;
            DeLanglade = tank.Achievements.MedalDeLanglade;

            Kay = tank.Achievements.MedalKay;
            Carius = tank.Achievements.MedalCarius;
            Knispel = tank.Achievements.MedalKnispel;
            Poppel = tank.Achievements.MedalPoppel;
            Abrams = tank.Achievements.MedalAbrams;
            Leclerk = tank.Achievements.MedalLeClerc;
            Lavrinenko = tank.Achievements.MedalLavrinenko;
            Ekins = tank.Achievements.MedalEkins;

            ReaperLongest = tank.Achievements.MaxKillingSeries;
            ReaperProgress = tank.Achievements.KillingSeries;
            SharpshooterLongest = tank.Achievements.MaxSniperSeries;
            SharpshooterProgress = tank.Achievements.SniperSeries;
            MasterGunnerLongest = tank.Achievements.MaxPiercingSeries;
            MasterGunnerProgress = tank.Achievements.PiercingSeries;
            InvincibleLongest = tank.Achievements.MaxInvincibleSeries;
            InvincibleProgress = tank.Achievements.InvincibleSeries;
            SurvivorLongest = tank.Achievements.MaxDiehardSeries;
            SurvivorProgress = tank.Achievements.DiehardSeries;

            Kamikaze = tank.Achievements.Kamikaze;
            Raider = tank.Achievements.Raider;
            Bombardier = tank.Achievements.Bombardier;
            Reaper = tank.Achievements.MaxKillingSeries;
            Sharpshooter = tank.Achievements.MaxSniperSeries;
            Invincible = tank.Achievements.MaxInvincibleSeries;
            Survivor = tank.Achievements.MaxDiehardSeries;
            MouseTrap = tank.Achievements.Mousebane;
            Hunter = tank.Achievements.Beasthunter;
            Sinai = tank.Achievements.Sinai;
            PattonValley = tank.Achievements.PattonValley;

            #endregion
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