using System;
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
            crypto_blake2b_init(ref ctx);
            Assert.AreNotEqual(0, (int)ctx.hash_size.Value, "Invalid hash_size returned from blake2b_init");
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
            RNGCryptoServiceProvider.Fill(key);
            Span<byte> nonce = stackalloc byte[24];
            RNGCryptoServiceProvider.Fill(nonce);

            crypto_lock(mac, cipherText, key, nonce, inputText);

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
            crypto_unlock(outputText, key, nonce, mac, cipherText);

            Assert.True(outputText.SequenceEqual(inputText), "crypto_unlock failed. Spans are different");
        }
    }
}