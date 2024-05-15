using System.Numerics;
using System.Collections.Generic;

using SpawnConverter.Types;

namespace SpawnConverter.Converters.Spawns
{
    public class DataLevels
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public List<GameGraph> GameGraphs { get; set; } = new(0);
    }

    public struct GameGraph
    {
        public ushort GameVertexID { get; set; }
        public Vector3 Local_Point { get; set; }
        public VectorVertex VertexType { get; set; }
        public Edge[] Edges { get; set; }
        public Edge this[byte ind]
        {
            get => Edges[ind];
            set => Edges[ind] = value;
        }
    }

    public struct Edge
    {
        public ushort Game_Vertex_ID { get; set; }
    }
}
