using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TeamBattlesStatisticEntityMapping"/>.
    /// </summary>
    public class TeamBattlesStatisticEntityMapping : StatisticClassMapBase<TeamBattlesStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesStatisticEntityMapping"/> class.
        /// </summary>
        public TeamBattlesStatisticEntityMapping()
        {
			Map(v => v.AchievementsId).ReadOnly();
			Map(v => v.AchievementsUId);
            Map(v => v.PlayerUId);
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();
            References(v => v.AchievementsIdObject).Column(Column(v => v.AchievementsId)).Insert().Update().Cascade.All().Fetch.Join();
        }
    }
}
