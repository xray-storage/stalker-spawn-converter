using SpawnConverter.Types;

namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_Shape
    public interface ICSE_Shape
    {
        public byte Shapes_Count { get; set; }
        public IShape[] Shapes { get; set; }
    }
}
