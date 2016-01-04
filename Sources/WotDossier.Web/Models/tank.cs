using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class tank
    {
        public tank()
        {
            tankhistoricalbattlestatistic = new HashSet<tankhistoricalbattlestatistic>();
            tankrandombattlesstatistic = new HashSet<tankrandombattlesstatistic>();
            tankteambattlestatistic = new HashSet<tankteambattlestatistic>();
        }

        public Guid uid { get; set; }
        public int countryid { get; set; }
        public string icon { get; set; }
        public int id { get; set; }
        public bool isfavorite { get; set; }
        public bool ispremium { get; set; }
        public string name { get; set; }
        public int playerid { get; set; }
        public Guid playeruid { get; set; }
        public int rev { get; set; }
        public int tankid { get; set; }
        public int tanktype { get; set; }
        public int tier { get; set; }

        public virtual ICollection<tankhistoricalbattlestatistic> tankhistoricalbattlestatistic { get; set; }
        public virtual ICollection<tankrandombattlesstatistic> tankrandombattlesstatistic { get; set; }
        public virtual ICollection<tankteambattlestatistic> tankteambattlestatistic { get; set; }
        public virtual player playeruidNavigation { get; set; }
    }
}
