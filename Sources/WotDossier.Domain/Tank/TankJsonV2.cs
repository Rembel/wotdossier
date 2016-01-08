using System.Collections.Generic;
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
            Frags = new List<FragsJson>(),
            Achievements7x7 = new Achievements7x7(),
            AchievementsHistorical = new AchievementsHistorical(),
            Historical = new StatisticJson(),
            FortBattles = new StatisticJson(),
            FortAchievements = new AchievementsFort(),
            FortSorties = new StatisticJson()
        };

        /// <summary>
        /// Gets or sets the clan achievements.
        /// </summary>
        public AchievementsClan AchievementsClan { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public TankDescription Description { get; set; }

        /// <summary>
        /// Gets or sets the common stat.
        /// </summary>
        public CommonJson Common { get; set; }

        /// <summary>
        /// Gets or sets the a15x15 stat.
        /// </summary>
        public StatisticJson A15x15 { get; set; }

        /// <summary>
        /// Gets or sets the a7x7 stat.
        /// </summary>
        public StatisticJson A7x7 { get; set; }

        /// <summary>
        /// Gets or sets the historical stat.
        /// </summary>
        public StatisticJson Historical { get; set; }

        /// <summary>
        /// Gets or sets the fort battles stat.
        /// </summary>
        public StatisticJson FortBattles { get; set; }

        /// <summary>
        /// Gets or sets the fort sorties.
        /// </summary>
        public StatisticJson FortSorties { get; set; }

        /// <summary>
        /// Gets or sets the fort achievements.
        /// </summary>
        public AchievementsFort FortAchievements { get; set; }

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

        /// <summary>
        /// Gets or sets the achievements7x7.
        /// </summary>
        public Achievements7x7 Achievements7x7 { get; set; }

        /// <summary>
        /// Gets or sets the achievements historical.
        /// </summary>
        public AchievementsHistorical AchievementsHistorical { get; set; }

        /// <summary>
        /// Gets or sets the frags list.
        /// </summary>
        public IList<IList<string>> FragsList { get; set; }

        /// <summary>
        /// Gets or sets the frags.
        /// </summary>
        public IEnumerable<FragsJson> Frags { get; set; }

        /// <summary>
        /// Gets or sets the raw.
        /// </summary>
        public byte[] Raw { get; set; }

        private int _uniqueId = -1;
        

        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(Common.countryid, Common.tankid);
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