namespace SpawnConverter.Converters.Spawns
{
    public static class CheckLevel
    {
        public static string LevelName { get; set; }
        public static ushort GameGraphStart { get; set; }
        public static ushort GameGraphCount { get; set; }

        public static bool IsOnLevel(ushort gvid) => GameGraphStart <= gvid && (GameGraphStart + GameGraphCount) > gvid;
    }
}
