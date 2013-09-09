using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class CommandResult
    {
        [DataMember]
        public Damaged Damage { get; set; }
        [DataMember]
        public Dictionary<long, Vehicle> Vehicles { get; set; }
        [DataMember]
        public Dictionary<long, FragsCount> Frags { get; set; }
    }
}