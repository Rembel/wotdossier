using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class HistoricalBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HistoricalBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(tank, list)
        {
            #region Achievements

            Mapper.Map<IHistoricalBattlesAchievements>(tank.AchievementsHistorical ?? new AchievementsHistorical(), this);

            #endregion

        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Historical ?? new StatisticJson(); }
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