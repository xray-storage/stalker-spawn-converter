namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_Alife_Trader_Abstract
    public interface ICSE_Alife_Trader_Abstract
    {
        public uint Money { get; set; }
        public string Specific_Character { get; set; }
        public uint Trader_Flags { get; set; }
        public string Character_Profile { get; set; }
        public int Community_Index { get; set; }
        public int Rank { get; set; }
        public int Reputation { get; set; }
        public string Character_Name { get; set; }
        public byte Deadbody_Can_Take { get; set; }
        public byte Deadbody_Closed { get; set; }
    }
}
