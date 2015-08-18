namespace WotDossier.Domain.Replay
{
    /// <summary>
    /// gameplayID = arenaTypeID >> 16
    /// mapID = arenaTypeID & 32767
    /// </summary>
    public enum Gameplay
    {
        ctf = 0,
        domination = 256,
        assault = 512,
        nations = 1024,
        assault2 = 2048,
        fallout = 4096,
        fallout2 = 8192,
        fallout3 = 16384,
        fallout4 = 32768,
        fallout5 = 65536,
        fallout6 = 131072,
    }
}
