using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJson
    {
        private IList<IList<string>> _kills = new List<IList<string>>();
        public byte[] Raw { get; set; }
        public TankDescription Description { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }

        public BattleJson Battle { get; set; }
        public ClanJson Clan { get; set; }
        public CommonJson Common { get; set; }
        public CompanyJson Company { get; set; }
        public EpicJson Epic { get; set; }
        public SeriesJson Series { get; set; }
        public MajorJson Major { get; set; }

        public IList<IList<string>> Kills
        {
            get { return _kills; }
            set { _kills = value; }
        }

        public SpecialJson Special { get; set; }
        public StatisticJson Tankdata { get; set; }

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
