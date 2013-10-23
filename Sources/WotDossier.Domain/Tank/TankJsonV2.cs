using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJsonV2
    {
        private IList<IList<string>> _fragsList = new List<IList<string>>();

        public TankDescription Description { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }
        
        /*8.9*/
        public StatisticJson8_9 A15x15 { get; set; }
        public StatisticJson8_9 A15x15_2 { get; set; }
        public StatisticJson8_9 Clan { get; set; }
        public StatisticJson8_9 Clan2 { get; set; }
        public StatisticJson8_9 Company { get; set; }
        public StatisticJson8_9 Company2 { get; set; }
        public StatisticJson8_9 A7x7 { get; set; }
        public AchievementsJson Achievements { get; set; }
        public CommonJson Common { get; set; }
        public TotalJson Total { get; set; }
        public MaxJson Max15x15 { get; set; }
        public MaxJson Max7x7 { get; set; }

        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        public byte[] Raw { get; set; }

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
