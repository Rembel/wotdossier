using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJsonV2
    {
        private IList<IList<string>> _fragsList = new List<IList<string>>();
        private StatisticJson8_9 _a15X15;
        private StatisticJson8_9 _a15X152;
        private StatisticJson8_9 _clan;
        private StatisticJson8_9 _clan2;
        private StatisticJson8_9 _company;
        private StatisticJson8_9 _company2;
        private StatisticJson8_9 _a7X7;
        private AchievementsJson _achievements;
        private CommonJson _common;
        private TotalJson _total;
        private MaxJson _max15X15;
        private MaxJson _max7X7;

        private TankDescription _description;
        private IEnumerable<FragsJson> _frags;

        public TankDescription Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public IEnumerable<FragsJson> Frags
        {
            get { return _frags; }
            set { _frags = value; }
        }
        
        /*8.9*/
        public StatisticJson8_9 A15x15
        {
            get { return _a15X15; }
            set { _a15X15 = value; }
        }

        public StatisticJson8_9 A15x15_2
        {
            get { return _a15X152; }
            set { _a15X152 = value; }
        }

        public StatisticJson8_9 Clan
        {
            get { return _clan; }
            set { _clan = value; }
        }

        public StatisticJson8_9 Clan2
        {
            get { return _clan2; }
            set { _clan2 = value; }
        }

        public StatisticJson8_9 Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public StatisticJson8_9 Company2
        {
            get { return _company2; }
            set { _company2 = value; }
        }

        public StatisticJson8_9 A7x7
        {
            get { return _a7X7; }
            set { _a7X7 = value; }
        }

        public AchievementsJson Achievements
        {
            get { return _achievements; }
            set { _achievements = value; }
        }

        public CommonJson Common
        {
            get { return _common; }
            set { _common = value; }
        }

        public TotalJson Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public MaxJson Max15x15
        {
            get { return _max15X15; }
            set { _max15X15 = value; }
        }

        public MaxJson Max7x7
        {
            get { return _max7X7; }
            set { _max7X7 = value; }
        }

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
