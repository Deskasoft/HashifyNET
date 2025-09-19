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
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HashifyNet.Algorithms.HighwayHash
{
	/// <summary>
	/// Provides an implementation of the HighwayHash algorithm, a fast and secure hash function designed for use cases
	/// such as checksums, hash tables, and message authentication.
	/// </summary>
	/// <remarks>This class supports hash sizes of 64, 128, or 256 bits, and requires a 32-byte key for
	/// initialization. It is optimized for performance and ensures strong security properties, making it suitable for
	/// scenarios where both speed and cryptographic strength are important.</remarks>
	[HashAlgorithmImplementation(typeof(IHighwayHash), typeof(HighwayHashConfig))]
	internal partial class HighwayHash_Implementation
		: StreamableHashFunctionBase<IHighwayHashConfig>,
		  IHighwayHash
	{
		public override IHighwayHashConfig Config => _config.Clone();

		private readonly IHighwayHashConfig _config;
		private readonly ulong[] _key;

		public HighwayHash_Implementation(IHighwayHashConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits != 64 && _config.HashSizeInBits != 128 && _config.HashSizeInBits != 256)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64, 128 or 256.");
			}

			if (_config.Key == null)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _config.Key, $"{nameof(config)}.{nameof(config.Key)} must not be null.");
			}

			if (_config.Key.Count != 32)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _config.Key, $"{nameof(config)}.{nameof(config.Key)} must be fixed at 32 bytes.");
			}

			_key = new ulong[4];
			var keyBytes = _config.Key as byte[] ?? new List<byte>(_config.Key).ToArray();
			for (int i = 0; i < 4; i++)
			{
				_key[i] = BinaryPrimitives.ReadUInt64LittleEndian(keyBytes.AsSpan(i * 8, 8));
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(_key, _config.HashSizeInBits);
		}

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private readonly int _hashSizeInBits;

			// Internal state vectors
			private ulong[] _v0;
			private ulong[] _v1;
			private ulong[] _mul0;
			private ulong[] _mul1;

			// Constants (64-bit lanes)
			private const ulong K0 = 0xdbe6d5d5fe4cce2fUL;
			private const ulong K1 = 0xa4093822299f31d0UL;
			private const ulong K2 = 0x13198a2e03707344UL;
			private const ulong K3 = 0x243f6a8885a308d3UL;

			private const ulong M0 = 0x3bd39e10cb0ef593UL;
			private const ulong M1 = 0xc0acf169b5f18a8cUL;
			private const ulong M2 = 0xbe5466cf34e90c6cUL;
			private const ulong M3 = 0x452821e638d01377UL;


			public BlockTransformer()
				: base(inputBlockSize: 32) // Process data in 32-byte blocks
			{
			}

			public BlockTransformer(ulong[] key, int hashSizeInBits)
				: this()
			{
				_hashSizeInBits = hashSizeInBits;

				_v0 = new ulong[4];
				_v1 = new ulong[4];
				_mul0 = new ulong[4];
				_mul1 = new ulong[4];

				_mul0[0] = K0;
				_mul0[1] = K1;
				_mul0[2] = K2;
				_mul0[3] = K3;

				_mul1[0] = M0;
				_mul1[1] = M1;
				_mul1[2] = M2;
				_mul1[3] = M3;

				_v0[0] = key[0] ^ _mul0[0];
				_v0[1] = key[1] ^ _mul0[1];
				_v0[2] = key[2] ^ _mul0[2];
				_v0[3] = key[3] ^ _mul0[3];

				_v1[0] = ((key[0] >> 32) | (key[0] << 32)) ^ _mul1[0];
				_v1[1] = ((key[1] >> 32) | (key[1] << 32)) ^ _mul1[1];
				_v1[2] = ((key[2] >> 32) | (key[2] << 32)) ^ _mul1[2];
				_v1[3] = ((key[3] >> 32) | (key[3] << 32)) ^ _mul1[3];
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);
				other._v0 = (ulong[])_v0.Clone();
				other._v1 = (ulong[])_v1.Clone();
				other._mul0 = (ulong[])_mul0.Clone();
				other._mul1 = (ulong[])_mul1.Clone();
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				if (data.Length != 32)
				{
					throw new ArgumentException("Data segment must be exactly 32 bytes for HighwayHash block processing. The current is " + data.Length + " bytes.");
				}

				Span<ulong> packet = stackalloc ulong[4];
				for (int i = 0; i < 4; i++)
				{
					packet[i] = BinaryPrimitives.ReadUInt64LittleEndian(data.Slice(i * 8, 8));
				}

				Update(packet);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void ZipperMergeAndAdd(ref ulong v1, ref ulong v2, ulong v3, ulong v4)
			{
				v1 += (((v4 & 0x00000000FF000000ul) | (v3 & 0x000000FF00000000ul)) >> 24)
					| (((v4 & 0x0000FF0000000000ul) | (v3 & 0x00FF000000000000ul)) >> 16)
					| (v4 & 0x0000000000FF0000ul)
					| ((v4 & 0x000000000000FF00ul) << 32)
					| ((v3 & 0xFF00000000000000ul) >> 8)
					| (v4 << 56);

				v2 += (((v3 & 0x00000000FF000000ul) | (v4 & 0x000000FF00000000ul)) >> 24)
					| (v3 & 0x0000000000FF0000ul)
					| ((v3 & 0x0000FF0000000000ul) >> 16)
					| ((v3 & 0x000000000000FF00ul) << 24)
					| ((v4 & 0x00FF000000000000ul) >> 8)
					| ((v3 & 0x00000000000000FFul) << 48)
					| (v4 & 0xFF00000000000000ul);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void Mix(ref ulong m0, ref ulong m1, ref ulong v0, ref ulong v1, ulong k)
			{
				v1 += m0 + k;
				m0 ^= (v1 & 0xFFFFFFFF) * (v0 >> 32);
				v0 += m1;
				m1 ^= (v0 & 0xFFFFFFFF) * (v1 >> 32);
			}

			private void Update(ReadOnlySpan<ulong> packet)
			{
				for (int i = 0; i < 4; ++i)
				{
					Mix(ref _mul0[i], ref _mul1[i], ref _v0[i], ref _v1[i], packet[i]);
				}

				ZipperMergeAndAdd(ref _v0[0], ref _v0[1], _v1[1], _v1[0]);
				ZipperMergeAndAdd(ref _v0[2], ref _v0[3], _v1[3], _v1[2]);
				ZipperMergeAndAdd(ref _v1[0], ref _v1[1], _v0[1], _v0[0]);
				ZipperMergeAndAdd(ref _v1[2], ref _v1[3], _v0[3], _v0[2]);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong Rotate32Wise(ulong v, int r)
			{
				r &= 31;
				uint lo = (uint)v;
				uint hi = (uint)(v >> 32);

				lo = (lo << r) | (lo >> (32 - r));
				hi = (hi << r) | (hi >> (32 - r));

				return ((ulong)hi << 32) | lo;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				ReadOnlySpan<byte> rem = leftover;
				int r = rem.Length;

				if (r > 0)
				{
					ulong remainderLength = (ulong)r;
					ulong increment = (remainderLength << 32) | remainderLength;

					for (int i = 0; i < 4; ++i)
					{
						_v0[i] += increment;
						_v1[i] = Rotate32Wise(_v1[i], r);
					}

					int r_mod_4 = r & 3;
					int r_div_4 = r & ~3;

					Span<byte> packetBytes = stackalloc byte[32];
					packetBytes.Clear();

					if (r_div_4 > 0)
					{
						rem.Slice(0, r_div_4).CopyTo(packetBytes);
					}

					if ((r & 16) != 0)
					{
						for (int i = 0; i < 4; ++i)
						{
							packetBytes[28 + i] = rem[r_div_4 + i + r_mod_4 - 4];
						}
					}
					else if (r_mod_4 > 0)
					{
						packetBytes[16] = rem[r_div_4];
						packetBytes[17] = rem[r_div_4 + (r_mod_4 >> 1)];
						packetBytes[18] = rem[r_div_4 + (r_mod_4 - 1)];
					}

					Span<ulong> packet = stackalloc ulong[4];
					for (int i = 0; i < 4; i++)
					{
						packet[i] = BinaryPrimitives.ReadUInt64LittleEndian(packetBytes.Slice(i * 8, 8));
					}
					Update(packet);
				}

				int finalizationRounds;
				switch (_hashSizeInBits)
				{
					case 64: finalizationRounds = 4; break;
					case 128: finalizationRounds = 6; break;
					default: finalizationRounds = 10; break;
				}

				for (int round = 0; round < finalizationRounds; round++)
				{
					cancellationToken.ThrowIfCancellationRequested();
					Span<ulong> permuted_v0 = new ulong[4]
					{
						(_v0[2] >> 32) | (_v0[2] << 32),
						(_v0[3] >> 32) | (_v0[3] << 32),
						(_v0[0] >> 32) | (_v0[0] << 32),
						(_v0[1] >> 32) | (_v0[1] << 32)
					};
					Update(permuted_v0);
				}

				if (_hashSizeInBits == 64)
				{
					ulong h0 = _v0[0] + _v1[0] + _mul0[0] + _mul1[0];
					Span<byte> out64 = stackalloc byte[8];
					BinaryPrimitives.WriteUInt64LittleEndian(out64, h0);
					return new HashValue(ValueEndianness.LittleEndian, out64.ToArray(), 64);
				}

				if (_hashSizeInBits == 128)
				{
					ulong h0 = _v0[0] + _mul0[0] + _v1[2] + _mul1[2];
					ulong h1 = _v0[1] + _mul0[1] + _v1[3] + _mul1[3];
					Span<byte> out128 = stackalloc byte[16];
					BinaryPrimitives.WriteUInt64LittleEndian(out128.Slice(0, 8), h0);
					BinaryPrimitives.WriteUInt64LittleEndian(out128.Slice(8, 8), h1);
					return new HashValue(ValueEndianness.LittleEndian, out128.ToArray(), 128);
				}

				// 256-bit hash
				{
					void MR(out ulong hash1, out ulong hash2, ulong v1, ulong m1, ulong v2, ulong m2, ulong v3, ulong m3, ulong v4, ulong m4)
					{
						ulong a0 = v1 + m1;
						ulong a1 = v2 + m2;
						ulong a2 = v3 + m3;
						ulong a3 = (v4 + m4) & 0x3FFFFFFFFFFFFFFFul;

						hash1 = a0 ^ (a2 << 1) ^ (a2 << 2);
						hash2 = a1 ^ ((a3 << 1) | (a2 >> 63)) ^ ((a3 << 2) | (a2 >> 62));
					}

					MR(out ulong h0, out ulong h1, _v0[0], _mul0[0], _v0[1], _mul0[1], _v1[0], _mul1[0], _v1[1], _mul1[1]);
					MR(out ulong h2, out ulong h3, _v0[2], _mul0[2], _v0[3], _mul0[3], _v1[2], _mul1[2], _v1[3], _mul1[3]);

					Span<byte> out256 = stackalloc byte[32];
					BinaryPrimitives.WriteUInt64LittleEndian(out256.Slice(0, 8), h0);
					BinaryPrimitives.WriteUInt64LittleEndian(out256.Slice(8, 8), h1);
					BinaryPrimitives.WriteUInt64LittleEndian(out256.Slice(16, 8), h2);
					BinaryPrimitives.WriteUInt64LittleEndian(out256.Slice(24, 8), h3);
					return new HashValue(ValueEndianness.LittleEndian, out256.ToArray(), 256);
				}
			}
		}
	}
}