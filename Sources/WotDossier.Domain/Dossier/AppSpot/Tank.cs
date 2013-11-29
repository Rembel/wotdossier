using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Dossier.AppSpot
{
    public class Tank
    {
        private IList<IList<string>> _kills = new List<IList<string>>();

        public TankStatistic _15x15 { get; set; }
        public TankStatistic _7x7 { get; set; }
        public Amounts amounts { get; set; }
        public Awards awards { get; set; }
        public Epic epics { get; set; }
        public int id { get; set; }
        public int version { get; set; }
        public int country { get; set; }
        public int last_time_played { get; set; }
        public int updated { get; set; }
        public int play_time { get; set; }
        public Series series { get; set; }
        public Medals medals { get; set; }

        public IList<IList<string>> frag_counts
        {
            get { return _kills; }
            set { _kills = value; }
        }

        public int UniqueId()
        {
            return Utils.ToUniqueId(country, id);
        }
    }
}
