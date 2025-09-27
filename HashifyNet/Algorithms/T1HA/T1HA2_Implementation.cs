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
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HashifyNet.Algorithms.T1HA
{
	[HashAlgorithmImplementation(typeof(IT1HA2), typeof(T1HA2Config))]
	internal class T1HA2_Implementation
		: StreamableHashFunctionBase<IT1HA2Config>,
			IT1HA2
	{
		public override IT1HA2Config Config => _config.Clone();
		private readonly IT1HA2Config _config;

		public T1HA2_Implementation(IT1HA2Config config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			_config = config.Clone();

			if (_config.HashSizeInBits != 64 && _config.HashSizeInBits != 128)
			{
				throw new ArgumentOutOfRangeException(
					$"{nameof(config)}.{nameof(config.HashSizeInBits)}",
					_config.HashSizeInBits,
					$"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64 or 128 bits for T1HA2.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer((ulong)_config.Seed, (ulong)_config.Seed2, _config.HashSizeInBits);
		}

		private class BlockTransformer : BlockTransformerBase<BlockTransformer>
		{
			public BlockTransformer() : this(0, 0, 64) { }

			private ulong a, b, c, d;
			private long totalBytes;
			private readonly int bitSize;

			public BlockTransformer(ulong seedX, ulong seedY, int bitSize) : base(inputBlockSize: 32)
			{
				this.bitSize = bitSize;

				a = seedX;
				b = seedY;
				c = Rot64(seedY, 23) + ~seedX;
				d = ~seedY + Rot64(seedX, 19);

				totalBytes = 0;
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				other.a = a; other.b = b; other.c = c; other.d = d;
				other.totalBytes = totalBytes;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				unchecked
				{
					int offset = 0;
					while (offset + 32 <= data.Length)
					{
						totalBytes += 32;

						ulong w0 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 0));
						ulong w1 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 8));
						ulong w2 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 16));
						ulong w3 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 24));
						offset += 32;

						// T1HA2_UPDATE(le, ALIGNESS, state, v)
						ulong d02 = w0 + Rot64(w2 + d, 56);
						ulong c13 = w1 + Rot64(w3 + c, 19);
						d ^= b + Rot64(w1, 38);
						c ^= a + Rot64(w0, 57);
						b ^= T1HAGlobals.PRIME_6 * (c13 + w2);
						a ^= T1HAGlobals.PRIME_5 * (d02 + w3);
					}
				}
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				totalBytes += leftover.Length;

				ulong bits = ((ulong)totalBytes << 3) ^ (1UL << 63);
				Span<byte> bitsBuf = stackalloc byte[8];
				BinaryPrimitives.WriteUInt64LittleEndian(bitsBuf, bits);

				int combinedLen = leftover.Length + 8;
				byte[] combined = new byte[combinedLen];
				leftover.CopyTo(combined.AsSpan(0, leftover.Length));
				bitsBuf.CopyTo(combined.AsSpan(leftover.Length, 8));

				int offset = 0;
				while (offset + 32 <= combinedLen)
				{
					ulong w0 = Endianness.ToUInt64LittleEndian(combined.AsSpan(offset + 0, 8));
					ulong w1 = Endianness.ToUInt64LittleEndian(combined.AsSpan(offset + 8, 8));
					ulong w2 = Endianness.ToUInt64LittleEndian(combined.AsSpan(offset + 16, 8));
					ulong w3 = Endianness.ToUInt64LittleEndian(combined.AsSpan(offset + 24, 8));
					offset += 32;

					ulong d02 = w0 + Rot64(w2 + d, 56);
					ulong c13 = w1 + Rot64(w3 + c, 19);
					d ^= b + Rot64(w1, 38);
					c ^= a + Rot64(w0, 57);
					b ^= T1HAGlobals.PRIME_6 * (c13 + w2);
					a ^= T1HAGlobals.PRIME_5 * (d02 + w3);
				}

				int tailLen = combinedLen - offset;

				if (bitSize == 64)
				{
					Squash();

					// T1HA2_TAIL_AB(le, ALIGNESS, state, data, len)
					switch (tailLen)
					{
						default:
							MixUp64(ref a, ref b, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_4);
							offset += 8; tailLen -= 8;
							goto case 24;

						case 24:
						case 23:
						case 22:
						case 21:
						case 20:
						case 19:
						case 18:
						case 17:
							MixUp64(ref b, ref a, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_3);
							offset += 8; tailLen -= 8;
							goto case 16;

						case 16:
						case 15:
						case 14:
						case 13:
						case 12:
						case 11:
						case 10:
						case 9:
							MixUp64(ref a, ref b, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_2);
							offset += 8; tailLen -= 8;
							goto case 8;

						case 8:
						case 7:
						case 6:
						case 5:
						case 4:
						case 3:
						case 2:
						case 1:
							MixUp64(ref b, ref a, Tail64LE(combined, offset, tailLen), T1HAGlobals.PRIME_1);
							goto case 0;

						case 0:
							break;
					}

					unchecked
					{
						ulong x = (a + Rot64(b, 41)) * T1HAGlobals.PRIME_0;
						ulong y = (Rot64(a, 23) + b) * T1HAGlobals.PRIME_6;
						ulong h = Mux64(x ^ y, T1HAGlobals.PRIME_5);

						var out64 = Endianness.GetBytesLittleEndian(h);
						return new HashValue(ValueEndianness.LittleEndian, out64, 64);
					}
				}
				else
				{
					switch (tailLen)
					{
						default:
							MixUp64(ref a, ref d, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_4);
							offset += 8; tailLen -= 8;
							goto case 24;

						case 24:
						case 23:
						case 22:
						case 21:
						case 20:
						case 19:
						case 18:
						case 17:
							MixUp64(ref b, ref a, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_3);
							offset += 8; tailLen -= 8;
							goto case 16;

						case 16:
						case 15:
						case 14:
						case 13:
						case 12:
						case 11:
						case 10:
						case 9:
							MixUp64(ref c, ref b, Endianness.ToUInt64LittleEndian(combined.AsSpan(offset, 8)), T1HAGlobals.PRIME_2);
							offset += 8; tailLen -= 8;
							goto case 8;

						case 8:
						case 7:
						case 6:
						case 5:
						case 4:
						case 3:
						case 2:
						case 1:
							MixUp64(ref d, ref c, Tail64LE(combined, offset, tailLen), T1HAGlobals.PRIME_1);
							goto case 0;

						case 0:
							break;
					}

					// final128(a,b,c,d)
					ulong extra;
					ulong h = Final128(a, b, c, d, out extra);
					byte[] out128 = Endianness.GetBytesLittleEndian(h, extra);
					return new HashValue(ValueEndianness.LittleEndian, out128, 128);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void Squash()
			{
				unchecked
				{
					a ^= T1HAGlobals.PRIME_6 * (c + Rot64(d, 23));
					b ^= T1HAGlobals.PRIME_5 * (Rot64(c, 19) + d);
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ulong Rot64(ulong x, int k) => (x >> k) | (x << (64 - k));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static void MixUp64(ref ulong a, ref ulong b, ulong v, ulong prime)
			{
				// *a ^= mul_64x64_128(*b + v, prime, &h); *b += h;
				BigMul128(b + v, prime, out var lo, out var hi);
				a ^= lo;
				b += hi;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong Mix64(ulong v, ulong p)
			{
				v *= p;
				return v ^ Rot64(v, 41);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ulong Mux64(ulong v, ulong prime)
			{
				BigMul128(v, prime, out var lo, out var hi);
				return lo ^ hi;
			}

			static void BigMul128(ulong a, ulong b, out ulong lo, out ulong hi)
			{
#if NET8_0_OR_GREATER
				hi = Math.BigMul(a, b, out lo);
#else
				ulong aLo = (uint)a, aHi = a >> 32;
				ulong bLo = (uint)b, bHi = b >> 32;

				ulong p0 = aLo * bLo;
				ulong p1 = aLo * bHi;
				ulong p2 = aHi * bLo;
				ulong p3 = aHi * bHi;

				ulong mid = (p1 << 32) + (p2 << 32);
				lo = p0 + mid;
				hi = p3 + (p1 >> 32) + (p2 >> 32) + (lo < p0 ? 1UL : 0UL);
#endif
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong Tail64LE(ReadOnlySpan<byte> data, int offset, int tailLen)
			{
				ulong r = 0;
				for (int i = 0; i < tailLen; i++)
					r |= (ulong)data[offset + i] << (8 * i);
				return r;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong Final128(ulong a, ulong b, ulong c, ulong d, out ulong extra)
			{
				MixUp64(ref a, ref b, Rot64(c, 41) ^ d, T1HAGlobals.PRIME_0);
				MixUp64(ref b, ref c, Rot64(d, 23) ^ a, T1HAGlobals.PRIME_6);
				MixUp64(ref c, ref d, Rot64(a, 19) ^ b, T1HAGlobals.PRIME_5);
				MixUp64(ref d, ref a, Rot64(b, 31) ^ c, T1HAGlobals.PRIME_4);
				extra = c + d;
				return a ^ b;
			}
		}
	}
}
