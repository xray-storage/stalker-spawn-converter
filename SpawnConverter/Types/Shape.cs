using System.Numerics;

namespace SpawnConverter.Types
{
    public interface IShape
    {
        byte Type { get; set; }
    }
    public struct Shape : IShape
    {
        public byte Type { get; set; }
        public Vector3 OffSet { get; set; }
        public float Radius { get; set; }
    }
    public struct Box : IShape
    {
        public byte Type { get; set; }
        public Matrix3x3 Axis { get; set; }
        public Vector3 OffSet { get; set; }
    }
}
