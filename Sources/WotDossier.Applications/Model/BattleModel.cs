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
            Province = battle.province_name;
            //Map = battle.arenas[0].name_i18n;
            GlobalMapId = battle.front_id;
        }

        public ClanBattleType Type { get; set; }
        public DateTime? Time { get; set; }
        public string Province { get; set; }
        public string Map { get; set; }
        public string GlobalMapId { get; set; }
    }
}
