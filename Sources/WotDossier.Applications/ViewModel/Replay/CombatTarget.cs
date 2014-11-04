using System;
using System.Collections.Generic;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class CombatTarget
    {
        private static readonly Version _version085 = new Version("0.8.5");

        public TeamMember TeamMember { get; set; }

        public int Crits { get; set; }

        public string CritsTooltip { get; set; }

        public int DamageAssisted { get; set; }

        public string DamageAssistedTooltip { get; set; }

        public int DamageDealt { get; set; }

        public string DamageDealtTooltip { get; set; }

        public int Fire { get; set; }

        public int HeHits { get; set; }

        public int Hits { get; set; }

        public int Killed { get; set; }

        public int Pierced { get; set; }

        public int Spotted { get; set; }

        public string SpottedTooltip { get; set; }

        public bool TeamMate { get; set; }

        public CombatTarget(KeyValuePair<long, DamagedVehicle> vehicleDamage, TeamMember teamMember, Version version)
        {
            TeamMember = teamMember;

            Crits = GetCritsCount(vehicleDamage.Value, version);
            CritsTooltip = string.Format(Resources.Resources.Tooltip_Replay_CriticalDamage, Crits);
            DamageAssisted = vehicleDamage.Value.damageAssisted;
            DamageAssistedTooltip = string.Format(Resources.Resources.Tooltip_Replay_AlliesDamage, DamageAssisted);
            DamageDealt = vehicleDamage.Value.damageDealt;
            DamageDealtTooltip = string.Format(Resources.Resources.Tooltip_Replay_Damage, DamageDealt);
            Fire = vehicleDamage.Value.fire;
            HeHits = vehicleDamage.Value.he_hits;
            Hits = vehicleDamage.Value.hits;
            Killed = vehicleDamage.Value.killed;
            Pierced = vehicleDamage.Value.pierced;
            Spotted = vehicleDamage.Value.spotted;
            SpottedTooltip = Spotted > 0 ? Resources.Resources.Tooltip_Replay_Detected : string.Empty;
            TeamMate = teamMember.TeamMate;
        }

        private int GetCritsCount(DamagedVehicle vehicleDamage, Version version)
        {
            //up to Version 0.8.5: The total number of critical Hits scored on this vehicle
            //since Version 0.8.6: Packed value. 
            if (version > _version085)
            {
                return vehicleDamage.tankDamageCrits.Count + vehicleDamage.crewCrits.Count + vehicleDamage.tankCrits.Count;
            }
            return Crits;
        }
    }
}