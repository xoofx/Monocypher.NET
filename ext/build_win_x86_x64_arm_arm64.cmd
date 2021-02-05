REM / @echo off
REM Build all Windows CPUs
setlocal
RMDIR %~dp0build /S /Q
set PACKAGE_FOLDER=build\package
set PACKAGE_FOLDER_X86=build\package\win-x86\native
set PACKAGE_FOLDER_X64=build\package\win-x64\native
set PACKAGE_FOLDER_ARM=build\package\win-arm\native
set PACKAGE_FOLDER_ARM64=build\package\win-arm64\native
set BUILD_PREFIX=build\win32_
mkdir %PACKAGE_FOLDER%
REM win32-x86
cmake -G"Visual Studio 16 2019" -Awin32 -B%BUILD_PREFIX%x86 -H.
cmake --build %BUILD_PREFIX%x86 --target ALL_BUILD --config Release
mkdir %PACKAGE_FOLDER_X86%
copy %BUILD_PREFIX%x86\Release\*.dll %PACKAGE_FOLDER_X86%\
REM win32-x64
cmake -G"Visual Studio 16 2019" -Ax64 -B%BUILD_PREFIX%x64 -H.
cmake --build %BUILD_PREFIX%x64 --target ALL_BUILD --config Release
mkdir %PACKAGE_FOLDER_X64%
copy %BUILD_PREFIX%x64\Release\*.dll %PACKAGE_FOLDER_X64%\
REM win32-arm
cmake -G"Visual Studio 16 2019" -Aarm -B%BUILD_PREFIX%arm -H.
cmake --build %BUILD_PREFIX%arm --target ALL_BUILD --config Release
mkdir %PACKAGE_FOLDER_ARM%
copy %BUILD_PREFIX%arm\Release\*.dll %PACKAGE_FOLDER_ARM%\
REM win32-arm64
cmake -G"Visual Studio 16 2019" -Aarm64 -B%BUILD_PREFIX%arm64 -H.
cmake --build %BUILD_PREFIX%arm64 --target ALL_BUILD --config Release
mkdir %PACKAGE_FOLDER_ARM64%
copy %BUILD_PREFIX%arm64\Release\*.dll %PACKAGE_FOLDER_ARM64%\