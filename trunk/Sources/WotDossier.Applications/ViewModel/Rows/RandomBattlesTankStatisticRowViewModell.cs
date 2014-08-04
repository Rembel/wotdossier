using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Mapper = WotDossier.Applications.Logic.Mapper;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class RandomBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomBattlesTankStatisticRowViewModel"/> class.
        /// </summary>
        protected RandomBattlesTankStatisticRowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomBattlesTankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public RandomBattlesTankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RandomBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list)
            : base(tank, list.Select(x => new RandomBattlesTankStatisticRowViewModel(x)).ToList())
        {
            #region [ IStatisticFrags ]
            BeastFrags = tank.Achievements.FragsBeast;
            SinaiFrags = tank.Achievements.FragsSinai;
            PattonFrags = tank.Achievements.FragsPatton;
            MouseFrags = tank.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count);
            #endregion

            #region Achievements

            Mapper.Map<IRandomBattlesAchievements>(tank.Achievements, this);

            #endregion
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.A15x15; }
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