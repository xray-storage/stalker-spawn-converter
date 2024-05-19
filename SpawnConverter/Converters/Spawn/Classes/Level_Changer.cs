using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Level_Changer : Template_Section, ICSE_Shape, ICSE_ALife_Space_Restrictor
    {
        // =============== BASIC ===============
        // CSE_Shape
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }

        // CSE_ALife_Space_Restrictor
        public byte Spase_Restrictor_Type { get; set; }

        // CSE_ALife_Level_Changer
        public ushort Next_Graph_ID { get; set; }
        public uint Next_Node_ID { get; set; }
        public Vector3 Next_Position { get; set; }
        public Vector3 Angles { get; set; }
        public string Level_To_Change { get; set; }
        public string Level_Point_To_Change { get; set; }
        public byte Silent_Mode { get; set; }

        // se_level_changer
        public byte? Save_Marker { get; set; }
        public string Level_Changer_Invitation { get; set; }
        public byte? Invitation_Enable { get; set; }
        public byte? Save_Marker_End { get; set; }

        public Level_Changer(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_Shape.ReadBasic(reader, this);
            CSE_ALife_Space_Restrictor.ReadBasic(reader, this);
            
            Next_Graph_ID = reader.ReadUInt16();
            Next_Node_ID = reader.ReadUInt32();
            Next_Position = reader.ReadVector();
            Angles = reader.ReadVector();
            Level_To_Change = reader.ReadStringZ();
            Level_Point_To_Change = reader.ReadStringZ();
            Silent_Mode = reader.ReadByte();

            if (reader.ChunkReamains() > 0)
            {
                Save_Marker = reader.ReadByte();
                Level_Changer_Invitation = reader.ReadStringZ();
                Invitation_Enable = reader.ReadByte();
                Save_Marker_End = reader.ReadByte();
            }
        }
    }
}
