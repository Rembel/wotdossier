using System;
using System.Windows.Media;
using WotDossier.Common;
using WotDossier.Domain.Player;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerStatisticClanViewModel
    {
        public PlayerStatisticClanViewModel(Clan clan)
        {
            id = clan.clan.id;
            name = clan.clan.name;
            if (!string.IsNullOrEmpty(clan.clan.abbreviation))
            {
                abbreviation = string.Format("[{0}]", clan.clan.abbreviation);
            }
            if (!string.IsNullOrEmpty(clan.clan.color))
            {
                color = (Color) ColorConverter.ConvertFromString(clan.clan.color);
            }

            since = Utils.UnixDateToDateTime((long)clan.member.since);
            role = clan.member.role;
            small = clan.clan.emblems_urls.small;
            large = clan.clan.emblems_urls.large;
            bw_tank = clan.clan.emblems_urls.bw_tank;
            medium = clan.clan.emblems_urls.medium;

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
    }
}
