/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */
using System;
using System.Runtime.InteropServices;

namespace SpawnConverter.Compression
{
    public static class LZO1x
    {
        #region Native Method
        [DllImport("lzo1x_64.dll", EntryPoint = "#67", CallingConvention = CallingConvention.StdCall)]
        private static extern int NativeCompress(byte[] inbuf, uint inlen, byte[] outbuf, ref uint outlen, byte[] workbuf);

        [DllImport("lzo1x_64.dll", EntryPoint = "#68", CallingConvention = CallingConvention.StdCall)]
        private static extern int NativeDecompress(byte[] inbuf, uint inlen, byte[] outbuf, ref uint outlen);
        #endregion

        private const int lzo_sizeof_dict_t = 2;
        private const int LZO1X_1_MEM_COMPRESS = 16384 * lzo_sizeof_dict_t;

        private static readonly byte[] CompressWork = new byte[LZO1X_1_MEM_COMPRESS];

        public static void Compress(byte[] @in, out byte[] @out)
        {
            lock (CompressWork)
            {
                @out = new byte[@in.Length + @in.Length / 64 + 16 + 3 + 1];
                uint size = 0;
                _ = NativeCompress(@in, (uint)@in.Length, @out, ref size, CompressWork);

                Array.Resize(ref @out, (int)size);
            }
        }

        public static void Decompress(byte[] inbuf, byte[] outbuf)
        {
            uint size = (uint)outbuf.Length;
            _ = NativeDecompress(inbuf, (uint)inbuf.Length, outbuf, ref size);
        }
    }
}
