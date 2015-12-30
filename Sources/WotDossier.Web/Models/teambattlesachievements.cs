using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class teambattlesachievements
    {
        public teambattlesachievements()
        {
            teambattlesstatistic = new HashSet<teambattlesstatistic>();
        }

        public Guid uid { get; set; }
        public int armoredfist { get; set; }
        public int crucialshot { get; set; }
        public int crucialshotmedal { get; set; }
        public int fightingreconnaissance { get; set; }
        public int fightingreconnaissancemedal { get; set; }
        public int fireandsteel { get; set; }
        public int fireandsteelmedal { get; set; }
        public int fortacticaloperations { get; set; }
        public int geniusforwar { get; set; }
        public int geniusforwarmedal { get; set; }
        public int godofwar { get; set; }
        public int heavyfire { get; set; }
        public int heavyfiremedal { get; set; }
        public int id { get; set; }
        public int kingofthehill { get; set; }
        public int maxtacticalbreakthroughseries { get; set; }
        public int nomansland { get; set; }
        public int promisingfighter { get; set; }
        public int promisingfightermedal { get; set; }
        public int pyromaniac { get; set; }
        public int pyromaniacmedal { get; set; }
        public int ranger { get; set; }
        public int rangermedal { get; set; }
        public int rev { get; set; }
        public int tacticalbreakthrough { get; set; }
        public int tacticalbreakthroughseries { get; set; }
        public int willtowinspirit { get; set; }
        public int wolfamongsheep { get; set; }
        public int wolfamongsheepmedal { get; set; }

        public virtual ICollection<teambattlesstatistic> teambattlesstatistic { get; set; }
    }
}
