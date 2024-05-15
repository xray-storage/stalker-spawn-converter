using System.Threading.Tasks;

using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns;

namespace SpawnConverter.Converters
{
    internal partial class SpawnManager : FileReader
    {
        public const uint COP_VERSION = 10;
        private readonly Spawn data = new();

        public SpawnManager(string level_name) => data.LevelName = level_name;

        internal async Task<bool> ReadAsync() => await Task.Run(() => Read());
        internal async Task<bool> WriteAsync() => await Task.Run(() => Write());
    }
}
