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
	internal static class Endianness
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte SafeRead(this Span<byte> buf, int index)
		{
			if (buf.IsEmpty) return 0;
			if (index < 0) return 0;
			if (index >= buf.Length) return 0;
			return buf[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte SafeRead(this ReadOnlySpan<byte> buf, int index)
		{
			if (buf.IsEmpty) return 0;
			if (index < 0) return 0;
			if (index >= buf.Length) return 0;
			return buf[index];
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(int value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((uint)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(uint value, Span<byte> buffer)
		{
			if (buffer.Length < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long.", nameof(buffer));
			}

			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			buffer[2] = (byte)(value >> 16);
			buffer[3] = (byte)(value >> 24);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(int value, Span<byte> buffer)
		{
			ToLittleEndianBytes((uint)value, buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(short value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((ushort)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ushort value, Span<byte> buffer)
		{
			if (buffer.Length < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(short value, Span<byte> buffer)
		{
			ToLittleEndianBytes((ushort)value, buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(long value, byte[] buffer, int offset)
		{
			ToLittleEndianBytes((ulong)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(ulong value, Span<byte> buffer)
		{
			if (buffer.Length < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long.", nameof(buffer));
			}

			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			buffer[2] = (byte)(value >> 16);
			buffer[3] = (byte)(value >> 24);
			buffer[4] = (byte)(value >> 32);
			buffer[5] = (byte)(value >> 40);
			buffer[6] = (byte)(value >> 48);
			buffer[7] = (byte)(value >> 56);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToLittleEndianBytes(long value, Span<byte> buffer)
		{
			ToLittleEndianBytes((ulong)value, buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(short value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((ushort)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ushort value, Span<byte> buffer)
		{
			if (buffer.Length < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long.", nameof(buffer));
			}

			buffer[0] = (byte)(value >> 8);
			buffer[1] = (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(short value, Span<byte> buffer)
		{
			ToBigEndianBytes((ushort)value, buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(int value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((uint)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(uint value, Span<byte> buffer)
		{
			if (buffer.Length < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long.", nameof(buffer));
			}

			buffer[0] = (byte)(value >> 24);
			buffer[1] = (byte)(value >> 16);
			buffer[2] = (byte)(value >> 8);
			buffer[3] = (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(int value, Span<byte> buffer)
		{
			ToBigEndianBytes((uint)value, buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(long value, byte[] buffer, int offset)
		{
			ToBigEndianBytes((ulong)value, buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(ulong value, Span<byte> buffer)
		{
			if (buffer.Length < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long.", nameof(buffer));
			}

			buffer[0] = (byte)(value >> 56);
			buffer[1] = (byte)(value >> 48);
			buffer[2] = (byte)(value >> 40);
			buffer[3] = (byte)(value >> 32);
			buffer[4] = (byte)(value >> 24);
			buffer[5] = (byte)(value >> 16);
			buffer[6] = (byte)(value >> 8);
			buffer[7] = (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToBigEndianBytes(long value, Span<byte> buffer)
		{
			ToBigEndianBytes((ulong)value, buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToLittleEndian(ulong value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToLittleEndian(long value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToLittleEndian(uint value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToLittleEndian(int value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToLittleEndian(ushort value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToLittleEndian(short value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToBigEndian(ulong value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToBigEndian(long value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToBigEndian(ushort value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToBigEndian(short value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToBigEndian(uint value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToBigEndian(int value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return value;
			}

			return BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least 8 bytes starting from the specified offset.");
			}

			return ToUInt64LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(byte[] buffer, int offset)
		{
			return (long)ToUInt64LittleEndian(buffer, offset);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt64LittleEndianSafe(buffer, offset);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return (long)ToUInt64LittleEndian(v1, v2, v3, v4, v5, v6, v7, v8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long.", nameof(buffer));
			}

			return ToUInt64LittleEndian(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4], buffer[5], buffer[6], buffer[7]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndian(ReadOnlySpan<byte> buffer)
		{
			return (long)ToUInt64LittleEndian(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt64LittleEndian(buffer.SafeRead(0), buffer.SafeRead(1), buffer.SafeRead(2), buffer.SafeRead(3), buffer.SafeRead(4), buffer.SafeRead(5), buffer.SafeRead(6), buffer.SafeRead(7));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (long)ToUInt64LittleEndianSafe(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(byte v1, byte v2)
		{
			return (ushort)(v1 | (v2 << 8));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(byte v1, byte v2)
		{
			return (short)ToUInt16LittleEndian(v1, v2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt16LittleEndian(buffer[offset], buffer[offset + 1]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(byte[] buffer, int offset)
		{
			return (short)ToUInt16LittleEndian(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 2)
			{
				throw new ArgumentException("Buffer must be at least 2 bytes long.", nameof(buffer));
			}

			return ToUInt16LittleEndian(buffer[0], buffer[1]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndian(ReadOnlySpan<byte> buffer)
		{
			return (short)ToUInt16LittleEndian(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt16LittleEndian(buffer.SafeRead(0), buffer.SafeRead(1));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (short)ToUInt16LittleEndianSafe(buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16LittleEndianSafe(byte[] buffer, int offset)
		{
			return (short)ToUInt16LittleEndianSafe(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return v1 |
				   ((uint)v2 << 8) |
				   ((uint)v3 << 16) |
				   ((uint)v4 << 24);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return (int)ToUInt32LittleEndian(v1, v2, v3, v4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long starting from the specified offset.", nameof(buffer));
			}

			return ToUInt32LittleEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt32LittleEndian(buffer, offset);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndianSafe(byte[] buffer, int offset)
		{
			return (int)ToUInt32LittleEndianSafe(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 4)
			{
				throw new ArgumentException("Buffer must be at least 4 bytes long.", nameof(buffer));
			}

			return ToUInt32LittleEndian(buffer[0], buffer[1], buffer[2], buffer[3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndian(ReadOnlySpan<byte> buffer)
		{
			return (int)ToUInt32LittleEndian(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt32LittleEndian(buffer.SafeRead(0), buffer.SafeRead(1), buffer.SafeRead(2), buffer.SafeRead(3));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32LittleEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (int)ToUInt32LittleEndianSafe(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 8)
			{
				throw new ArgumentException("Buffer must contain at least 8 bytes starting from the specified offset.", nameof(buffer));
			}

			return ToUInt64BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5], buffer[offset + 6], buffer[offset + 7]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(byte[] buffer, int offset)
		{
			return (long)ToUInt64BigEndian(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 8)
			{
				throw new ArgumentException("Buffer must be at least 8 bytes long.", nameof(buffer));
			}

			return ToUInt64BigEndian(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4], buffer[5], buffer[6], buffer[7]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(ReadOnlySpan<byte> buffer)
		{
			return (long)ToUInt64BigEndian(buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt64BigEndianSafe(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt64BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt64BigEndian(buffer.SafeRead(0), buffer.SafeRead(1), buffer.SafeRead(2), buffer.SafeRead(3), buffer.SafeRead(4), buffer.SafeRead(5), buffer.SafeRead(6), buffer.SafeRead(7));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (long)ToUInt64BigEndianSafe(buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt64BigEndian(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			return (long)ToUInt64BigEndian(v1, v2, v3, v4, v5, v6, v7, v8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ToUInt16BigEndian(byte v1, byte v2)
		{
			return (ushort)((v1 << 8) | v2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ToInt16BigEndian(byte v1, byte v2)
		{
			return (short)ToUInt16BigEndian(v1, v2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt16BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least two bytes starting from the specified offset.");
			}

			return ToUInt16BigEndian(buffer[offset], buffer[offset + 1]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt16BigEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt16BigEndian(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt16BigEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least two bytes.");
			}

			return ToUInt16BigEndian(buffer[0], buffer[1]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt16BigEndian(ReadOnlySpan<byte> buffer)
		{
			return (int)ToUInt16BigEndian(buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt16BigEndianSafe(byte[] buffer, int offset)
		{
			return (long)ToUInt16BigEndianSafe(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ToUInt16BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt16BigEndian(buffer.SafeRead(0), buffer.SafeRead(1));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToInt16BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (long)ToUInt16BigEndianSafe(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return ((uint)v1 << 24) |
				   ((uint)v2 << 16) |
				   ((uint)v3 << 8) |
				   v4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(byte v1, byte v2, byte v3, byte v4)
		{
			return (int)ToUInt32BigEndian(v1, v2, v3, v4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length - offset < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least four bytes starting from the specified offset.");
			}

			return ToUInt32BigEndian(buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(byte[] buffer, int offset)
		{
			return (int)ToUInt32BigEndian(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndian(ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer must contain at least four bytes.");
			}

			return ToUInt32BigEndian(buffer[0], buffer[1], buffer[2], buffer[3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndian(ReadOnlySpan<byte> buffer)
		{
			return (int)ToUInt32BigEndian(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt32BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return ToUInt32BigEndian(buffer.SafeRead(0), buffer.SafeRead(1), buffer.SafeRead(2), buffer.SafeRead(3));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndianSafe(ReadOnlySpan<byte> buffer)
		{
			return (int)ToUInt32BigEndianSafe(buffer);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt32BigEndianSafe(byte[] buffer, int offset)
		{
			return (int)ToUInt32BigEndianSafe(buffer, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(ulong value)
		{
			byte[] bytes = new byte[8];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(long value)
		{
			byte[] bytes = new byte[8];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(uint value)
		{
			byte[] bytes = new byte[4];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(int value)
		{
			byte[] bytes = new byte[4];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(ushort value)
		{
			byte[] bytes = new byte[2];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesLittleEndian(short value)
		{
			byte[] bytes = new byte[2];
			ToLittleEndianBytes(value, bytes, 0);
			return bytes;
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(ulong value)
		{
			byte[] bytes = new byte[8];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(uint value)
		{
			byte[] bytes = new byte[4];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(ushort value)
		{
			byte[] bytes = new byte[2];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(long value)
		{
			byte[] bytes = new byte[8];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(int value)
		{
			byte[] bytes = new byte[4];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetBytesBigEndian(short value)
		{
			byte[] bytes = new byte[2];
			ToBigEndianBytes(value, bytes, 0);
			return bytes;
		}

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
