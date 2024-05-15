namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_ALife_Item_Weapon
    public interface ICSE_ALife_Item_Weapon
    {
        public ushort Current { get; set; }
        public ushort Elapsed { get; set; }
        public byte WPN_State { get; set; }
        public byte Addon_Flags { get; set; }
        public byte Ammo_Type { get; set; }
        public byte Elapsed_Grenades { get; set; }

        public byte UPD_Condition { get; set; }
        public byte UPD_WPN_Flags { get; set; }
        public ushort UPD_Elapsed { get; set; }
        public byte UPD_Flags { get; set; }
        public byte UPD_Ammo_Type { get; set; }
        public byte UPD_WPN_State { get; set; }
        public byte UPD_Zoom { get; set; }
    }
}
