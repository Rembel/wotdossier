using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class BattleResult
    {
        [DataMember]
        public long arenaUniqueID { get; set; }
        [DataMember]
        public Common common { get; set; }
        //[DataMember]
        public Personal personal { get; set; }
        [DataMember]
        public Dictionary<long, Player> players { get; set; }
        //[DataMember]
        public Dictionary<long, VehicleResult> vehicles { get; set; }
    }
}