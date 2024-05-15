using System.Threading.Tasks;

using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns;

namespace SpawnConverter.Converters
{
    internal partial class SpawnManager : FileReader
    {
        public const uint COP_VERSION = 10;
        private readonly Spawn data = new();

        internal SpawnManager(string name) => data.LevelName = name;

        internal async Task<bool> ReadAsync() => await Task.Run(() => Read());
        internal async Task<bool> WriteAsync(bool all = false) => await Task.Run(() => WriteAll(all));
    }
}
