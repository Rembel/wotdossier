using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TankStatisticEntity"/>.
    /// </summary>
    public class TankTeamBattleStatisticEntityMapping : ClassMapBase<TankTeamBattleStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TankTeamBattleStatisticEntityMapping"/> class.
        /// </summary>
        public TankTeamBattleStatisticEntityMapping()
        {
			Map(v => v.Updated);
            Map(v => v.Raw).CustomSqlType("BinaryBlob");
			Map(v => v.TankId).ReadOnly();
            Map(v => v.Version);
            Map(v => v.BattlesCount);
		
			References(v => v.TankIdObject).Column(Column(v => v.TankId)).Insert();
        }
    }
}
