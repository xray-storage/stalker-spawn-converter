namespace SpawnConverter.Logs
{
    public sealed class ErrorMessage
    {
        public static string GetMessageByCode(CODES ID)
        {
            return ID switch
            {
                // Spawn
                CODES.NOT_TEMPLATE  => "No template found for the class: '{0}'",
                CODES.READ_SECTOR   => "Failed sector reading.",
                CODES.NOT_CLSID     => "No class found for the section: '{0}'.",

                // General
                CODES.NOT_FILE      => "File does not exist.",
                CODES.NO_VALID_FILE => "Invalid or unsupported version of the file.",
                CODES.NOT_DIRECTORY => "Directory does not exist.",
                CODES.PATH_CLEAR    => "The path to create the file is empty.",
                CODES.DATA_EMPTY    => "The data is empty.",

                CODES.NO_VALID_GAME => "Invalid or unsupported version of the game.",
                CODES.NOT_CHUNK     => "The next chunk was not found.",

                _ => "",
            };
        }
    }
}
