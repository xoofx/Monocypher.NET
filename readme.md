# Monocypher.NET<font color="A80016">.NET</span> [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/managed/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![Build Status](https://github.com/xoofx/Monocypher.NET/workflows/native/badge.svg?branch=master)](https://github.com/xoofx/Monocypher.NET/actions) [![NuGet](https://img.shields.io/nuget/v/Monocypher.NET.svg)](https://www.nuget.org/packages/Monocypher.NET/)

<img align="right" width="160px" height="160px" src="img/logo.png">

Monocypher.NET is a managed wrapper around [Monocypher](https://github.com/LoupVaillant/Monocypher) cryptographic library.

> The current _native_ version of Monocypher used by Monocypher.NET is `3.1.2`

## Features

TODO


## Usage

TODO

## Platforms

Monocypher.NET is supported on the following platforms:

- `win-x64`, `win-x86`, `win-arm64`, `win-arm`
- `linux-x64`, `linux-arm64`, `linux-arm`
- `osx-x64`

## How to Build?

You need to install the [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0). Then from the root folder:

```console
$ dotnet build src -c Release
```

In order to rebuild the native binaries, you need to run the build scripts from [ext](ext/readme.md)

## License

This software is released under the [BSD-Clause 2 license](https://opensource.org/licenses/BSD-2-Clause).

## Author

Alexandre Mutel aka [xoofx](http://xoofx.com).