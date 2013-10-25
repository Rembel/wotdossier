using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJson
    {
        public StatisticJson A15x15 { get; set; }
        public StatisticJson Clan { get; set; }
        public StatisticJson Company { get; set; }
        public StatisticJson A7x7 { get; set; }
        public AchievementsJson Achievements { get; set; }
        public CommonJson Common { get; set; }

        private IList<IList<string>> _fragsList = new List<IList<string>>();
        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        public TankDescription Description { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }
        public byte[] Raw { get; set; }

        /// <summary>
        /// Get tank unique id.
        /// </summary>
        /// <returns></returns>
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
