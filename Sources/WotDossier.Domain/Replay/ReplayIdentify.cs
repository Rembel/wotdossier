using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    public class ReplayIdentify
    {
        [DataMember]
        public long accountDBID { get; set; }
        [DataMember]
        public long arenaCreateTime { get; set; }
        [DataMember]
        public long arenaUniqueID { get; set; }
        [DataMember]
        public int countryid { get; set; }
        [DataMember]
        public string error { get; set; }
        [DataMember]
        public string error_details { get; set; }
        [DataMember]
        public long internaluserID { get; set; }
        [DataMember]
        public string mapName { get; set; }
        [DataMember]
        public int mapid { get; set; }
        [DataMember]
        public string playername { get; set; }
        [DataMember]
        public int tankid { get; set; }

        private int _uniqueId = -1;
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(countryid, tankid);
            }
            return _uniqueId;
        }
    }
}