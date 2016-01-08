using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class FortSortiesStatAdapter : AbstractStatisticAdapter<RandomBattlesStatisticEntity>, IFortAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FortSortiesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.FortSorties ?? new StatisticJson())
        {
            var achievementsFort = new AchievementsFort();
            Func<TankJson, AchievementsFort> fortAchievementsPredicate = tankJson => tankJson.FortAchievements ?? achievementsFort;

            Conqueror = tanks.Sum(x => fortAchievementsPredicate(x).Conqueror);
            FireAndSword = tanks.Sum(x => fortAchievementsPredicate(x).FireAndSword);
            Crusher = tanks.Sum(x => fortAchievementsPredicate(x).Crusher);
            CounterBlow = tanks.Sum(x => fortAchievementsPredicate(x).CounterBlow);
            SoldierOfFortune = tanks.Sum(x => fortAchievementsPredicate(x).SoldierOfFortune);
            Kampfer = tanks.Sum(x => fortAchievementsPredicate(x).Kampfer);
        }

        public List<ITankStatisticRow> Tanks { get; set; }

        #region Achievements

        public int Conqueror { get; set; }
        public int FireAndSword { get; set; }
        public int Crusher { get; set; }
        public int CounterBlow { get; set; }
        public int SoldierOfFortune { get; set; }
        public int Kampfer { get; set; }
        public int CapturedBasesInAttack { get; set; }
        public int CapturedBasesInDefence { get; set; }

        #endregion
        
        public override void Update(RandomBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RandomBattlesAchievementsEntity();
            }

            Mapper.Map<IFortAchievements>(this, entity.AchievementsIdObject);
        }
    }
}