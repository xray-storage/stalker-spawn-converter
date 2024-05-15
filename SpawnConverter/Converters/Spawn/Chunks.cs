namespace SpawnConverter.Converters
{
    internal struct CHUNK
    {
        internal const uint _HEADER   = 0x0000;
        internal const uint _ALIFE    = 0x0001;
        internal const uint _AF_SPAWN = 0x0002;
        internal const uint _WAY      = 0x0003;
        internal const uint _GRAPH    = 0x0004;

        internal struct ALIFE
        {
            internal const uint COUNT     = 0x0000;
            internal const uint SEC_LIST  = 0x0001;
            internal const uint UNK_EMPTY = 0x0002;
            internal const uint ID        = 0x0000;
            internal const uint SECTION   = 0x0001;
            internal const uint BASIC     = 0x0000;
            internal const uint UPDATE    = 0x0001;
        }
        internal struct WAY
        {
            internal const uint COUNT     = 0x0000;
            internal const uint WAYS      = 0x0001;
            internal const uint NAME      = 0x0000;
            internal const uint POINT     = 0x0001;
            internal const uint PNT_COUNT = 0x0000;
            internal const uint PNT_LIST  = 0x0001;
            internal const uint PNT_LINKS = 0x0002;
            internal const uint PNT_ID    = 0x0000;
            internal const uint PNT_PARAM = 0x0001;
        }
    }
}
