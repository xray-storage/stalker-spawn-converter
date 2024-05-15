namespace SpawnConverter.FStream
{
    public partial class LTXReader
    {
        private struct REGULAR
        {
            public const string CLEAR   = @"\s+";
            public const string SECTION = @"^\[{1}(?<section>[a-zA-Z_0-9.-]+)\]{1}\:?(?<parent>[a-zA-z_\,0-9.-]*)";
            public const string PARENT  = @"\:{1}(?<parent>(\,?\w+)+)\r?";
            public const string KEY     = @"^(?<key>[$$a-zA-Z0-9_\\]*)\=(?<value>[$$a-zA-Z0-9\.""\-_\,\\]*)";
            public const string INCLUDE = @"^\#include(""|')(?<pattern>[a-z0-9_\\]+(_\*?)?.ltx)(""|')";
        }
    }
}
