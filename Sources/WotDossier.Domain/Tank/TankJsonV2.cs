using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJsonV2
    {
        public byte[] Raw { get; set; }
        public TankDescription Description { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }
        
        /*8.9*/
        public TankDataJson a15x15 { get; set; }
        public TankDataJson a15x15_2 { get; set; }
        public TankDataJson clan { get; set; }
        public TankDataJson clan2 { get; set; }
        public TankDataJson company { get; set; }
        public TankDataJson company2 { get; set; }
        public TankDataJson a7x7 { get; set; }
        public TankDataJson achievements { get; set; }
        public CommonJson Common { get; set; }
        public TankDataJson total { get; set; }
        public TankDataJson max15x15 { get; set; }
        public TankDataJson max7x7 { get; set; }
        public IList<IList<string>> fragslist { get; set; }

        public int UniqueId()
        {
            return Utils.ToUniqueId(Common.countryid, Common.tankid);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", Description.Title);
        }
    }
}
