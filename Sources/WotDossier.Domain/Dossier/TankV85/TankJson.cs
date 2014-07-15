using System.Collections.Generic;
using WotDossier.Common;
using WotDossier.Domain.Dossier.TankV77;

namespace WotDossier.Domain.Dossier.TankV85
{
    /*0.9.2*/
    public class TankJson85
    {
        private CommonJson77 _common = new CommonJson77();

        public CommonJson77 Common
        {
            get { return _common; }
            set { _common = value; }
        }

        private StatisticJson77 _a15X15 = new StatisticJson77();

        public StatisticJson77 A15x15
        {
            get { return _a15X15; }
            set { _a15X15 = value; }
        }

        private StatisticJson77_2 _a15X152 = new StatisticJson77_2();

        public StatisticJson77_2 A15x15_2
        {
            get { return _a15X152; }
            set { _a15X152 = value; }
        }

        private MaxJson77 _max15X15 = new MaxJson77();

        public MaxJson77 Max15x15
        {
            get { return _max15X15; }
            set { _max15X15 = value; }
        }

        private StatisticJson77 _a7X7 = new StatisticJson77();

        public StatisticJson77 A7x7
        {
            get { return _a7X7; }
            set { _a7X7 = value; }
        }

        private MaxJson77 _max7X7 = new MaxJson77();

        public MaxJson77 Max7x7
        {
            get { return _max7X7; }
            set { _max7X7 = value; }
        }

        private StatisticJson77 _historical = new StatisticJson77();

        public StatisticJson77 Historical
        {
            get { return _historical; }
            set { _historical = value; }
        }

        private MaxJson77 _maxHistorical = new MaxJson77();

        public MaxJson77 MaxHistorical
        {
            get { return _maxHistorical; }
            set { _maxHistorical = value; }
        }

        private StatisticJson77 _clan = new StatisticJson77();

        public StatisticJson77 Clan
        {
            get { return _clan; }
            set { _clan = value; }
        }

        private StatisticJson77_2 _clan2 = new StatisticJson77_2();

        public StatisticJson77_2 Clan2
        {
            get { return _clan2; }
            set { _clan2 = value; }
        }

        private StatisticJson77 _company = new StatisticJson77();

        public StatisticJson77 Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private StatisticJson77_2 _company2 = new StatisticJson77_2();

        public StatisticJson77_2 Company2
        {
            get { return _company2; }
            set { _company2 = value; }
        }

        private TotalJson77 _total = new TotalJson77();

        public TotalJson77 Total
        {
            get { return _total; }
            set { _total = value; }
        }

        #region Achievements

        private Achievements_85 _achievements = new Achievements_85();

        public Achievements_85 Achievements
        {
            get { return _achievements; }
            set { _achievements = value; }
        }

        private Achievements7x7_85 _achievements7X7 = new Achievements7x7_85();

        public Achievements7x7_85 Achievements7X7
        {
            get { return _achievements7X7; }
            set { _achievements7X7 = value; }
        }

        private AchievementsSingle_85 _singleAchievements = new AchievementsSingle_85();

        public AchievementsSingle_85 SingleAchievements
        {
            get { return _singleAchievements; }
            set { _singleAchievements = value; }
        }

        private AchievementsClan_85 _clanAchievements = new AchievementsClan_85();
        public AchievementsClan_85 ClanAchievements
        {
            get { return _clanAchievements; }
            set { _clanAchievements = value; }
        }

        private AchievementsHistorical_85 _historicalAchievements = new AchievementsHistorical_85();

        public AchievementsHistorical_85 HistoricalAchievements
        {
            get { return _historicalAchievements; }
            set { _historicalAchievements = value; }
        }

        #endregion

        private IList<IList<string>> _fragsList = new List<IList<string>>();

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
