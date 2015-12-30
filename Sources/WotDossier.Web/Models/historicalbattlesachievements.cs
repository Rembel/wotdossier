using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class historicalbattlesachievements
    {
        public historicalbattlesachievements()
        {
            historicalbattlesstatistic = new HashSet<historicalbattlesstatistic>();
        }

        public Guid uid { get; set; }
        public int bothsideswins { get; set; }
        public int guardsman { get; set; }
        public int id { get; set; }
        public int makerofhistory { get; set; }
        public int rev { get; set; }
        public int weakvehicleswins { get; set; }

        public virtual ICollection<historicalbattlesstatistic> historicalbattlesstatistic { get; set; }
    }
}
