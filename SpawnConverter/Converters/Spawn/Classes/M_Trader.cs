using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class M_Trader : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_Alife_Trader_Abstract
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_ALife_Trader_Abstract
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

        public M_Trader(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_Alife_Trader_Abstract.ReadBasic(reader, this);
        }
    }
}
