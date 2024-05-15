using System;
using System.Text;
using System.Numerics;
using System.IO;
using System.Text.RegularExpressions;
using SpawnConverter.Types;

namespace SpawnConverter.FStream
{
    public class XrFileReader : FileReader
    {
        private readonly BinaryReader reader;
        private uint chunk_size = 0;

        private const uint COMPRESSED = 0x80000000;

        public long Position
        {
            get => reader.BaseStream.Position;
            set => reader.BaseStream.Position = reader.BaseStream.Length >= value ? value >= 0 ? value : 0 : reader.BaseStream.Length;
        }
        public long Length { get => reader.BaseStream.Length; }


        public XrFileReader(byte[] buffer) => reader = new(new MemoryStream(buffer));
        public XrFileReader(MemoryStream stream) => reader = new(stream);

        public XrFileReader(string fpath, bool search_db = false)
        {
            Stream stream;

            if (search_db && !File.Exists(fpath))
            {
                fpath = fpath.Replace(FilePath.GAME.ROOT, "");
                var matches = Regex.Match(fpath, @"gamedata\\(?<dir>[a-zA-Z]+)\\(?<name>.*)");

                if (!matches.Success)
                {
                    LogError(CODES.NOT_FILE);
                    return;
                }

                string dir = matches.Groups["dir"].Value;
                string name = Path.Combine(dir, matches.Groups["name"].Value);
                bool isCopRes = FilePath.DB.ROOT.Split('\\')[^1].CompareTo("resources") == 0;

                dir = dir.CompareTo("spawns") == 0 ? "configs" : dir;

                DBFile file = new(Path.Combine(FilePath.DB.ROOT, isCopRes ? string.Empty : dir));
                var data = file.SearchFile(name);

                if (data is null)
                {
                    LogError(CODES.NOT_FILE);
                    return;
                }

                stream = file.Unpack(data);
            }
            else
            {
                stream = File.OpenRead(fpath);
            }

            reader = new(stream);
        }

        public bool Eof(long offset = 0) => reader.BaseStream.Length <= reader.BaseStream.Position + offset;
        public void ResetPosition() => reader.BaseStream.Position = 0;
        public uint ChunkReamains() => chunk_size - (uint)Position;
        public uint FindChunk(uint ID)
        {
            chunk_size = 0;

            while (!Eof())
            {
                if (Eof(8))
                {
                    break;
                }

                uint dwType = reader.ReadUInt32();
                uint dwSize = reader.ReadUInt32();

                if ((dwType == ID || (dwType ^ COMPRESSED) == ID) && (reader.BaseStream.Position - 8 + dwSize <= reader.BaseStream.Length))
                {
                    chunk_size = (uint)Position + dwSize;
                    return dwSize;
                }
                else
                {
                    if (reader.BaseStream.Position + dwSize < reader.BaseStream.Length)
                    {
                        reader.BaseStream.Position += dwSize;
                    }
                    else if (reader.BaseStream.Position + 8 < reader.BaseStream.Length)
                    {
                        reader.BaseStream.Position += 4;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return chunk_size;
        }
        public uint FindChunkSafe(uint ID)
        {
            ResetPosition();
            return FindChunk(ID);
        }

        public short ReadInt16() => reader.ReadInt16();
        public ushort ReadUInt16() => reader.ReadUInt16();
        public UInt24 ReadUInt24() => new(new byte[3] { reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), });
        public uint ReadUInt32() => reader.ReadUInt32();
        public ulong ReadUInt64() => reader.ReadUInt64();
        public int ReadInt32() => reader.ReadInt32();
        public float ReadFloat() => reader.ReadSingle();

        public Vector3 ReadVector() => new() { X = ReadFloat(), Y = ReadFloat(), Z = ReadFloat() };
        public Vector2s16 ReadVector2s16() => new() { X = reader.ReadInt16(), Y = reader.ReadInt16() };
        public Vector3s16 ReadVector3s16() => new() { X = reader.ReadInt16(), Y = reader.ReadInt16(), Z = reader.ReadInt16() };
        public VectorVertex ReadVectorVertex() => new() { A = reader.ReadByte(), B = reader.ReadByte(), C = reader.ReadByte(), D = reader.ReadByte() };
        public Vector3u32str ReadVectorStr() => new() { A = ReadStringZ(), B = ReadStringZ(), C = ReadStringZ(), D = ReadStringZ() };
        public Matrix3x3 ReadMatrix3X3() => new() { X = ReadVector(), Y = ReadVector(), Z = ReadVector() };

        public byte ReadByte() => reader.ReadByte();
        public byte[] ReadBytes(int size = 0) => reader.ReadBytes(size <= 0 ? (int)reader.BaseStream.Length : size);
        public byte[] ReadBytes(uint size = 0) => ReadBytes((int)size);
        
        public Guid ReadGuid() => new(reader.ReadBytes(16));
        public Quaternion ReadQuaternion() => new() { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle(), W = reader.ReadSingle() };

        public string ReadStringZ()
        {
            string str = string.Empty;

            while (!Eof())
            {
                byte[] b = new byte[1] { reader.ReadByte() };

                if (b[0] == 0 || b[0] == 0xE)
                {
                    break;
                }

                str += Encoding.Default.GetString(b);
            }

            return str;
        }
        public IShape[] ReadShapes(byte count)
        {
            if (count == 0)
                return Array.Empty<IShape>();

            IShape[] shapes = new IShape[count];

            for (byte i = 0; i < count; i++)
            {
                byte type = ReadByte();

                shapes[i] = type switch
                {
                    0 => shapes[i] = new Shape { Type = type, OffSet = ReadVector(), Radius = ReadFloat() },
                    1 => shapes[i] = new Box { Type = type, Axis = ReadMatrix3X3(), OffSet = ReadVector() },
                    _ => null
                };
            }

            return shapes;
        }
        public Loophole[] ReadLoopholes(int count)
        {
            if (count <= 0)
                return Array.Empty<Loophole>();

            Loophole[] loop = new Loophole[count];

            for (byte i = 0; i < count; i++)
            {
                loop[i] = new();
                loop[i].Name = ReadStringZ();
                loop[i].Enable = ReadByte();
            }

            return loop;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                reader.Close();
            }
        }
    }
}
