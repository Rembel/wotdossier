using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Common
    {
        [DataMember]
        public long arenaCreateTime { get; set; }
        [DataMember]
        public int arenaTypeID { get; set; }
        [DataMember]
        public int bonusType { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public int finishReason { get; set; }
        [DataMember]
        public int vehLockMode { get; set; }
        [DataMember]
        public int winnerTeam { get; set; }
    }
}