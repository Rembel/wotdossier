using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class FirstBlock
    {
        [DataMember]
        public string dateTime { get; set; }
        [DataMember]
        public string gameplayID { get; set; }
        [DataMember]
        public int battleType { get; set; }
        [DataMember]
        public string mapDisplayName { get; set; }
        //since Version 0.8.6:
        [DataMember]
        public string clientVersionFromXml { get; set; }
        //since Version 0.8.6:
        [DataMember]
        public string clientVersionFromExe { get; set; }
        [DataMember]
        public string mapName { get; set; }
        [DataMember]
        public long playerID { get; set; }
        [DataMember]
        public string playerName { get; set; }
        [DataMember]
        public string playerVehicle { get; set; }
        [DataMember]
        public Dictionary<long, Vehicle> vehicles { get; set; }
    }
}