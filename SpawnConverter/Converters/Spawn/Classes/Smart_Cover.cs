using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Smart_Cover : Template_Section, ICSE_Shape
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }

        // CSE_Smart_Cover
        public string Description { get; set; }
        public float Hold_Position_Time { get; set; }
        public float Enter_MIN_Enemy_Distance { get; set; }
        public float Exit_MIN_Enemy_Distance { get; set; }
        public byte Is_Combat_Cover { get; set; }
        public byte Can_Fire { get; set; }

        // se_smart_cover
        public string Last_Description { get; set; } = null;
        public byte? Loophole_Count { get; set; } = null;
        public Loophole[] Loophole { get; set; } = null;

        public Smart_Cover(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);

            Description = reader.ReadStringZ();
            Hold_Position_Time = reader.ReadFloat();
            Enter_MIN_Enemy_Distance = reader.ReadFloat();
            Exit_MIN_Enemy_Distance = reader.ReadFloat();
            Is_Combat_Cover = reader.ReadByte();
            Can_Fire = reader.ReadByte();
            
            if (reader.ChunkReamains() > 0)
            {
                Last_Description = reader.ReadStringZ();
                Loophole_Count = reader.ReadByte();
                Loophole = reader.ReadLoopholes((byte)Loophole_Count);
            }
        }
    }
}
