using System;
using System.Collections.Generic;

namespace WotDossier.Web
{
    public partial class randombattlesstatistic
    {
        public Guid uid { get; set; }
        public int? achievementsid { get; set; }
        public Guid? achievementsuid { get; set; }
        public double avglevel { get; set; }
        public double? battleavgxp { get; set; }
        public int battlescount { get; set; }
        public int capturepoints { get; set; }
        public int damagedealt { get; set; }
        public int damagetaken { get; set; }
        public int droppedcapturepoints { get; set; }
        public int frags { get; set; }
        public double? hitspercents { get; set; }
        public int id { get; set; }
        public int losses { get; set; }
        public int markofmastery { get; set; }
        public int maxdamage { get; set; }
        public int maxfrags { get; set; }
        public int maxxp { get; set; }
        public double performancerating { get; set; }
        public int playerid { get; set; }
        public Guid? playeruid { get; set; }
        public double rbr { get; set; }
        public int rev { get; set; }
        public int spotted { get; set; }
        public int survivedbattles { get; set; }
        public DateTime updated { get; set; }
        public int wins { get; set; }
        public double wn8rating { get; set; }
        public int xp { get; set; }

        public virtual randombattlesachievements achievementsuidNavigation { get; set; }
        public virtual player playeruidNavigation { get; set; }
    }
}
