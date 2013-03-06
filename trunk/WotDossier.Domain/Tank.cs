using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain
{
    public class Tank
    {
        public string Name;
        private TankInfo _info;
        private TankContour _tankContour;
        public battle Battle { get; set; }
        public clan Clan { get; set; }
        public common Common { get; set; }
        public company Company { get; set; }
        public epic Epic { get; set; }
        public series Series { get; set; }
        public major Major { get; set; }
        public IList<IList<string>> Kills { get; set; }
        public Dictionary<int, string> Rawdata { get; set; }
        public special Special { get; set; }
        public tankdata Tankdata { get; set; }

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
    }
}
