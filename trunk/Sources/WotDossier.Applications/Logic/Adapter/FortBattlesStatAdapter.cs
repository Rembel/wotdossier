using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class FortBattlesStatAdapter : AbstractStatisticAdapter<PlayerStatisticEntity>, IFortAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FortBattlesStatAdapter(List<TankJson> tanks)
            : base(tanks, tank => tank.FortBattles)
        {
            Conqueror = tanks.Sum(x => x.FortAchievements.Conqueror);
            FireAndSword = tanks.Sum(x => x.FortAchievements.FireAndSword);
            Crusher = tanks.Sum(x => x.FortAchievements.Crusher);
            CounterBlow = tanks.Sum(x => x.FortAchievements.CounterBlow);
            SoldierOfFortune = tanks.Sum(x => x.FortAchievements.SoldierOfFortune);
            Kampfer = tanks.Sum(x => x.FortAchievements.Kampfer);
        }

        public List<ITankStatisticRow> Tanks { get; set; }

        #region Achievements

        public int Conqueror { get; set; }
        public int FireAndSword { get; set; }
        public int Crusher { get; set; }
        public int CounterBlow { get; set; }
        public int SoldierOfFortune { get; set; }
        public int Kampfer { get; set; }

        #endregion

        public override void Update(PlayerStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new PlayerAchievementsEntity();
            }

            Mapper.Map<IFortAchievements>(this, entity.AchievementsIdObject);
        }
    }
}