using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Monocypher
{
    public static partial class Monocypher
    {
        /// <summary>
        /// Returns a 8-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 8 bytes.</param>
        /// <returns>A reference to a 8-byte struct mapping to the same array</returns>
        public static ref Byte8 AsByte8(this byte[] buffer) => ref AsRef<Byte8>(buffer);
        /// <summary>
        /// Returns a 12-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 12 bytes.</param>
        /// <returns>A reference to a 12-byte struct mapping to the same array</returns>
        public static ref Byte12 AsByte12(this byte[] buffer) => ref AsRef<Byte12>(buffer);
        /// <summary>
        /// Returns a 16-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 16 bytes.</param>
        /// <returns>A reference to a 16-byte struct mapping to the same array</returns>
        public static ref Byte16 AsByte16(this byte[] buffer) => ref AsRef<Byte16>(buffer);
        /// <summary>
        /// Returns a 24-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 24 bytes.</param>
        /// <returns>A reference to a 24-byte struct mapping to the same array</returns>
        public static ref Byte24 AsByte24(this byte[] buffer) => ref AsRef<Byte24>(buffer);
        /// <summary>
        /// Returns a 32-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 32 bytes.</param>
        /// <returns>A reference to a 32-byte struct mapping to the same array</returns>
        public static ref Byte32 AsByte32(this byte[] buffer) => ref AsRef<Byte32>(buffer);
        /// <summary>
        /// Returns a 64-byte struct reference from the specified byte array.
        /// </summary>
        /// <param name="buffer">A byte array with 64 bytes.</param>
        /// <returns>A reference to a 64-byte struct mapping to the same array</returns>
        public static ref Byte64 AsByte64(this byte[] buffer) => ref AsRef<Byte64>(buffer);

        /// <summary>
        /// Returns a 8-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 8 bytes.</param>
        /// <returns>A reference to a 8-byte struct mapping from the specified span.</returns>
        public static ref Byte8 AsByte8(this Span<byte> buffer) => ref AsRef<Byte8>(buffer);
        /// <summary>
        /// Returns a 12-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 12 bytes.</param>
        /// <returns>A reference to a 12-byte struct mapping from the specified span.</returns>
        public static ref Byte12 AsByte12(this Span<byte> buffer) => ref AsRef<Byte12>(buffer);
        /// <summary>
        /// Returns a 16-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 16 bytes.</param>
        /// <returns>A reference to a 16-byte struct mapping from the specified span.</returns>
        public static ref Byte16 AsByte16(this Span<byte> buffer) => ref AsRef<Byte16>(buffer);
        /// <summary>
        /// Returns a 24-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 24 bytes.</param>
        /// <returns>A reference to a 24-byte struct mapping from the specified span.</returns>
        public static ref Byte24 AsByte24(this Span<byte> buffer) => ref AsRef<Byte24>(buffer);
        /// <summary>
        /// Returns a 32-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 32 bytes.</param>
        /// <returns>A reference to a 32-byte struct mapping from the specified span.</returns>
        public static ref Byte32 AsByte32(this Span<byte> buffer) => ref AsRef<Byte32>(buffer);
        /// <summary>
        /// Returns a 64-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 64 bytes.</param>
        /// <returns>A reference to a 64-byte struct mapping from the specified span.</returns>
        public static ref Byte64 AsByte64(this Span<byte> buffer) => ref AsRef<Byte64>(buffer);

        /// <summary>
        /// Returns a 8-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 8 bytes.</param>
        /// <returns>A reference to a 8-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte8 AsByte8(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte8>(buffer);
        /// <summary>
        /// Returns a 12-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 12 bytes.</param>
        /// <returns>A reference to a 12-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte12 AsByte12(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte12>(buffer);
        /// <summary>
        /// Returns a 16-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 16 bytes.</param>
        /// <returns>A reference to a 16-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte16 AsByte16(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte16>(buffer);
        /// <summary>
        /// Returns a 24-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 24 bytes.</param>
        /// <returns>A reference to a 24-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte24 AsByte24(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte24>(buffer);
        /// <summary>
        /// Returns a 32-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 32 bytes.</param>
        /// <returns>A reference to a 32-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte32 AsByte32(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte32>(buffer);
        /// <summary>
        /// Returns a 64-byte struct reference from the specified span.
        /// </summary>
        /// <param name="buffer">A span with 64 bytes.</param>
        /// <returns>A reference to a 64-byte struct mapping from the specified span.</returns>
        public static ref readonly Byte64 AsByte64(this ReadOnlySpan<byte> buffer) => ref AsRef<Byte64>(buffer);

        /// <summary>
        /// Returns a 8-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 8-byte struct.</param>
        /// <returns>A 8-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte8 buffer) => ToSpan(ref buffer);
        /// <summary>
        /// Returns a 12-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 12-byte struct.</param>
        /// <returns>A 12-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte12 buffer) => ToSpan(ref buffer);
        /// <summary>
        /// Returns a 16-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 16-byte struct.</param>
        /// <returns>A 16-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte16 buffer) => ToSpan(ref buffer);
        /// <summary>
        /// Returns a 24-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 24-byte struct.</param>
        /// <returns>A 24-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte24 buffer) => ToSpan(ref buffer);
        /// <summary>
        /// Returns a 32-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 32-byte struct.</param>
        /// <returns>A 32-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte32 buffer) => ToSpan(ref buffer);
        /// <summary>
        /// Returns a 64-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 64-byte struct.</param>
        /// <returns>A 64-byte span mapping from the specified struct reference.</returns>
        public static Span<byte> AsSpan(this ref Byte64 buffer) => ToSpan(ref buffer);

        /// <summary>
        /// Returns a 8-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 8-byte struct.</param>
        /// <returns>A 8-byte span mapping from the specified struct reference.</returns>
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte8 buffer) => ToReadOnlySpan(in buffer);
        /// <summary>
        /// Returns a 12-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 12-byte struct.</param>
        /// <returns>A 12-byte span mapping from the specified struct reference.</returns>
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte12 buffer) => ToReadOnlySpan(in buffer);
        /// <summary>
        /// Returns a 16-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 16-byte struct.</param>
        /// <returns>A 16-byte span mapping from the specified struct reference.</returns>
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte16 buffer) => ToReadOnlySpan(in buffer);
        /// <summary>
        /// Returns a 24-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 24-byte struct.</param>
        /// <returns>A 24-byte span mapping from the specified struct reference.</returns>
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte24 buffer) => ToReadOnlySpan(in buffer);
        /// <summary>
        /// Returns a 32-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 32-byte struct.</param>
        /// <returns>A 32-byte span mapping from the specified struct reference.</returns>
        public static ReadOnlySpan<byte> AsReadOnlySpan(this in Byte32 buffer) => ToReadOnlySpan(in buffer);
        /// <summary>
        /// Returns a 64-byte span from the specified struct reference.
        /// </summary>
        /// <param name="buffer">A 64-byte struct.</param>
        /// <returns>A 64-byte span mapping from the specified struct reference.</returns>
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