using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Interfaces;
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
        /// Initializes a new instance of the <see cref="RandomBattlesTankStatisticRowViewModel"/> class.
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
            #region Achievements

            Mapper.Map<IHistoricalBattlesAchievements>(tank.AchievementsHistorical, this);

            #endregion

        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Historical; }
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