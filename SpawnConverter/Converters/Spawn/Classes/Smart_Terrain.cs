using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Smart_Terrain : Template_Section, ICSE_Shape, ICSE_ALife_Space_Restrictor
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }
        
        // CSE_ALife_Space_Restrictor
        public byte Spase_Restrictor_Type { get; set; }

        // se_smart_terrain
        public byte? Arriving_NPC_Count { get; set; } = null;
        public byte? NPC_Info_Count { get; set; } = null;
        public byte? Dead_Time_Count { get; set; } = null;
        public byte? Respawn_Point { get; set; } = null;
        public uint? Smart_Alarm_Time { get; set; } = null;

        public Smart_Terrain(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            CSE_ALife_Space_Restrictor.ReadBasic(reader, this);

            if (reader.ChunkReamains() > 0)
            {
                Arriving_NPC_Count = reader.ReadByte();
                NPC_Info_Count = reader.ReadByte();
                Dead_Time_Count = reader.ReadByte();
                Respawn_Point = reader.ReadByte();
                Smart_Alarm_Time = reader.ReadUInt32();
            }
        }
    }
}