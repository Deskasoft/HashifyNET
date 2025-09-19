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
using System.Runtime.CompilerServices;
using System.Threading;

namespace HashifyNet.Algorithms.SipHash
{
	/// <summary>
	/// Implementation of the SipHash keyed hash function.
	/// SipHash is designed for performance and security against hash-flooding attacks.
	/// It is not a general-purpose cryptographic hash function.
	/// </summary>
	[HashAlgorithmImplementation(typeof(ISipHash), typeof(SipHashConfig))]
	internal partial class SipHash_Implementation
		: CryptographicStreamableHashFunctionBase<ISipHashConfig>,
		  ISipHash
	{
		public override ISipHashConfig Config => _config.Clone();

		private readonly ISipHashConfig _config;
		private readonly ulong _k0;
		private readonly ulong _k1;

		public SipHash_Implementation(ISipHashConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits != 64)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64 bits.");
			}

			if (_config.Key == null)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _config.Key, $"{nameof(config)}.{nameof(config.Key)} must not be null.");
			}

			if (_config.Key.Count != 16)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _config.Key, $"{nameof(config)}.{nameof(config.Key)} must be fixed at 16 bytes.");
			}

			// Extract key halves as little-endian ulongs
			_k0 = Endianness.ToUInt64LittleEndian(_config.Key as byte[] ?? new List<byte>(_config.Key).ToArray(), 0);
			_k1 = Endianness.ToUInt64LittleEndian(_config.Key as byte[] ?? new List<byte>(_config.Key).ToArray(), 8);
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(_k0, _k1, _config.C_Rounds, _config.D_Rounds);
		}

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private readonly int _cRounds;
			private readonly int _dRounds;

			// Internal state
			private ulong _v0, _v1, _v2, _v3;
			private ulong _messageLength;

			public BlockTransformer()
				: base(inputBlockSize: 8) // Process data in 8-byte blocks
			{
			}

			public BlockTransformer(ulong k0, ulong k1, int cRounds, int dRounds)
				: this()
			{
				_cRounds = cRounds;
				_dRounds = dRounds;
				_messageLength = 0;

				// Initialize state with key and magic constants
				_v0 = k0 ^ 0x736f6d6570736575;
				_v1 = k1 ^ 0x646f72616e646f6d;
				_v2 = k0 ^ 0x6c7967656e657261;
				_v3 = k1 ^ 0x7465646279746573;
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);
				other._v0 = _v0;
				other._v1 = _v1;
				other._v2 = _v2;
				other._v3 = _v3;
				other._messageLength = _messageLength;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				if (data.Length != 8)
				{
					throw new InvalidOperationException("Expected 8 bytes per byte group.");
				}

				_messageLength += (ulong)data.Length;
				ulong m = Endianness.ToUInt64LittleEndian(data, 0);

				_v3 ^= m;
				for (int i = 0; i < _cRounds; ++i)
				{
					SipRound();
				}
				_v0 ^= m;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				int remainderCount = leftover.Length;
				_messageLength += (ulong)remainderCount;

				// Pad the final block with zeros and append message length
				ulong finalBlock = _messageLength << 56;
				for (int i = 0; i < remainderCount; ++i)
				{
					finalBlock |= (ulong)leftover[i] << (i * 8);
				}

				cancellationToken.ThrowIfCancellationRequested();

				// Process the final block
				_v3 ^= finalBlock;
				for (int i = 0; i < _cRounds; ++i)
				{
					SipRound();
				}
				_v0 ^= finalBlock;

				// Finalization
				_v2 ^= 0xff;
				for (int i = 0; i < _dRounds; ++i)
				{
					SipRound();
				}

				ulong finalHash = _v0 ^ _v1 ^ _v2 ^ _v3;
				var hashValueBytes = Endianness.GetBytesLittleEndian(finalHash);

				return new HashValue(ValueEndianness.LittleEndian, hashValueBytes, 64);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void SipRound()
			{
				_v0 += _v1;
				_v1 = RotateLeft(_v1, 13);
				_v1 ^= _v0;
				_v0 = RotateLeft(_v0, 32);

				_v2 += _v3;
				_v3 = RotateLeft(_v3, 16);
				_v3 ^= _v2;

				_v0 += _v3;
				_v3 = RotateLeft(_v3, 21);
				_v3 ^= _v0;

				_v2 += _v1;
				_v1 = RotateLeft(_v1, 17);
				_v1 ^= _v2;
				_v2 = RotateLeft(_v2, 32);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static ulong RotateLeft(ulong value, int bits)
			{
				return (value << bits) | (value >> (64 - bits));
			}
		}
	}
}