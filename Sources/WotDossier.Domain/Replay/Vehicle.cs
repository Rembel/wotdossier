using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public string clanAbbrev { get; set; }
        //        "events": {}, 
        [DataMember]
        public bool isAlive { get; set; }
        [DataMember]
        public bool isTeamKiller { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int team { get; set; }
        [DataMember]
        public string vehicleType { get; set; }
    }
}