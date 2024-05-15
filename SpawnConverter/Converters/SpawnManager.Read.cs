﻿using System;
using System.IO;

using SpawnConverter.FStream;
using static SpawnConverter.FStream.FilePath;

namespace SpawnConverter.Converters
{
    partial class SpawnManager
    {
        private bool Read()
        {
            Log();
            Log($"Target Level: {data.LevelName}");
            Log();

            bool result = false;

            try
            {
                using XrFileReader reader = new(Path.Combine(GAME.SPAWNS, FILE.SPAWN), true);
                result = data.SetData(reader);
            }
            catch (Exception e)
            {
                LogError(CODES.UNKNOWN, e.Message);
            }

            return result;
        }
    }
}
