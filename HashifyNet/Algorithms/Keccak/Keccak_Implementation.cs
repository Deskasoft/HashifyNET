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
using System.Threading;

namespace HashifyNet.Algorithms.Keccak
{
	/// <summary>
	/// Provides an implementation of the Keccak cryptographic hash function, including support for SHA-3 padding.
	/// </summary>
	/// <remarks>This class implements the Keccak-f[1600] permutation and supports configurable hash sizes and
	/// padding modes. It is designed to be used as a streamable hash function, allowing data to be processed in
	/// chunks.</remarks>
	[HashAlgorithmImplementation(typeof(IKeccak), typeof(KeccakConfig))]
	internal partial class Keccak_Implementation
		: CryptographicStreamableHashFunctionBase<IKeccakConfig>,
		  IKeccak
	{
		public override IKeccakConfig Config => _config.Clone();

		private readonly IKeccakConfig _config;
		public Keccak_Implementation(IKeccakConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			// The rate 'r' must be positive. r = 1600 - 2 * hashSize.
			// So, 1600 - 2 * hashSize > 0  =>  1600 > 2 * hashSize  =>  800 > hashSize.
			if (_config.HashSizeInBits <= 0 ||/* (_config.HashSizeInBits & (_config.HashSizeInBits - 1)) != 0 ||*/ _config.HashSizeInBits >= 800)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", config.HashSizeInBits, "Hash size must be greater than 0 and less than 800 for Keccak-f[1600].");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(_config.HashSizeInBits, _config.UseSha3Padding);
		}

		private class BlockTransformer : BlockTransformerBase<BlockTransformer>
		{
			private readonly ulong[,] _state = new ulong[5, 5];
			private readonly int _rateInBytes;
			private readonly int _hashSizeInBits;
			private readonly bool _useSha3Padding;

			public BlockTransformer() : this(512, false)
			{
			}

			public BlockTransformer(int hashSizeInBits, bool useSha3Padding) : base(GetRate(hashSizeInBits), GetRate(hashSizeInBits))
			{
				_hashSizeInBits = hashSizeInBits;
				_rateInBytes = GetRate(hashSizeInBits);
				_useSha3Padding = useSha3Padding;
				Initialize();
			}

			private static int GetRate(int hashSizeInBits)
			{
				return (1600 - (2 * hashSizeInBits)) / 8;
			}

			private void Initialize()
			{
				Array.Clear(_state, 0, 25);
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				Buffer.BlockCopy(_state, 0, other._state, 0, 25 * sizeof(ulong));
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				Absorb(data.Array, data.Offset);
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				byte[] remainder = FinalizeInputBuffer ?? Array.Empty<byte>();
				byte paddingByte = _useSha3Padding ? (byte)0x06 : (byte)0x01;

				byte[] padded = new byte[_rateInBytes];
				Buffer.BlockCopy(remainder, 0, padded, 0, remainder.Length);
				padded[remainder.Length] = paddingByte;
				padded[_rateInBytes - 1] |= 0x80;

				Absorb(padded, 0);

				int bytesToSqueeze = (_hashSizeInBits + 7) / 8;
				byte[] hash = Squeeze(bytesToSqueeze);

				return new HashValue(hash, _hashSizeInBits);
			}

			private void Absorb(byte[] data, int offset)
			{
				int byteIndex = 0;
				for (int y = 0; y < 5; y++)
				{
					for (int x = 0; x < 5; x++)
					{
						if (byteIndex < _rateInBytes)
						{
							_state[x, y] ^= BitConverter.ToUInt64(data, offset + byteIndex);
							byteIndex += 8;
						}
						else
						{
							goto permute;
						}
					}
				}

				permute:
				KeccakF1600_Permute();
			}

			private byte[] Squeeze(int lengthInBytes)
			{
				byte[] output = new byte[lengthInBytes];
				int outputOffset = 0;
				int bytesToCopy;

				while (outputOffset < lengthInBytes)
				{
					bytesToCopy = Math.Min(_rateInBytes, lengthInBytes - outputOffset);
					byte[] stateBytes = new byte[_rateInBytes];
					int stateByteIndex = 0;

					for (int y = 0; y < 5; y++)
					{
						for (int x = 0; x < 5; x++)
						{
							if (stateByteIndex < _rateInBytes)
							{
								byte[] laneBytes = BitConverter.GetBytes(_state[x, y]);
								Buffer.BlockCopy(laneBytes, 0, stateBytes, stateByteIndex, 8);
								stateByteIndex += 8;
							}
						}
					}

					Buffer.BlockCopy(stateBytes, 0, output, outputOffset, bytesToCopy);
					outputOffset += bytesToCopy;

					if (outputOffset < lengthInBytes)
					{
						KeccakF1600_Permute();
					}
				}
				return output;
			}

			#region Keccak-f[1600] Permutation

			private static readonly ulong[] RoundConstants = {
				0x0000000000000001UL, 0x0000000000008082UL, 0x800000000000808aUL,
				0x8000000080008000UL, 0x000000000000808bUL, 0x0000000080000001UL,
				0x8000000080008081UL, 0x8000000000008009UL, 0x000000000000008aUL,
				0x0000000000000088UL, 0x0000000080008009UL, 0x000000008000000aUL,
				0x000000008000808bUL, 0x800000000000008bUL, 0x8000000000008089UL,
				0x8000000000008003UL, 0x8000000000008002UL, 0x8000000000000080UL,
				0x000000000000800aUL, 0x800000008000000aUL, 0x8000000080008081UL,
				0x8000000000008080UL, 0x0000000080000001UL, 0x8000000080008008UL
			};

			private static ulong RotL64(ulong x, int y) => (x << y) | (x >> (64 - y));

			private void KeccakF1600_Permute()
			{
				for (int round = 0; round < 24; round++)
				{
					// Theta step
					var C = new ulong[5];
					for (int x = 0; x < 5; x++)
					{
						C[x] = _state[x, 0] ^ _state[x, 1] ^ _state[x, 2] ^ _state[x, 3] ^ _state[x, 4];
					}

					var D = new ulong[5];
					for (int x = 0; x < 5; x++)
					{
						D[x] = C[(x + 4) % 5] ^ RotL64(C[(x + 1) % 5], 1);
					}

					for (int x = 0; x < 5; x++)
					{
						for (int y = 0; y < 5; y++)
						{
							_state[x, y] ^= D[x];
						}
					}

					// Rho and Pi steps
					var B = new ulong[5, 5];
					for (int x = 0; x < 5; x++)
					{
						for (int y = 0; y < 5; y++)
						{
							B[y, ((2 * x) + (3 * y)) % 5] = RotL64(_state[x, y], KeccakRhoOffsets[x, y]);
						}
					}

					// Chi step
					for (int x = 0; x < 5; x++)
					{
						for (int y = 0; y < 5; y++)
						{
							_state[x, y] = B[x, y] ^ ((~B[(x + 1) % 5, y]) & B[(x + 2) % 5, y]);
						}
					}

					// Iota step
					_state[0, 0] ^= RoundConstants[round];
				}
			}

			private static readonly int[,] KeccakRhoOffsets = {
				{0, 36, 3, 41, 18},
				{1, 44, 10, 45, 2},
				{62, 6, 43, 15, 61},
				{28, 55, 25, 21, 56},
				{27, 20, 39, 8, 14}
			};

			#endregion
		}
	}
}