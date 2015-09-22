using System;
using WotDossier.Applications.Logic;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Server;

namespace WotDossier.Applications.Model
{
    public class BattleModel
    {
        private const string FormatClanName = "[{0}] {1}";

        public BattleModel(BattleJson battle)
        {
            Type = battle.type;
            Time = Utils.UnixDateToDateTime(battle.time, true, true);
            Province = battle.province_name;
            //Map = battle.arenas[0].name_i18n;
            GlobalMapId = battle.front_id;
        }

        public BattleModel(StrongholdBattleJson battle)
        {
            Type = battle.battle_type == StrongholBattleType.defense ? ClanBattleType.meeting_engagement : ClanBattleType.for_province;
            Time = Utils.UnixDateToDateTime(battle.battle_planned_date, true, true);
            Attacker = string.Format(FormatClanName, battle.attacker_clan_tag, battle.attacker_clan_name);
            AttackerId = battle.attacker_clan_id;
            AttackerTag = battle.attacker_clan_tag;
            Defender = string.Format(FormatClanName, battle.defender_clan_tag, battle.defender_clan_name);
            DefenderId = battle.defender_clan_id;
            DefenderTag = battle.defender_clan_tag;


        }

        public string DefenderLink
        {
            get { return string.Format(RatingHelper.NOOBMETER_CLAN_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, DefenderTag, DefenderId); }
        }

        public string AttackerLink
        {
            get { return string.Format(RatingHelper.NOOBMETER_CLAN_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, AttackerTag, AttackerId); }
        }

        public ClanBattleType Type { get; set; }
        public DateTime? Time { get; set; }
        public string Province { get; set; }
        public string Map { get; set; }
        public string Attacker { get; set; }
        public string AttackerTag { get; set; }
        public int AttackerId { get; set; }
        public string Defender { get; set; }
        public string DefenderTag { get; set; }
        public int DefenderId { get; set; }
        public string GlobalMapId { get; set; }
    }
}
