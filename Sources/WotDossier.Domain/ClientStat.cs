using System.Collections.Generic;
using System.Runtime.Serialization;
using WotDossier.Domain.Entities;

namespace WotDossier.Domain
{
    [DataContract]
    public class ClientStat
    {
        [DataMember]
        public PlayerEntity Player { get; set; }

        [DataMember]
        public IList<TankEntity> Tanks { get; set; }

        [DataMember]
        public IEnumerable<RandomBattlesStatisticEntity> RandomStatistic { get; set; }

        [DataMember]
        public IEnumerable<TankRandomBattlesStatisticEntity> TankRandomStatistic { get; set; }
    }
}
