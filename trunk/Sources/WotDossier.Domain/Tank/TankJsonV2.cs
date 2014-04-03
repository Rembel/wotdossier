using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJson
    {
        public static TankJson Initial = new TankJson
        {
            A15x15 = new StatisticJson(),
            Clan = new StatisticJson(),
            Company = new StatisticJson(),
            A7x7 = new StatisticJson(),
            Achievements = new AchievementsJson(),
            Common = new CommonJson(),
            Description = new TankDescription(),
            Frags = new BindingList<FragsJson>(),
            Achievements7x7 = new Achievements7x7(),
            AchievementsHistorical = new AchievementsHistorical(),
            Historical = new StatisticJson()
        };

        private StatisticJson _a7X7;
        private Achievements7x7 _achievements7X7 = new Achievements7x7();
        private AchievementsHistorical _achievementsHistorical = new AchievementsHistorical();
        private IList<IList<string>> _fragsList = new List<IList<string>>();
        private StatisticJson _historical = new StatisticJson();

        public StatisticJson A15x15 { get; set; }
        public StatisticJson Clan { get; set; }
        public StatisticJson Company { get; set; }

        public StatisticJson A7x7
        {
            get
            {
                if (_a7X7 == null)
                {
                    _a7X7 = new StatisticJson();
                }
                return _a7X7;
            }
            set { _a7X7 = value; }
        }

        public AchievementsJson Achievements { get; set; }

        public Achievements7x7 Achievements7x7
        {
            get { return _achievements7X7; }
            set { _achievements7X7 = value; }
        }

        public CommonJson Common { get; set; }

        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        public TankDescription Description { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }
        public byte[] Raw { get; set; }

        public AchievementsHistorical AchievementsHistorical
        {
            get { return _achievementsHistorical; }
            set { _achievementsHistorical = value; }
        }

        public StatisticJson Historical
        {
            get { return _historical; }
            set { _historical = value; }
        }

        /// <summary>
        ///     Get tank unique id.
        /// </summary>
        /// <returns></returns>
        public int UniqueId()
        {
            return Utils.ToUniqueId(Common.countryid, Common.tankid);
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", Description.Title);
        }
    }
}