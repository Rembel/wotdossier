using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Dossier.TankV29
{
    public class TankJson29
    {
        private IList<IList<string>> _kills = new List<IList<string>>();

        public BattleJson29 Battle { get; set; }
        public ClanJson29 Clan { get; set; }
        public CommonJson29 Common { get; set; }
        public CompanyJson29 Company { get; set; }
        public EpicJson29 Epic { get; set; }
        public SeriesJson29 Series { get; set; }
        public MajorJson29 Major { get; set; }

        public IList<IList<string>> Kills
        {
            get { return _kills; }
            set { _kills = value; }
        }

        public SpecialJson29 Special { get; set; }
        public StatisticJson29 Tankdata { get; set; }

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
