using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Breakable_Object : Template_Section, ICSE_ALife_Dynamic_Object_Visual
    {
        // =============== BASIC ===============
        // CSE_Alife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_Alife_Object_Breakable
        public float Health { get; set; }

        public Breakable_Object(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            Health = reader.ReadFloat();
        }
    }
}
