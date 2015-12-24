using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class ClanBattlesStatAdapter : AbstractStatisticAdapter<RandomBattlesStatisticEntity>, IClanBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ClanBattlesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.Clan)
        {
            MedalRotmistrov = tanks.Sum(x => x.AchievementsClan.MedalRotmistrov);
        }

        public List<ITankStatisticRow> Tanks { get; set; }

        #region Achievments

        public int MedalRotmistrov { get; set; }

        #endregion

        public override void Update(RandomBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RandomBattlesAchievementsEntity();
            }

            Mapper.Map<IClanBattlesAchievements>(this, entity.AchievementsIdObject);
        }
    }
}