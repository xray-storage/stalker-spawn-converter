using System;
using System.IO;

using SpawnConverter.FStream;
using static SpawnConverter.FStream.FilePath;

namespace SpawnConverter.Converters
{
    partial class SpawnManager
    {
        private bool Read()
        {
            Log($"Target Level: {data.LevelName}");
            Log();

            if(!Directory.Exists(GAME.LEVELS) && !Directory.Exists(DB.LEVELS))
            {
                LogError(CODES.NOT_DIRECTORY);
                return false;
            }

            string fpath = Path.Combine(GAME.SPAWNS, FILE.SPAWN);
            Log($"Read file: {fpath}");

            bool result = false;

            try
            {
                using XrFileReader reader = new(fpath, true);

                uint size = reader.FindChunk(CHUNK._HEADER);
                Log($"Read HEADER: {size} bytes.");

                if (size == 0)
                {
                    LogError(CODES.NO_VALID_FILE);
                    return false;
                }

                uint version = reader.ReadUInt32();
                Log($"* Version: {version}");

                if (version != COP_VERSION)
                {
                    LogError(CODES.NO_VALID_FILE);
                    return false;
                }

                result = data.SetData(reader);
            }
            catch(Exception e)
            {
                LogError(CODES.UNKNOWN, e.Message);
            }

            return result;
        }
    }
}
