using System;

namespace WotDossier.Domain.Replay
{
    [Flags]
    public enum CrewMember : short
    {
        commander = 1,
        driver = 2,
        radioman = 4,
        gunner = 8,
        loader = 16
    }
}