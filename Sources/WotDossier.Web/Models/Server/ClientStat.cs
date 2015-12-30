using System.Collections.Generic;
using WotDossier.Domain.Entities;

namespace WotDossier.Domain
{
    public class ClientStat
    {
        public ClientStat()
        {
        }

        public PlayerEntity Player { get; set; }
        public IList<TankEntity> Tanks { get; set; }
        public IEnumerable<RandomBattlesStatisticEntity> RandomStatistic { get; set; }
        public IEnumerable<TankRandomBattlesStatisticEntity> TankRandomStatistic { get; set; }
    }
}
