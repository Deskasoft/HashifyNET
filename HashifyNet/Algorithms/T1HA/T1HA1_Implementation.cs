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
	[HashAlgorithmImplementation(typeof(IT1HA1), typeof(T1HA1Config))]
	internal class T1HA1_Implementation
		: HashFunctionBase<IT1HA1Config>,
			IT1HA1
	{
		public override IT1HA1Config Config => _config.Clone();
		private readonly IT1HA1Config _config;

		public T1HA1_Implementation(IT1HA1Config config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			_config = config.Clone();

			if (_config.HashSizeInBits != 64)
			{
				throw new ArgumentOutOfRangeException(
					$"{nameof(config)}.{nameof(config.HashSizeInBits)}",
					_config.HashSizeInBits,
					$"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64 bits for T1HA1.");
			}
		}

		protected override IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			unchecked
			{
				ulong seed = (ulong)_config.Seed;
				int len = data.Length;
				ulong a = seed;
				ulong b = (ulong)len;

				int offset = 0;

				if (len > 32)
				{
					ulong c = Rot64((ulong)len, 17) + seed;
					ulong d = (ulong)len ^ Rot64(seed, 17);

					int detent = len - 31;
					while (offset < detent)
					{
						cancellationToken.ThrowIfCancellationRequested();

						ulong w0 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 0));
						ulong w1 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 8));
						ulong w2 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 16));
						ulong w3 = Endianness.ToUInt64LittleEndian(data.Slice(offset + 24));
						offset += 32;

						ulong d02 = w0 ^ Rot64(w2 + d, 17);
						ulong c13 = w1 ^ Rot64(w3 + c, 17);

						d -= (b ^ Rot64(w1, 31));
						c += (a ^ Rot64(w0, 41));

						b ^= T1HAGlobals.PRIME_0 * (c13 + w2);
						a ^= T1HAGlobals.PRIME_1 * (d02 + w3);
					}

					a ^= T1HAGlobals.PRIME_6 * (Rot64(c, 17) + d);
					b ^= T1HAGlobals.PRIME_5 * (c + Rot64(d, 17));

					len &= 31; // tail size in 0..31
				}

				switch (len)
				{
					default: // 25..31 bytes
						b += Mux64(Endianness.ToUInt64LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME_4);
						offset += 8;
						len -= 8;
						goto case 24;

					case 24:
					case 23:
					case 22:
					case 21:
					case 20:
					case 19:
					case 18:
					case 17:
						a += Mux64(Endianness.ToUInt64LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME_3);
						offset += 8;
						len -= 8;
						goto case 16;

					case 16:
					case 15:
					case 14:
					case 13:
					case 12:
					case 11:
					case 10:
					case 9:
						b += Mux64(Endianness.ToUInt64LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME_2);
						offset += 8;
						len -= 8;
						goto case 8;

					case 8:
					case 7:
					case 6:
					case 5:
					case 4:
					case 3:
					case 2:
					case 1:
						a += Mux64(Tail64LE(data, offset, len), T1HAGlobals.PRIME_1);
						goto case 0;

					case 0:
						break;
				}

				// final_weak_avalanche(a, b)
				ulong result = unchecked(Mux64(Rot64(a + b, 17), T1HAGlobals.PRIME_4) + Mix64(a ^ b, T1HAGlobals.PRIME_0));

				byte[] finalBytes = Endianness.GetBytesLittleEndian(result);
				return new HashValue(ValueEndianness.LittleEndian, finalBytes, 64);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Rot64(ulong v, int s) => unchecked((v >> s) | (v << (64 - s)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Mix64(ulong v, ulong p)
		{
			v = unchecked(v * p);
			return unchecked(v ^ Rot64(v, 41));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Mux64(ulong v, ulong prime)
		{
			T1HAGlobals.BigMul128(v, prime, out ulong lo, out ulong hi);
			return unchecked(lo ^ hi);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Tail64LE(ReadOnlySpan<byte> data, int offset, int tailLen)
		{
			ulong r = 0;
			for (int i = 0; i < tailLen; i++)
				r |= unchecked((ulong)data[offset + i] << (8 * i));
			return r;
		}
	}
}
