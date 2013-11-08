using System.Collections.Generic;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class CombatTarget
    {
        public TeamMember TeamMember { get; set; }

        public CombatTarget(KeyValuePair<long, DamagedVehicle> vehicleDamage, TeamMember teamMember)
        {
            TeamMember = teamMember;

            crits = vehicleDamage.Value.critsCount;
            critsTooltip = string.Format(Resources.Resources.Tooltip_Replay_CriticalDamage, crits);
            damageAssisted = vehicleDamage.Value.damageAssisted;
            damageAssistedTooltip = string.Format(Resources.Resources.Tooltip_Replay_AlliesDamage, damageAssisted);
            damageDealt = vehicleDamage.Value.damageDealt;
            damageDealtTooltip = string.Format(Resources.Resources.Tooltip_Replay_Damage, damageDealt);
            fire = vehicleDamage.Value.fire;
            he_hits = vehicleDamage.Value.he_hits;
            hits = vehicleDamage.Value.hits;
            killed = vehicleDamage.Value.killed;
            pierced = vehicleDamage.Value.pierced;
            spotted = vehicleDamage.Value.spotted;
            spottedTooltip = spotted > 0 ? Resources.Resources.Tooltip_Replay_Detected : string.Empty;
            TeamMate = teamMember.TeamMate;
        }

        public int crits { get; set; }
        public string critsTooltip { get; set; }
        public int damageAssisted { get; set; }
        public string damageAssistedTooltip { get; set; }
        public int damageDealt { get; set; }
        public string damageDealtTooltip { get; set; }
        public int fire { get; set; }
        public int he_hits { get; set; }
        public int hits { get; set; }
        public int killed { get; set; }
        public int pierced { get; set; }
        public int spotted { get; set; }
        public string spottedTooltip { get; set; }
        public bool TeamMate { get; set; }
    }
}