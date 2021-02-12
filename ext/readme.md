# Building `monocypher_native` shared library

Prerequisites, you need to install:

- Powershell core [pwsh](https://github.com/PowerShell/PowerShell)
- [CMake >= 3.16](https://cmake.org/download/)

Then per platform, you also need to install:
- On Windows: VS 2019 + C++ toolchain x86/x64/arm/arm64
- On Linux: C++ toolchain x64/arm/arm64

The script can be run `.\build_monocypher_native.ps1` and should generate `monocypher_native.dll` and platform equivalents to `build\package` folder.