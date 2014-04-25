using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TankStatisticEntity"/>.
    /// </summary>
    public class TankHistoricalBattleStatisticEntityMapping : ClassMapBase<TankHistoricalBattleStatisticEntity>
    {
        public TankHistoricalBattleStatisticEntityMapping()
        {
			Map(v => v.Updated, "Updated");
            Map(v => v.Raw, "Raw").CustomSqlType("BinaryBlob");
			Map(v => v.TankId, "TankId").ReadOnly();
            Map(v => v.Version, "Version");
            Map(v => v.BattlesCount, "BattlesCount");
		
			References(v => v.TankIdObject).Column(Column(v => v.TankId)).Insert();
        }
    }
}
