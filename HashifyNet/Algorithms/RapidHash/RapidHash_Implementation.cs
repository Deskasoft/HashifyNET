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
					return new BlockTransformer_MOriginal((ulong)_config.Seed);
				case RapidHashMode.Micro:
					return new BlockTransformer_MMicro((ulong)_config.Seed);
				case RapidHashMode.Nano:
					return new BlockTransformer_MNano((ulong)_config.Seed);
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
				{
					Buffer.BlockCopy(_pendingBlock, 0, other._pendingBlock, 0, RAPID_BLOCK_SIZE);
				}
			}

			private void ProcessBlock(byte[] p, int offset)
			{
				Buffer.BlockCopy(p, offset + RAPID_BLOCK_SIZE - 16, _tailBuffer, 0, 16);
				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(
					Endianness.ToUInt64LittleEndian(p, offset) ^ RapidHashShared.Secret[0],
					Endianness.ToUInt64LittleEndian(p, offset + 8) ^ _seed);

				_see1 = RapidHashShared.RapidMix(
					Endianness.ToUInt64LittleEndian(p, offset + 16) ^ RapidHashShared.Secret[1],
					Endianness.ToUInt64LittleEndian(p, offset + 24) ^ _see1);

				_see2 = RapidHashShared.RapidMix(
					Endianness.ToUInt64LittleEndian(p, offset + 32) ^ RapidHashShared.Secret[2],
					Endianness.ToUInt64LittleEndian(p, offset + 40) ^ _see2);

				_see3 = RapidHashShared.RapidMix(
					Endianness.ToUInt64LittleEndian(p, offset + 48) ^ RapidHashShared.Secret[3],
					Endianness.ToUInt64LittleEndian(p, offset + 56) ^ _see3);

				_see4 = RapidHashShared.RapidMix(
					Endianness.ToUInt64LittleEndian(p, offset + 64) ^ RapidHashShared.Secret[4],
					Endianness.ToUInt64LittleEndian(p, offset + 72) ^ _see4);
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				Span<byte> pendingBlock = _pendingBlock.AsSpan();
				if (!_hasPendingBlock)
				{
					data.CopyTo(pendingBlock);
					_hasPendingBlock = true;
				}
				else
				{
					ProcessBlock(_pendingBlock, 0);
					data.CopyTo(pendingBlock);
				}

				_totalLength += (ulong)data.Length;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				_totalLength += (ulong)leftover.Length;
				ReadOnlySpan<byte> remainder;
				int pOffset;
				int i;

				if (leftover.Length > 0)
				{
					if (_hasPendingBlock)
					{
						ProcessBlock(_pendingBlock, 0);
						_hasPendingBlock = false;
					}

					remainder = leftover;
					pOffset = 0;
					i = leftover.Length;
				}
				else if (_hasPendingBlock)
				{
					remainder = _pendingBlock.AsSpan();
					pOffset = 0;
					i = RAPID_BLOCK_SIZE; // 80
										  // Do NOT call ProcessBlock on this pending block; it's the "i=80" tail.
				}
				else
				{
					remainder = ReadOnlySpan<byte>.Empty;
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
							a = Endianness.ToUInt64LittleEndian(remainder, pOffset);
							b = Endianness.ToUInt64LittleEndian(remainder, pOffset + i - 8);
						}
						else
						{
							a = Endianness.ToUInt32LittleEndian(remainder, pOffset);
							b = Endianness.ToUInt32LittleEndian(remainder, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)remainder[pOffset] << 45) | remainder[pOffset + i - 1];
						b = remainder[pOffset + (i >> 1)];
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
							Endianness.ToUInt64LittleEndian(remainder, pOffset) ^ RapidHashShared.Secret[2],
							Endianness.ToUInt64LittleEndian(remainder, pOffset + 8) ^ _seed);

						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(
								Endianness.ToUInt64LittleEndian(remainder, pOffset + 16) ^ RapidHashShared.Secret[2],
								Endianness.ToUInt64LittleEndian(remainder, pOffset + 24) ^ _seed);

							if (i > 48)
							{
								_seed = RapidHashShared.RapidMix(
									Endianness.ToUInt64LittleEndian(remainder, pOffset + 32) ^ RapidHashShared.Secret[1],
									Endianness.ToUInt64LittleEndian(remainder, pOffset + 40) ^ _seed);

								if (i > 64)
								{
									_seed = RapidHashShared.RapidMix(
										Endianness.ToUInt64LittleEndian(remainder, pOffset + 48) ^ RapidHashShared.Secret[1],
										Endianness.ToUInt64LittleEndian(remainder, pOffset + 56) ^ _seed);
								}
							}
						}
					}

					Span<byte> last16Bytes = stackalloc byte[16];
					if (i >= 16)
					{
						remainder.Slice(pOffset + i - 16, 16).CopyTo(last16Bytes);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							_tailBuffer.AsSpan(16 - from_tail, from_tail).CopyTo(last16Bytes);
							if (i > 0)
							{
								remainder.Slice(pOffset, i).CopyTo(last16Bytes.Slice(from_tail, i));
							}
						}
						else
						{
							remainder.Slice(pOffset, i).CopyTo(last16Bytes.Slice(16 - i, i));
						}
					}

					a = Endianness.ToUInt64LittleEndian(last16Bytes, 0) ^ length_to_mix;
					b = Endianness.ToUInt64LittleEndian(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(
					a ^ RapidHashShared.Secret[7],
					b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);
				return new HashValue(ValueEndianness.LittleEndian, result, 64);
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

			private void ProcessBlock(ReadOnlySpan<byte> p)
			{
				Span<byte> tailBuffer = _tailBuffer.AsSpan();
				p.Slice(RAPID_BLOCK_SIZE - 16, 16).CopyTo(tailBuffer);

				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 0) ^ RapidHashShared.Secret[0], Endianness.ToUInt64LittleEndian(p, 8) ^ _seed);
				_see1 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 16) ^ RapidHashShared.Secret[1], Endianness.ToUInt64LittleEndian(p, 24) ^ _see1);
				_see2 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 32) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(p, 40) ^ _see2);
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				ProcessBlock(data);
				_totalLength += (ulong)data.Length;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				_totalLength += (ulong)leftover.Length;

				ulong a = 0, b = 0;
				int pOffset = 0;
				int i = leftover.Length;
				ulong length_to_mix;

				if (_totalLength <= 16)
				{
					length_to_mix = _totalLength;
					if (_totalLength >= 4)
					{
						_seed ^= _totalLength;
						if (_totalLength >= 8)
						{
							a = Endianness.ToUInt64LittleEndian(leftover, pOffset);
							b = Endianness.ToUInt64LittleEndian(leftover, pOffset + i - 8);
						}
						else
						{
							a = Endianness.ToUInt32LittleEndian(leftover, pOffset);
							b = Endianness.ToUInt32LittleEndian(leftover, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)leftover[pOffset] << 45) | leftover[pOffset + i - 1];
						b = leftover[pOffset + (i >> 1)];
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
						_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(leftover, pOffset + 8) ^ _seed);
						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 16) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(leftover, pOffset + 24) ^ _seed);
						}
					}

					Span<byte> last16Bytes = stackalloc byte[16];
					if (i >= 16)
					{
						leftover.Slice(pOffset + i - 16, 16).CopyTo(last16Bytes);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							_tailBuffer.AsSpan(16 - from_tail, from_tail).CopyTo(last16Bytes);
							if (i > 0)
							{
								leftover.Slice(pOffset, i).CopyTo(last16Bytes.Slice(from_tail, i));
							}
						}
						else
						{
							leftover.Slice(pOffset, i).CopyTo(last16Bytes.Slice(16 - i, i));
						}
					}

					a = Endianness.ToUInt64LittleEndian(last16Bytes, 0) ^ (ulong)i;
					b = Endianness.ToUInt64LittleEndian(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(a ^ RapidHashShared.Secret[7], b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);

				return new HashValue(ValueEndianness.LittleEndian, result, 64);
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

			private void ProcessBlock(ReadOnlySpan<byte> p)
			{
				Span<byte> tailBuffer = _tailBuffer.AsSpan();
				p.Slice(RAPID_BLOCK_SIZE - 16, 16).CopyTo(tailBuffer);

				_hasProcessedBlocks = true;

				_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 0) ^ RapidHashShared.Secret[0], Endianness.ToUInt64LittleEndian(p, 8) ^ _seed);
				_see1 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 16) ^ RapidHashShared.Secret[1], Endianness.ToUInt64LittleEndian(p, 24) ^ _see1);
				_see2 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 32) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(p, 40) ^ _see2);
				_see3 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 48) ^ RapidHashShared.Secret[3], Endianness.ToUInt64LittleEndian(p, 56) ^ _see3);
				_see4 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 64) ^ RapidHashShared.Secret[4], Endianness.ToUInt64LittleEndian(p, 72) ^ _see4);
				_see5 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 80) ^ RapidHashShared.Secret[5], Endianness.ToUInt64LittleEndian(p, 88) ^ _see5);
				_see6 = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(p, 96) ^ RapidHashShared.Secret[6], Endianness.ToUInt64LittleEndian(p, 104) ^ _see6);
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				ProcessBlock(data);
				_totalLength += (ulong)data.Length;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				_totalLength += (ulong)leftover.Length;

				ulong a = 0, b = 0;
				int pOffset = 0;
				int i = leftover.Length;
				ulong length_to_mix;

				if (_totalLength <= 16)
				{
					length_to_mix = _totalLength;
					if (_totalLength >= 4)
					{
						_seed ^= _totalLength;
						if (_totalLength >= 8)
						{
							a = Endianness.ToUInt64LittleEndian(leftover, pOffset);
							b = Endianness.ToUInt64LittleEndian(leftover, pOffset + i - 8);
						}
						else
						{
							a = Endianness.ToUInt32LittleEndian(leftover, pOffset);
							b = Endianness.ToUInt32LittleEndian(leftover, pOffset + i - 4);
						}
					}
					else if (_totalLength > 0)
					{
						a = ((ulong)leftover[pOffset] << 45) | leftover[pOffset + i - 1];
						b = leftover[pOffset + (i >> 1)];
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
						_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(leftover, pOffset + 8) ^ _seed);
						if (i > 32)
						{
							_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 16) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(leftover, pOffset + 24) ^ _seed);
							if (i > 48)
							{
								_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 32) ^ RapidHashShared.Secret[1], Endianness.ToUInt64LittleEndian(leftover, pOffset + 40) ^ _seed);
								if (i > 64)
								{
									_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 48) ^ RapidHashShared.Secret[1], Endianness.ToUInt64LittleEndian(leftover, pOffset + 56) ^ _seed);
									if (i > 80)
									{
										_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 64) ^ RapidHashShared.Secret[2], Endianness.ToUInt64LittleEndian(leftover, pOffset + 72) ^ _seed);
										if (i > 96)
										{
											_seed = RapidHashShared.RapidMix(Endianness.ToUInt64LittleEndian(leftover, pOffset + 80) ^ RapidHashShared.Secret[1], Endianness.ToUInt64LittleEndian(leftover, pOffset + 88) ^ _seed);
										}
									}
								}
							}
						}
					}

					Span<byte> last16Bytes = stackalloc byte[16];
					if (i >= 16)
					{
						leftover.Slice(pOffset + i - 16, 16).CopyTo(last16Bytes);
					}
					else
					{
						if (_hasProcessedBlocks)
						{
							int from_tail = 16 - i;
							_tailBuffer.AsSpan(16 - from_tail, from_tail).CopyTo(last16Bytes);
							if (i > 0)
							{
								leftover.Slice(pOffset, i).CopyTo(last16Bytes.Slice(from_tail, i));
							}
						}
						else
						{
							// This branch is unreachable if _totalLength > 16, but kept for safety.
							leftover.Slice(pOffset, i).CopyTo(last16Bytes.Slice(16 - i, i));
						}
					}

					a = Endianness.ToUInt64LittleEndian(last16Bytes, 0) ^ (ulong)i;
					b = Endianness.ToUInt64LittleEndian(last16Bytes, 8);
				}

				a ^= RapidHashShared.Secret[1];
				b ^= _seed;
				RapidHashShared.RapidMum(ref a, ref b);

				ulong finalHash = RapidHashShared.RapidMix(a ^ RapidHashShared.Secret[7], b ^ RapidHashShared.Secret[1] ^ length_to_mix);

				byte[] result = new byte[8];
				Endianness.ToLittleEndianBytes(finalHash, result, 0);

				return new HashValue(ValueEndianness.LittleEndian, result, 64);
			}
		}
	}
}