using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using SpawnConverter.Compression;
using static SpawnConverter.FStream.FilePath;

namespace SpawnConverter.FStream
{
    public class DBFile : FileReader
    {
        private struct CHUNK
        {
            internal const uint DATA    = 0x0000;
            internal const uint LIST    = 0x0001;
            internal const uint HEADER  = 0x029A;
        }

        private readonly string FileDir = string.Empty;
        public List<DBData> Files { get; private set; } = new(0);

        public DBFile(string fpath)
        {
            FileDir = fpath;
            GetFiles();
        }

        private void GetFiles()
        {
            try
            {
                foreach (var f in Directory.GetFiles(FileDir, "*.db*"))
                {
                    uint size;
                    var reader = new XrFileReader(f);

                    if ((size = reader.FindChunk(CHUNK.LIST)) == 0)
                    {
                        LogError(CODES.NOT_CHUNK);
                        return;
                    }

                    byte[] buff = LZHuf.Decompress(reader.ReadBytes((int)size));
                    reader.Close();

                    reader = new(buff);

                    while (!reader.Eof())
                    {
                        ushort block_size = reader.ReadUInt16();

                        if (block_size == 0)
                        {
                            break;
                        }

                        uint nsize = reader.ReadUInt32();
                        uint csize = reader.ReadUInt32();
                        uint crc = reader.ReadUInt32();

                        byte[] el = reader.ReadBytes(block_size - 16);
                        string str = Encoding.Default.GetString(el, 0, el.Length);

                        uint offset = reader.ReadUInt32();

                        if (str.Split('.').Length > 1)
                        {
                            DBData data = new()
                            {
                                FilePath = f,
                                SizeNormal = nsize,
                                SizeCompress = csize,
                                CRC = crc,
                                Name = str,
                                Dir = Path.GetDirectoryName(str),
                                Offset = offset,
                            };

                            Files.Add(data);
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public DBData SearchFile(string fpath)
        {
            if(Files.Count == 0)
            {
                return null;
            }

            fpath = fpath.Replace(GAME.GAMEDATA, "").TrimStart('\\');

            return Files.Find(x => x.Name.CompareTo(fpath) == 0);
        }

        public List<DBData> SearchFilesByPattern(string pattern)
        {
            if (Files.Count == 0)
            {
                return new List<DBData>(0);
            }

            pattern = pattern.Replace(Path.Combine(GAME.GAMEDATA), "").TrimStart('\\');

            if (pattern.Contains("_*.ltx"))
            {
                pattern = pattern.Replace("*.ltx", "");
                return Files.Where(x => x.Name.Contains(pattern)).ToList();
            }

            return Files.Where(x => x.Name.CompareTo(pattern) == 0).ToList();
        }

        public MemoryStream Unpack(DBData data)
        {
            if(data.CRC == 0 && data.SizeCompress == 0 && data.SizeNormal == 0)
            {
                LogError(CODES.NO_VALID_FILE);
                return null;
            }

            using XrFileReader reader = new(data.FilePath);

            try
            {
                reader.Position = data.Offset;
                byte[] buff = reader.ReadBytes(data.SizeCompress);

                if (data.SizeNormal == data.SizeCompress)
                {
                    return new MemoryStream(buff);
                }

                byte[] outbuff = new byte[data.SizeNormal];
                LZO1x.Decompress(buff, outbuff);

                if (CRC32.Compute(outbuff) != data.CRC)
                {
                    return null;
                }

                return new MemoryStream(outbuff);
            }
            catch(Exception e)
            {
                LogError(CODES.UNKNOWN, e.Message);
            }

            return null;
        }
    }
    public sealed class DBData
    {
        public string FilePath      = string.Empty;
        public string Dir           = string.Empty;
        public string Name          = string.Empty;
        public uint Offset          = default;
        public uint SizeNormal      = default;
        public uint SizeCompress    = default;
        public uint CRC             = default;
    }
}
