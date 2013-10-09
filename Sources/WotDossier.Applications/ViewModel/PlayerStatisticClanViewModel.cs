using System;
using System.Windows.Media;
using WotDossier.Common;
using WotDossier.Domain.Player;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerStatisticClanViewModel
    {
        public PlayerStatisticClanViewModel(ClanData clan, string member_role, long member_since)
        {
            id = clan.clan_id;
            name = clan.name;
            abbreviation = string.Format("[{0}]", clan.abbreviation);
            color = (Color)ColorConverter.ConvertFromString(clan.clan_color ?? "#BD3838");

            since = Utils.UnixDateToDateTime(member_since);
            Clan = clan;
            role = member_role;
            small = clan.emblems.small;
            large = clan.emblems.large;
            bw_tank = clan.emblems.bw_tank;
            medium = clan.emblems.medium;

            Days = (DateTime.Now - since).Days;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public Color color { get; set; }

        public DateTime since { get; set; }
        public int Days { get; set; }
        public string role { get; set; }

        public string small { get; set; }
        public string large { get; set; }
        public string bw_tank { get; set; }
        public string medium { get; set; }

        public ClanData Clan { get; set; }
    }
}
