using System.Numerics;

using SpawnConverter.Logs;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;

namespace SpawnConverter.Converters.Spawns.Templates
{
    public class Template_Section : LogEvent, ISection
    {
        private protected enum Vel : byte
        {
            NoAngular = 2,
            NoLinear = 4,
        }

        public ushort SectionID { get; set; }
        public string TypeName { get; set; }

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


        internal Template_Section(XrFileReader reader)
        {
            if (reader.FindChunk(CHUNK.ALIFE.ID) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return;
            }

            SectionID = reader.ReadUInt16();

            if (reader.FindChunk(CHUNK.ALIFE.SECTION) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return;
            }

            ReadProperties(reader);
        }

        private protected virtual void ReadProperties(XrFileReader reader)
        {
            if (reader.FindChunk(CHUNK.ALIFE.BASIC) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return;
            }

            ReadAbstract(reader);
            ReadObjects(reader);
            ReadBasic(reader);

            if (reader.FindChunk(CHUNK.ALIFE.UPDATE) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return;
            }

            _ = reader.ReadUInt16(); // не нужен, размер пакета
            _ = reader.ReadUInt16(); // не нужен, какой-то прараметр, константа = 0х0000

            ReadUpdate(reader);
        }
        private protected virtual void ReadAbstract(XrFileReader reader)
        {
            _ = reader.ReadUInt16(); // не нужен, размер всего пакета. 

            Dummy16 = reader.ReadUInt16();
            Section_Name = reader.ReadStringZ();
            Name = reader.ReadStringZ();
            Game_ID = reader.ReadByte();
            RP = reader.ReadByte();
            Position = reader.ReadVector();
            Direction = reader.ReadVector();
            Respawn_Time = reader.ReadUInt16();
            ID = reader.ReadUInt16();
            Parent_ID = reader.ReadUInt16();
            Phantom_ID = reader.ReadUInt16();
            Flags = reader.ReadUInt16();
            Version = reader.ReadUInt16();
            Game_Type = reader.ReadUInt16();
            Script_Version = reader.ReadUInt16();
            Client_Data_SIze = reader.ReadUInt16();
            Spawn_ID = reader.ReadUInt16();

            _ = reader.ReadUInt16(); // не нужен, размер остальной части пакета.
        }
        private protected virtual void ReadObjects(XrFileReader reader)
        {
            Game_Vertex_ID = reader.ReadUInt16();
            Distance = reader.ReadFloat();
            Direct_Control = reader.ReadUInt32();
            Level_Vertex_ID = reader.ReadUInt32();
            Object_flags = reader.ReadUInt32();
            Custom_Data = reader.ReadStringZ();
            Story_ID = reader.ReadUInt32();
            Spawn_Story_ID = reader.ReadUInt32();
        }

        private protected virtual void ReadBasic(XrFileReader reader) { }
        private protected virtual void ReadUpdate(XrFileReader reader) { }
    }
}
