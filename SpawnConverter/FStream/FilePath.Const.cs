namespace SpawnConverter.FStream
{
    internal partial class FilePath
    {
        private const string PATTERN = @"\$(?<key>[a-z_]+)\$=(true|false)\|(true|false)\|\$(?<path>[a-z_]+)\$\|(?<value>[a-z_]+)";
        private const string PATTERN_RAWDATA = @"\$(?<key>[a-z_]+)\$=(true|false)\|(true|false)\|(?<value>[a-z_]+)";

        internal enum RESULT : uint
        {
            NO              = 0x00000000,

            GAME_ROOT       = 0x00000001,
            GAME_FSGAME     = 0x00000010,

            SDK_ROOT        = 0x00001000,
            SDK_FS          = 0x00010000,
            SDK_RAWDATA     = 0x00100000,

            GAME_VALID      = GAME_ROOT | GAME_FSGAME,
            SDK_VALID       = SDK_ROOT | SDK_FS | SDK_RAWDATA,

            ALL_VALID       = GAME_VALID | SDK_VALID,
        }

        internal struct FILE
        {
            internal const string GAME_FS     = "fsgame.ltx";
            internal const string SDK_FS      = "fs.ltx";
            internal const string SYSTEM      = "system.ltx";
            internal const string SPAWN       = "all.spawn";
            internal const string SDK_SPAWN   = "spawn.part";
            internal const string SDK_WAY     = "way.part";
        }

        private struct KEYS
        {
            internal const string ROOT      = "fs_root";
            internal const string GAMEDATA  = "game_data";
            internal const string CONFIGS   = "game_config";
            internal const string SPAWNS    = "game_spawn";
            internal const string LEVELS    = "game_levels";
            internal const string RAWDATA   = "server_data_root";
            internal const string MAPS      = "maps";
            internal const string DB        = "arch_dir";
            internal const string DBCOP     = "arch_dir_resources";
            internal const string DBCONFIGS = "arch_dir_configs";
        }

        internal struct GAME
        {
            internal static string ROOT     = string.Empty;
            internal static string GAMEDATA = string.Empty;
            internal static string CONFIGS  = string.Empty;
            internal static string SPAWNS   = string.Empty;
            internal static string LEVELS   = string.Empty;
        }
        internal struct DB
        {
            internal static string ROOT     = string.Empty;
            internal static string CONFIGS  = string.Empty;
        }
        internal struct SDK
        {
            internal static string ROOT     = string.Empty;
            internal static string GAMEDATA = string.Empty;
            internal static string RAWDATA  = string.Empty;
            internal static string LEVELS   = string.Empty;
        }

        private struct MESSAGE
        {
            internal const string GAME_ROOT         = "!Game root directory was not found";
            internal const string GAME_FS           = "!File 'fsgame.ltx' was not found";

            internal const string SDK_ROOT          = "!SDK root directory not found";
            internal const string SDK_FS            = "!File 'fs.ltx' was not found";
            internal const string SDK_RAWDATA       = "!The 'rawdata' folder was not found";
        }
    }
}
