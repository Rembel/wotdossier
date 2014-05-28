using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TeamBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase<TeamBattlesTankStatisticRowViewModel>
    {
        protected TeamBattlesTankStatisticRowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public TeamBattlesTankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TeamBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list)
            : base(tank, list.Select(x => new TeamBattlesTankStatisticRowViewModel(x)).ToList())
        {
            OriginalXP = tank.A7x7.originalXP;
            DamageAssistedTrack = tank.A7x7.damageAssistedTrack;
            DamageAssistedRadio = tank.A7x7.damageAssistedRadio;
            ShotsReceived = tank.A7x7.shotsReceived;
            NoDamageShotsReceived = tank.A7x7.noDamageShotsReceived;
            PiercedReceived = tank.A7x7.piercedReceived;
            HeHitsReceived = tank.A7x7.heHitsReceived;
            HeHits = tank.A7x7.he_hits;
            Pierced = tank.A7x7.pierced;
            XpBefore88 = tank.A7x7.xpBefore8_8;
            BattlesCountBefore88 = tank.A7x7.battlesCountBefore8_8;
            BattlesCount88 = tank.A7x7.battlesCount - BattlesCountBefore88;
            IsPremium = tank.Common.premium == 1;

            #region [ IStatisticBattles ]
            BattlesCount = tank.A7x7.battlesCount;
            Wins = tank.A7x7.wins;
            Losses = tank.A7x7.losses;
            SurvivedBattles = tank.A7x7.survivedBattles;
            SurvivedAndWon = tank.A7x7.winAndSurvived;
            #endregion

            #region [ IStatisticDamage ]
            DamageDealt = tank.A7x7.damageDealt;
            DamageTaken = tank.A7x7.damageReceived;
            MaxDamage = tank.A7x7.maxDamage;
            #endregion

            #region [ IStatisticPerformance ]
            Shots = tank.A7x7.shots;
            Hits = tank.A7x7.hits;
            if (Shots > 0)
            {
                HitsPercents = Hits / (double)Shots * 100.0;
            }
            CapturePoints = tank.A7x7.capturePoints;
            DroppedCapturePoints = tank.A7x7.droppedCapturePoints;
            Spotted = tank.A7x7.spotted;
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.A7x7.xp;
            MaxXp = tank.A7x7.maxXP;
            #endregion

            #region [ IStatisticFrags ]
            Frags = tank.A7x7.frags;
            MaxFrags = tank.A7x7.maxFrags;
            Tier8Frags = tank.A7x7.frags8p;
            #endregion

            #region [ IStatisticTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (tank.A7x7.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime / tank.A7x7.battlesCount);
            }
            #endregion

            #region Achievements

            KingOfTheHill = tank.Achievements7x7.kingOfTheHill;
            ArmoredFist = tank.Achievements7x7.armoredFist;
            CrucialShot = tank.Achievements7x7.crucialShot;
            CrucialShotMedal = tank.Achievements7x7.crucialShotMedal;
            FightingReconnaissance = tank.Achievements7x7.fightingReconnaissance;
            FightingReconnaissanceMedal = tank.Achievements7x7.fightingReconnaissanceMedal;
            ForTacticalOperations = tank.Achievements7x7.forTacticalOperations;
            GeniusForWar = tank.Achievements7x7.geniusForWar;
            GeniusForWarMedal = tank.Achievements7x7.geniusForWarMedal;
            GodOfWar = tank.Achievements7x7.godOfWar;
            MaxTacticalBreakthroughSeries = tank.Achievements7x7.maxTacticalBreakthroughSeries;
            TacticalBreakthrough = tank.Achievements7x7.tacticalBreakthrough;
            TacticalBreakthroughSeries = tank.Achievements7x7.tacticalBreakthroughSeries;
            WillToWinSpirit = tank.Achievements7x7.willToWinSpirit;
            WolfAmongSheep = tank.Achievements7x7.wolfAmongSheep;
            WolfAmongSheepMedal = tank.Achievements7x7.wolfAmongSheepMedal;

            PromisingFighter = tank.Achievements7x7.promisingFighter;
            PromisingFighterMedal = tank.Achievements7x7.promisingFighterMedal;
            HeavyFire = tank.Achievements7x7.heavyFire;
            HeavyFireMedal = tank.Achievements7x7.heavyFireMedal;
            Ranger = tank.Achievements7x7.ranger;
            RangerMedal = tank.Achievements7x7.rangerMedal;
            FireAndSteel = tank.Achievements7x7.fireAndSteel;
            FireAndSteelMedal = tank.Achievements7x7.fireAndSteelMedal;
            Pyromaniac = tank.Achievements7x7.pyromaniac;
            PyromaniacMedal = tank.Achievements7x7.pyromaniacMedal;
            NoMansLand = tank.Achievements7x7.noMansLand;

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