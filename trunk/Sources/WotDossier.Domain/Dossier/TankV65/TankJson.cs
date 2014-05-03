using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Dossier.TankV65
{
    public class TankJson65
    {
        private IList<IList<string>> _fragsList = new List<IList<string>>();
        private StatisticJson65 _a15X15 = new StatisticJson65();
        private StatisticJson65 _clan = new StatisticJson65();
        private StatisticJson65_2 _clan2 = new StatisticJson65_2();
        private StatisticJson65 _company = new StatisticJson65();
        private StatisticJson65_2 _company2 = new StatisticJson65_2();
        private StatisticJson65 _a7X7 = new StatisticJson65();
        private AchievementsJson65 _achievements = new AchievementsJson65();
        private CommonJson65 _common = new CommonJson65();
        private TotalJson65 _total = new TotalJson65();
        private MaxJson65 _max15X15 = new MaxJson65();
        private MaxJson65 _max7X7 = new MaxJson65();
        private StatisticJson65_2 _a15X152 = new StatisticJson65_2();
        private Achievements7x7_65 _achievements7X7 = new Achievements7x7_65();

        /*8.9*/

        public StatisticJson65 A15x15
        {
            get { return _a15X15; }
            set { _a15X15 = value; }
        }

        public StatisticJson65_2 A15x15_2
        {
            get { return _a15X152; }
            set { _a15X152 = value; }
        }

        public StatisticJson65 Clan
        {
            get { return _clan; }
            set { _clan = value; }
        }

        public StatisticJson65_2 Clan2
        {
            get { return _clan2; }
            set { _clan2 = value; }
        }

        public StatisticJson65 Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public StatisticJson65_2 Company2
        {
            get { return _company2; }
            set { _company2 = value; }
        }

        public StatisticJson65 A7x7
        {
            get { return _a7X7; }
            set { _a7X7 = value; }
        }

        public AchievementsJson65 Achievements
        {
            get { return _achievements; }
            set { _achievements = value; }
        }

        public Achievements7x7_65 Achievements7X7
        {
            get { return _achievements7X7; }
            set { _achievements7X7 = value; }
        }

        public CommonJson65 Common
        {
            get { return _common; }
            set { _common = value; }
        }

        public TotalJson65 Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public MaxJson65 Max15x15
        {
            get { return _max15X15; }
            set { _max15X15 = value; }
        }

        public MaxJson65 Max7x7
        {
            get { return _max7X7; }
            set { _max7X7 = value; }
        }

        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        private int _uniqueId = -1;
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = Utils.ToUniqueId(Common.countryid, Common.tankid);
            }
            return _uniqueId;
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
