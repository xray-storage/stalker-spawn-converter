using System;
using System.Linq;

namespace SpawnConverter.Types
{
    public struct UInt24
    {
        private readonly uint Value;

        public UInt24(byte[] buff) => Value = BitConverter.ToUInt32(buff.Concat(new byte[1] { 0 }).ToArray());
        public UInt24(uint value) => Value = value;

        public byte[] ToByteArray()
        {
            byte[] buff = BitConverter.GetBytes(Value);
            return new byte[3] { buff[0], buff[1], buff[2] };
        }

        public uint ToUInt32() => Value;

        public static bool operator ==(UInt24 l, UInt24 r) => l.Value == r.Value;
        public static bool operator !=(UInt24 l, UInt24 r) => l.Value != r.Value;
        public static UInt24 operator +(UInt24 l, UInt24 r) => new(l.Value + r.Value);
        public static UInt24 operator -(UInt24 l, UInt24 r) => new(l.Value - r.Value);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
