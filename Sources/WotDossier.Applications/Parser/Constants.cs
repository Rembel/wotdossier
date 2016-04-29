namespace WotDossier.Applications.Parser
{
    public class ARENA_UPDATE
    {
        public const uint VEHICLE_LIST = 0x01;
        public const uint VEHICLE_ADDED = 0x02;
        public const uint PERIOD = 0x03;
        public const uint STATISTICS = 0x04;
        public const uint VEHICLE_STATISTICS = 0x05;
        public const uint VEHICLE_KILLED = 0x06;
        public const uint AVATAR_READY = 0x07;
        public const uint BASE_POINTS = 0x08;
        public const uint BASE_CAPTURED = 0x09;
        public const uint TEAM_KILLER = 0x10;
        public const uint VEHICLE_UPDATED = 0x11;
        public const uint COMBAT_EQUIPMENT_USED = 0x12;
        public const uint RESPAWN_AVAILABLE_VEHICLES = 0x13;
        public const uint RESPAWN_COOLDOWNS = 0x14;
        public const uint RESPAWN_RANDOM_VEHICLE = 0x15;
        public const uint FLAG_TEAMS = 0x16;
        public const uint FLAG_STATE_CHANGED = 0x17;
        public const uint RESPAWN_RESURRECTED = 0x18;
        public const uint INTERACTIVE_STATS = 0x19;
        public const uint DISAPPEAR_BEFORE_RESPAWN = 0x20;
        public const uint RESOURCE_POINT_STATE_CHANGED = 0x21;
        public const uint OWN_VEHICLE_INSIDE_RP = 0x22;
        public const uint OWN_VEHICLE_LOCKED_FOR_RP = 0x23;
        public const uint FIRST_APRIL_ACTION_SCHEDULED = 0x24;
    }

    public class ARENA_PERIOD
    {
        public const uint IDLE = 0x00;
        public const uint WAITING = 0x01;
        public const uint PREBATTLE = 0x02;
        public const uint BATTLE = 0x03;
        public const uint AFTERBATTLE = 0x04;
    }
}
