using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Monocypher
{
    /// <summary>
    /// Static class containing all cryptographic functions.
    /// </summary>
    public static partial class Monocypher
    {
        private static readonly char[] HexBytes = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private const string MonocypherDll = "monocypher_native";

#if !NETSTANDARD2_0
        // size_t in NET5.0 is relying on nint
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public readonly partial struct size_t : IEquatable<size_t>
        {
            public size_t(nint value) => this.Value = value;

            public readonly nint Value;

            public bool Equals(size_t other) => Value.Equals(other.Value);

            public override bool Equals(object obj) => obj is size_t other && Equals(other);

            public override int GetHashCode() => Value.GetHashCode();

            public override string ToString() => Value.ToString();

            public static implicit operator nint(size_t from) => from.Value;

            public static implicit operator size_t(nint from) => new size_t(from);

            public static bool operator ==(size_t left, size_t right) => left.Equals(right);

            public static bool operator !=(size_t left, size_t right) => !left.Equals(right);
        }
#endif
        /// <summary>
        /// A native int size (e.g 8 bytes for a 64bit processor, 4 bytes for a 32bit processor).
        /// </summary>
        public partial struct size_t
        {
            public static explicit operator int(size_t value)
            {
                return (int)value.Value;
            }

            public static implicit operator size_t(int value)
            {
#if !NETSTANDARD2_0
                return new size_t(value);
#else
                return new size_t(new IntPtr(value));
#endif
            }
        }

        /// <summary>
        /// Converts the input span to hexadecimal bytes representation (e.g "0x12, 0xaf, 0x56...")
        /// </summary>
        /// <param name="buffer">The buffer to convert to hex bytes.</param>
        /// <param name="builder">The builder to received hexadecimal bytes.</param>
        public static void ToHexBytes(this ReadOnlySpan<byte> buffer, StringBuilder builder)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i > 0) builder.Append(", ");
                builder.Append("0x");
                AppendByteToHex(builder, buffer[i]);
            }
        }

        /// <summary>
        /// Converts the input span to hexadecimal bytes representation (e.g "0x12, 0xaf, 0x56...")
        /// </summary>
        /// <param name="buffer">The buffer to convert to hex bytes.</param>
        /// <returns>A string representation with hexadecimal bytes from the input span buffer.</returns>
        public static string ToHexBytes(this ReadOnlySpan<byte> buffer)
        {
            var builder = new StringBuilder();
            ToHexBytes(buffer, builder);
            return builder.ToString();
        }

        /// <summary>
        /// Converts the input span to hexadecimal bytes representation (e.g "0x12, 0xaf, 0x56...")
        /// </summary>
        /// <param name="buffer">The buffer to convert to hex bytes.</param>
        /// <param name="builder">The builder to received hexadecimal bytes.</param>
        public static void ToHexBytes(this Span<byte> buffer, StringBuilder builder)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i > 0) builder.Append(", ");
                builder.Append("0x");
                AppendByteToHex(builder, buffer[i]);
            }
        }

        /// <summary>
        /// Converts the input span to hexadecimal bytes representation (e.g "0x12, 0xaf, 0x56...")
        /// </summary>
        /// <param name="buffer">The buffer to convert to hex bytes.</param>
        /// <returns>A string representation with hexadecimal bytes from the input span buffer.</returns>
        public static string ToHexBytes(this Span<byte> buffer)
        {
            var builder = new StringBuilder();
            ToHexBytes(buffer, builder);
            return builder.ToString();
        }


        /// <summary>
        /// A 8-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 8)]
        public struct Byte8
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;

            public override string ToString()
            {
                var builder = new StringBuilder(8 * 2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                return builder.ToString();
            }
        }

        /// <summary>
        /// A 12-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 12)]
        public struct Byte12
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;
            public byte Value8;
            public byte Value9;
            public byte Value10;
            public byte Value11;
            public override string ToString()
            {
                var builder = new StringBuilder(12 * 2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                AppendByteToHex(builder, Value8);
                AppendByteToHex(builder, Value9);
                AppendByteToHex(builder, Value10);
                AppendByteToHex(builder, Value11);
                return builder.ToString();
            }
        }

        /// <summary>
        /// A 16-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public struct Byte16
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;
            public byte Value8;
            public byte Value9;
            public byte Value10;
            public byte Value11;
            public byte Value12;
            public byte Value13;
            public byte Value14;
            public byte Value15;
            public override string ToString()
            {
                var builder = new StringBuilder(16 * 2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                AppendByteToHex(builder, Value8);
                AppendByteToHex(builder, Value9);
                AppendByteToHex(builder, Value10);
                AppendByteToHex(builder, Value11);
                AppendByteToHex(builder, Value12);
                AppendByteToHex(builder, Value13);
                AppendByteToHex(builder, Value14);
                AppendByteToHex(builder, Value15);
                return builder.ToString();
            }
        }

        /// <summary>
        /// A 24-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 24)]
        public struct Byte24
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;
            public byte Value8;
            public byte Value9;
            public byte Value10;
            public byte Value11;
            public byte Value12;
            public byte Value13;
            public byte Value14;
            public byte Value15;
            public byte Value16;
            public byte Value17;
            public byte Value18;
            public byte Value19;
            public byte Value20;
            public byte Value21;
            public byte Value22;
            public byte Value23;

            public override string ToString()
            {
                var builder = new StringBuilder(24*2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                AppendByteToHex(builder, Value8);
                AppendByteToHex(builder, Value9);
                AppendByteToHex(builder, Value10);
                AppendByteToHex(builder, Value11);
                AppendByteToHex(builder, Value12);
                AppendByteToHex(builder, Value13);
                AppendByteToHex(builder, Value14);
                AppendByteToHex(builder, Value15);
                AppendByteToHex(builder, Value16);
                AppendByteToHex(builder, Value17);
                AppendByteToHex(builder, Value18);
                AppendByteToHex(builder, Value19);
                AppendByteToHex(builder, Value20);
                AppendByteToHex(builder, Value21);
                AppendByteToHex(builder, Value22);
                AppendByteToHex(builder, Value23);
                return builder.ToString();
            }
        }

        /// <summary>
        /// A 32-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 32)]
        public struct Byte32
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;
            public byte Value8;
            public byte Value9;
            public byte Value10;
            public byte Value11;
            public byte Value12;
            public byte Value13;
            public byte Value14;
            public byte Value15;
            public byte Value16;
            public byte Value17;
            public byte Value18;
            public byte Value19;
            public byte Value20;
            public byte Value21;
            public byte Value22;
            public byte Value23;
            public byte Value24;
            public byte Value25;
            public byte Value26;
            public byte Value27;
            public byte Value28;
            public byte Value29;
            public byte Value30;
            public byte Value31;

            public override string ToString()
            {
                var builder = new StringBuilder(32*2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                AppendByteToHex(builder, Value8);
                AppendByteToHex(builder, Value9);
                AppendByteToHex(builder, Value10);
                AppendByteToHex(builder, Value11);
                AppendByteToHex(builder, Value12);
                AppendByteToHex(builder, Value13);
                AppendByteToHex(builder, Value14);
                AppendByteToHex(builder, Value15);
                AppendByteToHex(builder, Value16);
                AppendByteToHex(builder, Value17);
                AppendByteToHex(builder, Value18);
                AppendByteToHex(builder, Value19);
                AppendByteToHex(builder, Value20);
                AppendByteToHex(builder, Value21);
                AppendByteToHex(builder, Value22);
                AppendByteToHex(builder, Value23);
                AppendByteToHex(builder, Value24);
                AppendByteToHex(builder, Value25);
                AppendByteToHex(builder, Value26);
                AppendByteToHex(builder, Value27);
                AppendByteToHex(builder, Value28);
                AppendByteToHex(builder, Value29);
                AppendByteToHex(builder, Value30);
                AppendByteToHex(builder, Value31);
                return builder.ToString();
            }
        }

        /// <summary>
        /// A 64-byte struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 64)]
        public struct Byte64
        {
            public byte Value0;
            public byte Value1;
            public byte Value2;
            public byte Value3;
            public byte Value4;
            public byte Value5;
            public byte Value6;
            public byte Value7;
            public byte Value8;
            public byte Value9;
            public byte Value10;
            public byte Value11;
            public byte Value12;
            public byte Value13;
            public byte Value14;
            public byte Value15;
            public byte Value16;
            public byte Value17;
            public byte Value18;
            public byte Value19;
            public byte Value20;
            public byte Value21;
            public byte Value22;
            public byte Value23;
            public byte Value24;
            public byte Value25;
            public byte Value26;
            public byte Value27;
            public byte Value28;
            public byte Value29;
            public byte Value30;
            public byte Value31;
            public byte Value32;
            public byte Value33;
            public byte Value34;
            public byte Value35;
            public byte Value36;
            public byte Value37;
            public byte Value38;
            public byte Value39;
            public byte Value40;
            public byte Value41;
            public byte Value42;
            public byte Value43;
            public byte Value44;
            public byte Value45;
            public byte Value46;
            public byte Value47;
            public byte Value48;
            public byte Value49;
            public byte Value50;
            public byte Value51;
            public byte Value52;
            public byte Value53;
            public byte Value54;
            public byte Value55;
            public byte Value56;
            public byte Value57;
            public byte Value58;
            public byte Value59;
            public byte Value60;
            public byte Value61;
            public byte Value62;
            public byte Value63;

            public override string ToString()
            {
                var builder = new StringBuilder(64*2);
                AppendByteToHex(builder, Value0);
                AppendByteToHex(builder, Value1);
                AppendByteToHex(builder, Value2);
                AppendByteToHex(builder, Value3);
                AppendByteToHex(builder, Value4);
                AppendByteToHex(builder, Value5);
                AppendByteToHex(builder, Value6);
                AppendByteToHex(builder, Value7);
                AppendByteToHex(builder, Value8);
                AppendByteToHex(builder, Value9);
                AppendByteToHex(builder, Value10);
                AppendByteToHex(builder, Value11);
                AppendByteToHex(builder, Value12);
                AppendByteToHex(builder, Value13);
                AppendByteToHex(builder, Value14);
                AppendByteToHex(builder, Value15);
                AppendByteToHex(builder, Value16);
                AppendByteToHex(builder, Value17);
                AppendByteToHex(builder, Value18);
                AppendByteToHex(builder, Value19);
                AppendByteToHex(builder, Value20);
                AppendByteToHex(builder, Value21);
                AppendByteToHex(builder, Value22);
                AppendByteToHex(builder, Value23);
                AppendByteToHex(builder, Value24);
                AppendByteToHex(builder, Value25);
                AppendByteToHex(builder, Value26);
                AppendByteToHex(builder, Value27);
                AppendByteToHex(builder, Value28);
                AppendByteToHex(builder, Value29);
                AppendByteToHex(builder, Value30);
                AppendByteToHex(builder, Value31);
                AppendByteToHex(builder, Value32);
                AppendByteToHex(builder, Value33);
                AppendByteToHex(builder, Value34);
                AppendByteToHex(builder, Value35);
                AppendByteToHex(builder, Value36);
                AppendByteToHex(builder, Value37);
                AppendByteToHex(builder, Value38);
                AppendByteToHex(builder, Value39);
                AppendByteToHex(builder, Value40);
                AppendByteToHex(builder, Value41);
                AppendByteToHex(builder, Value42);
                AppendByteToHex(builder, Value43);
                AppendByteToHex(builder, Value44);
                AppendByteToHex(builder, Value45);
                AppendByteToHex(builder, Value46);
                AppendByteToHex(builder, Value47);
                AppendByteToHex(builder, Value48);
                AppendByteToHex(builder, Value49);
                AppendByteToHex(builder, Value50);
                AppendByteToHex(builder, Value51);
                AppendByteToHex(builder, Value52);
                AppendByteToHex(builder, Value53);
                AppendByteToHex(builder, Value54);
                AppendByteToHex(builder, Value55);
                AppendByteToHex(builder, Value56);
                AppendByteToHex(builder, Value57);
                AppendByteToHex(builder, Value58);
                AppendByteToHex(builder, Value59);
                AppendByteToHex(builder, Value60);
                AppendByteToHex(builder, Value61);
                AppendByteToHex(builder, Value62);
                AppendByteToHex(builder, Value63);
                return builder.ToString();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char ByteHighToHex(byte value) => HexBytes[(value >> 4) & 0x0F];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char ByteLowToHex(byte value) => HexBytes[value & 0xF];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendByteToHex(StringBuilder builder, byte value)
        {
            builder.Append(ByteHighToHex(value));
            builder.Append(ByteLowToHex(value));
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
