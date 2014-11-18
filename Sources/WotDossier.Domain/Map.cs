using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;

namespace WotDossier.Domain
{
    /// <summary>
    /// Map description class
    /// </summary>
    [DataContract]
    public class Map : IMapDescription
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
        /// Gets or sets the gameplay.
        /// </summary>
        [DataMember(Name = "gameplay")]
        public Gameplay Gameplay { get; set; }
        
        [DataMember(Name = "team")]
        public int Team { get; set; }

        /// <summary>
        /// The localized map name
        /// </summary>
        [DataMember(Name = "localizedMapName")]
        public string LocalizedMapName { get; set; }

        [DataMember(Name = "bottomLeft")]
        public string BottomLeft{ get; set; }

        [DataMember(Name = "upperRight")]
        public string UpperRight{ get; set; }
    }
}
