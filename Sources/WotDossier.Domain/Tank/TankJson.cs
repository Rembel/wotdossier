using System.Collections.Generic;

namespace WotDossier.Domain.Tank
{
    public class TankJson
    {
        public string Name;
        private TankInfo _info;
        private TankContour _tankContour;
        public BattleJson Battle { get; set; }
        public ClanJson Clan { get; set; }
        public CommonJson Common { get; set; }
        public CompanyJson Company { get; set; }
        public EpicJson Epic { get; set; }
        public SeriesJson Series { get; set; }
        public MajorJson Major { get; set; }
        public IList<IList<string>> Kills { get; set; }
        public IEnumerable<FragsJson> Frags { get; set; }
        //public Dictionary<int, string> Rawdata { get; set; }
        public SpecialJson Special { get; set; }
        public TankDataJson Tankdata { get; set; }

        public TankInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public TankContour TankContour
        {
            get { return _tankContour; }
            set { _tankContour = value; }
        }

        public byte[] Raw { get; set; }
    }
}
