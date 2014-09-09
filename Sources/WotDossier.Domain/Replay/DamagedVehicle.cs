using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class DamagedVehicle : StatBase
    {
        private int _killed;
        private List<Device> _tankDamageCrits;
        private List<CrewMember> _crewCrits;
        private List<Device> _tankCrits;

        //up to Version 0.8.5: The total number of critical hits scored on this vehicle
        //since Version 0.8.6: Packed value. 
        [DataMember]
        public int crits { get; set; }

        public List<Device> tankDamageCrits
        {
            get
            {
                if (_tankDamageCrits == null)
                {
                    _tankDamageCrits = new List<Device>();
                    int critsFlags = crits >> 12 & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankDamageCrits.Add(device);
                        }
                    }
                }
                return _tankDamageCrits;
            }
        }



        public List<Device> tankCrits
        {
            get
            {
                if (_tankCrits == null)
                {
                    _tankCrits = new List<Device>();
                    int critsFlags = crits & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankCrits.Add(device);
                        }
                    }
                }
                return _tankCrits;
            }
        }

        public List<CrewMember> crewCrits
        {
            get
            {
                if (_crewCrits == null)
                {
                    _crewCrits = new List<CrewMember>();
                    int critsFlags = crits >> 24 & 255;
                    Array array = Enum.GetValues(typeof(CrewMember));
                    foreach (CrewMember member in array)
                    {
                        if ((critsFlags & (short)member) == (short)member)
                        {
                            _crewCrits.Add(member);
                        }
                    }
                }
                return _crewCrits;
            }
        }

        [DataMember]
        public int fire { get; set; }

        [DataMember]
        //NOTE: Obsolete - "0.8.6"
        public int killed
        {
            get
            {
                if (deathReason >= 0)
                {
                    return 1;
                }
                return _killed;
            }
            set { _killed = value; }
        }
    }
}