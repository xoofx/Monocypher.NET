# Monocypher<font color="FF8C44">.NET</font> [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/managed/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/native/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![NuGet](https://img.shields.io/nuget/v/Monocypher.NET.svg)](https://www.nuget.org/packages/Monocypher.NET/)

<img align="right" width="160px" height="160px" src="img/monocypher_dotnet.png">

Monocypher.NET is a managed wrapper around [Monocypher](https://github.com/LoupVaillant/Monocypher) cryptographic library.

> The current _native_ version of Monocypher used by Monocypher.NET is `3.1.2`
## Features

- Provides the entire native Monocypher API in an efficient 1-to-1 mapping:
  - Authenticated Encryption
  - Hashing
  - Password Key Derivation
  - Key Exchange
  - Public Key Signatures
  - Constant Time Comparison
  - ...and more...
- For each function, a duplicated API is provided using `Span`/`ReadOnlySpan` parameters.
- Compatible with `.NET 5.0+` and `.NET Standard 2.0+`

## Usage

Example of the [`crypto_lock`](https://monocypher.org/manual/aead) API

```csharp
// Add this at the beginning of your file
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

Because Monocypher.NET is a raw wrapper of Monocypher, the [Monocypher manual](https://monocypher.org/manual/) can be used to easily dig into the API.

## Platforms

Monocypher.NET is supported on the following platforms:

- `win-x64`, `win-x86`, `win-arm64`, `win-arm`
- `linux-x64`, `linux-arm64`, `linux-arm`
- `osx-x64`

## Performance

The primary usage for Monocypher is for resources constrained platforms (e.g micro-controllers)
where the [code size and performance must be balanced](https://monocypher.org/speed).

For .NET, this constraint might be less important, so if you are looking for the fastest  cryptographic library, Monocypher.NET might not be the best candidate.

That being said, if you are building an IoT project using the C Monocypher and you want to communicate with a .NET project, you might want to make sure that the cryptographic library used is the same between the client and the server (even though that's not strictly required). In that case **Monocypher.NET is a good compromise**.

## How to Build?

You need to install the [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0). Then from the root folder:

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