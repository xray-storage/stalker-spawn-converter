using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Climable_Object : Template_Section, ICSE_Shape
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }


        // CSE_ALife_Object_Climable
        public string Material { get; set; }


        public Climable_Object(XrFileReader reader) : base(reader) => TypeName = GetType().Name;


        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            Material = reader.ReadStringZ();
        }
    }
}
