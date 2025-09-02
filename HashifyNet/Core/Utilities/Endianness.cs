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
			buffer[offset] = (byte)value;
			buffer[offset + 1] = (byte)(value >> 8);
			buffer[offset + 2] = (byte)(value >> 16);
			buffer[offset + 3] = (byte)(value >> 24);
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
		/// Converts the specified 64-bit unsigned integer to little-endian format.
		/// </summary>
		/// <remarks>This method ensures that the returned value is in little-endian format, regardless of the 
		/// system's endianness. It is optimized for performance and uses aggressive inlining.</remarks>
		/// <param name="value">The 64-bit unsigned integer to convert.</param>
		/// <returns>The value in little-endian format. If the system architecture is already little-endian,  the original value is
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
			return buffer[offset] |
				   ((ulong)buffer[offset + 1] << 8) |
				   ((ulong)buffer[offset + 2] << 16) |
				   ((ulong)buffer[offset + 3] << 24) |
				   ((ulong)buffer[offset + 4] << 32) |
				   ((ulong)buffer[offset + 5] << 40) |
				   ((ulong)buffer[offset + 6] << 48) |
				   ((ulong)buffer[offset + 7] << 56);
		}

		/// <summary>
		/// Converts a sequence of bytes from the specified buffer, starting at the given offset,  into a 64-bit unsigned
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
			return buffer[offset] |
				   ((ulong)buffer[offset + 1] << 8) |
				   ((ulong)buffer[offset + 2] << 16) |
				   ((ulong)buffer[offset + 3] << 24) |
				   ((ulong)buffer[offset + 4] << 32) |
				   ((ulong)buffer[offset + 5] << 40) |
				   ((ulong)buffer[offset + 6] << 48) |
				   ((ulong)buffer[offset + 7] << 56);
		}

		/// <summary>
		/// Converts a sequence of four bytes from the specified buffer, starting at the given offset,  into a 32-bit unsigned
		/// integer using little-endian byte order.
		/// </summary>
		/// <param name="buffer">The byte array containing the data to convert. Must contain at least four bytes starting from <paramref
		/// name="offset"/>.</param>
		/// <param name="offset">The zero-based index in <paramref name="buffer"/> at which to begin reading the four bytes.</param>
		/// <returns>A 32-bit unsigned integer representing the value of the four bytes in little-endian order.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(byte[] buffer, int offset)
		{
			return buffer[offset] |
				   ((uint)buffer[offset + 1] << 8) |
				   ((uint)buffer[offset + 2] << 16) |
				   ((uint)buffer[offset + 3] << 24);
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
			return buffer[offset] |
				   ((uint)buffer[offset + 1] << 8) |
				   ((uint)buffer[offset + 2] << 16) |
				   ((uint)buffer[offset + 3] << 24);
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
	}
}
