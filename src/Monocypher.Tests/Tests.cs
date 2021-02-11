using Microsoft.VisualBasic;
using NUnit.Framework;

using static Monocypher.monocypher;

namespace Monocypher.Tests
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            crypto_blake2b_ctx ctx = default;
            crypto_blake2b_init(ref ctx);
        }


        public void TestCryptoLockUnlock()
        {
            // TODO
        }
    }
}