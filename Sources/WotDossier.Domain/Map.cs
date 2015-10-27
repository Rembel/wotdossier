using System.Runtime.Serialization;

namespace WotDossier.Domain
{
    /// <summary>
    /// Map description class
    /// </summary>
    [DataContract]
    public class Map
    {
        /// <summary>
        /// The map id
        /// </summary>
        [DataMember(Name = "mapid")]
        public int MapId { get; set; }

        /// <summary>
        /// The map id name
        /// </summary>
        [DataMember(Name = "mapidname")]
        public string MapNameId { get; set; }

        /// <summary>
        /// The map name
        /// </summary>
        [DataMember(Name = "mapname")]
        public string MapName { get; set; }

        /// <summary>
        /// The localized map name
        /// </summary>
        [DataMember(Name = "localizedMapName")]
        public string LocalizedMapName { get; set; }

        public MapConfig Config { get; set; }

        public override string ToString()
        {
            return MapNameId;
        }
    }
}
