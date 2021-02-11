cmake_minimum_required(VERSION 3.19)
project(monocypher)

set(MONOCYPHER_SRC_FOLDER "Monocypher/src")

set(MONOCYPHER_SOURCES
    "${MONOCYPHER_SRC_FOLDER}/monocypher.h"
    "${MONOCYPHER_SRC_FOLDER}/monocypher.c"
    "${MONOCYPHER_SRC_FOLDER}/optional/monocypher-ed25519.h"
    "${MONOCYPHER_SRC_FOLDER}/optional/monocypher-ed25519.c"
)

include_directories(${MONOCYPHER_SRC_FOLDER})

if(MSVC)
    # use static linking with CRT
    set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
endif()

add_library(monocypher_native SHARED ${MONOCYPHER_SOURCES} Monocypher.def)