using System;

namespace Monocypher
{
    public static partial class monocypher
    {
        public const string MonocypherDll = "monocypher_native";

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

        private static void ExpectSize(string arg, int expectedSize, int size)
        {
            if (size != expectedSize) throw new ArgumentException($"Invalid length {size} for key. Expecting {expectedSize} bytes instead.", arg);
        }

        private static void ExpectSameBufferSize(string leftArg, int leftSize, string rightArg, int rightSize)
        {
            if (leftSize != rightSize) throw new ArgumentException($"Invalid length {leftSize} for {leftArg} not matching the length {rightSize} of {rightArg}", leftArg);
        }

        private static void ExpectSize8(string arg, int size) => ExpectSize(arg, 8, size);
        private static void ExpectSize12(string arg, int size) => ExpectSize(arg, 12, size);
        private static void ExpectSize16(string arg, int size) => ExpectSize(arg, 16, size);
        private static void ExpectSize24(string arg, int size) => ExpectSize(arg, 24, size);
        private static void ExpectSize32(string arg, int size) => ExpectSize(arg, 32, size);
        private static void ExpectSize64(string arg, int size) => ExpectSize(arg, 64, size);

    }
}
