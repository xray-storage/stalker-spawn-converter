using System.Collections.Generic;
using SpawnConverter.Logs;
using SpawnConverter.FStream;

namespace SpawnConverter.Converters.Spawns
{
    public sealed class Spawn : LogEvent
    {
        public string LevelName { get; set; }

        public List<DataLevels> Levels { get; set; } = new(0);
        public DataSections Sections { get; set; } = new();
        public DataWays Ways { get; set; } = new();

        public bool SetData(XrFileReader reader)
        {
            uint size;

            if ((size = reader.FindChunkSafe(CHUNK._GRAPH)) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            Log($"Read GRAPH: {size} bytes");

            reader.Position += 1;
            ushort gg_count = reader.ReadUInt16();
            reader.Position += 24;
            byte lvl_count = reader.ReadByte();

            Log($"* Level count: {lvl_count}");
            Log($"* Vertex count: {gg_count}");

            for (byte i = 0; i < lvl_count; i++)
            {
                DataLevels level = new()
                {
                    Name = reader.ReadStringZ(),
                };

                Levels.Add(level);

                _ = reader.ReadVector();

                level.ID = reader.ReadByte();

                _ = reader.ReadStringZ();
                _ = reader.ReadGuid();
            }

            long reset_pos = reader.Position;

            for (ushort i = 0; i < gg_count; i++)
            {
                GameGraph graph = new()
                {
                    GameVertexID = i,
                    Local_Point = reader.ReadVector(),
                };

                _ = reader.ReadVector();

                byte lvlid = reader.ReadByte();

                _ = reader.ReadUInt24();

                graph.VertexType = reader.ReadVectorVertex();
                uint edge_offset = reader.ReadUInt32();

                _ = reader.ReadUInt32();

                byte edge_count = reader.ReadByte();

                _ = reader.ReadByte();

                if (edge_count > 0)
                {
                    long safe_pos = reader.Position;
                    graph.Edges = new Edge[edge_count];

                    reader.Position = reset_pos + edge_offset;

                    for (byte j = 0; j < edge_count; j++)
                    {
                        graph.Edges[j] = new()
                        {
                            Game_Vertex_ID = reader.ReadUInt16(),
                        };

                        _ = reader.ReadFloat();
                    }

                    reader.Position = safe_pos;
                }

                int id = Levels.FindIndex(x => x.ID == lvlid);
                Levels[id].GameGraphs.Add(graph);
            }

            if ((size = reader.FindChunkSafe(CHUNK._ALIFE)) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            Log($"Read ALIFE: {size} bytes");
            
            if(!Sections.SetData(reader))
            {
                return false;
            }

            if ((size = reader.FindChunkSafe(CHUNK._WAY)) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            Log($"Read WAY: {size} bytes");
            
            return Ways.SetData(reader);
        }
    }
}
