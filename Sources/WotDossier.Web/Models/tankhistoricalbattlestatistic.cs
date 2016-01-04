using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class tankhistoricalbattlestatistic
    {
        public Guid uid { get; set; }
        public int battlescount { get; set; }
        public int id { get; set; }
        public byte[] raw { get; set; }
        public int rev { get; set; }
        public int tankid { get; set; }
        public Guid tankuid { get; set; }
        public DateTime updated { get; set; }
        public int version { get; set; }

        public virtual tank tankuidNavigation { get; set; }
    }
}
