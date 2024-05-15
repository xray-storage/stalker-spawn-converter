using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Torrid_Zone : Template_Section, ICSE_Shape, ICSE_ALife_Space_Restrictor, ICSE_ALife_Custom_Zone, ICSE_Motion
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }

        // CSE_ALife_Space_Restrictor
        public byte Spase_Restrictor_Type { get; set; }

        // CSE_ALife_Custom_Zone
        public float Max_Power { get; set; }
        public uint Owner_ID { get; set; }
        public uint Enabled_Time { get; set; }
        public uint Disabled_Time { get; set; }
        public uint Start_Time_Shift { get; set; }

        // CSE_Motion
        public string Motion_Name { get; set; }

        // se_zones
        public byte? Last_Spawn_Time { get; set; }

        public Torrid_Zone(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            CSE_ALife_Space_Restrictor.ReadBasic(reader, this);
            CSE_ALife_Custom_Zone.ReadBasic(reader, this);
            CSE_Motion.ReadBasic(reader, this);

            if (reader.ChunkReamains() > 0)
            {
                Last_Spawn_Time = reader.ReadByte();
            }
        }
    }
}
