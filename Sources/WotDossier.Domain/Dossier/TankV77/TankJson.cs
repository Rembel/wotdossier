using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Dossier.TankV77
{
    /*0.9.0*/
    public class TankJson77
    {
        private IList<IList<string>> _fragsList = new List<IList<string>>();
        private StatisticJson77 _a15X15 = new StatisticJson77();
        private StatisticJson77 _clan = new StatisticJson77();
        private StatisticJson77_2 _clan2 = new StatisticJson77_2();
        private StatisticJson77 _company = new StatisticJson77();
        private StatisticJson77_2 _company2 = new StatisticJson77_2();
        private StatisticJson77 _a7X7 = new StatisticJson77();
        private AchievementsJson77 _achievements = new AchievementsJson77();
        private CommonJson77 _common = new CommonJson77();
        private TotalJson77 _total = new TotalJson77();
        private MaxJson77 _max15X15 = new MaxJson77();
        private MaxJson77 _max7X7 = new MaxJson77();
        private StatisticJson77_2 _a15X152 = new StatisticJson77_2();
        private Achievements7x7_77 _achievements7X7 = new Achievements7x7_77();
        private StatisticJson77 _historical = new StatisticJson77();
        private AchievementsHistorical_77 _historicalAchievements = new AchievementsHistorical_77();
        private MaxJson77 _maxHistorical = new MaxJson77();
        public StatisticJson77 A15x15
        {
            get { return _a15X15; }
            set { _a15X15 = value; }
        }

        public StatisticJson77 Historical
        {
            get { return _historical; }
            set { _historical = value; }
        }

        public StatisticJson77_2 A15x15_2
        {
            get { return _a15X152; }
            set { _a15X152 = value; }
        }

        public StatisticJson77 Clan
        {
            get { return _clan; }
            set { _clan = value; }
        }

        public StatisticJson77_2 Clan2
        {
            get { return _clan2; }
            set { _clan2 = value; }
        }

        public StatisticJson77 Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public StatisticJson77_2 Company2
        {
            get { return _company2; }
            set { _company2 = value; }
        }

        public StatisticJson77 A7x7
        {
            get { return _a7X7; }
            set { _a7X7 = value; }
        }

        public AchievementsJson77 Achievements
        {
            get { return _achievements; }
            set { _achievements = value; }
        }

        public Achievements7x7_77 Achievements7X7
        {
            get { return _achievements7X7; }
            set { _achievements7X7 = value; }
        }

        public CommonJson77 Common
        {
            get { return _common; }
            set { _common = value; }
        }

        public TotalJson77 Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public MaxJson77 Max15x15
        {
            get { return _max15X15; }
            set { _max15X15 = value; }
        }

        public MaxJson77 Max7x7
        {
            get { return _max7X7; }
            set { _max7X7 = value; }
        }

        public MaxJson77 MaxHistorical
        {
            get { return _maxHistorical; }
            set { _maxHistorical = value; }
        }

        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        public AchievementsHistorical_77 HistoricalAchievements
        {
            get { return _historicalAchievements; }
            set { _historicalAchievements = value; }
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
