using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Tank
{
    public class TankJson
    {
        public byte[] Raw { get; set; }
        public TankInfo Info { get; set; }
        public TankIcon Icon { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }

        public BattleJson Battle { get; set; }
        public ClanJson Clan { get; set; }
        public CommonJson Common { get; set; }
        public CompanyJson Company { get; set; }
        public EpicJson Epic { get; set; }
        public SeriesJson Series { get; set; }
        public MajorJson Major { get; set; }
        public IList<IList<string>> Kills { get; set; }
        public SpecialJson Special { get; set; }
        public TankDataJson Tankdata { get; set; }

        //public Dictionary<int, string> Rawdata { get; set; }
        public int UniqueId()
        {
            return Utils.ToUniqueId(Common.countryid, Common.tankid);
        }
    }
}
