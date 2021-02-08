using System;
using System.Runtime.InteropServices;

namespace Monocypher
{
    public static partial class monocypher
    {
        public const string MonocypherDll = "monocypher";

        public partial struct size_t
        {
            public static implicit operator size_t(int value)
            {
                return new size_t(new IntPtr(value));
            }
        }

        public unsafe struct Byte8
        {
            public fixed byte Data[8];
        }
        public unsafe struct Byte12
        {
            public fixed byte Data[12];
        }
        public unsafe struct Byte16
        {
            public fixed byte Data[16];
        }
        public unsafe struct Byte24
        {
            public fixed byte Data[24];
        }
        public unsafe struct Byte32
        {
            public fixed byte Data[32];
        }
        public unsafe struct Byte64
        {
            public fixed byte Data[64];
        }

    }
}
