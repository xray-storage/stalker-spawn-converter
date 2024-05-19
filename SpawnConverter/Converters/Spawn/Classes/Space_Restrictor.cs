using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Space_Restrictor : Template_Section, ICSE_Shape, ICSE_ALife_Space_Restrictor
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }

        // CSE_ALife_Space_Restrictor
        public byte Spase_Restrictor_Type { get; set; }

        public Space_Restrictor(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            CSE_ALife_Space_Restrictor.ReadBasic(reader, this);
        }
    }
}
