// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
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

using HashifyNet.Core;
using HashifyNet.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HashifyNet.Algorithms.CityHash
{
	/// <summary>
	/// Provides an implementation of the CityHash algorithm for computing hash values based on the specified
	/// configuration. Supports hash sizes of 32, 64, and 128 bits.
	/// </summary>
	/// <remarks>CityHash is a family of non-cryptographic hash functions designed for high performance on large
	/// datasets. This implementation supports configurable hash sizes and ensures that the configuration is validated
	/// during initialization. <para> The hash computation is optimized for different input sizes, with specialized
	/// handling for small, medium, and large data segments. The algorithm is designed to be efficient and suitable for
	/// scenarios where fast hashing is required. </para> <para> This class is internal and intended for use within the
	/// library. It is not recommended for cryptographic purposes or scenarios requiring strong collision resistance.
	/// </para></remarks>
	[HashAlgorithmImplementation(typeof(ICityHash), typeof(CityHashConfig))]
	internal class CityHash_Implementation
		: HashFunctionBase<ICityHashConfig>,
			ICityHash
	{
		public override ICityHashConfig Config => _config.Clone();

		private const ulong K0 = 0xc3a5c85c97cb3127;
		private const ulong K1 = 0xb492b66fbe98f273;
		private const ulong K2 = 0x9ae16a3b2f90404f;

		private const uint C1 = 0xcc9e2d51;
		private const uint C2 = 0x1b873593;

		private readonly ICityHashConfig _config;
		private static readonly IEnumerable<int> _validHashSizes = new HashSet<int>() { 32, 64, 128 };

		public CityHash_Implementation(ICityHashConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (!_validHashSizes.Contains(_config.HashSizeInBits))
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within CityHash.ValidHashSizes.");
			}
		}
		protected override IHashValue ComputeHashInternal(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			switch (_config.HashSizeInBits)
			{
				case 32:
					return ComputeHash32(data, cancellationToken);

				case 64:
					return ComputeHash64(data, cancellationToken);

				case 128:
					return ComputeHash128(data, cancellationToken);

				default:
					throw new NotImplementedException();
			}
		}

		#region ComputeHash32

		private IHashValue ComputeHash32(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var dataCount = data.Count;

			uint hashValue;

			if (dataCount > 24)
			{
				hashValue = Hash32Len25Plus(data, cancellationToken);

			}
			else if (dataCount > 12)
			{
				hashValue = Hash32Len13to24(data);

			}
			else if (dataCount > 4)
			{
				hashValue = Hash32Len5to12(data);

			}
			else
			{
				hashValue = Hash32Len0to4(data);
			}

			return new HashValue(
				Endianness.GetBytesLittleEndian(hashValue),
				32);
		}


		private uint Hash32Len0to4(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			uint b = 0;
			uint c = 9;

			for (var currentOffset = dataOffset; currentOffset < endOffset; currentOffset += 1)
			{
				b = (b * C1) + dataArray[currentOffset];
				c ^= b;
			}

			return Mix(Mur(b, Mur((uint)dataCount, c)));
		}

		private uint Hash32Len5to12(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			uint a = (uint)dataCount;
			uint b = (uint)dataCount * 5;

			uint c = 9;
			uint d = b;

			a += Endianness.ToUInt32LittleEndian(dataArray, dataOffset);
			b += Endianness.ToUInt32LittleEndian(dataArray, dataOffset + dataCount - 4);
			c += Endianness.ToUInt32LittleEndian(dataArray, dataOffset + ((dataCount >> 1) & 4));

			return Mix(Mur(c, Mur(b, Mur(a, d))));
		}

		private uint Hash32Len13to24(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;


			uint a = Endianness.ToUInt32LittleEndian(dataArray, dataOffset + (dataCount >> 1) - 4);
			uint b = Endianness.ToUInt32LittleEndian(dataArray, dataOffset + 4);
			uint c = Endianness.ToUInt32LittleEndian(dataArray, dataOffset + dataCount - 8);
			uint d = Endianness.ToUInt32LittleEndian(dataArray, dataOffset + (dataCount >> 1));
			uint e = Endianness.ToUInt32LittleEndian(dataArray, dataOffset);
			uint f = Endianness.ToUInt32LittleEndian(dataArray, dataOffset + dataCount - 4);
			uint h = (uint)dataCount;

			return Mix(Mur(f, Mur(e, Mur(d, Mur(c, Mur(b, Mur(a, h)))))));
		}

		private uint Hash32Len25Plus(ArraySegment<byte> data, CancellationToken cancellationToken)
		{

			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			cancellationToken.ThrowIfCancellationRequested();

			// dataCount > 24
			uint h = (uint)dataCount;
			uint g = (uint)dataCount * C1;
			uint f = g;
			{
				uint a0 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, endOffset - 4) * C1, 17) * C2;
				uint a1 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, endOffset - 8) * C1, 17) * C2;
				uint a2 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, endOffset - 16) * C1, 17) * C2;
				uint a3 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, endOffset - 12) * C1, 17) * C2;
				uint a4 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, endOffset - 20) * C1, 17) * C2;

				h ^= a0;
				h = RotateRight(h, 19);
				h = (h * 5) + 0xe6546b64;
				h ^= a2;
				h = RotateRight(h, 19);
				h = (h * 5) + 0xe6546b64;

				g ^= a1;
				g = RotateRight(g, 19);
				g = (g * 5) + 0xe6546b64;
				g ^= a3;
				g = RotateRight(g, 19);
				g = (g * 5) + 0xe6546b64;

				f += a4;
				f = RotateRight(f, 19);
				f = (f * 5) + 0xe6546b64;
			}


			var groupsToProcess = (dataCount - 1) / 20;
			var groupEndOffset = dataOffset + (groupsToProcess * 20);

			for (int groupOffset = dataOffset; groupOffset < groupEndOffset; groupOffset += 20)
			{
				cancellationToken.ThrowIfCancellationRequested();

				uint a0 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, groupOffset + 0) * C1, 17) * C2;
				uint a1 = Endianness.ToUInt32LittleEndian(dataArray, groupOffset + 4);
				uint a2 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, groupOffset + 8) * C1, 17) * C2;
				uint a3 = RotateRight(Endianness.ToUInt32LittleEndian(dataArray, groupOffset + 12) * C1, 17) * C2;
				uint a4 = Endianness.ToUInt32LittleEndian(dataArray, groupOffset + 16);

				h ^= a0;
				h = RotateRight(h, 18);
				h = (h * 5) + 0xe6546b64;

				f += a1;
				f = RotateRight(f, 19);
				f = f * C1;

				g += a2;
				g = RotateRight(g, 18);
				g = (g * 5) + 0xe6546b64;

				h ^= a3 + a1;
				h = RotateRight(h, 19);
				h = (h * 5) + 0xe6546b64;

				g ^= a4;
				g = ReverseByteOrder(g) * 5;

				h += a4 * 5;
				h = ReverseByteOrder(h);

				f += a0;

				Permute3(ref f, ref h, ref g);
			}

			cancellationToken.ThrowIfCancellationRequested();

			g = RotateRight(g, 11) * C1;
			g = RotateRight(g, 17) * C1;

			f = RotateRight(f, 11) * C1;
			f = RotateRight(f, 17) * C1;

			h = RotateRight(h + g, 19);
			h = (h * 5) + 0xe6546b64;
			h = RotateRight(h, 17) * C1;
			h = RotateRight(h + f, 19);
			h = (h * 5) + 0xe6546b64;
			h = RotateRight(h, 17) * C1;

			return h;
		}

		#endregion

		#region ComputeHash64
		private IHashValue ComputeHash64(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var dataCount = data.Count;
			ulong hashValue;

			if (dataCount > 64)
			{
				hashValue = Hash64Len65Plus(data, cancellationToken);

			}
			else if (dataCount > 32)
			{
				hashValue = Hash64Len33to64(data);

			}
			else if (dataCount > 16)
			{
				hashValue = Hash64Len17to32(data);

			}
			else
			{
				hashValue = Hash64Len0to16(data);
			}

			return new HashValue(
				Endianness.GetBytesLittleEndian(hashValue),
				64);
		}
		private ulong Hash64Len16(ulong u, ulong v)
		{
			return Hash128to64(
				new UInt128(v, u));
		}

		private static ulong Hash64Len16(ulong u, ulong v, ulong mul)
		{
			ulong a = (u ^ v) * mul;
			a ^= a >> 47;

			ulong b = (v ^ a) * mul;
			b ^= b >> 47;
			b *= mul;

			return b;
		}

		private ulong Hash64Len0to16(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			if (dataCount >= 8)
			{
				ulong mul = K2 + ((ulong)dataCount * 2);
				ulong a = Endianness.ToUInt64LittleEndian(dataArray, dataOffset) + K2;
				ulong b = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 8);
				ulong c = (RotateRight(b, 37) * mul) + a;
				ulong d = (RotateRight(a, 25) + b) * mul;

				return Hash64Len16(c, d, mul);
			}

			if (dataCount >= 4)
			{
				ulong mul = K2 + ((ulong)dataCount * 2);
				ulong a = Endianness.ToUInt32LittleEndian(dataArray, dataOffset);
				return Hash64Len16((ulong)dataCount + (a << 3), Endianness.ToUInt32LittleEndian(dataArray, endOffset - 4), mul);
			}

			if (dataCount > 0)
			{
				byte a = dataArray[dataOffset];
				byte b = dataArray[dataOffset + (dataCount >> 1)];
				byte c = dataArray[endOffset - 1];

				uint y = a + ((uint)b << 8);
				uint z = (uint)dataCount + ((uint)c << 2);

				return Mix((y * K2) ^ (z * K0)) * K2;
			}

			return K2;
		}

		// This probably works well for 16-byte strings as well, but it may be overkill
		// in that case.
		private static ulong Hash64Len17to32(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			ulong mul = K2 + ((ulong)dataCount * 2);
			ulong a = Endianness.ToUInt64LittleEndian(dataArray, dataOffset) * K1;
			ulong b = Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 8);
			ulong c = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 8) * mul;
			ulong d = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 16) * K2;

			return Hash64Len16(
				RotateRight(a + b, 43) +
					RotateRight(c, 30) + d,
				a + RotateRight(b + K2, 18) + c,
				mul);
		}

		// Return a 16-byte hash for 48 bytes.  Quick and dirty.
		// Callers do best to use "random-looking" values for a and b.
		private UInt128 WeakHashLen32WithSeeds(
			ulong w, ulong x, ulong y, ulong z, ulong a, ulong b)
		{
			a += w;
			b = RotateRight(b + a + z, 21);

			ulong c = a;
			a += x;
			a += y;

			b += RotateRight(a, 44);

			return new UInt128(b + c, a + z);
		}

		// Return a 16-byte hash for s[0] ... s[31], a, and b.  Quick and dirty.
		private UInt128 WeakHashLen32WithSeeds(byte[] data, int startIndex, ulong a, ulong b)
		{
			return WeakHashLen32WithSeeds(
				Endianness.ToUInt64LittleEndian(data, startIndex),
				Endianness.ToUInt64LittleEndian(data, startIndex + 8),
				Endianness.ToUInt64LittleEndian(data, startIndex + 16),
				Endianness.ToUInt64LittleEndian(data, startIndex + 24),
				a,
				b);
		}

		// Return an 8-byte hash for 33 to 64 bytes.
		private ulong Hash64Len33to64(ArraySegment<byte> data)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			ulong mul = K2 + ((ulong)dataCount * 2);
			ulong a = Endianness.ToUInt64LittleEndian(dataArray, dataOffset) * K2;
			ulong b = Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 8);
			ulong c = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 24);
			ulong d = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 32);
			ulong e = Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 16) * K2;
			ulong f = Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 24) * 9;
			ulong g = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 8);
			ulong h = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 16) * mul;

			ulong u = RotateRight(a + g, 43) + ((RotateRight(b, 30) + c) * 9);
			ulong v = ((a + g) ^ d) + f + 1;
			ulong w = ReverseByteOrder((u + v) * mul) + h;
			ulong x = RotateRight(e + f, 42) + c;
			ulong y = (ReverseByteOrder((v + w) * mul) + g) * mul;
			ulong z = e + f + c;

			a = ReverseByteOrder(((x + z) * mul) + y) + b;
			b = Mix(((z + a) * mul) + d + h) * mul;
			return b + x;
		}

		private ulong Hash64Len65Plus(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			// For strings over 64 bytes we hash the end first, and then as we
			// loop we keep 56 bytes of state: v, w, x, y, and z.
			ulong x = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 40);
			ulong y = Endianness.ToUInt64LittleEndian(dataArray, endOffset - 16) + Endianness.ToUInt64LittleEndian(dataArray, endOffset - 56);
			ulong z = Hash64Len16(
				Endianness.ToUInt64LittleEndian(dataArray, endOffset - 48) + (ulong)dataCount,
				Endianness.ToUInt64LittleEndian(dataArray, endOffset - 24));

			UInt128 v = WeakHashLen32WithSeeds(dataArray, endOffset - 64, (ulong)dataCount, z);
			UInt128 w = WeakHashLen32WithSeeds(dataArray, endOffset - 32, y + K1, x);

			x = (x * K1) + Endianness.ToUInt64LittleEndian(dataArray, 0);

			// For each 64-byte chunk
			var groupEndOffset = dataOffset + ((dataCount - 1) - ((dataCount - 1) % 64));

			for (var currentOffset = dataOffset; currentOffset < groupEndOffset; currentOffset += 64)
			{
				cancellationToken.ThrowIfCancellationRequested();

				x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, currentOffset + 8), 37) * K1;
				y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(dataArray, currentOffset + 48), 42) * K1;
				x ^= w.GetUpper();
				y += v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, currentOffset + 40);
				z = RotateRight(z + w.GetLower(), 33) * K1;
				v = WeakHashLen32WithSeeds(dataArray, currentOffset, v.GetUpper() * K1, x + w.GetLower());
				w = WeakHashLen32WithSeeds(dataArray, currentOffset + 32, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(dataArray, currentOffset + 16));

				ulong temp = x;
				x = z;
				z = temp;
			}

			return Hash64Len16(Hash64Len16(v.GetLower(), w.GetLower()) + (Mix(y) * K1) + z,
							Hash64Len16(v.GetUpper(), w.GetUpper()) + x);
		}

		#endregion

		#region ComputeHash128

		private IHashValue ComputeHash128(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			var dataCount = data.Count;

			UInt128 hashValue;

			if (dataCount >= 16)
			{
				var dataArray = data.Array;
				var dataOffset = data.Offset;

				hashValue = CityHash128WithSeed(
					new ArraySegment<byte>(dataArray, dataOffset + 16, dataCount - 16),
					new UInt128(
						Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 8) + K0,
						Endianness.ToUInt64LittleEndian(dataArray, dataOffset)),
					cancellationToken);

			}
			else
			{
				hashValue = CityHash128WithSeed(data, new UInt128(K1, K0), cancellationToken);
			}

			var hashValueBytes = Endianness.GetBytesLittleEndian(hashValue.GetLower())
				.Concat(Endianness.GetBytesLittleEndian(hashValue.GetUpper()));

			return new HashValue(hashValueBytes, 128);
		}

		private UInt128 CityHash128WithSeed(ArraySegment<byte> data, UInt128 seed, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var dataCount = data.Count;

			if (dataCount < 128)
			{
				return CityMurmur(data, seed);
			}

			var dataArray = data.Array;
			var dataOffset = data.Offset;

			var endOffset = dataOffset + dataCount;

			// We expect len >= 128 to be the common case.  Keep 56 bytes of state:
			// v, w, x, y, and z.
			UInt128 v;
			{
				var vLow = (RotateRight(seed.GetUpper() ^ K1, 49) * K1) + Endianness.ToUInt64LittleEndian(dataArray, dataOffset);
				v = new UInt128(
					(RotateRight(vLow, 42) * K1) + Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 8),
					vLow);
			}

			UInt128 w = new UInt128(
				RotateRight(seed.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 88), 53) * K1,
				(RotateRight(seed.GetUpper() + ((ulong)dataCount * K1), 35) * K1) + seed.GetLower());

			ulong x = seed.GetLower();
			ulong y = seed.GetUpper();
			ulong z = (ulong)dataCount * K1;

			// This is the same inner loop as CityHash64()
			int lastGroupEndOffset;
			{
				var groupEndOffset = dataOffset + (dataCount - (dataCount % 128));

				for (var groupCurrentOffset = dataOffset; groupCurrentOffset < groupEndOffset; groupCurrentOffset += 128)
				{
					cancellationToken.ThrowIfCancellationRequested();

					x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 8), 37) * K1;
					y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 48), 42) * K1;
					x ^= w.GetUpper();
					y += v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 40);
					z = RotateRight(z + w.GetLower(), 33) * K1;
					v = WeakHashLen32WithSeeds(dataArray, groupCurrentOffset, v.GetUpper() * K1, x + w.GetLower());
					w = WeakHashLen32WithSeeds(dataArray, groupCurrentOffset + 32, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 16));

					{
						ulong temp = z;
						z = x;
						x = temp;
					}

					x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 72), 37) * K1;
					y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 112), 42) * K1;
					x ^= w.GetUpper();
					y += v.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 104);
					z = RotateRight(z + w.GetLower(), 33) * K1;
					v = WeakHashLen32WithSeeds(dataArray, groupCurrentOffset + 64, v.GetUpper() * K1, x + w.GetLower());
					w = WeakHashLen32WithSeeds(dataArray, groupCurrentOffset + 96, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 80));

					{
						ulong temp = z;
						z = x;
						x = temp;
					}
				}

				lastGroupEndOffset = groupEndOffset;
			}

			cancellationToken.ThrowIfCancellationRequested();

			x += RotateRight(v.GetLower() + z, 49) * K0;
			y = (y * K0) + RotateRight(w.GetUpper(), 37);
			z = (z * K0) + RotateRight(w.GetLower(), 27);
			w = new UInt128(w.GetUpper(), w.GetLower() * 9);
			v = new UInt128(v.GetUpper(), v.GetLower() * K0);

			// Hash up to 4 chunks of 32 bytes each from the end of data.
			{
				var groupEndOffset = lastGroupEndOffset - 32;

				for (var groupCurrentOffset = endOffset - 32; groupCurrentOffset > groupEndOffset; groupCurrentOffset -= 32)
				{
					cancellationToken.ThrowIfCancellationRequested();

					y = (RotateRight(x + y, 42) * K0) + v.GetUpper();
					w = new UInt128(w.GetUpper(), w.GetLower() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 16));
					x = (x * K0) + w.GetLower();
					z += w.GetUpper() + Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset);
					w = new UInt128(w.GetUpper() + v.GetLower(), w.GetLower());
					v = WeakHashLen32WithSeeds(dataArray, groupCurrentOffset, v.GetLower() + z, v.GetUpper());
					v = new UInt128(v.GetUpper(), v.GetLower() * K0);
				}
			}

			// At this point our 56 bytes of state should contain more than
			// enough information for a strong 128-bit hash.  We use two
			// different 56-byte-to-8-byte hashes to get a 16-byte final result.
			x = Hash64Len16(x, v.GetLower());
			y = Hash64Len16(y + z, w.GetLower());

			return new UInt128(
								Hash64Len16(x + w.GetUpper(), y + v.GetUpper()),
								Hash64Len16(x + v.GetUpper(), w.GetUpper()) + y
							  );
		}

		// A subroutine for CityHash128().  Returns a decent 128-bit hash for strings
		// of any length representable in signed long.  Based on City and Murmur.
		private UInt128 CityMurmur(ArraySegment<byte> data, UInt128 seed)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;

			ulong a = seed.GetLower();
			ulong b = seed.GetUpper();
			ulong c;
			ulong d;

			if (dataCount <= 16)
			{
				// len <= 16
				a = Mix(a * K1) * K1;
				c = (b * K1) + Hash64Len0to16(data);
				d = Mix(a + (dataCount >= 8 ? Endianness.ToUInt64LittleEndian(dataArray, dataOffset) : c));

			}
			else
			{
				// len > 16
				c = Hash64Len16(Endianness.ToUInt64LittleEndian(dataArray, endOffset - 8) + K1, a);
				d = Hash64Len16(b + (ulong)dataCount, c + Endianness.ToUInt64LittleEndian(dataArray, endOffset - 16));
				a += d;

				var groupEndOffset = dataOffset + dataCount - 16;

				for (var groupCurrentOffset = dataOffset; groupCurrentOffset < groupEndOffset; groupCurrentOffset += 16)
				{
					a ^= Mix(Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset) * K1) * K1;
					a *= K1;
					b ^= a;
					c ^= Mix(Endianness.ToUInt64LittleEndian(dataArray, groupCurrentOffset + 8) * K1) * K1;
					c *= K1;
					d ^= c;
				}
			}

			a = Hash64Len16(a, c);
			b = Hash64Len16(d, b);
			return new UInt128(Hash64Len16(b, a), a ^ b);
		}

		#endregion

		#region Shared Utilities

		private static uint Mix(uint h)
		{
			h ^= h >> 16;
			h *= 0x85ebca6b;
			h ^= h >> 13;
			h *= 0xc2b2ae35;
			h ^= h >> 16;
			return h;
		}

		private static ulong Mix(ulong value) =>
			value ^ (value >> 47);

		private uint Mur(uint a, uint h)
		{
			// Helper from Murmur3 for combining two 32-bit values.
			a *= C1;
			a = RotateRight(a, 17);
			a *= C2;
			h ^= a;
			h = RotateRight(h, 19);
			return (h * 5) + 0xe6546b64;
		}

		private static void Permute3(ref uint a, ref uint b, ref uint c)
		{
			uint temp = a;
			a = c;
			c = b;
			b = temp;
		}

		private static ulong Hash128to64(UInt128 x)
		{
			const ulong kMul = 0x9ddfea08eb382d69;

			ulong a = (x.GetLower() ^ x.GetUpper()) * kMul;
			a ^= a >> 47;

			ulong b = (x.GetUpper() ^ a) * kMul;
			b ^= b >> 47;
			b *= kMul;

			return b;
		}

		private static uint RotateRight(uint operand, int shiftCount)
		{
			shiftCount &= 0x1f;

			return
				(operand >> shiftCount) |
				(operand << (32 - shiftCount));
		}

		private static ulong RotateRight(ulong operand, int shiftCount)
		{
			shiftCount &= 0x3f;

			return
				(operand >> shiftCount) |
				(operand << (64 - shiftCount));
		}

		private static uint ReverseByteOrder(uint operand)
		{
			return
				(operand >> 24) |
				((operand & 0x00ff0000) >> 8) |
				((operand & 0x0000ff00) << 8) |
				(operand << 24);
		}

		private static ulong ReverseByteOrder(ulong operand)
		{
			return
				(operand >> 56) |
				((operand & 0x00ff000000000000) >> 40) |
				((operand & 0x0000ff0000000000) >> 24) |
				((operand & 0x000000ff00000000) >> 8) |
				((operand & 0x00000000ff000000) << 8) |
				((operand & 0x0000000000ff0000) << 24) |
				((operand & 0x000000000000ff00) << 40) |
				(operand << 56);
		}
		#endregion
	}
}
