using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpawnConverter.Converters
{
    internal sealed class Converter
    {
        internal static async Task<bool> Run(string name)
        {
            bool result = false;
            using SpawnManager spawn = new(name);

            if (await spawn.ReadAsync())
            {
                result = await spawn.WriteAsync();
            }

            return result;
        }
    }
}
