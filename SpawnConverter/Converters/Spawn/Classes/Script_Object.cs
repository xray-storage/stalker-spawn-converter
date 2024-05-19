using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Script_Object : Template_Section, ICSE_ALife_Dynamic_Object_Visual
    {
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        public Script_Object(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
        }
    }
}
