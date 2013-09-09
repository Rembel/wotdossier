using System;

namespace WotDossier.Domain.Replay
{
    [Flags]
    public enum Device : short
    {
        engine = 1,
        ammoBay = 2,
        fuelTank = 4,
        radio = 8,
        track = 16,
        gun = 32,
        turretRotator = 64,
        surveyingDevice = 128
    }
}