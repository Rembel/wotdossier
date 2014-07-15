using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class HistoricalBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase<HistoricalBattlesTankStatisticRowViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalBattlesTankStatisticRowViewModel"/> class.
        /// </summary>
        protected HistoricalBattlesTankStatisticRowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public HistoricalBattlesTankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HistoricalBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list)
            : base(tank, list.Select(x => new HistoricalBattlesTankStatisticRowViewModel(x)).ToList())
        {
            OriginalXP = tank.Historical.originalXP;
            DamageAssistedTrack = tank.Historical.damageAssistedTrack;
            DamageAssistedRadio = tank.Historical.damageAssistedRadio;
            ShotsReceived = tank.Historical.shotsReceived;
            NoDamageShotsReceived = tank.Historical.noDamageShotsReceived;
            PiercedReceived = tank.Historical.piercedReceived;
            HeHitsReceived = tank.Historical.heHitsReceived;
            HeHits = tank.Historical.he_hits;
            Pierced = tank.Historical.pierced;
            XpBefore88 = tank.Historical.xpBefore8_8;
            BattlesCountBefore88 = tank.Historical.battlesCountBefore8_8;
            BattlesCount88 = tank.Historical.battlesCount - BattlesCountBefore88;
            IsPremium = tank.Common.premium == 1;

            #region [ IStatisticBattles ]
            BattlesCount = tank.Historical.battlesCount;
            Wins = tank.Historical.wins;
            Losses = tank.Historical.losses;
            SurvivedBattles = tank.Historical.survivedBattles;
            SurvivedAndWon = tank.Historical.winAndSurvived;
            #endregion

            #region [ IStatisticDamage ]
            DamageDealt = tank.Historical.damageDealt;
            DamageTaken = tank.Historical.damageReceived;
            MaxDamage = tank.Historical.maxDamage;
            #endregion

            #region [ IStatisticPerformance ]
            Shots = tank.Historical.shots;
            Hits = tank.Historical.hits;
            if (Shots > 0)
            {
                HitsPercents = Hits / (double)Shots * 100.0;
            }
            CapturePoints = tank.Historical.capturePoints;
            DroppedCapturePoints = tank.Historical.droppedCapturePoints;
            Spotted = tank.Historical.spotted;
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.Historical.xp;
            MaxXp = tank.Historical.maxXP;
            #endregion

            #region [ IStatisticFrags ]
            Frags = tank.Historical.frags;
            MaxFrags = tank.Historical.maxFrags;
            Tier8Frags = tank.Historical.frags8p;
            #endregion

            #region [ IStatisticTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (tank.Historical.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime / tank.Historical.battlesCount);
            }
            #endregion

            #region Achievements

            GuardsMan = tank.AchievementsHistorical.GuardsMan;
            MakerOfHistory = tank.AchievementsHistorical.MakerOfHistory;
            BothSidesWins = tank.AchievementsHistorical.BothSidesWins;
            WeakVehiclesWins = tank.AchievementsHistorical.WeakVehiclesWins;

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