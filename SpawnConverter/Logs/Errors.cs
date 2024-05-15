namespace SpawnConverter
{
    public enum CODES
    {
        // Spawn
        NOT_TEMPLATE    = 0xA010,
        READ_SECTOR     = 0xA011,
        NOT_CLSID       = 0xA012,

        // General
        NOT_FILE        = 0x0100,
        NO_VALID_FILE   = 0x0101,
        NOT_DIRECTORY   = 0x0102,
        PATH_CLEAR      = 0x0103,
        DATA_EMPTY      = 0x0104,

        // Other
        NO_VALID_GAME   = 0x00ff,
        NOT_CHUNK       = 0x0fff,
        UNKNOWN         = 0xffff,
    }
}
