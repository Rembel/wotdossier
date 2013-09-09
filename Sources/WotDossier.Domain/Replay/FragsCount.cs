using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class FragsCount
    {
        [DataMember]
        public int frags { get; set; }
    }
}