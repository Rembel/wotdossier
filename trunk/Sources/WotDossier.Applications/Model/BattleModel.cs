using System;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Domain.Server;

namespace WotDossier.Applications.Model
{
    public class BattleModel
    {
        public BattleModel(BattleJson battle)
        {
            Type = battle.type;
            Time = Utils.UnixDateToDateTime(battle.time, true, true);
            Province = battle.provinceDescriptions[0].province_i18n;
            Map = battle.arenas[0].name_i18n;
        }

        public ClanBattleType Type { get; set; }
        public DateTime? Time { get; set; }
        public string Province { get; set; }
        public string Map { get; set; }
        public int GlobalMapId { get; set; }
    }
}
