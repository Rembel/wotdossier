using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModel : TankStatisticRowViewModelBase<TankStatisticRowViewModel>
    {
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

            #region [ ITankRowPerformance ]
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

            #region [ ITankRowFrags ]
            Frags = tank.A15x15.frags;
            MaxFrags = tank.A15x15.maxFrags;
            Tier8Frags = tank.A15x15.frags8p;
            #endregion

            #region [ ITankRowTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (tank.A15x15.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime / tank.A15x15.battlesCount);
            }
            #endregion

            #region [ ITankRowFrags ]
            BeastFrags = tank.Achievements.fragsBeast;
            SinaiFrags = tank.Achievements.fragsSinai;
            PattonFrags = tank.Achievements.fragsPatton;
            MouseFrags = tank.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count);
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