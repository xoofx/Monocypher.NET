using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Monocypher
{
    public static partial class Monocypher
    {
        public static ref Byte8 AsByte8(this byte[] buffer) => ref AsRef<Byte8>(buffer);
        public static ref Byte12 AsByte12(this byte[] buffer) => ref AsRef<Byte12>(buffer);
        public static ref Byte16 AsByte16(this byte[] buffer) => ref AsRef<Byte16>(buffer);
        public static ref Byte24 AsByte24(this byte[] buffer) => ref AsRef<Byte24>(buffer);
        public static ref Byte32 AsByte32(this byte[] buffer) => ref AsRef<Byte32>(buffer);
        public static ref Byte64 AsByte64(this byte[] buffer) => ref AsRef<Byte64>(buffer);

        public static ref Byte8 AsByte8(this Span<byte> buffer) => ref AsRef<Byte8>(buffer);
        public static ref Byte12 AsByte12(this Span<byte> buffer) => ref AsRef<Byte12>(buffer);
        public static ref Byte16 AsByte16(this Span<byte> buffer) => ref AsRef<Byte16>(buffer);
        public static ref Byte24 AsByte24(this Span<byte> buffer) => ref AsRef<Byte24>(buffer);
        public static ref Byte32 AsByte32(this Span<byte> buffer) => ref AsRef<Byte32>(buffer);
        public static ref Byte64 AsByte64(this Span<byte> buffer) => ref AsRef<Byte64>(buffer);

        public static ref readonly Byte8 AsByte8(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte8>(buffer);
        public static ref readonly Byte12 AsByte12(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte12>(buffer);
        public static ref readonly Byte16 AsByte16(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte16>(buffer);
        public static ref readonly Byte24 AsByte24(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte24>(buffer);
        public static ref readonly Byte32 AsByte32(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte32>(buffer);
        public static ref readonly Byte64 AsByte64(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte64>(buffer);

        public static Span<byte> AsSpan(this ref Byte8 buffer) => ToSpan(ref buffer);
        public static Span<byte> AsSpan(this ref Byte12 buffer) => ToSpan(ref buffer);
        public static Span<byte> AsSpan(this ref Byte16 buffer) => ToSpan(ref buffer);
        public static Span<byte> AsSpan(this ref Byte24 buffer) => ToSpan(ref buffer);
        public static Span<byte> AsSpan(this ref Byte32 buffer) => ToSpan(ref buffer);
        public static Span<byte> AsSpan(this ref Byte64 buffer) => ToSpan(ref buffer);

        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte8 buffer) => ToReadOnlySpan(in buffer);
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte12 buffer) => ToReadOnlySpan(in buffer);
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte16 buffer) => ToReadOnlySpan(in buffer);
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte24 buffer) => ToReadOnlySpan(in buffer);
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte32 buffer) => ToReadOnlySpan(in buffer);
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte64 buffer) => ToReadOnlySpan(in buffer);

        private static unsafe Span<byte> ToSpan<T>(ref T data) where T : unmanaged
        {
            fixed (void* ptr = &data)
            {
                return new Span<byte>(ptr, Unsafe.SizeOf<T>());
            }
        }

        private static unsafe ReadOnlySpan<byte> ToReadOnlySpan<T>(in T data) where T : unmanaged
        {
            fixed (void* ptr = &data)
            {
                return new ReadOnlySpan<byte>(ptr, Unsafe.SizeOf<T>());
            }
        }

        private static ref T AsRef<T>(Span<byte> buffer) where T : unmanaged
        {
            return ref MemoryMarshal.GetReference(MemoryMarshal.Cast<byte, T>(buffer));
        }

        private static ref T AsRef<T>(ReadOnlySpan<byte> buffer) where T : unmanaged
        {
            return ref MemoryMarshal.GetReference(MemoryMarshal.Cast<byte, T>(buffer));
        }
    }
}