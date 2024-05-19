using System.Numerics;

namespace SpawnConverter.Converters.Spawns.Interfaces
{
    public interface ISection
    {
        public ushort SectionID { get; set; }

        // CSE_Abstract
        public ushort Dummy16 { get; set; }
        public string Section_Name { get; set; }
        public string Name { get; set; }
        public byte Game_ID { get; set; }
        public byte RP { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public ushort Respawn_Time { get; set; }
        public ushort ID { get; set; }
        public ushort Parent_ID { get; set; }
        public ushort Phantom_ID { get; set; }
        public ushort Flags { get; set; }
        public ushort Version { get; set; }
        public ushort Game_Type { get; set; }
        public ushort Script_Version { get; set; }
        public ushort Client_Data_SIze { get; set; }
        public ushort Spawn_ID { get; set; }

        // CSE_ALife_Object
        public ushort Game_Vertex_ID { get; set; }
        public float Distance { get; set; }
        public uint Direct_Control { get; set; }
        public uint Level_Vertex_ID { get; set; }
        public uint Object_flags { get; set; }
        public string Custom_Data { get; set; }
        public uint Story_ID { get; set; }
        public uint Spawn_Story_ID { get; set; }
    }
}
