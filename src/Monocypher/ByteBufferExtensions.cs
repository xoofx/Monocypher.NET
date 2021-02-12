using System;
using System.Runtime.InteropServices;

namespace Monocypher
{
    public static partial class Monocypher
    {
        public static ref Byte8 AsByte8(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte8>(buffer);
        public static ref Byte12 AsByte12(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte12>(buffer);
        public static ref Byte16 AsByte16(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte16>(buffer);
        public static ref Byte24 AsByte24(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte24>(buffer);
        public static ref Byte32 AsByte32(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte32>(buffer);
        public static ref Byte64 AsByte64(this byte[] buffer) => ref MemoryMarshal.AsRef<Byte64>(buffer);

        public static ref Byte8 AsByte8(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte8>(buffer);
        public static ref Byte12 AsByte12(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte12>(buffer);
        public static ref Byte16 AsByte16(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte16>(buffer);
        public static ref Byte24 AsByte24(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte24>(buffer);
        public static ref Byte32 AsByte32(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte32>(buffer);
        public static ref Byte64 AsByte64(this Span<byte> buffer) => ref MemoryMarshal.AsRef<Byte64>(buffer);

        public static ref readonly Byte8 AsReadOnlyByte8(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte8>(buffer);
        public static ref readonly Byte12 AsReadOnlyByte12(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte12>(buffer);
        public static ref readonly Byte16 AsReadOnlyByte16(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte16>(buffer);
        public static ref readonly Byte24 AsReadOnlyByte24(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte24>(buffer);
        public static ref readonly Byte32 AsReadOnlyByte32(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte32>(buffer);
        public static ref readonly Byte64 AsReadOnlyByte64(this ReadOnlySpan<byte> buffer) => ref MemoryMarshal.AsRef<Byte64>(buffer);

        public static Span<byte> AsSpan(this ref Byte8 buffer) => MemoryMarshal.Cast<Byte8, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static Span<byte> AsSpan(this ref Byte12 buffer) => MemoryMarshal.Cast<Byte12, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static Span<byte> AsSpan(this ref Byte16 buffer) => MemoryMarshal.Cast<Byte16, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static Span<byte> AsSpan(this ref Byte24 buffer) => MemoryMarshal.Cast<Byte24, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static Span<byte> AsSpan(this ref Byte32 buffer) => MemoryMarshal.Cast<Byte32, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static Span<byte> AsSpan(this ref Byte64 buffer) => MemoryMarshal.Cast<Byte64, byte>(MemoryMarshal.CreateSpan(ref buffer, 1));

        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte8 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte12 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte16 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte24 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte32 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
        public static ReadOnlySpan<byte> AsReadOnlySpan(this ref Byte64 buffer) => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref buffer, 1));
    }
}