using System.IO;
using System.Text.RegularExpressions;

namespace SpawnConverter.FStream
{
    internal partial class FilePath
    {   
        internal static RESULT Validation()
        {
            return ValidationGamePath() | ValidationSdkPath();
        }
        private static RESULT ValidationGamePath()
        {
            RESULT result = RESULT.NO;

            string root = Settings.GamePath;
            string fsgame = Path.Combine(root, FILE.GAME_FS);

            result |= Directory.Exists(root) ? RESULT.GAME_ROOT : RESULT.NO;
            result |= File.Exists(fsgame) ? RESULT.GAME_FSGAME : RESULT.NO;

            if (result == (RESULT.GAME_ROOT | RESULT.GAME_FSGAME))
            {
                GAME.ROOT = root;
                SetPathes(fsgame);

                result |= Directory.Exists(GAME.GAMEDATA) ? RESULT.GAME_GAMEDATA : RESULT.NO;
            }

            return result;
        }
        private static RESULT ValidationSdkPath()
        {
            RESULT result = RESULT.NO;

            string root = Settings.SdkPath;
            string fs = Path.Combine(root, FILE.SDK_FS);

            result |= Directory.Exists(root) ? RESULT.SDK_ROOT : RESULT.NO;
            result |= File.Exists(fs) ? RESULT.SDK_FS : RESULT.NO;

            if (result == (RESULT.SDK_ROOT | RESULT.SDK_FS))
            {
                SDK.ROOT = root;
                SetPathes(fs);

                result |= Directory.Exists(SDK.RAWDATA) ? RESULT.SDK_RAWDATA : RESULT.NO;
            }

            return result;
        }
        private static void SetPathes(string fpath)
        {
            bool isSDK = Path.GetFileName(fpath).CompareTo(FILE.SDK_FS) == 0;
            using StreamReader reader = new(fpath);

            while (!reader.EndOfStream)
            {
                string line = Regex.Replace(reader.ReadLine(), @"\s+", "");

                string key = string.Empty;
                string path = string.Empty;
                string value = string.Empty;

                if (Regex.IsMatch(line, PATTERN))
                {
                    key = Regex.Match(line, PATTERN).Groups["key"].Value;
                    path = Regex.Match(line, PATTERN).Groups["path"].Value;
                    value = Regex.Match(line, PATTERN).Groups["value"].Value;
                }
                else if(Regex.IsMatch(line, PATTERN_RAWDATA))
                {
                    key = Regex.Match(line, PATTERN_RAWDATA).Groups["key"].Value;
                    value = Regex.Match(line, PATTERN_RAWDATA).Groups["value"].Value;
                }
                else
                {
                    continue;
                }

                if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (isSDK)
                {
                    _ = key switch
                    {
                        KEYS.GAMEDATA   => SDK.GAMEDATA = CreatePath(path, value, isSDK),
                        KEYS.RAWDATA    => SDK.RAWDATA  = CreatePath(path, value, isSDK),
                        KEYS.MAPS       => SDK.LEVELS   = CreatePath(path, value, isSDK),
                        _               => null
                    };
                }
                else
                {
                    _ = key switch
                    {
                        KEYS.GAMEDATA   => GAME.GAMEDATA = CreatePath(path, value, isSDK),
                        KEYS.CONFIGS    => GAME.CONFIGS  = CreatePath(path, value, isSDK),
                        KEYS.LEVELS     => GAME.LEVELS   = CreatePath(path, value, isSDK),
                        KEYS.SPAWNS     => GAME.SPAWNS   = CreatePath(path, value, isSDK),

                        KEYS.DB         => DB.ROOT       = CreatePath(path, value, isSDK),
                        KEYS.DBCONFIGS  => DB.CONFIGS    = CreatePath(path, value, isSDK),
                        KEYS.DBMAPS     => DB.LEVELS     = CreatePath(path, value, isSDK),

                        _               => null
                    };
                }
            }
        }
        private static string CreatePath(string path, string value, bool isSDK)
        {
            return path switch
            {
                KEYS.ROOT       => Path.Combine(isSDK ? SDK.ROOT : GAME.ROOT, value),
                KEYS.GAMEDATA   => Path.Combine(isSDK ? SDK.GAMEDATA : GAME.GAMEDATA, value),
                KEYS.RAWDATA    => Path.Combine(SDK.RAWDATA, value),
                KEYS.DB         => Path.Combine(DB.ROOT, value),
                _               => Path.Combine(isSDK ? SDK.ROOT : GAME.ROOT, value)
            };
        }

        internal static string GetErrorsMessage(RESULT result)
        {
            string message = "\n";

            if ((result & RESULT.GAME_ROOT) != RESULT.GAME_ROOT)
            {
                message += MESSAGE.GAME_ROOT + '\n';
            }
            else
            {
                if((result & RESULT.GAME_FSGAME) != RESULT.GAME_FSGAME)
                {
                    message += MESSAGE.GAME_FS + '\n';
                }

                if((result & RESULT.GAME_GAMEDATA) != RESULT.GAME_GAMEDATA)
                {
                    message += MESSAGE.GAME_GAMEDATA + '\n';
                }
            }

            if ((result & RESULT.SDK_ROOT) != RESULT.SDK_ROOT)
            {
                message += MESSAGE.SDK_ROOT + '\n';
            }
            else
            {
                if((result & RESULT.SDK_FS) != RESULT.SDK_FS)
                {
                    message += MESSAGE.SDK_FS + '\n';
                }

                if((result & RESULT.SDK_RAWDATA) != RESULT.SDK_RAWDATA)
                {
                    message += MESSAGE.SDK_RAWDATA + '\n';
                }
            }

            return message;
        }
    }
}
