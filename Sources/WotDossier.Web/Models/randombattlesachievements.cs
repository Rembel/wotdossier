using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class randombattlesachievements
    {
        public randombattlesachievements()
        {
            randombattlesstatistic = new HashSet<randombattlesstatistic>();
        }

        public Guid uid { get; set; }
        public int abrams { get; set; }
        public int arsonist { get; set; }
        public int billotte { get; set; }
        public int boelter { get; set; }
        public int bombardier { get; set; }
        public int bonecrusher { get; set; }
        public int brothersinarms { get; set; }
        public int brunopietro { get; set; }
        public int burda { get; set; }
        public int carius { get; set; }
        public int charmed { get; set; }
        public int confederate { get; set; }
        public int coolheaded { get; set; }
        public int crucialcontribution { get; set; }
        public int defender { get; set; }
        public int delanglade { get; set; }
        public int demolition { get; set; }
        public int duelist { get; set; }
        public int dumitru { get; set; }
        public int ekins { get; set; }
        public int even { get; set; }
        public int fadin { get; set; }
        public int fighter { get; set; }
        public int halonen { get; set; }
        public int heroesofrassenay { get; set; }
        public int hunter { get; set; }
        public int id { get; set; }
        public int impenetrable { get; set; }
        public int invader { get; set; }
        public int invincible { get; set; }
        public int kamikaze { get; set; }
        public int kay { get; set; }
        public int knispel { get; set; }
        public int kolobanov { get; set; }
        public int lafayettepool { get; set; }
        public int lavrinenko { get; set; }
        public int leclerk { get; set; }
        public int lehvaslaiho { get; set; }
        public int luckydevil { get; set; }
        public int maingun { get; set; }
        public int marksongun { get; set; }
        public int mastergunnerlongest { get; set; }
        public int maxaimerseries { get; set; }
        public int medalantispgfire { get; set; }
        public int medalcoolblood { get; set; }
        public int medalgore { get; set; }
        public int medalmonolith { get; set; }
        public int medalstark { get; set; }
        public int mousetrap { get; set; }
        public int movingavgdamage { get; set; }
        public int nikolas { get; set; }
        public int orlik { get; set; }
        public int oskin { get; set; }
        public int pascucci { get; set; }
        public int patrolduty { get; set; }
        public int pattonvalley { get; set; }
        public int poppel { get; set; }
        public int radleywalters { get; set; }
        public int raider { get; set; }
        public int ranger { get; set; }
        public int reaper { get; set; }
        public int rev { get; set; }
        public int scout { get; set; }
        public int sharpshooterlongest { get; set; }
        public int shoottokill { get; set; }
        public int sinai { get; set; }
        public int sniper { get; set; }
        public int sniper2 { get; set; }
        public int spartan { get; set; }
        public int steelwall { get; set; }
        public int survivor { get; set; }
        public int tamadayoshio { get; set; }
        public int tarczay { get; set; }
        public int warrior { get; set; }

        public virtual ICollection<randombattlesstatistic> randombattlesstatistic { get; set; }
    }
}
