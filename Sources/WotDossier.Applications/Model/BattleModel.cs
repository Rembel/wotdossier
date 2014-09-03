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
            if (battle.provinceDescriptions != null && battle.provinceDescriptions.Count > 0)
            {
                Province = battle.provinceDescriptions[0].province_i18n;
            }
            if (battle.arenas != null && battle.arenas.Length > 0)
            {
                Map = battle.arenas[0].name_i18n;
            }
            GlobalMapId = battle.GlobalMapId;
        }

        public ClanBattleType Type { get; set; }
        public DateTime? Time { get; set; }
        public string Province { get; set; }
        public string Map { get; set; }
        public string GlobalMapId { get; set; }
    }
}
