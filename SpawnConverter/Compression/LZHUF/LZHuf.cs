namespace SpawnConverter.Compression
{
    /// <summary>
    /// Класс для сжатия алгоритмом LZ HUFFMAN
    /// </summary>
    public partial class LZHuf
    {
        /// <summary>
        /// Распаковывает сжатые данные до их исходного состояния.
        /// </summary>
        /// <param name="in">Исходный массив байт, подлежащий распаковке</param>
        /// <returns>Массив, распакованных данных</returns>
        public static byte[] Decompress(byte[] @in)
        {
            FS.Init_Decode(@in);
            Decode();

            return FS.OutPointer();
        }
        /// <summary>
        /// Распаковывает сжатые данные до их исходного состояния.
        /// </summary>
        /// <param name="in">Исходный массив байт, подлежащий распаковке</param>
        /// <param name="out">Массив байт, содержащий распакованные данные</param>
        /// <returns>Размер распакованных данных</returns>
        public static int Decompress(byte[] @in, out byte[] @out)
        {
            FS.Init_Decode(@in);
            Decode();
            @out = FS.OutPointer();

            return @out.Length;
        }
        /// <summary>
        /// Распаковывает сжатые данные до их исходного состояния.
        /// </summary>
        /// <param name="in">Исходный поток байт, подлежащий распаковке</param>
        /// <returns>Массив, распакованных данных</returns>
        public static byte[] Decompress(System.IO.FileStream @in)
        {
            FS.Init_Decode(@in);
            Decode();

            return FS.OutPointer();
        }
        /// <summary>
        /// Распаковывает сжатые данные до их исходного состояния.
        /// </summary>
        /// <param name="in">Исходный поток байт, подлежащий распаковке</param>
        /// <param name="out">Массив байт, содержащий распакованные данные</param>
        /// <returns>Размер распакованных данных</returns>
        public static int Decompress(System.IO.FileStream @in, out byte[] @out)
        {
            FS.Init_Decode(@in);
            Decode();

            @out = FS.OutPointer();

            return @out.Length;
        }
    }
}
