using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

using static Monocypher.Monocypher;

namespace Monocypher.Tests
{
    /// <summary>
    /// Tests for all <see cref="Byte8"/>, <see cref="Byte12"/>, <see cref="Byte16"/>,
    /// <see cref="Byte24"/>, <see cref="Byte32"/> and <see cref="Byte64"/>.
    /// </summary>
    public class BufferTests
    {
        [Test]
        public void AsByte8()
        {
            const int size = 8;
            var buffer = Enumerable.Range(0, size).Select(x => (byte)x).ToArray().AsByte8();
            var bufferSpan = buffer.AsReadOnlySpan();
            Assert.AreEqual(size, bufferSpan.Length);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, bufferSpan[i], $"Invalid ByteBuffer value at position {i}");
            }

            var bufferStr = buffer.ToString();
            Assert.AreEqual(bufferStr, ToHexString(bufferSpan));
        }

        [Test]
        public void AsByte12()
        {
            const int size = 12;
            var buffer = Enumerable.Range(0, size).Select(x => (byte)x).ToArray().AsByte12();
            var bufferSpan = buffer.AsReadOnlySpan();
            Assert.AreEqual(size, bufferSpan.Length);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, bufferSpan[i], $"Invalid ByteBuffer value at position {i}");
            }

            var bufferStr = buffer.ToString();
            Assert.AreEqual(bufferStr, ToHexString(bufferSpan));
        }

        [Test]
        public void AsByte16()
        {
            const int size = 16;
            var buffer = Enumerable.Range(0, size).Select(x => (byte)x).ToArray().AsByte16();
            var bufferSpan = buffer.AsReadOnlySpan();
            Assert.AreEqual(size, bufferSpan.Length);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, bufferSpan[i], $"Invalid ByteBuffer value at position {i}");
            }

            var bufferStr = buffer.ToString();
            Assert.AreEqual(bufferStr, ToHexString(bufferSpan));
        }

        [Test]
        public void AsByte32()
        {
            const int size = 32;
            var buffer = Enumerable.Range(0, size).Select(x => (byte)x).ToArray().AsByte32();
            var bufferSpan = buffer.AsReadOnlySpan();
            Assert.AreEqual(size, bufferSpan.Length);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, bufferSpan[i], $"Invalid ByteBuffer value at position {i}");
            }

            var bufferStr = buffer.ToString();
            Assert.AreEqual(bufferStr, ToHexString(bufferSpan));
        }

        [Test]
        public void AsByte64()
        {
            const int size = 64;
            var buffer = Enumerable.Range(0, size).Select(x => (byte)x).ToArray().AsByte64();
            var bufferSpan = buffer.AsReadOnlySpan();
            Assert.AreEqual(size, bufferSpan.Length);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, bufferSpan[i], $"Invalid ByteBuffer value at position {i}");
            }

            var bufferStr = buffer.ToString();
            Assert.AreEqual(bufferStr, ToHexString(bufferSpan));
        }

        private static string ToHexString(ReadOnlySpan<byte> buffer)
        {
            var builder = new StringBuilder();
            foreach (var b in buffer)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}