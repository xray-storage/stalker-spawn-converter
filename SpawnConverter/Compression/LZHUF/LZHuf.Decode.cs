/**************************************************************
	lzhuf.c
	written by Haruyasu Yoshizaki 11/20/1988
	some minor changes 4/6/1989
	comments translated by Haruhiko Okumura 4/7/1989
**************************************************************/

/*
LZHUF.C (c)1989 by Haruyasu Yoshizaki, Haruhiko Okumura, and Kenji Rikitake.
All rights reserved. Permission granted for non-commercial use.
*/

namespace SpawnConverter.Compression
{
    public partial class LZHuf
    {
        //---------------------------------------------------------
        /* декодирование символа */
        private static int DecodeChar()
        {
            int c = son[R];

            while (c < T)
            {
                c += FS.GetBit();
                c = son[c];
            }

            c -= T;
            Update(c);

            return c;
        }

        //---------------------------------------------------------
        /* декодирование позиции */
        private static int DecodePosition()
        {
            int i = FS.GetByte();
            int c = d_code[i] << 6;
            int j = d_len[i];

            j -= 2;

            while (j-- > 0)
                i = (i << 1) + FS.GetBit();

            return c | (i & 0x3f);
        }

        //---------------------------------------------------------
        /* Декомпрессия */
        private static void Decode()
        {
            int size = FS.GetSize();

            if (size == 0)
                return;

            StartHuff();

            for (uint i = 0; i < N - F; i++)
                buffer[i] = 0x20;

            int r = N - F;

            for (int pos = 0; pos < size;)
            {
                int c = DecodeChar();

                if (c < 0x100)
                {
                    FS.Putc(c);
                    buffer[r++] = (byte)c;
                    r &= N - 1;
                    pos++;

                    continue;
                }

                int i = (r - DecodePosition() - 1) & (N - 1);
                int j = c - 255 + THRESHOLD;
                
                for (uint k = 0; k < j; k++)
                {
                    c = buffer[(i + k) & (N - 1)];
                    FS.Putc(c);
                    buffer[r++] = (byte)c;
                    r &= N - 1;
                    pos++;
                }

            }
        }
    }
}
