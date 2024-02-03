using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

using static Monocypher.Monocypher;

namespace Monocypher.Tests
{
    public class Tests
    {
        [Test]
        public void SimpleBlake2Init()
        {
            crypto_blake2b_ctx ctx = default;
            crypto_blake2b_init(ref ctx, 256);
            Assert.AreNotEqual(0, (int)ctx.hash_size.Value, "Invalid hash_size returned from blake2b_init");
        }

        [Test]
        public void TestChaCha20()
        {
            Span<byte> ciptherText = stackalloc byte[32];
            Span<byte> key = new byte[32]
            {
                0xee, 0xc7, 0xb5, 0x3d, 0x05, 0xd9, 0xcc, 0x81, 0x61, 0x84, 0xc4, 0x9f, 0x65, 0x2f, 0x37, 0x04, 0x70, 0xa4, 0x52, 0x22, 0xa5, 0xc7, 0x4d, 0xa4, 0x2e, 0x7f, 0x09, 0xd6, 0x86, 0x2d, 0x6d, 0xd0,
            };
            crypto_chacha20_h(ciptherText, key, new Byte16().AsReadOnlySpan());

            Span<byte> expected = new byte[32]
            {
                0xf5, 0xc3, 0xeb, 0x70, 0xb2, 0x26, 0x55, 0x5e, 0x34, 0x90, 0xc3, 0x2e, 0xd1, 0x62, 0xd3, 0x8d, 0x83, 0x71, 0xba, 0x8c, 0x95, 0xe5, 0xa4, 0x0c, 0x33, 0x59, 0x01, 0x20, 0x65, 0x8c, 0x15, 0x5a
            };

            Console.WriteLine(ciptherText.ToHexBytes());
            Assert.IsTrue(expected.SequenceEqual(ciptherText), "Expected output is invalid for chacha20");
        }

        [Test]
        public void CryptoLockUnlock()
        {
            Span<byte> mac = stackalloc byte[16];
            Span<byte> emptyText = stackalloc byte[16];
            Span<byte> cipherText = stackalloc byte[16];
            Span<byte> inputText = stackalloc byte[16];
            inputText[0] = (byte)'a';
            inputText[1] = (byte)'b';
            inputText[2] = (byte)'c';
            inputText[3] = (byte)'d';

            Span<byte> key = stackalloc byte[32];
            RandomNumberGenerator.Fill(key);
            Span<byte> nonce = stackalloc byte[24];
            RandomNumberGenerator.Fill(nonce);

            crypto_aead_lock(cipherText, mac, key, nonce, ReadOnlySpan<byte>.Empty, inputText);

            var builder = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                builder.Append($"{cipherText[i]:X2}");
            }
            Console.WriteLine($"cipher: {builder}");

            Assert.False(cipherText.SequenceEqual(inputText), "crypto_lock failed. Spans are the same");
            Assert.False(cipherText.SequenceEqual(emptyText), "crypto_lock failed. cipher text is empty");

            // Verify that we get the same output from unlock
            Span<byte> outputText = stackalloc byte[16];
            crypto_aead_unlock(outputText, mac, key, nonce, ReadOnlySpan<byte>.Empty, cipherText);

            Assert.True(outputText.SequenceEqual(inputText), "crypto_unlock failed. Spans are different");
        }
    }
}