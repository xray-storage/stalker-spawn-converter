using System.IO;

namespace SpawnConverter.Compression
{
    public partial class LZHuf
    {
        static class FS
        {
            private static BinaryReader in_buff;
            private static MemoryStream out_buff;

            private static uint getlen;
            private static uint getbuf;

            private static int size = 0;

            private static void Init() => getlen = getbuf = 0;
            public static void Init_Decode(byte[] @in)
            {
                in_buff = new(new MemoryStream(@in));
                out_buff = new(in_buff.ReadInt32());
                size = out_buff.Capacity;
                Init();
            }
            public static void Init_Decode(FileStream @in)
            {
                in_buff = new(@in);
                out_buff = new(in_buff.ReadInt32());
                size = out_buff.Capacity;
                Init();
            }

            public static byte[] OutPointer()
            {
                byte[] res = out_buff.GetBuffer();

                in_buff.Close();
                out_buff.Close();

                return res;
            }
            public static int GetSize() => size;

            //---------------------------------------------------------
            /* записать байт */
            public static void Putc(int c)
            {
                if (out_buff.Position == out_buff.Capacity)
                    out_buff.Capacity += 1;

                out_buff.WriteByte((byte)c);
            }

            //---------------------------------------------------------
            /* прочитать следующий байт */
            public static int Getc()
            {
                if (in_buff.BaseStream.Position < in_buff.BaseStream.Length)
                    return in_buff.ReadByte();

                return EOF;
            }

            //---------------------------------------------------------
            /* получить 1 бит */
            public static int GetBit()
            {
                int i;

                while (getlen <= 8)
                {
                    int c = Getc();
                    i = (c < 0) ? 0 : c;

                    getbuf |= (uint)(i << (int)(8 - getlen));
                    getlen += 8;
                }

                i = (int)getbuf;

                getbuf <<= 1;
                getlen--;

                return (i >> 15) & 1;
            }

            //---------------------------------------------------------
            /* получить байт */
            public static int GetByte()
            {
                int i;

                while (getlen <= 8)
                {
                    int c = Getc();
                    i = (c < 0) ? 0 : c;

                    getbuf |= (uint)(i << (int)(8 - getlen));
                    getlen += 8;
                }

                i = (int)getbuf;

                getbuf <<= 8;
                getlen -= 8;

                return (i & 0xff00) >> 8;
            }
        }
    }
}
