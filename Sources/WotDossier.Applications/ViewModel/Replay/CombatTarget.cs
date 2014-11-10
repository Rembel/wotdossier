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

        public int DamageAssisted { get; set; }

        public int DamageAssistedTrack { get; set; }
        
        public int DamageAssistedRadio { get; set; }

        public int DamageDealt { get; set; }

        public int Fire { get; set; }

        public int HeHits { get; set; }

        public int Hits { get; set; }

        public int Killed { get; set; }

        public int Pierced { get; set; }

        public int Spotted { get; set; }

        public bool TeamMate { get; set; }

        public CombatTarget(KeyValuePair<long, DamagedVehicle> vehicleDamage, TeamMember teamMember, Version version)
        {
            TeamMember = teamMember;

            Crits = GetCritsCount(vehicleDamage.Value, version);
            DamageAssisted = vehicleDamage.Value.damageAssisted;
            DamageAssistedTrack = vehicleDamage.Value.damageAssistedTrack;
            DamageAssistedRadio = vehicleDamage.Value.damageAssistedRadio;
            DamageDealt = vehicleDamage.Value.damageDealt;
            Fire = vehicleDamage.Value.fire;
            HeHits = vehicleDamage.Value.he_hits;
            Hits = vehicleDamage.Value.hits;
            Killed = vehicleDamage.Value.killed;
            Pierced = vehicleDamage.Value.pierced;
            Spotted = vehicleDamage.Value.spotted;
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