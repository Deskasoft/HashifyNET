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
using System.Threading;

namespace HashifyNet.Algorithms.T1HA
{
	[HashAlgorithmImplementation(typeof(IT1HA0), typeof(T1HA0Config))]
	internal class T1HA0_Implementation
		: HashFunctionBase<IT1HA0Config>,
			IT1HA0
	{
		public override IT1HA0Config Config => _config.Clone();
		private readonly IT1HA0Config _config;

		public T1HA0_Implementation(IT1HA0Config config)
		{
			if (config == null)
				throw new ArgumentNullException(nameof(config));

			_config = config.Clone();

			if (_config.HashSizeInBits != 64)
			{
				throw new ArgumentOutOfRangeException(
					$"{nameof(config)}.{nameof(config.HashSizeInBits)}",
					_config.HashSizeInBits,
					$"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64 bits for T1HA0.");
			}
		}

		protected override IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			ulong seed = (ulong)_config.Seed;
			int len = data.Length;

			uint a = Rot32((uint)len, 17) + (uint)seed;
			uint b = (uint)len ^ (uint)(seed >> 32);

			int offset = 0;

			if (len > 16)
			{
				uint c = ~a;
				uint d = Rot32(b, 5);
				int detent = len - 15;

				while (offset < detent)
				{
					cancellationToken.ThrowIfCancellationRequested();

					uint w0 = Endianness.ToUInt32LittleEndian(data.Slice(offset + 0));
					uint w1 = Endianness.ToUInt32LittleEndian(data.Slice(offset + 4));
					uint w2 = Endianness.ToUInt32LittleEndian(data.Slice(offset + 8));
					uint w3 = Endianness.ToUInt32LittleEndian(data.Slice(offset + 12));
					offset += 16;

					uint d13 = w1 + Rot32(w3 + d, 17);
					uint c02 = w0 ^ Rot32(w2 + c, 11);
					d ^= Rot32(a + w0, 3);
					c ^= Rot32(b + w1, 7);

					b = T1HAGlobals.PRIME32_1 * (c02 + w3);
					a = T1HAGlobals.PRIME32_0 * (d13 ^ w2);
				}

				c += a;
				d += b;
				a ^= T1HAGlobals.PRIME32_6 * (Rot32(c, 16) + d);
				b ^= T1HAGlobals.PRIME32_5 * (c + Rot32(d, 16));

				len &= 15;
			}

			switch (len)
			{
				default: // len >= 13
					MixUp32(ref a, ref b, Endianness.ToUInt32LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME32_4);
					offset += 4;
					len -= 4;
					goto case 12;

				case 12:
				case 11:
				case 10:
				case 9:
					MixUp32(ref b, ref a, Endianness.ToUInt32LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME32_3);
					offset += 4;
					len -= 4;
					goto case 8;

				case 8:
				case 7:
				case 6:
				case 5:
					MixUp32(ref a, ref b, Endianness.ToUInt32LittleEndian(data.Slice(offset)), T1HAGlobals.PRIME32_2);
					offset += 4;
					len -= 4;
					goto case 4;

				case 4:
				case 3:
				case 2:
				case 1:
					MixUp32(ref b, ref a, Tail32LE(data, offset, len), T1HAGlobals.PRIME32_1);
					goto case 0;

				case 0:
					break;
			}

			ulong result = Final32(a, b);

			byte[] finalBytes = Endianness.GetBytesLittleEndian(result);
			return new HashValue(ValueEndianness.LittleEndian, finalBytes, 64);
		}

		private static uint Rot32(uint v, int s)
		{
			return (v >> s) | (v << (32 - s));
		}

		private static uint Tail32LE(ReadOnlySpan<byte> data, int offset, int tailLen)
		{
			int tail = tailLen & 3;

			if (tail == 0 && tailLen > 0)
				return Endianness.ToUInt32LittleEndian(data.Slice(offset));

			uint r = 0;
			var p = data.Slice(offset);

			switch (tail)
			{
				case 3:
					r = (uint)p[2] << 16;
					goto case 2;

				case 2:
					return r + ((uint)p[0] | ((uint)p[1] << 8));

				case 1:
					return p[0];

				default:
					return 0;
			}
		}

		private static void MixUp32(ref uint a, ref uint b, uint v, uint prime)
		{
			// mul_32x32_64(*b + v, prime)
			ulong l = (ulong)(b + v) * (ulong)prime;
			a ^= (uint)l;
			b += (uint)(l >> 32);
		}

		private static ulong Final32(uint a, uint b)
		{
			// low = (b ^ rot32(a, 13)), high = a
			ulong l = (ulong)(b ^ Rot32(a, 13)) | ((ulong)a << 32);
			l *= T1HAGlobals.PRIME_0;
			l ^= l >> 41;
			l *= T1HAGlobals.PRIME_4;
			l ^= l >> 47;
			l *= T1HAGlobals.PRIME_6;
			return l;
		}
	}
}
