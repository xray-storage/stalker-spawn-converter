using System.Threading.Tasks;

namespace SpawnConverter.Converters
{
    internal sealed class Converter
    {
        internal static async Task<bool> Run(string name)
        {
            bool result = false;
            bool all = string.IsNullOrEmpty(name);

            using SpawnManager spawn = new(name);

            if (await spawn.ReadAsync())
            {
                result = await spawn.WriteAsync(all);
            }

            return result;
        }
    }
}
