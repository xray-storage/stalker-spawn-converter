using System.Numerics;
using System.Collections.Generic;

using SpawnConverter.Logs;
using SpawnConverter.FStream;

namespace SpawnConverter.Converters.Spawns
{
    public class DataWays : LogEvent 
    {
        public List<Way> Ways { get; set; } = new(0);

        public bool SetData(XrFileReader reader)
        {
            bool result = true;

            if (reader.FindChunk(CHUNK.WAY.COUNT) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            uint count = reader.ReadUInt32();

            if (reader.FindChunk(CHUNK.WAY.WAYS) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            for (uint i = 0; i < count; i++)
            {
                if (reader.FindChunk(i) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return false;
                }

                result = AddWayData(reader);
            }

            Log($"* Found patrol ways: {Ways.Count}");

            return result;
        }
        private bool AddWayData(XrFileReader reader)
        {
            if (reader.FindChunk(CHUNK.WAY.NAME) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            Way way = new()
            {
                Name = reader.ReadStringZ(),
            };

            if (reader.FindChunk(CHUNK.WAY.POINT) == 0 || reader.FindChunk(CHUNK.WAY.PNT_COUNT) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            uint count = reader.ReadUInt32();

            if (reader.FindChunk(CHUNK.WAY.PNT_LIST) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            way.Points = new WayPoint[count];

            for (uint i = 0; i < count; i++)
            {
                if (reader.FindChunk(i) == 0 || reader.FindChunk(CHUNK.WAY.PNT_ID) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return false;
                }

                way.Points[i] = new WayPoint() { ID = reader.ReadUInt32() };

                if (reader.FindChunk(CHUNK.WAY.PNT_PARAM) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return false;
                }

                way.Points[i].Name = reader.ReadStringZ();
                way.Points[i].Position = reader.ReadVector();
                way.Points[i].Flag = reader.ReadUInt32();
                _ = reader.ReadUInt32();
                way.Points[i].GameVertexID = reader.ReadUInt16();
            }

            if (reader.FindChunk(CHUNK.WAY.PNT_LINKS) > 0)
            {
                while (reader.ChunkReamains() > 0)
                {
                    uint id = reader.ReadUInt32();
                    uint link_count = reader.ReadUInt32();

                    if (link_count > 0)
                    {
                        way.Points[id].Links = new Link[link_count];

                        for (uint i = 0; i < link_count; i++)
                        {
                            way.Points[id].Links[i].Target_ID = reader.ReadUInt32();
                            way.Points[id].Links[i].Weight = reader.ReadFloat();
                        }
                    }
                }
            }

            Ways.Add(way);

            return true;
        }
    }
    public class Way
    {
        public string Name { get; set; }
        public WayPoint[] Points { get; set; }
    }
    public class WayPoint
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public uint Flag { get; set; }
        public ushort GameVertexID { get; set; }
        public Link[] Links { get; set; }
    }
    public struct Link
    {
        public uint Target_ID { get; set; }
        public float Weight { get; set; }
    }
}
