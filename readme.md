# Monocypher<font color="FF8C44">.NET</font> [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/managed/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/native/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![NuGet](https://img.shields.io/nuget/v/Monocypher.svg)](https://www.nuget.org/packages/Monocypher)

<img align="right" width="160px" height="160px" src="https://raw.githubusercontent.com/xoofx/Monocypher.NET/master/img/monocypher_dotnet.png">

Monocypher.NET is a managed wrapper around [Monocypher](https://github.com/LoupVaillant/Monocypher) cryptographic library.

> The current _native_ version of Monocypher used by Monocypher.NET is `3.1.2`
## Features

- Provides the entire native Monocypher API in an efficient 1-to-1 mapping:
  - Authenticated Encryption (`RFC 8439` with `XChacha20` and `Poly1305`)
  - Hashing (`Blake2b`)
  - Password Key Derivation (`Argon2i`)
  - Key Exchange (`X25519`)
  - Public Key Signatures (`EdDSA` (RFC `8032`) with Blake2b and `edwards25519`)
  - ...[and more](https://monocypher.org/manual/)...
- Each raw native function is duplicated with a more friendly API using `Span`/`ReadOnlySpan` parameters.
- Compatible with `.NET 6.0+` and `.NET Standard 2.0+`

## Usage

Example of using the [`crypto_lock`](https://monocypher.org/manual/aead) API

```csharp
// Use static at the beginning of your file to
// import functions
using static Monocypher.Monocypher;

// ...

// Message authentication code
Span<byte> mac = stackalloc byte[16];
// Encrypted message
Span<byte> cipherText = stackalloc byte[16];
// Secret message
Span<byte> inputText = stackalloc byte[16];
inputText[0] = (byte)'a';
inputText[1] = (byte)'b';
inputText[2] = (byte)'c';
inputText[3] = (byte)'d';

// Random, secret session key
Span<byte> key = stackalloc byte[32];
RNGCryptoServiceProvider.Fill(key);
// Use only once per key
Span<byte> nonce = stackalloc byte[24];
RNGCryptoServiceProvider.Fill(nonce);

crypto_lock(mac, cipherText, key, nonce, inputText);

// mac contains the authenticated code
// cipherText contains the encrypted message

```

## Documentation

Because Monocypher.NET is a raw wrapper of Monocypher, the excellent [Monocypher manual](https://monocypher.org/manual/) can be used to easily dig into the API.

For example, the `crypto_lock` C API defined like this:

```c
void crypto_lock(uint8_t mac[16], uint8_t *cipher_text, const uint8_t key[32], const uint8_t nonce[24], const uint8_t *plain_text, size_t text_size);
```

is exposed with the following 2 functions in Monocypher.NET, one being a strict equivalent and the other using Span/ReadOnlySpan

```csharp
// Pure translation of the C API
public static void crypto_lock(ref Byte16 mac, IntPtr cipher_text, in Byte32 key, in Byte24 nonce, IntPtr plain_text, Monocypher.size_t text_size);

// API using Span/ReadOnlySpan
public static void crypto_lock(Span<byte> mac, Span<byte> cipher_text, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> plain_text)
```

## Platforms

Monocypher.NET is supported on the following platforms:

- `win-x64`, `win-x86`, `win-arm64`, `win-arm`
- `linux-x64`, `linux-arm64`, `linux-arm`
- `osx-x64`, `osx-arm64`

## Performance

The primary usage for Monocypher is for resources constrained platforms (e.g micro-controllers)
where the [code size and performance must be balanced](https://monocypher.org/speed).

For .NET, this constraint might be less important, so if you are looking for the fastest  cryptographic library, Monocypher.NET might not be the best candidate.

That being said, if you are building an IoT project using the C Monocypher and you want to communicate with a .NET project, you might want to make sure that the cryptographic library used is the same between the client and the server (even though that's not strictly required). In that case **Monocypher.NET is a good compromise**.

## How to Build?

You need to install the [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0). Then from the root folder:

```console
$ dotnet build src -c Release
```

In order to rebuild the native binaries, you need to run the build scripts from [ext](ext/readme.md)

## Credits

Monocypher.NET is just a wrapper and is entirely relying on the [Monocypher](https://monocypher.org/) C implementation developed by [Loup Vaillant](https://loup-vaillant.fr/).

## License

This software is released under the [BSD-Clause 2 license](https://opensource.org/licenses/BSD-2-Clause).

The native Monocypher is released with the following [BSD-Clause 2 license](https://github.com/LoupVaillant/Monocypher/blob/master/LICENCE.md) terms.

## Author

Alexandre Mutel aka [xoofx](http://xoofx.com).