using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Mapper = WotDossier.Applications.Logic.Mapper;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class RandomBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RandomBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(tank, list)
        {
            Func<TankJson, AchievementsJson> achievementsPredicate = tankJson => tankJson.Achievements ?? new AchievementsJson();

            #region [ IStatisticFrags ]
            BeastFrags = achievementsPredicate(tank).FragsBeast;
            SinaiFrags = achievementsPredicate(tank).FragsSinai;
            PattonFrags = achievementsPredicate(tank).FragsPatton;
            var fragsJsons = tank.Frags ?? new List<FragsJson>();
            MouseFrags = fragsJsons.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count);
            #endregion

            #region Achievements

            Mapper.Map<IRandomBattlesAchievements>(achievementsPredicate(tank), this);

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