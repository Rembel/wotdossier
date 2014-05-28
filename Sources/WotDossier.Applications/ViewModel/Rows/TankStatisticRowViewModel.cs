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
            BeastFrags = tank.Achievements.fragsBeast;
            SinaiFrags = tank.Achievements.fragsSinai;
            PattonFrags = tank.Achievements.fragsPatton;
            MouseFrags = tank.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count);
            #endregion

            #region Achievements

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
            IronMan = tank.Achievements.ironMan;
            LuckyDevil = tank.Achievements.luckyDevil;
            Sturdy = tank.Achievements.sturdy;
            Huntsman = tank.Achievements.huntsman;
            MainGun = tank.Achievements.mainGun;
            Sniper2 = tank.Achievements.sniper2;

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

            Kay = tank.Achievements.medalKay;
            Carius = tank.Achievements.medalCarius;
            Knispel = tank.Achievements.medalKnispel;
            Poppel = tank.Achievements.medalPoppel;
            Abrams = tank.Achievements.medalAbrams;
            Leclerk = tank.Achievements.medalLeClerc;
            Lavrinenko = tank.Achievements.medalLavrinenko;
            Ekins = tank.Achievements.medalEkins;

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