using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Factors
    {
        [DataMember]
        public int aogasFactor10 { get; set; }
        [DataMember]
        public int dailyXPFactor10 { get; set; }
    }
}