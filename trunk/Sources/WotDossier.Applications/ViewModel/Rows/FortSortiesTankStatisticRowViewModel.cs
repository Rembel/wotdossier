using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Mapper = WotDossier.Applications.Logic.Mapper;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class FortSortiesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FortSortiesTankStatisticRowViewModel"/> class.
        /// </summary>
        protected FortSortiesTankStatisticRowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FortSortiesTankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public FortSortiesTankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FortSortiesTankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list)
            : base(tank, list.Select(x => new FortSortiesTankStatisticRowViewModel(x)).ToList())
        {
            #region Achievements

            Mapper.Map<IFortAchievements>(tank.FortAchievements, this);

            #endregion
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.FortSorties; }
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