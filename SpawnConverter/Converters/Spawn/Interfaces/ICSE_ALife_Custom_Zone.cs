namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_ALife_Custom_Zone
    public interface ICSE_ALife_Custom_Zone
    {
        public float Max_Power { get; set; }
        public uint Owner_ID { get; set; }
        public uint Enabled_Time { get; set; }
        public uint Disabled_Time { get; set; }
        public uint Start_Time_Shift { get; set; }
    }
}
