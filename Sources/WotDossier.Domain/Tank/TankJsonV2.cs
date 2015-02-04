using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    public class TankJson : ITankDescription
    {
        public static TankJson Initial = new TankJson
        {
            A15x15 = new StatisticJson(),
            Clan = new StatisticJson(),
            AchievementsClan = new AchievementsClan(),
            Company = new StatisticJson(),
            A7x7 = new StatisticJson(),
            Achievements = new AchievementsJson(),
            Common = new CommonJson(),
            Description = TankDescription.Unknown(),
            Frags = new BindingList<FragsJson>(),
            Achievements7x7 = new Achievements7x7(),
            AchievementsHistorical = new AchievementsHistorical(),
            Historical = new StatisticJson(),
            FortBattles = new StatisticJson(),
            FortAchievements = new AchievementsFort(),
            FortSorties = new StatisticJson()
        };

        private AchievementsClan _achievementsClan = new AchievementsClan();

        /// <summary>
        /// Gets or sets the clan achievements.
        /// </summary>
        public AchievementsClan AchievementsClan
        {
            get { return _achievementsClan; }
            set { _achievementsClan = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public TankDescription Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the common stat.
        /// </summary>
        public CommonJson Common { get; set; }

        /// <summary>
        /// Gets or sets the a15x15 stat.
        /// </summary>
        public StatisticJson A15x15 { get; set; }

        private StatisticJson _a7X7;
        /// <summary>
        /// Gets or sets the a7x7 stat.
        /// </summary>
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

        private StatisticJson _historical = new StatisticJson();
        /// <summary>
        /// Gets or sets the historical stat.
        /// </summary>
        public StatisticJson Historical
        {
            get { return _historical; }
            set { _historical = value; }
        }

        private StatisticJson _fortBattles = new StatisticJson();
        /// <summary>
        /// Gets or sets the fort battles stat.
        /// </summary>
        public StatisticJson FortBattles
        {
            get { return _fortBattles; }
            set { _fortBattles = value; }
        }

        private StatisticJson _fortSorties = new StatisticJson();
        /// <summary>
        /// Gets or sets the fort sorties.
        /// </summary>
        public StatisticJson FortSorties
        {
            get { return _fortSorties; }
            set { _fortSorties = value; }
        }

        private AchievementsFort _fortAchievements = new AchievementsFort();
        /// <summary>
        /// Gets or sets the fort achievements.
        /// </summary>
        public AchievementsFort FortAchievements
        {
            get { return _fortAchievements; }
            set { _fortAchievements = value; }
        }

        /// <summary>
        /// Gets or sets the clan stat.
        /// </summary>
        public StatisticJson Clan { get; set; }

        /// <summary>
        /// Gets or sets the company stat.
        /// </summary>
        public StatisticJson Company { get; set; }

        /// <summary>
        /// Gets or sets the achievements.
        /// </summary>
        public AchievementsJson Achievements { get; set; }

        private Achievements7x7 _achievements7X7 = new Achievements7x7();
        /// <summary>
        /// Gets or sets the achievements7x7.
        /// </summary>
        public Achievements7x7 Achievements7x7
        {
            get { return _achievements7X7; }
            set { _achievements7X7 = value; }
        }

        private AchievementsHistorical _achievementsHistorical = new AchievementsHistorical();
        /// <summary>
        /// Gets or sets the achievements historical.
        /// </summary>
        public AchievementsHistorical AchievementsHistorical
        {
            get { return _achievementsHistorical; }
            set { _achievementsHistorical = value; }
        }

        private IList<IList<string>> _fragsList = new List<IList<string>>();
        /// <summary>
        /// Gets or sets the frags list.
        /// </summary>
        public IList<IList<string>> FragsList
        {
            get { return _fragsList; }
            set { _fragsList = value; }
        }

        private IEnumerable<FragsJson> _frags = new List<FragsJson>();
        /// <summary>
        /// Gets or sets the frags.
        /// </summary>
        public IEnumerable<FragsJson> Frags
        {
            get { return _frags; }
            set { _frags = value; }
        }

        /// <summary>
        /// Gets or sets the raw.
        /// </summary>
        public byte[] Raw { get; set; }

        private int _uniqueId = -1;
        private TankDescription _description;

        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = Utils.ToUniqueId(Common.countryid, Common.tankid);
            }
            return _uniqueId;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Common.tanktitle;
        }
    }
}