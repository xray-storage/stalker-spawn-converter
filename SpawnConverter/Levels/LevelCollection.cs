using System;
using System.IO;
using System.Collections.Generic;

using SpawnConverter.FStream;
using SpawnConverter.Converters;

using static SpawnConverter.FStream.FilePath;

namespace SpawnConverter.Levels
{
    internal class LevelCollection : FileReader
    {
        internal const uint COP_VERSION = 10;
        internal List<string> Levels { get; private set; } = new(0);

        internal LevelCollection() => GetLevelCollection();

        private void GetLevelCollection()
        {
            string fpath = Path.Combine(GAME.SPAWNS, FILE.SPAWN);
            Log($"Read file: {fpath}");

            try
            {
                using XrFileReader reader = new(fpath, true);

                uint size;

                if ((size = reader.FindChunk(CHUNK._HEADER)) == 0)
                {
                    LogError(CODES.NO_VALID_FILE);
                    return;
                }

                Log($"Read HEADER: {size} bytes.");

                uint version = reader.ReadUInt32();
                
                if (version != COP_VERSION)
                {
                    LogError(CODES.NO_VALID_FILE);
                    return;
                }

                Log($"* Version: {version}");

                if ((size = reader.FindChunkSafe(CHUNK._GRAPH)) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return;
                }

                reader.Position += 27;
                byte lvl_count = reader.ReadByte();

                for (byte i = 0; i < lvl_count; i++)
                {
                    string name = reader.ReadStringZ();

                    if(!string.IsNullOrEmpty(name))
                    {
                        Levels.Add(name);
                    }

                    _ = reader.ReadVector();
                    _ = reader.ReadByte();
                    _ = reader.ReadStringZ();
                    _ = reader.ReadGuid();
                }
            }
            catch(Exception e)
            {
                LogError(CODES.UNKNOWN, e.Message);
            }
        }
    }
}
