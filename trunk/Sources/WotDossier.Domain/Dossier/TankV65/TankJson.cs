using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Dossier.TankV65
{
    public class TankJson65
    {
        private IList<IList<string>> _fragsList = new List<IList<string>>();

        /*8.9*/
        public StatisticJson65 A15x15 { get; set; }
        public StatisticJson65_2 A15x15_2 { get; set; }
        public StatisticJson65 Clan { get; set; }
        public StatisticJson65_2 Clan2 { get; set; }
        public StatisticJson65 Company { get; set; }
        public StatisticJson65_2 Company2 { get; set; }
        public StatisticJson65 A7x7 { get; set; }
        public AchievementsJson65 Achievements { get; set; }
        public CommonJson65 Common { get; set; }
        public TotalJson65 Total { get; set; }
        public MaxJson65 Max15x15 { get; set; }
        public MaxJson65 Max7x7 { get; set; }

        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

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
            return string.Format("{0}", Common.tanktitle);
        }
    }
}
