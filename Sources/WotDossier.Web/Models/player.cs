using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class player
    {
        public player()
        {
            historicalbattlesstatistic = new HashSet<historicalbattlesstatistic>();
            randombattlesstatistic = new HashSet<randombattlesstatistic>();
            tank = new HashSet<tank>();
            teambattlesstatistic = new HashSet<teambattlesstatistic>();
        }

        public Guid uid { get; set; }
        public int accountid { get; set; }
        public DateTime creaded { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int rev { get; set; }
        public string server { get; set; }

        public virtual ICollection<historicalbattlesstatistic> historicalbattlesstatistic { get; set; }
        public virtual ICollection<randombattlesstatistic> randombattlesstatistic { get; set; }
        public virtual ICollection<tank> tank { get; set; }
        public virtual ICollection<teambattlesstatistic> teambattlesstatistic { get; set; }
    }
}
