namespace WotDossier.Domain
{
    /// <summary>
    /// Countries enum
    /// </summary>
    public enum Country : int
    {
        Unknown = -1,
        Ussr = 0,
        Germany = 1,
        Usa = 2,
        China = 3,
        France = 4,
        Uk = 5,
        Japan = 6,
        Czech = 7
    }

    /// <summary>
    /// Slot types enum
    /// </summary>
    public enum SlotType : int
    {
        Unknown = -1,
        Reserved,
        Vehicle,
        VehicleChassis,
        VehicleTurret,
        VehicleGun,
        VehicleEngine,
        VehicleFuelTank,
        VehicleRadio,
        Tankman,
        OptionalDevice,
        Shell,
        Equipment
    }
}