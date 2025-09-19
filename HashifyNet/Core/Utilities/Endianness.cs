// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the ""Software""), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// * SOFTWARE.
// *
// *
// * Please refer to LICENSE file.
// *
// ******************************************************************************
// *

using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace HashifyNet.Core.Utilities
{
	/// <summary>
	/// Provides utility methods for converting numeric values to and from their little-endian byte representations.
	/// </summary>
	/// <remarks>This class includes methods for encoding unsigned integers into little-endian byte arrays and
	/// decoding little-endian byte arrays back into unsigned integers. It is designed for scenarios where explicit control
	/// over byte order is required, such as working with binary protocols or file formats that use little-endian
	/// encoding.</remarks>
	internal static class Endianness
	{
		/// <summary>
		/// Converts the specified 32-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(uint value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
			buffer[offset + 2] = (byte)(value >> 16);
			buffer[offset + 3] = (byte)(value >> 24);
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(int value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((uint)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(uint value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
			buffer[offset + 2] = (byte)(value >> 16);
			buffer[offset + 3] = (byte)(value >> 24);
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(int value, Span<byte> buffer, int offset)
		{
			ToLittleEndianBytes((uint)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ushort value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(short value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((ushort)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ushort value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(short value, Span<byte> buffer, int offset)
		{
			ToLittleEndianBytes((ushort)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the least significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in increasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ulong value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
			buffer[offset + 2] = (byte)(value >> 16);
			buffer[offset + 3] = (byte)(value >> 24);
			buffer[offset + 4] = (byte)(value >> 32);
			buffer[offset + 5] = (byte)(value >> 40);
			buffer[offset + 6] = (byte)(value >> 48);
			buffer[offset + 7] = (byte)(value >> 56);
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the least significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in increasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(long value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((ulong)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the least significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in increasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ulong value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
			buffer[offset + 2] = (byte)(value >> 16);
			buffer[offset + 3] = (byte)(value >> 24);
			buffer[offset + 4] = (byte)(value >> 32);
			buffer[offset + 5] = (byte)(value >> 40);
			buffer[offset + 6] = (byte)(value >> 48);
			buffer[offset + 7] = (byte)(value >> 56);
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to its little-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the least significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in increasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the little-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(long value, Span<byte> buffer, int offset)
		{
			ToLittleEndianBytes((ulong)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ushort value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 8);
			buffer[offset + 1] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(short value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((ushort)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ushort value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 8);
			buffer[offset + 1] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 2 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(short value, Span<byte> buffer, int offset)
		{
			ToBigEndianBytes((ushort)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(uint value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(int value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((uint)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(uint value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes exactly 4 bytes to the buffer, starting at the specified offset. Ensure that
		/// the buffer has sufficient capacity to accommodate the bytes being written.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation will be written.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(int value, Span<byte> buffer, int offset)
		{
			ToBigEndianBytes((uint)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the most significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in decreasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ulong value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 56);
			buffer[offset + 1] = (byte)(value >> 48);
			buffer[offset + 2] = (byte)(value >> 40);
			buffer[offset + 3] = (byte)(value >> 32);
			buffer[offset + 4] = (byte)(value >> 24);
			buffer[offset + 5] = (byte)(value >> 16);
			buffer[offset + 6] = (byte)(value >> 8);
			buffer[offset + 7] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the most significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in decreasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(long value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((ulong)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the most significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in decreasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ulong value, Span<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[offset] = (byte)(value >> 56);
			buffer[offset + 1] = (byte)(value >> 48);
			buffer[offset + 2] = (byte)(value >> 40);
			buffer[offset + 3] = (byte)(value >> 32);
			buffer[offset + 4] = (byte)(value >> 24);
			buffer[offset + 5] = (byte)(value >> 16);
			buffer[offset + 6] = (byte)(value >> 8);
			buffer[offset + 7] = (byte)value;
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to its big-endian byte representation and writes the result to
		/// the specified buffer at the given offset.
		/// </summary>
		/// <remarks>This method writes the most significant byte of <paramref name="value"/> to <paramref
		/// name="buffer"/> at the specified <paramref name="offset"/>, followed by the remaining bytes in decreasing order
		/// of significance.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <param name="buffer">The byte array to which the big-endian representation of <paramref name="value"/> will be written.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin writing the bytes.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(long value, Span<byte> buffer, int offset)
		{
			ToBigEndianBytes((ulong)value, buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToLittleEndian(ulong value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToLittleEndian(long value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToLittleEndian(uint value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToLittleEndian(int value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToLittleEndian(ushort value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToLittleEndian(short value)
		{
			// If the system architecture is already little-endian, the value doesn't need to be changed.
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is big-endian, we must reverse the byte order to make it little-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToBigEndian(ulong value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToBigEndian(long value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToBigEndian(ushort value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToBigEndian(short value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToBigEndian(uint value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to big-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in big-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <returns>The value in big-endian format. If the system architecture is already big-endian, the original value is
		/// returned unchanged. Otherwise, the byte order is reversed.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToBigEndian(int value)
		{
			// If the system architecture is already big-endian, the value doesn't need to be changed.
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			// If the system is little-endian, we must reverse the byte order to make it big-endian.
			return BinaryPrimitives.ReverseEndianness(value);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least 8 bytes starting from the specified offset.");
			}

			return ToUInt64LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(byte[] buffer, int offset)
		{
			return (long)ToUInt64LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			byte b5 = offset + 4 < len ? buffer[offset + 4] : (byte)0;
			byte b6 = offset + 5 < len ? buffer[offset + 5] : (byte)0;
			byte b7 = offset + 6 < len ? buffer[offset + 6] : (byte)0;
			byte b8 = offset + 7 < len ? buffer[offset + 7] : (byte)0;
			return ToUInt64LittleEndian(b1, b2, b3, b4, b5, b6, b7, b8);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt64LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Combines eight bytes into a 64-bit unsigned integer, interpreting the bytes in little-endian order.
		/// </summary>
		/// <param name="v1">The least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v2">The second least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v3">The third least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v4">The fourth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v5">The fifth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v6">The sixth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v7">The seventh least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v8">The most significant byte of the resulting 64-bit integer.</param>
		/// <returns>A 64-bit unsigned integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return v1 |
				   ((ulong)v2 << 8) |
				   ((ulong)v3 << 16) |
				   ((ulong)v4 << 24) |
				   ((ulong)v5 << 32) |
				   ((ulong)v6 << 40) |
				   ((ulong)v7 << 48) |
				   ((ulong)v8 << 56);
		}

		/// <summary>
		/// Combines eight bytes into a 64-bit signed integer, interpreting the bytes in little-endian order.
		/// </summary>
		/// <param name="v1">The least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v2">The second least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v3">The third least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v4">The fourth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v5">The fifth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v6">The sixth least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v7">The seventh least significant byte of the resulting 64-bit integer.</param>
		/// <param name="v8">The most significant byte of the resulting 64-bit integer.</param>
		/// <returns>A 64-bit signed integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return (long)ToUInt64LittleEndian(v1, v2, v3, v4, v5, v6, v7, v8);
		}

		/// <summary>
		/// Converts a sequence of bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <remarks>This method assumes that the bytes in the buffer are stored in little-endian format, where the
		/// least significant byte comes first.</remarks>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least 8 bytes available starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the bytes.</param>
		/// <returns>A 64-bit unsigned integer constructed from the 8 bytes starting at <paramref name="offset"/> in little-endian
		/// order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt64LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		/// <summary>
		/// Converts a sequence of bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using little-endian byte order.
		/// </summary>
		/// <remarks>This method assumes that the bytes in the buffer are stored in little-endian format, where the
		/// least significant byte comes first.</remarks>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least 8 bytes available starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the bytes.</param>
		/// <returns>A 64-bit signed integer constructed from the 8 bytes starting at <paramref name="offset"/> in little-endian
		/// order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (long)ToUInt64LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			byte b5 = offset + 4 < len ? buffer[offset + 4] : (byte)0;
			byte b6 = offset + 5 < len ? buffer[offset + 5] : (byte)0;
			byte b7 = offset + 6 < len ? buffer[offset + 6] : (byte)0;
			byte b8 = offset + 7 < len ? buffer[offset + 7] : (byte)0;
			return ToUInt64LittleEndian(b1, b2, b3, b4, b5, b6, b7, b8);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the little-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (long)ToUInt64LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts two bytes to a 16-bit unsigned integer, assuming little-endian byte order.
		/// </summary>
		/// <param name="v1">The least significant byte of the 16-bit unsigned integer.</param>
		/// <param name="v2">The most significant byte of the 16-bit unsigned integer.</param>
		/// <returns>A 16-bit unsigned integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(byte v1, byte v2)
		{
			return (ushort)(v1 | (v2 << 8));
		}

		/// <summary>
		/// Converts two bytes to a 16-bit signed integer, assuming little-endian byte order.
		/// </summary>
		/// <param name="v1">The least significant byte of the 16-bit unsigned integer.</param>
		/// <param name="v2">The most significant byte of the 16-bit unsigned integer.</param>
		/// <returns>A 16-bit signed integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(byte v1, byte v2)
		{
			return (short)ToUInt16LittleEndian(v1, v2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt16LittleEndian(buffer[offset], buffer[offset + 1]);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(byte[] buffer, int offset)
		{
			return (short)ToUInt16LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt16LittleEndian(buffer[offset], buffer[offset + 1]);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (short)ToUInt16LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			return ToUInt16LittleEndian(b1, b2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (short)ToUInt16LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			return ToUInt16LittleEndian(b1, b2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndianSafe(byte[] buffer, int offset)
		{
			return (short)ToUInt16LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts four bytes to a 32-bit unsigned integer using little-endian byte order.
		/// </summary>
		/// <param name="v1">The least significant byte of the resulting integer.</param>
		/// <param name="v2">The second least significant byte of the resulting integer.</param>
		/// <param name="v3">The second most significant byte of the resulting integer.</param>
		/// <param name="v4">The most significant byte of the resulting integer.</param>
		/// <returns>A 32-bit unsigned integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return v1 |
				   ((uint)v2 << 8) |
				   ((uint)v3 << 16) |
				   ((uint)v4 << 24);
		}

		/// <summary>
		/// Converts four bytes to a 32-bit signed integer using little-endian byte order.
		/// </summary>
		/// <param name="v1">The least significant byte of the resulting integer.</param>
		/// <param name="v2">The second least significant byte of the resulting integer.</param>
		/// <param name="v3">The second most significant byte of the resulting integer.</param>
		/// <param name="v4">The most significant byte of the resulting integer.</param>
		/// <returns>A 32-bit signed integer constructed from the specified bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return (int)ToUInt32LittleEndian(v1, v2, v3, v4);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt32LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit signed
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt32LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			return ToUInt32LittleEndian(b1, b2, b3, b4);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndianSafe(byte[] buffer, int offset)
		{
			return (int)ToUInt32LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit unsigned integer, assuming
		/// little-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as little-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt32LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit signed integer, assuming
		/// little-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as little-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (int)ToUInt32LittleEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit unsigned
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			return ToUInt32LittleEndian(b1, b2, b3, b4);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit signed
		/// integer using little-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (int)ToUInt32LittleEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must contain at least 8 bytes starting from the specified offset.", nameof(buffer));
			}

			return ToUInt64BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(byte[] buffer, int offset)
		{
			return (long)ToUInt64BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using big-endian byte order.
		/// </summary>
		/// <remarks>This method assumes that the bytes in the buffer are stored in big-endian format, where the
		/// most significant byte comes first.</remarks>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least 8 bytes available starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the bytes.</param>
		/// <returns>A 64-bit unsigned integer constructed from the 8 bytes starting at <paramref name="offset"/> in big-endian
		/// order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt64BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		/// <summary>
		/// Converts a sequence of bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using big-endian byte order.
		/// </summary>
		/// <remarks>This method assumes that the bytes in the buffer are stored in big-endian format, where the
		/// most significant byte comes first.</remarks>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least 8 bytes available starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the bytes.</param>
		/// <returns>A 64-bit signed integer constructed from the 8 bytes starting at <paramref name="offset"/> in big-endian
		/// order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (long)ToUInt64BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			byte b5 = offset + 4 < len ? buffer[offset + 4] : (byte)0;
			byte b6 = offset + 5 < len ? buffer[offset + 5] : (byte)0;
			byte b7 = offset + 6 < len ? buffer[offset + 6] : (byte)0;
			byte b8 = offset + 7 < len ? buffer[offset + 7] : (byte)0;
			return ToUInt64BigEndian(b1, b2, b3, b4, b5, b6, b7, b8);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt64BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit unsigned
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit unsigned integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset + 0 < len ? buffer[offset + 0] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			byte b5 = offset + 4 < len ? buffer[offset + 4] : (byte)0;
			byte b6 = offset + 5 < len ? buffer[offset + 5] : (byte)0;
			byte b7 = offset + 6 < len ? buffer[offset + 6] : (byte)0;
			byte b8 = offset + 7 < len ? buffer[offset + 7] : (byte)0;
			return ToUInt64BigEndian(b1, b2, b3, b4, b5, b6, b7, b8);
		}

		/// <summary>
		/// Converts a sequence of 8 bytes from the specified buffer, starting at the given offset, into a 64-bit signed
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least 8 bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the 8 bytes.</param>
		/// <returns>A 64-bit signed integer representing the big-endian interpretation of the 8 bytes starting at <paramref
		/// name="offset"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (long)ToUInt64BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Combines eight bytes into a single 64-bit unsigned integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <remarks>This method shifts each byte to its appropriate position in the 64-bit integer, with the most
		/// significant byte placed at the highest position. It is optimized for performance and assumes the caller provides
		/// all eight bytes in the correct order.</remarks>
		/// <param name="v1">The most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v2">The second most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v3">The third most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v4">The fourth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v5">The fifth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v6">The sixth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v7">The seventh most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v8">The least significant byte of the resulting 64-bit unsigned integer.</param>
		/// <returns>A 64-bit unsigned integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return ((ulong)v1 << 56) |
				   ((ulong)v2 << 48) |
				   ((ulong)v3 << 40) |
				   ((ulong)v4 << 32) |
				   ((ulong)v5 << 24) |
				   ((ulong)v6 << 16) |
				   ((ulong)v7 << 8) |
				   v8;
		}

		/// <summary>
		/// Combines eight bytes into a single 64-bit signed integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <remarks>This method shifts each byte to its appropriate position in the 64-bit integer, with the most
		/// significant byte placed at the highest position. It is optimized for performance and assumes the caller provides
		/// all eight bytes in the correct order.</remarks>
		/// <param name="v1">The most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v2">The second most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v3">The third most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v4">The fourth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v5">The fifth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v6">The sixth most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v7">The seventh most significant byte of the resulting 64-bit unsigned integer.</param>
		/// <param name="v8">The least significant byte of the resulting 64-bit unsigned integer.</param>
		/// <returns>A 64-bit signed integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return (long)ToUInt64BigEndian(v1, v2, v3, v4, v5, v6, v7, v8);
		}

		/// <summary>
		/// Converts two bytes to a 16-bit unsigned integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <param name="v1">The most significant byte of the 16-bit unsigned integer.</param>
		/// <param name="v2">The least significant byte of the 16-bit unsigned integer.</param>
		/// <returns>A 16-bit unsigned integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16BigEndian(byte v1, byte v2)
		{
			return (ushort)((v1 << 8) | v2);
		}

		/// <summary>
		/// Converts two bytes to a 16-bit signed integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <param name="v1">The most significant byte of the 16-bit unsigned integer.</param>
		/// <param name="v2">The least significant byte of the 16-bit unsigned integer.</param>
		/// <returns>A 16-bit signed integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16BigEndian(byte v1, byte v2)
		{
			return (short)ToUInt16BigEndian(v1, v2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt16BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least two bytes starting from the specified offset.");
			}

			return ToUInt16BigEndian(buffer[offset], buffer[offset + 1]);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt16BigEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt16BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt16BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least two bytes starting from the specified offset.");
			}

			return ToUInt16BigEndian(buffer[offset], buffer[offset + 1]);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt16BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (int)ToUInt16BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt16BigEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset < len ? buffer[offset] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			return ToUInt16BigEndian(b1, b2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt16BigEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt16BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit unsigned
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit unsigned integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt16BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset < len ? buffer[offset] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			return ToUInt16BigEndian(b1, b2);
		}

		/// <summary>
		/// Converts a sequence of two bytes from the specified buffer, starting at the given offset, into a 16-bit signed
		/// integer using big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the data to convert. Must contain at least two bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the two bytes.</param>
		/// <returns>A 16-bit signed integer representing the value of the two bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt16BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (long)ToUInt16BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts four bytes to a 32-bit unsigned integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <remarks>This method assumes the input bytes are provided in big-endian order, where the most significant
		/// byte is first and the least significant byte is last.</remarks>
		/// <param name="v1">The most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v2">The second most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v3">The third most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v4">The least significant byte of the 32-bit unsigned integer.</param>
		/// <returns>A 32-bit unsigned integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return ((uint)v1 << 24) |
				   ((uint)v2 << 16) |
				   ((uint)v3 << 8) |
				   v4;
		}

		/// <summary>
		/// Converts four bytes to a 32-bit signed integer, interpreting the bytes in big-endian order.
		/// </summary>
		/// <remarks>This method assumes the input bytes are provided in big-endian order, where the most significant
		/// byte is first and the least significant byte is last.</remarks>
		/// <param name="v1">The most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v2">The second most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v3">The third most significant byte of the 32-bit unsigned integer.</param>
		/// <param name="v4">The least significant byte of the 32-bit unsigned integer.</param>
		/// <returns>A 32-bit signed integer constructed from the specified bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return (int)ToUInt32BigEndian(v1, v2, v3, v4);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit unsigned
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least four bytes starting from the specified offset.");
			}

			return ToUInt32BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset, into a 32-bit signed
		/// integer using big-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt32BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit unsigned integer, assuming
		/// big-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.Length - offset < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least four bytes starting from the specified offset.");
			}

			return ToUInt32BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit signed integer, assuming
		/// big-endian byte order.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(ReadOnlySpan<byte> buffer, int offset)
		{
			return (int)ToUInt32BigEndian(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit unsigned integer, assuming
		/// big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			if (buffer.IsEmpty)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset < len ? buffer[offset] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			return ToUInt32BigEndian(b1, b2, b3, b4);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit signed integer, assuming
		/// big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndianSafe(ReadOnlySpan<byte> buffer, int offset)
		{
			return (int)ToUInt32BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit unsigned integer, assuming
		/// big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndianSafe(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				return 0;
			}

			int len = buffer.Length;
			byte b1 = offset < len ? buffer[offset] : (byte)0;
			byte b2 = offset + 1 < len ? buffer[offset + 1] : (byte)0;
			byte b3 = offset + 2 < len ? buffer[offset + 2] : (byte)0;
			byte b4 = offset + 3 < len ? buffer[offset + 3] : (byte)0;
			return ToUInt32BigEndian(b1, b2, b3, b4);
		}

		/// <summary>
		/// Converts a sequence of bytes from a specified offset in a buffer to a 32-bit signed integer, assuming
		/// big-endian byte order. This never throws an exception, but will apply 0 to any byte
		/// that is out of range of the provided buffer.
		/// </summary>
		/// <param name="buffer">The byte array containing the bytes to convert. Must have at least four bytes available starting at the specified
		/// offset.</param>
		/// <param name="offset">The zero-based index in the buffer at which to begin reading the bytes.</param>
		/// <returns>A 32-bit signed integer representing the value of the four bytes starting at the specified offset, interpreted
		/// as big-endian.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndianSafe(byte[] buffer, int offset)
		{
			return (int)ToUInt32BigEndianSafe(buffer, offset);
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to an array of bytes in little-endian order.
		/// </summary>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <returns>An array of 8 bytes representing the <paramref name="value"/> in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(ulong value)
		{
			byte[] bytes = new byte[8];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to an array of bytes in little-endian order.
		/// </summary>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <returns>An array of 8 bytes representing the <paramref name="value"/> in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(long value)
		{
			byte[] bytes = new byte[8];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(uint value)
		{
			byte[] bytes = new byte[4];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(int value)
		{
			byte[] bytes = new byte[4];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(ushort value)
		{
			byte[] bytes = new byte[2];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(short value)
		{
			byte[] bytes = new byte[2];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 64-bit double-precision floating point number to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 64-bit double-precision floating point number to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(double value)
		{
			byte[] bytes = new byte[8];
			unsafe
			{
				ToLittleEndianBytes(*(ulong*)&value, bytes, 0);
			}
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit single-precision floating point number to a byte array in little-endian format.
		/// </summary>
		/// <param name="value">The 32-bit single-precision floating point number to convert.</param>
		/// <returns>A byte array containing the little-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(float value)
		{
			byte[] bytes = new byte[4];
			unsafe
			{
				ToLittleEndianBytes(*(uint*)&value, bytes, 0);
			}
			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit unsigned integers to their little-endian byte representation.
		/// </summary>
		/// <remarks>This method ensures that each <see cref="ulong"/> value is converted to its little-endian byte
		/// order, regardless of the system's endianness.</remarks>
		/// <param name="values">An array of <see cref="ulong"/> values to convert. Each value is represented as 8 bytes in the resulting array.</param>
		/// <returns>A byte array containing the little-endian representation of the input values. The length of the array is 
		/// <c>values.Length * sizeof(ulong)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params ulong[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(ulong)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(ulong));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of unsigned 32-bit integers to their little-endian byte representation.
		/// </summary>
		/// <remarks>Each integer in the input array is converted to its little-endian byte representation and stored
		/// sequentially in the resulting byte array. The method ensures that the byte order is consistent with little-endian
		/// format, regardless of the system's endianness.</remarks>
		/// <param name="values">An array of unsigned 32-bit integers to convert. Cannot be null.</param>
		/// <returns>A byte array containing the little-endian representation of the input integers. The length of the returned array
		/// is <c>values.Length * sizeof(uint)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params uint[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(uint)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(uint));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 16-bit unsigned integers to a byte array in little-endian order.
		/// </summary>
		/// <remarks>Each <see cref="ushort"/> value in the input array is converted to its little-endian byte
		/// representation and stored sequentially in the resulting byte array. The method ensures that the byte order is
		/// consistent with little-endian systems, regardless of the platform's native endianness.</remarks>
		/// <param name="values">An array of <see cref="ushort"/> values to convert to little-endian byte representation.</param>
		/// <returns>A byte array containing the little-endian representation of the input values. The length of the array is
		/// <c>values.Length * 2</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params ushort[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(ushort)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(ushort));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit signed integers to their little-endian byte representation.
		/// </summary>
		/// <remarks>This method ensures that each <see cref="long"/> value is converted to its little-endian byte
		/// order, regardless of the system's endianness.</remarks>
		/// <param name="values">An array of <see cref="long"/> values to convert. Each value is represented as 8 bytes in the resulting array.</param>
		/// <returns>A byte array containing the little-endian representation of the input values. The length of the array is 
		/// <c>values.Length * sizeof(long)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params long[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(long)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(long));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of signed 32-bit integers to their little-endian byte representation.
		/// </summary>
		/// <remarks>Each integer in the input array is converted to its little-endian byte representation and stored
		/// sequentially in the resulting byte array. The method ensures that the byte order is consistent with little-endian
		/// format, regardless of the system's endianness.</remarks>
		/// <param name="values">An array of signed 32-bit integers to convert. Cannot be null.</param>
		/// <returns>A byte array containing the little-endian representation of the input integers. The length of the returned array
		/// is <c>values.Length * sizeof(int)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params int[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(int)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(int));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 16-bit signed integers to a byte array in little-endian order.
		/// </summary>
		/// <remarks>Each <see cref="short"/> value in the input array is converted to its little-endian byte
		/// representation and stored sequentially in the resulting byte array. The method ensures that the byte order is
		/// consistent with little-endian systems, regardless of the platform's native endianness.</remarks>
		/// <param name="values">An array of <see cref="short"/> values to convert to little-endian byte representation.</param>
		/// <returns>A byte array containing the little-endian representation of the input values. The length of the array is
		/// <c>values.Length * 2</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params short[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(short)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToLittleEndianBytes(values[i], bytes, i * sizeof(short));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit double-precision floating point number to a little-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="double"/> value in the input array is converted to its little-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="double"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in little-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params double[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(double)];
			for (int i = 0; i < values.Length; ++i)
			{
				unsafe
				{
					double d = values[i];
					ToLittleEndianBytes(*(ulong*)&d, bytes, i * sizeof(double));
				}
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 32-bit single-precision floating point number to a little-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="float"/> value in the input array is converted to its little-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="float"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in little-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(params float[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(float)];
			for (int i = 0; i < values.Length; ++i)
			{
				unsafe
				{
					float d = values[i];
					ToLittleEndianBytes(*(uint*)&d, bytes, i * sizeof(float));
				}
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit unsigned integers to their big-endian byte representation.
		/// </summary>
		/// <remarks>Each <see cref="ulong"/> value in the input array is converted to an 8-byte big-endian sequence
		/// and written sequentially into the returned byte array. The caller is responsible for ensuring that the input array
		/// is not null.</remarks>
		/// <param name="values">An array of <see cref="ulong"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array containing the big-endian representation of the input values. The length of the returned array is
		/// equal to <c>values.Length * sizeof(ulong)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params ulong[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(ulong)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(ulong));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 32-bit unsigned integers to their big-endian byte representation.
		/// </summary>
		/// <remarks>Each 32-bit unsigned integer in the input array is converted to a sequence of 4 bytes in
		/// big-endian order (most significant byte first). The resulting bytes are concatenated into a single
		/// array.</remarks>
		/// <param name="values">An array of 32-bit unsigned integers to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the input values. The length of the returned array is
		/// <c>values.Length * sizeof(uint)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params uint[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(uint)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(uint));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 16-bit unsigned integers to a big-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="ushort"/> value in the input array is converted to its big-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="ushort"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in big-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params ushort[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(ushort)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(ushort));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit signed integers to their big-endian byte representation.
		/// </summary>
		/// <remarks>Each <see cref="long"/> value in the input array is converted to an 8-byte big-endian sequence
		/// and written sequentially into the returned byte array. The caller is responsible for ensuring that the input array
		/// is not null.</remarks>
		/// <param name="values">An array of <see cref="long"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array containing the big-endian representation of the input values. The length of the returned array is
		/// equal to <c>values.Length * sizeof(long)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params long[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(long)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(long));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 32-bit signed integers to their big-endian byte representation.
		/// </summary>
		/// <remarks>Each 32-bit signed integer in the input array is converted to a sequence of 4 bytes in
		/// big-endian order (most significant byte first). The resulting bytes are concatenated into a single
		/// array.</remarks>
		/// <param name="values">An array of 32-bit signed integers to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the input values. The length of the returned array is
		/// <c>values.Length * sizeof(int)</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params int[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(int)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(int));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 16-bit signed integers to a big-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="short"/> value in the input array is converted to its big-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="short"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in big-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params short[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(short)];
			for (int i = 0; i < values.Length; ++i)
			{
				ToBigEndianBytes(values[i], bytes, i * sizeof(short));
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 64-bit double-precision floating point number to a big-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="double"/> value in the input array is converted to its big-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="double"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in big-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params double[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(double)];
			for (int i = 0; i < values.Length; ++i)
			{
				unsafe
				{
					double d = values[i];
					ToBigEndianBytes(*(ulong*)&d, bytes, i * sizeof(double));
				}
			}

			return bytes;
		}

		/// <summary>
		/// Converts an array of 32-bit single-precision floating point number to a big-endian byte array.
		/// </summary>
		/// <remarks>Each <see cref="float"/> value in the input array is converted to its big-endian representation
		/// and appended to the resulting byte array. The caller is responsible for ensuring the input array is not
		/// null.</remarks>
		/// <param name="values">An array of <see cref="float"/> values to convert. Cannot be null.</param>
		/// <returns>A byte array representing the input values in big-endian format. The length of the array is twice the number of
		/// input values.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(params float[] values)
		{
			_ = values ?? throw new ArgumentNullException(nameof(values));

			byte[] bytes = new byte[values.Length * sizeof(float)];
			for (int i = 0; i < values.Length; ++i)
			{
				unsafe
				{
					float d = values[i];
					ToBigEndianBytes(*(ulong*)&d, bytes, i * sizeof(float));
				}
			}

			return bytes;
		}

		/// <summary>
		/// Converts the specified 64-bit unsigned integer to an array of bytes in big-endian order.
		/// </summary>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <returns>An array of 8 bytes representing the <paramref name="value"/> in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(ulong value)
		{
			byte[] bytes = new byte[8];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit unsigned integer to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 32-bit unsigned integer to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(uint value)
		{
			byte[] bytes = new byte[4];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 16-bit unsigned integer to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 16-bit unsigned integer to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(ushort value)
		{
			byte[] bytes = new byte[2];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to an array of bytes in big-endian order.
		/// </summary>
		/// <param name="value">The 64-bit signed integer to convert.</param>
		/// <returns>An array of 8 bytes representing the <paramref name="value"/> in big-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(long value)
		{
			byte[] bytes = new byte[8];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 32-bit signed integer to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(int value)
		{
			byte[] bytes = new byte[4];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 16-bit signed integer to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(short value)
		{
			byte[] bytes = new byte[2];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		/// <summary>
		/// Converts the specified 64-bit double-precision floating point number to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 64-bit double-precision floating point number to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(double value)
		{
			byte[] bytes = new byte[8];
			unsafe
			{
				ToBigEndianBytes(*(ulong*)&value, bytes, 0);
			}
			return bytes;
		}

		/// <summary>
		/// Converts the specified 32-bit single-precision floating point number to a byte array in big-endian format.
		/// </summary>
		/// <param name="value">The 32-bit single-precision floating point number to convert.</param>
		/// <returns>A byte array containing the big-endian representation of the specified value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(float value)
		{
			byte[] bytes = new byte[4];
			unsafe
			{
				ToBigEndianBytes(*(uint*)&value, bytes, 0);
			}
			return bytes;
		}
	}
}
