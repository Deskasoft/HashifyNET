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

using HashifyNet.Core;
using HashifyNet.Core.Utilities;
using System;
using System.Linq;
using System.Threading;

namespace HashifyNet.Algorithms.FarmHash
{
	/// <summary>
	/// Provides an internal implementation of the FarmHash fingerprint algorithm, supporting hash sizes of 32, 64, or 128
	/// bits.
	/// </summary>
	/// <remarks>This class is used internally to compute hash fingerprints based on the FarmHash algorithm. It
	/// supports configurable hash sizes of 32, 64, or 128 bits, and ensures that the appropriate implementation is
	/// selected based on the configuration.</remarks>
	[HashAlgorithmImplementation(typeof(IFarmHashFingerprint), typeof(FarmHashConfig))]
	internal class FarmHashFingerprint_Implementation
		: HashFunctionBase<IFarmHashConfig>,
			IFarmHashFingerprint
	{
		private readonly IFarmHashConfig _config;
		public override IFarmHashConfig Config => _config.Clone();

		private readonly IFarmHashFingerprintInternal _impl;
		public FarmHashFingerprint_Implementation(IFarmHashConfig config)
		{
			_config = config?.Clone() ?? throw new ArgumentNullException(nameof(config));
			if (config.HashSizeInBits != 128 && config.HashSizeInBits != 64 && config.HashSizeInBits != 32)
			{
				throw new ArgumentOutOfRangeException(
					nameof(config.HashSizeInBits),
					"Hash size for FarmHashFingerprint must be 32, 64 or 128 bits.");
			}

			switch (config.HashSizeInBits)
			{
				case 32:
					_impl = new FarmHashFingerprint32();
					break;
				case 64:
					_impl = new FarmHashFingerprint64();
					break;
				case 128:
					_impl = new FarmHashFingerprint128();
					break;
			}
		}

		protected override IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			return _impl.ComputeHashInternal(data, cancellationToken);
		}

		private interface IFarmHashFingerprintInternal
		{
			IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken);
		}

		private class FarmHashFingerprint32 : IFarmHashFingerprintInternal
		{
			private const uint c1 = 0xcc9e2d51;
			private const uint c2 = 0x1b873593;

			public IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
			{
				var dataCount = data.Length;
				uint hashValue;

				if (dataCount > 24)
				{
					hashValue = ComputeHash25Plus(data, cancellationToken);

				}
				else if (dataCount > 12)
				{
					hashValue = ComputeHash13To24(data);

				}
				else if (dataCount > 4)
				{
					hashValue = ComputeHash5To12(data);

				}
				else
				{
					hashValue = ComputeHash0To4(data);
				}

				return new HashValue(
					ValueEndianness.LittleEndian,
					Endianness.GetBytesLittleEndian(hashValue),
					32);
			}

			private static uint ComputeHash25Plus(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
			{
				var dataCount = data.Length;

				var h = (uint)dataCount;
				var g = (uint)(c1 * dataCount);
				var f = g;

				var a0 = RotateRight(Endianness.ToUInt32LittleEndian(data, dataCount - 4) * c1, 17) * c2;
				var a1 = RotateRight(Endianness.ToUInt32LittleEndian(data, dataCount - 8) * c1, 17) * c2;
				var a2 = RotateRight(Endianness.ToUInt32LittleEndian(data, dataCount - 16) * c1, 17) * c2;
				var a3 = RotateRight(Endianness.ToUInt32LittleEndian(data, dataCount - 12) * c1, 17) * c2;
				var a4 = RotateRight(Endianness.ToUInt32LittleEndian(data, dataCount - 20) * c1, 17) * c2;

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
				f = RotateRight(f, 19) + 113;

				// Process groups of 20 bytes, leaving 1 to 20 bytes remaining.
				{
					var groupEndOffset = dataCount - 20;

					for (var currentOffset = 0; currentOffset < groupEndOffset; currentOffset += 20)
					{

						var a = Endianness.ToUInt32LittleEndian(data, currentOffset);
						var b = Endianness.ToUInt32LittleEndian(data, currentOffset + 4);
						var c = Endianness.ToUInt32LittleEndian(data, currentOffset + 8);
						var d = Endianness.ToUInt32LittleEndian(data, currentOffset + 12);
						var e = Endianness.ToUInt32LittleEndian(data, currentOffset + 16);

						h += a;
						g += b;
						f += c;
						h = Mur(d, h) + e;
						g = Mur(c, g) + a;
						f = Mur(b + (e * c1), f) + d;
						f += g;
						g += f;
					}
				}

				g = RotateRight(g, 11) * c1;
				g = RotateRight(g, 17) * c1;
				f = RotateRight(f, 11) * c1;
				f = RotateRight(f, 17) * c1;
				h = RotateRight(h + g, 19);

				h = (h * 5) + 0xe6546b64;
				h = RotateRight(h, 17) * c1;
				h = RotateRight(h + f, 19);
				h = (h * 5) + 0xe6546b64;
				h = RotateRight(h, 17) * c1;

				return h;
			}

			private static uint ComputeHash13To24(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var a = Endianness.ToUInt32LittleEndian(data, (dataCount >> 1) - 4);
				var b = Endianness.ToUInt32LittleEndian(data, 4);
				var c = Endianness.ToUInt32LittleEndian(data, dataCount - 8);
				var d = Endianness.ToUInt32LittleEndian(data, dataCount >> 1);
				var e = Endianness.ToUInt32LittleEndian(data, 0);
				var f = Endianness.ToUInt32LittleEndian(data, dataCount - 4);
				var h = (d * c1) + (uint)dataCount;

				a = RotateRight(a, 12) + f;
				h = Mur(c, h) + a;
				a = RotateRight(a, 3) + c;
				h = Mur(e, h) + a;
				a = RotateRight(a + f, 12) + d;
				h = Mur(b, h) + a;

				return FMix(h);
			}

			private static uint ComputeHash5To12(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var a = (uint)dataCount;
				var b = (uint)(dataCount * 5);
				var c = 9U;
				var d = b;

				a += Endianness.ToUInt32LittleEndian(data, 0);
				b += Endianness.ToUInt32LittleEndian(data, dataCount - 4);
				c += Endianness.ToUInt32LittleEndian(data, (dataCount >> 1) & 4);

				return FMix(Mur(c, Mur(b, Mur(a, d))));
			}

			private static uint ComputeHash0To4(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var b = 0U;
				var c = 9U;

				for (var currentOffset = 0; currentOffset < data.Length; currentOffset += 1)
				{
					var v = (sbyte)data[currentOffset];

					b = (uint)((b * c1) + v);
					c ^= b;
				}

				return FMix(Mur(b, Mur((uint)dataCount, c)));
			}

			#region Utilities

			private static uint FMix(uint h)
			{
				h ^= h >> 16;
				h *= 0x85ebca6b;
				h ^= h >> 13;
				h *= 0xc2b2ae35;
				h ^= h >> 16;

				return h;
			}

			private static uint Mur(uint a, uint h)
			{
				a *= c1;
				a = RotateRight(a, 17);
				a *= c2;

				h ^= a;
				h = RotateRight(h, 19);

				return (h * 5) + 0xe6546b64;
			}

			private static uint RotateRight(uint operand, int shiftCount)
			{
				shiftCount &= 0x1f;

				return
					(operand >> shiftCount) |
					(operand << (32 - shiftCount));
			}

			#endregion
		}

		private class FarmHashFingerprint64 : IFarmHashFingerprintInternal
		{
			private const ulong _k0 = 0xc3a5c85c97cb3127UL;
			private const ulong _k1 = 0xb492b66fbe98f273UL;
			private const ulong _k2 = 0x9ae16a3b2f90404fUL;
			private const ulong _seed = 81UL;

			public IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
			{
				var dataCount = data.Length;
				ulong hashValue;

				if (dataCount > 64)
				{
					hashValue = ComputeHash65Plus(data, cancellationToken);

				}
				else if (dataCount > 32)
				{
					hashValue = ComputeHash33To64(data);

				}
				else if (dataCount > 16)
				{
					hashValue = ComputeHash17To32(data);

				}
				else
				{
					hashValue = ComputeHash0To16(data);
				}

				return new HashValue(
					ValueEndianness.LittleEndian,
					Endianness.GetBytesLittleEndian(hashValue),
					64);
			}

			private static ulong ComputeHash16(ulong u, ulong v, ulong mul)
			{
				var a = (u ^ v) * mul;
				a ^= a >> 47;

				var b = (v ^ a) * mul;
				b ^= b >> 47;
				b *= mul;

				return b;
			}

			private static ulong ComputeHash0To16(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				if (dataCount >= 8)
				{
					var mul = _k2 + (ulong)(dataCount * 2);
					var a = Endianness.ToUInt64LittleEndian(data, 0) + _k2;
					var b = Endianness.ToUInt64LittleEndian(data, dataCount - 8);
					var c = (RotateRight(b, 37) * mul) + a;
					var d = (RotateRight(a, 25) + b) * mul;

					return ComputeHash16(c, d, mul);
				}

				if (dataCount >= 4)
				{
					var mul = _k2 + ((ulong)dataCount * 2);
					ulong a = Endianness.ToUInt32LittleEndian(data, 0);

					return ComputeHash16((ulong)dataCount + (a << 3), Endianness.ToUInt32LittleEndian(data, dataCount - 4), mul);
				}

				if (dataCount > 0)
				{
					var a = data[0];
					var b = data[dataCount >> 1];
					var c = data[dataCount - 1];

					var y = a + (((uint)b) << 8);
					var z = (uint)dataCount + (((uint)c) << 2);

					return ShiftMix((y * _k2) ^ (z * _k0)) * _k2;
				}

				return _k2;
			}

			private static ulong ComputeHash17To32(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var mul = _k2 + ((ulong)dataCount * 2);
				var a = Endianness.ToUInt64LittleEndian(data, 0) * _k1;
				var b = Endianness.ToUInt64LittleEndian(data, 8);
				var c = Endianness.ToUInt64LittleEndian(data, dataCount - 8) * mul;
				var d = Endianness.ToUInt64LittleEndian(data, dataCount - 16) * _k2;

				return ComputeHash16(
					RotateRight(a + b, 43) + RotateRight(c, 30) + d,
					a + RotateRight(b + _k2, 18) + c,
					mul);
			}

			private static UInt128 WeakHashLen32WithSeeds(ulong w, ulong x, ulong y, ulong z, ulong a, ulong b)
			{
				a += w;
				b = RotateRight(b + a + z, 21);

				var c = a;

				a += x;
				a += y;
				b += RotateRight(a, 44);

				return new UInt128(b + c, a + z);
			}

			private static UInt128 WeakHashLen32WithSeeds(ReadOnlySpan<byte> dataArray, int dataOffset, ulong a, ulong b) =>
				WeakHashLen32WithSeeds(
					Endianness.ToUInt64LittleEndian(dataArray, dataOffset),
					Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 8),
					Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 16),
					Endianness.ToUInt64LittleEndian(dataArray, dataOffset + 24),
					a,
					b);

			private static ulong ComputeHash33To64(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var mul = _k2 + ((ulong)dataCount * 2);
				var a = Endianness.ToUInt64LittleEndian(data, 0) * _k2;
				var b = Endianness.ToUInt64LittleEndian(data, 8);
				var c = Endianness.ToUInt64LittleEndian(data, dataCount - 8) * mul;
				var d = Endianness.ToUInt64LittleEndian(data, dataCount - 16) * _k2;

				var y = RotateRight(a + b, 43) + RotateRight(c, 30) + d;
				var z = ComputeHash16(y, a + RotateRight(b + _k2, 18) + c, mul);

				var e = Endianness.ToUInt64LittleEndian(data, 16) * mul;
				var f = Endianness.ToUInt64LittleEndian(data, 24);
				var g = (y + Endianness.ToUInt64LittleEndian(data, dataCount - 32)) * mul;
				var h = (z + Endianness.ToUInt64LittleEndian(data, dataCount - 24)) * mul;

				return ComputeHash16(
					RotateRight(e + f, 43) + RotateRight(g, 30) + h,
					e + RotateRight(f + a, 18) + g,
					mul);
			}

			private static ulong ComputeHash65Plus(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
			{
				var dataCount = data.Length;

				var x = _seed;
				var y = unchecked(_seed * _k1) + 113;
				var z = ShiftMix((y * _k2) + 113) * _k2;

				var v = new UInt128();
				var w = new UInt128();

				x = (x * _k2) + Endianness.ToUInt64LittleEndian(data, 0);

				// Process 64-byte groups, leaving a final group of 1-64 bytes in size.
				{
					var groupEndOffset = dataCount - 64;

					for (var currentOffset = 0; currentOffset < groupEndOffset; currentOffset += 64)
					{
						cancellationToken.ThrowIfCancellationRequested();

						x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(data, currentOffset + 8), 37) * _k1;
						y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(data, currentOffset + 48), 42) * _k1;
						x ^= w.GetUpper();
						y += v.GetLower() + Endianness.ToUInt64LittleEndian(data, currentOffset + 40);
						z = RotateRight(z + w.GetLower(), 33) * _k1;
						v = WeakHashLen32WithSeeds(data, currentOffset, v.GetUpper() * _k1, x + w.GetLower());
						w = WeakHashLen32WithSeeds(data, currentOffset + 32, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(data, currentOffset + 16));

						Swap(ref z, ref x);
					}
				}

				cancellationToken.ThrowIfCancellationRequested();

				var mul = _k1 + ((z & 0xff) << 1);

				w = new UInt128(w.GetUpper(), w.GetLower() + (ulong)((dataCount - 1) & 63));
				v = new UInt128(v.GetUpper(), v.GetLower() + w.GetLower());
				w = new UInt128(w.GetUpper(), w.GetLower() + v.GetLower());

				x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(data, dataCount - 56), 37) * mul;
				y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(data, dataCount - 16), 42) * mul;

				x ^= w.GetUpper() * 9;
				y += (v.GetLower() * 9) + Endianness.ToUInt64LittleEndian(data, dataCount - 24);
				z = RotateRight(z + w.GetLower(), 33) * mul;

				v = WeakHashLen32WithSeeds(data, dataCount - 64, v.GetUpper() * mul, x + w.GetLower());
				w = WeakHashLen32WithSeeds(data, dataCount - 32, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(data, dataCount - 48));

				Swap(ref z, ref x);

				cancellationToken.ThrowIfCancellationRequested();

				return ComputeHash16(
					ComputeHash16(v.GetLower(), w.GetLower(), mul) + (ShiftMix(y) * _k0) + z,
					ComputeHash16(v.GetUpper(), w.GetUpper(), mul) + x,
					mul);
			}

			#region Utilities

			private static ulong RotateRight(ulong operand, int shiftCount)
			{
				shiftCount &= 0x3f;

				return
					(operand >> shiftCount) |
					(operand << (64 - shiftCount));
			}

			private static ulong ShiftMix(ulong value) =>
				value ^ (value >> 47);

			private static void Swap(ref ulong first, ref ulong second)
			{
				var temp = first;

				first = second;
				second = temp;
			}

			#endregion
		}

		private class FarmHashFingerprint128 : IFarmHashFingerprintInternal
		{
			private const ulong k0 = 0xc3a5c85c97cb3127UL;
			private const ulong k1 = 0xb492b66fbe98f273UL;
			private const ulong k2 = 0x9ae16a3b2f90404fUL;

			public IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
			{
				var dataCount = data.Length;

				UInt128 hashValue;

				if (dataCount >= 16)
				{
					hashValue = CityHash128WithSeed(
						data.Slice(16, dataCount - 16),
						new UInt128(
							Endianness.ToUInt64LittleEndian(data, 8) + k0,
							Endianness.ToUInt64LittleEndian(data, 0)),
						cancellationToken);

				}
				else
				{
					hashValue = CityHash128WithSeed(data, new UInt128(k1, k0), cancellationToken);
				}

				var hashValueBytes = Endianness.GetBytesLittleEndian(hashValue.GetLower())
					.Concat(Endianness.GetBytesLittleEndian(hashValue.GetUpper()));

				return new HashValue(ValueEndianness.LittleEndian, hashValueBytes, 128);
			}

			private UInt128 CityHash128WithSeed(ReadOnlySpan<byte> data, UInt128 seed, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var dataCount = data.Length;

				if (dataCount < 128)
				{
					return CityMurmur(data, seed);
				}

				// We expect len >= 128 to be the common case.  Keep 56 bytes of state:
				// v, w, x, y, and z.
				UInt128 v;
				{
					var vLow = (RotateRight(seed.GetUpper() ^ k1, 49) * k1) + Endianness.ToUInt64LittleEndian(data, 0);
					v = new UInt128(
						(RotateRight(vLow, 42) * k1) + Endianness.ToUInt64LittleEndian(data, 8),
						vLow);
				}

				UInt128 w = new UInt128(
					RotateRight(seed.GetLower() + Endianness.ToUInt64LittleEndian(data, 88), 53) * k1,
					(RotateRight(seed.GetUpper() + ((ulong)dataCount * k1), 35) * k1) + seed.GetLower());

				ulong x = seed.GetLower();
				ulong y = seed.GetUpper();
				ulong z = (ulong)dataCount * k1;

				// This is the same inner loop as CityHash64()
				int lastGroupEndOffset;
				{
					var groupEndOffset = dataCount - (dataCount % 128);

					for (var groupCurrentOffset = 0; groupCurrentOffset < groupEndOffset; groupCurrentOffset += 128)
					{
						cancellationToken.ThrowIfCancellationRequested();

						x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 8), 37) * k1;
						y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 48), 42) * k1;
						x ^= w.GetUpper();
						y += v.GetLower() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 40);
						z = RotateRight(z + w.GetLower(), 33) * k1;
						v = WeakHashLen32WithSeeds(data, groupCurrentOffset, v.GetUpper() * k1, x + w.GetLower());
						w = WeakHashLen32WithSeeds(data, groupCurrentOffset + 32, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 16));

						{
							ulong temp = z;
							z = x;
							x = temp;
						}

						x = RotateRight(x + y + v.GetLower() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 72), 37) * k1;
						y = RotateRight(y + v.GetUpper() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 112), 42) * k1;
						x ^= w.GetUpper();
						y += v.GetLower() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 104);
						z = RotateRight(z + w.GetLower(), 33) * k1;
						v = WeakHashLen32WithSeeds(data, groupCurrentOffset + 64, v.GetUpper() * k1, x + w.GetLower());
						w = WeakHashLen32WithSeeds(data, groupCurrentOffset + 96, z + w.GetUpper(), y + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 80));

						{
							ulong temp = z;
							z = x;
							x = temp;
						}
					}

					lastGroupEndOffset = groupEndOffset;
				}

				cancellationToken.ThrowIfCancellationRequested();

				x += RotateRight(v.GetLower() + z, 49) * k0;
				y = (y * k0) + RotateRight(w.GetUpper(), 37);
				z = (z * k0) + RotateRight(w.GetLower(), 27);
				w = new UInt128(w.GetUpper(), w.GetLower() * 9);
				v = new UInt128(v.GetUpper(), v.GetLower() * k0);

				// Hash up to 4 chunks of 32 bytes each from the end of data.
				{
					var groupEndOffset = lastGroupEndOffset - 32;

					for (var groupCurrentOffset = dataCount - 32; groupCurrentOffset > groupEndOffset; groupCurrentOffset -= 32)
					{
						cancellationToken.ThrowIfCancellationRequested();

						y = (RotateRight(x + y, 42) * k0) + v.GetUpper();
						w = new UInt128(w.GetUpper(), w.GetLower() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 16));
						x = (x * k0) + w.GetLower();
						z += w.GetUpper() + Endianness.ToUInt64LittleEndian(data, groupCurrentOffset);
						w = new UInt128(w.GetUpper() + v.GetLower(), w.GetLower());
						v = WeakHashLen32WithSeeds(data, groupCurrentOffset, v.GetLower() + z, v.GetUpper());
						v = new UInt128(v.GetUpper(), v.GetLower() * k0);
					}
				}

				// At this point our 56 bytes of state should contain more than
				// enough information for a strong 128-bit hash.  We use two
				// different 56-byte-to-8-byte hashes to get a 16-byte final result.
				x = HashLen16(x, v.GetLower());
				y = HashLen16(y + z, w.GetLower());

				return new UInt128(
					HashLen16(x + w.GetUpper(), y + v.GetUpper()),
					HashLen16(x + v.GetUpper(), w.GetUpper()) + y);
			}

			// A subroutine for CityHash128().  Returns a decent 128-bit hash for strings
			// of any length representable in signed long.  Based on City and Murmur.
			private static UInt128 CityMurmur(ReadOnlySpan<byte> data, UInt128 seed)
			{
				var dataCount = data.Length;

				ulong a = seed.GetLower();
				ulong b = seed.GetUpper();
				ulong c;
				ulong d;

				if (dataCount <= 16)
				{
					// len <= 16
					a = Mix(a * k1) * k1;
					c = (b * k1) + HashLen0to16(data);
					d = Mix(a + (dataCount >= 8 ? Endianness.ToUInt64LittleEndian(data, 0) : c));
				}
				else
				{
					// len > 16
					c = HashLen16(Endianness.ToUInt64LittleEndian(data, dataCount - 8) + k1, a);
					d = HashLen16(b + (ulong)dataCount, c + Endianness.ToUInt64LittleEndian(data, dataCount - 16));
					a += d;

					var groupEndOffset = dataCount - 16;

					for (var groupCurrentOffset = 0; groupCurrentOffset < groupEndOffset; groupCurrentOffset += 16)
					{
						a ^= Mix(Endianness.ToUInt64LittleEndian(data, groupCurrentOffset) * k1) * k1;
						a *= k1;
						b ^= a;
						c ^= Mix(Endianness.ToUInt64LittleEndian(data, groupCurrentOffset + 8) * k1) * k1;
						c *= k1;
						d ^= c;
					}
				}

				a = HashLen16(a, c);
				b = HashLen16(d, b);
				return new UInt128(HashLen16(b, a), a ^ b);
			}

			private static ulong HashLen16(ulong u, ulong v)
			{
				return Hash128to64(
					new UInt128(v, u));
			}

			private static ulong HashLen16(ulong u, ulong v, ulong mul)
			{
				ulong a = (u ^ v) * mul;
				a ^= a >> 47;

				ulong b = (v ^ a) * mul;
				b ^= b >> 47;
				b *= mul;

				return b;
			}

			private static ulong HashLen0to16(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				if (dataCount >= 8)
				{
					ulong mul = k2 + ((ulong)dataCount * 2);
					ulong a = Endianness.ToUInt64LittleEndian(data, 0) + k2;
					ulong b = Endianness.ToUInt64LittleEndian(data, dataCount - 8);
					ulong c = (RotateRight(b, 37) * mul) + a;
					ulong d = (RotateRight(a, 25) + b) * mul;

					return HashLen16(c, d, mul);
				}

				if (dataCount >= 4)
				{
					ulong mul = k2 + ((ulong)dataCount * 2);
					ulong a = Endianness.ToUInt32LittleEndian(data, 0);
					return HashLen16((ulong)dataCount + (a << 3), Endianness.ToUInt32LittleEndian(data, dataCount - 4), mul);
				}

				if (dataCount > 0)
				{
					byte a = data[0];
					byte b = data[dataCount >> 1];
					byte c = data[dataCount - 1];

					uint y = a + ((uint)b << 8);
					uint z = (uint)dataCount + ((uint)c << 2);

					return Mix((y * k2) ^ (z * k0)) * k2;
				}

				return k2;
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

			// Return a 16-byte hash for 48 bytes.  Quick and dirty.
			// Callers do best to use "random-looking" values for a and b.
			private static UInt128 WeakHashLen32WithSeeds(
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
			private static UInt128 WeakHashLen32WithSeeds(ReadOnlySpan<byte> data, int startIndex, ulong a, ulong b)
			{
				return WeakHashLen32WithSeeds(
					Endianness.ToUInt64LittleEndian(data, startIndex),
					Endianness.ToUInt64LittleEndian(data, startIndex + 8),
					Endianness.ToUInt64LittleEndian(data, startIndex + 16),
					Endianness.ToUInt64LittleEndian(data, startIndex + 24),
					a,
					b);
			}

			#region Utilities

			private static ulong RotateRight(ulong operand, int shiftCount)
			{
				shiftCount &= 0x3f;

				return
					(operand >> shiftCount) |
					(operand << (64 - shiftCount));
			}

			private static ulong Mix(ulong value) =>
				value ^ (value >> 47);

			#endregion
		}
	}
}
