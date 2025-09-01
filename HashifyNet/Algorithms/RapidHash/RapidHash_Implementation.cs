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
using System.Runtime.CompilerServices;
using System.Threading;

namespace HashifyNet.Algorithms.RapidHash
{
	/// <summary>
	/// Provides an implementation of the <see cref="IRapidHash"/> interface, representing a 64-bit hash function optimized
	/// for high performance and streaming scenarios.
	/// </summary>
	/// <remarks>This class is designed to compute 64-bit hash values using a configurable seed value. It enforces
	/// that the hash size is fixed at 64 bits, and any attempt to configure a different hash size will result in an
	/// exception. The implementation supports streaming input data and is optimized for processing large data blocks
	/// efficiently.</remarks>
	[HashAlgorithmImplementation(typeof(IRapidHash), typeof(RapidHashConfig))]
	internal class RapidHash_Implementation
		: StreamableHashFunctionBase<IRapidHashConfig>,
			IRapidHash
	{
		public override IRapidHashConfig Config => _config.Clone();

		private readonly IRapidHashConfig _config;
		public RapidHash_Implementation(IRapidHashConfig config)
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
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			switch (_config.Mode)
			{
				case RapidHashMode.Original:
					return new BlockTransformer_MOriginal(_config.Seed);
				case RapidHashMode.Micro:
					return new BlockTransformer_MMicro(_config.Seed);
				case RapidHashMode.Nano:
					return new BlockTransformer_MNano(_config.Seed);
				default:
					throw new NotImplementedException();
			}
		}

		private static class RapidHashShared
		{
			public static readonly ulong[] Secret = new ulong[8] {
				0x2d358dccaa6c78a5ul, 0x8bb84b93962eacc9ul, 0x4b33a62ed433d4a3ul, 0x4d5a2da51de1aa47ul,
				0xa0761d6478bd642ful, 0xe7037ed1a0b428dbul, 0x90ed1765281c388cul, 0xaaaaaaaaaaaaaaaaul
			};

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void RapidMum(ref ulong a, ref ulong b)
			{
				ulong ha = a >> 32, hb = b >> 32, la = (uint)a, lb = (uint)b;
				ulong rh = ha * hb, rm0 = ha * lb, rm1 = hb * la, rl = la * lb, t = rl + (rm0 << 32);
				ulong c = t < rl ? 1UL : 0UL;
				ulong lo = t + (rm1 << 32);
				c += lo < t ? 1UL : 0UL;

				ulong hi = rh + (rm0 >> 32) + (rm1 >> 32) + c;

#if RAPIDHASH_PROTECTED
    a ^= lo;
    b ^= hi;
#else
				a = lo;
				b = hi;
#endif
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ulong RapidMix(ulong a, ulong b)
			{
				RapidMum(ref a, ref b);
				return a ^ b;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ulong RapidRead64(byte[] p, int offset)
			{
				return Endianness.ToUInt64LittleEndian(p, offset);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint RapidRead32(byte[] p, int offset)
			{
				return Endianness.ToUInt32LittleEndian(p, offset);
			}
		}

		private class BlockTransformer_MMicro
	: BlockTransformerBase<BlockTransformer_MMicro>
		{
			private const int RAPID_BLOCK_SIZE = 80;
			private ulong _seed;
			private ulong _see1, _see2, _see3, _see4;
			private ulong _totalLength;
			private readonly byte[] _tailBuffer = new byte[16];
			private bool _hasProcessedBlocks;
			private readonly byte[] _pendingBlock = new byte[RAPID_BLOCK_SIZE];
			private bool _hasPendingBlock;

			public BlockTransformer_MMicro() : base(inputBlockSize: RAPID_BLOCK_SIZE)
			{
			}

			public BlockTransformer_MMicro(ulong seed) : this()
			{
				_totalLength = 0;
				_hasProcessedBlocks = false;

				_seed = seed;
				_seed ^= RapidHashShared.RapidMix(_seed ^ RapidHashShared.Secret[2], RapidHashShared.Secret[1]);

				_see1 = _seed;
				_see2 = _seed;
				_see3 = _seed;
				_see4 = _seed;
			}

			protected override void CopyStateTo(BlockTransformer_MMicro other)
			{
				base.CopyStateTo(other);

				other._seed = _seed;
				other._see1 = _see1;
				other._see2 = _see2;
				other._see3 = _see3;
				other._see4 = _see4;
				other._totalLength = _totalLength;
				other._hasProcessedBlocks = _hasProcessedBlocks;

				Buffer.BlockCopy(_tailBuffer, 0, other._tailBuffer, 0, 16);

				other._hasPendingBlock = _hasPendingBlock;
				if (_hasPendingBlock)
					Buffer.BlockCopy(_pendingBlock, 0, other._pendingBlock, 0, RAPID_BLOCK_SIZE);
			}

			private void ProcessBlock(byte[] p, int offset)
			{
				Buffer.BlockCopy(p, offset + RAPID_BLOCK_SIZE - 16, _tailBuffer, 0, 16);
				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(
					RapidHashShared.RapidRead64(p, offset) ^ RapidHashShared.Secret[0],
					RapidHashShared.RapidRead64(p, offset + 8) ^ _seed);

				_see1 = RapidHashShared.RapidMix(
					RapidHashShared.RapidRead64(p, offset + 16) ^ RapidHashShared.Secret[1],
					RapidHashShared.RapidRead64(p, offset + 24) ^ _see1);

				_see2 = RapidHashShared.RapidMix(
					RapidHashShared.RapidRead64(p, offset + 32) ^ RapidHashShared.Secret[2],
					RapidHashShared.RapidRead64(p, offset + 40) ^ _see2);

				_see3 = RapidHashShared.RapidMix(
					RapidHashShared.RapidRead64(p, offset + 48) ^ RapidHashShared.Secret[3],
					RapidHashShared.RapidRead64(p, offset + 56) ^ _see3);

				_see4 = RapidHashShared.RapidMix(
					RapidHashShared.RapidRead64(p, offset + 64) ^ RapidHashShared.Secret[4],
					RapidHashShared.RapidRead64(p, offset + 72) ^ _see4);
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				if (!_hasPendingBlock)
				{
					Buffer.BlockCopy(data.Array, data.Offset, _pendingBlock, 0, RAPID_BLOCK_SIZE);
					_hasPendingBlock = true;
				}
				else
				{
					ProcessBlock(_pendingBlock, 0);
					Buffer.BlockCopy(data.Array, data.Offset, _pendingBlock, 0, RAPID_BLOCK_SIZE);
				}

				_totalLength += (ulong)data.Count;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var tailSeg = new ArraySegment<byte>(FinalizeInputBuffer ?? Array.Empty<byte>());
				_totalLength += (ulong)tailSeg.Count;
				byte[] remainderArray;
				int pOffset;
				int i;

				if (tailSeg.Count > 0)
				{
					if (_hasPendingBlock)
					{
						ProcessBlock(_pendingBlock, 0);
						_hasPendingBlock = false;
					}

					remainderArray = tailSeg.Array ?? Array.Empty<byte>();
					pOffset = tailSeg.Offset;
					i = tailSeg.Count;
				}
				else if (_hasPendingBlock)
				{
					remainderArray = _pendingBlock;
					pOffset = 0;
					i = RAPID_BLOCK_SIZE; // 80
										  // Do NOT call ProcessBlock on this pending block; it's the "i=80" tail.
				}
				else
				{
					remainderArray = Array.Empty<byte>();
					pOffset = 0;
					i = 0;
				}

				ulong a = 0, b = 0;
				ulong length_to_mix;

				if (_totalLength <= 16)
				{
					length_to_mix = _totalLength;
					if (_totalLength >= 4)
					{
						_seed ^= _totalLength;
						if (_totalLength >= 8)
						{
							a = RapidHashShared.RapidRead64(remainderArray, pOffset);
							b = RapidHashShared.RapidRead64(remainderArray, pOffset + i - 8);
						}
						else
						{
							a = RapidHashShared.RapidRead32(remainderArray, pOffset);
							b = RapidHashShared.RapidRead32(remainderArray, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)remainderArray[pOffset] << 45) | remainderArray[pOffset + i - 1];
						b = remainderArray[pOffset + (i >> 1)];
					}
				}
				else
				{
					length_to_mix = (ulong)i;

					if (_totalLength > RAPID_BLOCK_SIZE)
					{
						_seed ^= _see1;
						_see2 ^= _see3;
						_seed ^= _see4;
						_seed ^= _see2;
					}

					if (i > 16)
					{
						_seed = RapidHashShared.RapidMix(
							RapidHashShared.RapidRead64(remainderArray, pOffset) ^ RapidHashShared.Secret[2],
							RapidHashShared.RapidRead64(remainderArray, pOffset + 8) ^ _seed);

						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(
								RapidHashShared.RapidRead64(remainderArray, pOffset + 16) ^ RapidHashShared.Secret[2],
								RapidHashShared.RapidRead64(remainderArray, pOffset + 24) ^ _seed);

							if (i > 48)
							{
								_seed = RapidHashShared.RapidMix(
									RapidHashShared.RapidRead64(remainderArray, pOffset + 32) ^ RapidHashShared.Secret[1],
									RapidHashShared.RapidRead64(remainderArray, pOffset + 40) ^ _seed);

								if (i > 64)
								{
									_seed = RapidHashShared.RapidMix(
										RapidHashShared.RapidRead64(remainderArray, pOffset + 48) ^ RapidHashShared.Secret[1],
										RapidHashShared.RapidRead64(remainderArray, pOffset + 56) ^ _seed);
								}
							}
						}
					}

					byte[] last16Bytes = new byte[16];
					if (i >= 16)
					{
						Buffer.BlockCopy(remainderArray, pOffset + i - 16, last16Bytes, 0, 16);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							Buffer.BlockCopy(_tailBuffer, 16 - from_tail, last16Bytes, 0, from_tail);
							if (i > 0) Buffer.BlockCopy(remainderArray, pOffset, last16Bytes, from_tail, i);
						}
						else
						{
							Buffer.BlockCopy(remainderArray, pOffset, last16Bytes, 16 - i, i);
						}
					}

					a = RapidHashShared.RapidRead64(last16Bytes, 0) ^ length_to_mix;
					b = RapidHashShared.RapidRead64(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(
					a ^ RapidHashShared.Secret[7],
					b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);
				return new HashValue(result, 64);
			}
		}

		private class BlockTransformer_MNano
			: BlockTransformerBase<BlockTransformer_MNano>
		{
			private const int RAPID_BLOCK_SIZE = 48;
			private ulong _seed;
			private ulong _see1, _see2;
			private ulong _totalLength;
			private readonly byte[] _tailBuffer = new byte[16];
			private bool _hasProcessedBlocks;

			public BlockTransformer_MNano() : base(inputBlockSize: RAPID_BLOCK_SIZE)
			{
			}

			public BlockTransformer_MNano(ulong seed) : this()
			{
				_totalLength = 0;
				_hasProcessedBlocks = false;

				_seed = seed;
				_seed ^= RapidHashShared.RapidMix(_seed ^ RapidHashShared.Secret[2], RapidHashShared.Secret[1]);

				_see1 = _seed;
				_see2 = _seed;
			}

			protected override void CopyStateTo(BlockTransformer_MNano other)
			{
				base.CopyStateTo(other);

				other._seed = _seed;
				other._see1 = _see1;
				other._see2 = _see2;
				other._totalLength = _totalLength;
				other._hasProcessedBlocks = _hasProcessedBlocks;
				Buffer.BlockCopy(_tailBuffer, 0, other._tailBuffer, 0, 16);
			}

			private void ProcessBlock(byte[] p, int offset)
			{
				Buffer.BlockCopy(p, offset + RAPID_BLOCK_SIZE - 16, _tailBuffer, 0, 16);
				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset) ^ RapidHashShared.Secret[0], RapidHashShared.RapidRead64(p, offset + 8) ^ _seed);
				_see1 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 16) ^ RapidHashShared.Secret[1], RapidHashShared.RapidRead64(p, offset + 24) ^ _see1);
				_see2 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 32) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(p, offset + 40) ^ _see2);
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				ProcessBlock(data.Array, data.Offset);
				_totalLength += (ulong)data.Count;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				ArraySegment<byte> remainder = new ArraySegment<byte>(FinalizeInputBuffer ?? new byte[0]);
				_totalLength += (ulong)remainder.Count;

				ulong a = 0, b = 0;
				int pOffset = remainder.Offset;
				int i = remainder.Count;
				ulong length_to_mix;

				if (_totalLength <= 16)
				{
					length_to_mix = _totalLength;
					if (_totalLength >= 4)
					{
						_seed ^= _totalLength;
						if (_totalLength >= 8)
						{
							a = RapidHashShared.RapidRead64(remainder.Array, pOffset);
							b = RapidHashShared.RapidRead64(remainder.Array, pOffset + i - 8);
						}
						else
						{
							a = RapidHashShared.RapidRead32(remainder.Array, pOffset);
							b = RapidHashShared.RapidRead32(remainder.Array, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)remainder.Array[pOffset] << 45) | remainder.Array[pOffset + i - 1];
						b = remainder.Array[pOffset + (i >> 1)];
					}
				}
				else
				{
					length_to_mix = (ulong)i;
					if (_totalLength > RAPID_BLOCK_SIZE)
					{
						_seed ^= _see1;
						_seed ^= _see2;
					}

					if (i > 16)
					{
						_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(remainder.Array, pOffset + 8) ^ _seed);
						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 16) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(remainder.Array, pOffset + 24) ^ _seed);
						}
					}

					byte[] last16Bytes = new byte[16];
					if (i >= 16)
					{
						Buffer.BlockCopy(remainder.Array, pOffset + i - 16, last16Bytes, 0, 16);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							Buffer.BlockCopy(_tailBuffer, 16 - from_tail, last16Bytes, 0, from_tail);
							if (i > 0) Buffer.BlockCopy(remainder.Array, pOffset, last16Bytes, from_tail, i);
						}
						else
						{
							Buffer.BlockCopy(remainder.Array, pOffset, last16Bytes, 16 - i, i);
						}
					}

					a = RapidHashShared.RapidRead64(last16Bytes, 0) ^ (ulong)i;
					b = RapidHashShared.RapidRead64(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(a ^ RapidHashShared.Secret[7], b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);

				return new HashValue(result, 64);
			}
		}

		private class BlockTransformer_MOriginal
	  : BlockTransformerBase<BlockTransformer_MOriginal>
		{
			private const int RAPID_BLOCK_SIZE = 112;
			private ulong _seed;
			private ulong _see1, _see2, _see3, _see4, _see5, _see6;
			private ulong _totalLength;
			private readonly byte[] _tailBuffer = new byte[16];
			private bool _hasProcessedBlocks;

			public BlockTransformer_MOriginal() : base(inputBlockSize: RAPID_BLOCK_SIZE)
			{
			}

			public BlockTransformer_MOriginal(ulong seed) : this()
			{
				_totalLength = 0;
				_hasProcessedBlocks = false;

				_seed = seed;
				_seed ^= RapidHashShared.RapidMix(_seed ^ RapidHashShared.Secret[2], RapidHashShared.Secret[1]);

				_see1 = _seed;
				_see2 = _seed;
				_see3 = _seed;
				_see4 = _seed;
				_see5 = _seed;
				_see6 = _seed;
			}

			protected override void CopyStateTo(BlockTransformer_MOriginal other)
			{
				base.CopyStateTo(other);

				other._seed = _seed;
				other._see1 = _see1;
				other._see2 = _see2;
				other._see3 = _see3;
				other._see4 = _see4;
				other._see5 = _see5;
				other._see6 = _see6;
				other._totalLength = _totalLength;
				other._hasProcessedBlocks = _hasProcessedBlocks;
				Buffer.BlockCopy(_tailBuffer, 0, other._tailBuffer, 0, 16);
			}

			private void ProcessBlock(byte[] p, int offset)
			{
				Buffer.BlockCopy(p, offset + RAPID_BLOCK_SIZE - 16, _tailBuffer, 0, 16);
				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset) ^ RapidHashShared.Secret[0], RapidHashShared.RapidRead64(p, offset + 8) ^ _seed);
				_see1 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 16) ^ RapidHashShared.Secret[1], RapidHashShared.RapidRead64(p, offset + 24) ^ _see1);
				_see2 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 32) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(p, offset + 40) ^ _see2);
				_see3 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 48) ^ RapidHashShared.Secret[3], RapidHashShared.RapidRead64(p, offset + 56) ^ _see3);
				_see4 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 64) ^ RapidHashShared.Secret[4], RapidHashShared.RapidRead64(p, offset + 72) ^ _see4);
				_see5 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 80) ^ RapidHashShared.Secret[5], RapidHashShared.RapidRead64(p, offset + 88) ^ _see5);
				_see6 = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(p, offset + 96) ^ RapidHashShared.Secret[6], RapidHashShared.RapidRead64(p, offset + 104) ^ _see6);
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				ProcessBlock(data.Array, data.Offset);
				_totalLength += (ulong)data.Count;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				ArraySegment<byte> remainder = new ArraySegment<byte>(FinalizeInputBuffer ?? new byte[0]);
				_totalLength += (ulong)remainder.Count;

				ulong a = 0, b = 0;
				int pOffset = remainder.Offset;
				int i = remainder.Count;
				ulong length_to_mix;

				if (_totalLength <= 16)
				{
					length_to_mix = _totalLength;
					if (_totalLength >= 4)
					{
						_seed ^= _totalLength;
						if (_totalLength >= 8)
						{
							a = RapidHashShared.RapidRead64(remainder.Array, pOffset);
							b = RapidHashShared.RapidRead64(remainder.Array, pOffset + i - 8);
						}
						else
						{
							a = RapidHashShared.RapidRead32(remainder.Array, pOffset);
							b = RapidHashShared.RapidRead32(remainder.Array, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)remainder.Array[pOffset] << 45) | remainder.Array[pOffset + i - 1];
						b = remainder.Array[pOffset + (i >> 1)];
					}
				}
				else
				{
					length_to_mix = (ulong)i;
					if (_totalLength > 112)
					{
						_seed ^= _see1;
						_see2 ^= _see3;
						_see4 ^= _see5;
						_seed ^= _see6;
						_see2 ^= _see4;
						_seed ^= _see2;
					}

					if (i > 16)
					{
						_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(remainder.Array, pOffset + 8) ^ _seed);
						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 16) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(remainder.Array, pOffset + 24) ^ _seed);
							if (i > 48)
							{
								_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 32) ^ RapidHashShared.Secret[1], RapidHashShared.RapidRead64(remainder.Array, pOffset + 40) ^ _seed);
								if (i > 64)
								{
									_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 48) ^ RapidHashShared.Secret[1], RapidHashShared.RapidRead64(remainder.Array, pOffset + 56) ^ _seed);
									if (i > 80)
									{
										_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 64) ^ RapidHashShared.Secret[2], RapidHashShared.RapidRead64(remainder.Array, pOffset + 72) ^ _seed);
										if (i > 96)
										{
											_seed = RapidHashShared.RapidMix(RapidHashShared.RapidRead64(remainder.Array, pOffset + 80) ^ RapidHashShared.Secret[1], RapidHashShared.RapidRead64(remainder.Array, pOffset + 88) ^ _seed);
										}
									}
								}
							}
						}
					}

					byte[] last16Bytes = new byte[16];
					if (i >= 16)
					{
						Buffer.BlockCopy(remainder.Array, pOffset + i - 16, last16Bytes, 0, 16);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							Buffer.BlockCopy(_tailBuffer, 16 - from_tail, last16Bytes, 0, from_tail);
							if (i > 0) Buffer.BlockCopy(remainder.Array, pOffset, last16Bytes, from_tail, i);
						}
						else
						{
							// This branch is unreachable if _totalLength > 16, but kept for safety.
							Buffer.BlockCopy(remainder.Array, pOffset, last16Bytes, 16 - i, i);
						}
					}

					a = RapidHashShared.RapidRead64(last16Bytes, 0) ^ (ulong)i;
					b = RapidHashShared.RapidRead64(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(a ^ RapidHashShared.Secret[7], b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);

				return new HashValue(result, 64);
			}
		}
	}
}