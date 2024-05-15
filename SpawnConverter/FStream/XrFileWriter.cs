using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.IO;
using SpawnConverter.Types;


namespace SpawnConverter.FStream
{
    public class XrFileWriter : FileReader
    {
        private long chunk_pos = 0;
        private readonly BinaryWriter writer;
        private readonly Dictionary<string, long> key_list = new(0);

        public long Position
        {
            get => writer.BaseStream.Position;
            set => writer.BaseStream.Position = writer.BaseStream.Length >= value ? value >= 0 ? value : 0 : writer.BaseStream.Length;
        }
        public long Length { get => writer.BaseStream.Length; }


        public XrFileWriter(string fpath) => writer = new(new FileStream(fpath, FileMode.Create, FileAccess.Write));
        public XrFileWriter(Stream stream) => writer = new(stream);


        public void Write(short value) => writer.Write(value);
        public void Write(ushort value) => writer.Write(value);
        public void Write(UInt24 value) => Write(value.ToByteArray());
        public void Write(uint value) => writer.Write(value);
        public void Write(ulong value) => writer.Write(value);
        public void Write(int value) => writer.Write(value);
        public void Write(byte value) => writer.Write(value);
        public void Write(byte[] value) => writer.Write(value);
        public void Write(float value) => writer.Write(value);
        public void Write(Guid value) => writer.Write(value.ToByteArray());
        public void Write(string value, bool noend = false) => Write(Encoding.ASCII.GetBytes(noend ? value : value + '\0'));

        public void Write(Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }
        public void Write(Vector3 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }

        public void WriteInPos(ushort value, long pos)
        {
            long save_pos = Position;
            Position = pos;
            Write(value);
            Position = save_pos;
        }
        public void WriteInPos(uint value, long pos)
        {
            long save_pos = Position;
            Position = pos;
            Write(value);
            Position = save_pos;
        }


        public void OpenChunk(uint ID, string key = "")
        {
            Write(ID);

            if (string.IsNullOrEmpty(key))
                chunk_pos = Position;
            else
                key_list.Add(key, Position);

            Write((uint)0);
        }
        public void CloseChunk(string key = "")
        {
            long pos = Position;
            long temp = chunk_pos;

            if (!string.IsNullOrEmpty(key) && key_list.TryGetValue(key, out long val))
            {
                temp = val;
                key_list.Remove(key);
            }

            Position = temp;
            Write((uint)(pos - temp - sizeof(uint)));
            Position = pos;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                writer.Close();
            }
        }
    }
}
