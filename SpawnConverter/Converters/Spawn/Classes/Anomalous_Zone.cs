using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Anomalous_Zone : Template_Section, ICSE_Shape, ICSE_ALife_Space_Restrictor, ICSE_ALife_Custom_Zone, ICSE_ALife_Anomalous_Zone
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

        // CSE_ALife_Anomalous_Zone
        public float Offline_Interactive_Radius { get; set; }
        public ushort Artefact_Spawn_Count { get; set; }
        public uint Artefact_Position_Offset { get; set; }
        
        // se_zones
        public byte? Last_Spawn_Time { get; set; }

        public Anomalous_Zone(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            CSE_ALife_Space_Restrictor.ReadBasic(reader, this);
            CSE_ALife_Custom_Zone.ReadBasic(reader, this);
            CSE_ALife_Anomalous_Zone.ReadBasic(reader, this);

            if (reader.ChunkReamains() > 0)
            {
                Last_Spawn_Time = reader.ReadByte();
            }
        }
    }
}
