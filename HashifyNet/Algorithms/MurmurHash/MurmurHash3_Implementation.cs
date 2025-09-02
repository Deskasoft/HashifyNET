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

namespace HashifyNet.Algorithms.MurmurHash
{
	/// <summary>
	/// Provides an implementation of the MurmurHash3 algorithm, a non-cryptographic hash function  designed for high
	/// performance and uniform distribution. This implementation supports both  32-bit and 128-bit hash sizes.
	/// </summary>
	/// <remarks>MurmurHash3 is a widely used hash function for scenarios where speed and uniformity are  critical,
	/// such as hash-based data structures and checksums. This implementation is  configurable via an <see
	/// cref="IMurmurHash3Config"/> object, which allows specifying the  hash size (32 or 128 bits) and an optional seed
	/// value for hash computation. <para> The class is internal and intended for use within the library. It validates the
	/// configuration  during construction and ensures that only supported hash sizes are used. </para></remarks>
	[HashAlgorithmImplementation(typeof(IMurmurHash3), typeof(MurmurHash3Config))]
	internal class MurmurHash3_Implementation
		: StreamableHashFunctionBase<IMurmurHash3Config>,
			IMurmurHash3
	{
		public override IMurmurHash3Config Config => _config.Clone();

		private const uint c1_32 = 0xcc9e2d51;
		private const uint c2_32 = 0x1b873593;

		private const ulong c1_128 = 0x87c37b91114253d5;
		private const ulong c2_128 = 0x4cf5ad432745937f;

		private readonly IMurmurHash3Config _config;
		private static readonly IEnumerable<int> _validHashSizes = new HashSet<int>() { 32, 128 };

		public MurmurHash3_Implementation(IMurmurHash3Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (!_validHashSizes.Contains(_config.HashSizeInBits))
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within MurmurHash3.ValidHashSizes.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			switch (_config.HashSizeInBits)
			{
				case 32:
					return new BlockTransformer32(_config.Seed);

				case 128:
					return new BlockTransformer128(_config.Seed);

				default:
					throw new NotImplementedException();
			}
		}

		private class BlockTransformer32
			: BlockTransformerBase<BlockTransformer32>
		{
			private uint _hashValue;
			private int _bytesProcessed = 0;

			public BlockTransformer32()
				: base(inputBlockSize: 4)
			{
			}

			public BlockTransformer32(uint seed)
				: this()
			{
				_hashValue = seed;
			}

			protected override void CopyStateTo(BlockTransformer32 other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;

				other._bytesProcessed = _bytesProcessed;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataOffset = data.Offset;
				var dataCount = data.Count;

				var endOffset = dataOffset + dataCount;

				var tempHashValue = _hashValue;

				for (var currentOffset = dataOffset; currentOffset < endOffset; currentOffset += 4)
				{
					uint k1 = Endianness.ToUInt32LittleEndian(dataArray, currentOffset);

					k1 *= c1_32;
					k1 = RotateLeft(k1, 15);
					k1 *= c2_32;

					tempHashValue ^= k1;
					tempHashValue = RotateLeft(tempHashValue, 13);
					tempHashValue = (tempHashValue * 5) + 0xe6546b64;
				}

				_hashValue = tempHashValue;

				_bytesProcessed += dataCount;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var remainder = FinalizeInputBuffer;
				var remainderCount = (remainder?.Length).GetValueOrDefault();

				var tempHashValue = _hashValue;

				var tempBytesProcessed = _bytesProcessed;

				if (remainderCount > 0)
				{
					uint k2 = 0;

					switch (remainderCount)
					{
						case 3: k2 ^= (uint)remainder[2] << 16; goto case 2;
						case 2: k2 ^= (uint)remainder[1] << 8; goto case 1;
						case 1:
							k2 ^= remainder[0];
							break;
					}

					k2 *= c1_32;
					k2 = RotateLeft(k2, 15);
					k2 *= c2_32;
					tempHashValue ^= k2;

					tempBytesProcessed += remainderCount;
				}

				tempHashValue ^= (uint)tempBytesProcessed;
				Mix(ref tempHashValue);

				return new HashValue(
					Endianness.GetBytesLittleEndian(tempHashValue),
					32);
			}

			private static void Mix(ref uint h)
			{
				h ^= h >> 16;
				h *= 0x85ebca6b;
				h ^= h >> 13;
				h *= 0xc2b2ae35;
				h ^= h >> 16;
			}

			private static uint RotateLeft(uint operand, int shiftCount)
			{
				shiftCount &= 0x1f;

				return
					(operand << shiftCount) |
					(operand >> (32 - shiftCount));
			}
		}

		private class BlockTransformer128
			: BlockTransformerBase<BlockTransformer128>
		{
			private ulong _hashValue1;
			private ulong _hashValue2;

			private int _bytesProcessed = 0;

			public BlockTransformer128()
				: base(inputBlockSize: 16)
			{
			}

			public BlockTransformer128(ulong seed)
				: this()
			{
				_hashValue1 = seed;
				_hashValue2 = seed;
			}

			protected override void CopyStateTo(BlockTransformer128 other)
			{
				base.CopyStateTo(other);

				other._hashValue1 = _hashValue1;
				other._hashValue2 = _hashValue2;

				other._bytesProcessed = _bytesProcessed;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataOffset = data.Offset;
				var dataCount = data.Count;

				var endOffset = dataOffset + dataCount;

				var tempHashValue1 = _hashValue1;
				var tempHashValue2 = _hashValue2;

				for (var currentOffset = dataOffset; currentOffset < endOffset; currentOffset += 16)
				{
					ulong k1 = Endianness.ToUInt64LittleEndian(dataArray, currentOffset);
					ulong k2 = Endianness.ToUInt64LittleEndian(dataArray, currentOffset + 8);

					k1 *= c1_128;
					k1 = RotateLeft(k1, 31);
					k1 *= c2_128;
					tempHashValue1 ^= k1;

					tempHashValue1 = RotateLeft(tempHashValue1, 27);
					tempHashValue1 += tempHashValue2;
					tempHashValue1 = (tempHashValue1 * 5) + 0x52dce729;

					k2 *= c2_128;
					k2 = RotateLeft(k2, 33);
					k2 *= c1_128;
					tempHashValue2 ^= k2;

					tempHashValue2 = RotateLeft(tempHashValue2, 31);
					tempHashValue2 += tempHashValue1;
					tempHashValue2 = (tempHashValue2 * 5) + 0x38495ab5;
				}

				_hashValue1 = tempHashValue1;
				_hashValue2 = tempHashValue2;

				_bytesProcessed += dataCount;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var remainder = FinalizeInputBuffer;
				var remainderCount = (remainder?.Length).GetValueOrDefault();

				var tempHashValue1 = _hashValue1;
				var tempHashValue2 = _hashValue2;

				var tempBytesProcessed = _bytesProcessed;

				if (remainderCount > 0)
				{
					ulong k1 = 0;
					ulong k2 = 0;

					switch (remainderCount)
					{
						case 15: k2 ^= (ulong)remainder[14] << 48; goto case 14;
						case 14: k2 ^= (ulong)remainder[13] << 40; goto case 13;
						case 13: k2 ^= (ulong)remainder[12] << 32; goto case 12;
						case 12: k2 ^= (ulong)remainder[11] << 24; goto case 11;
						case 11: k2 ^= (ulong)remainder[10] << 16; goto case 10;
						case 10: k2 ^= (ulong)remainder[9] << 8; goto case 9;
						case 9:
							k2 ^= remainder[8];
							k2 *= c2_128;
							k2 = RotateLeft(k2, 33);
							k2 *= c1_128;
							tempHashValue2 ^= k2;

							goto case 8;

						case 8:
							k1 ^= Endianness.ToUInt64LittleEndian(remainder, 0);
							break;

						case 7: k1 ^= (ulong)remainder[6] << 48; goto case 6;
						case 6: k1 ^= (ulong)remainder[5] << 40; goto case 5;
						case 5: k1 ^= (ulong)remainder[4] << 32; goto case 4;
						case 4: k1 ^= (ulong)remainder[3] << 24; goto case 3;
						case 3: k1 ^= (ulong)remainder[2] << 16; goto case 2;
						case 2: k1 ^= (ulong)remainder[1] << 8; goto case 1;
						case 1:
							k1 ^= remainder[0];
							break;
					}

					k1 *= c1_128;
					k1 = RotateLeft(k1, 31);
					k1 *= c2_128;
					tempHashValue1 ^= k1;

					tempBytesProcessed += remainderCount;
				}

				tempHashValue1 ^= (ulong)tempBytesProcessed;
				tempHashValue2 ^= (ulong)tempBytesProcessed;

				tempHashValue1 += tempHashValue2;
				tempHashValue2 += tempHashValue1;

				Mix(ref tempHashValue1);
				Mix(ref tempHashValue2);

				tempHashValue1 += tempHashValue2;
				tempHashValue2 += tempHashValue1;

				var hashValueBytes = Endianness.GetBytesLittleEndian(tempHashValue1)
					.Concat(Endianness.GetBytesLittleEndian(tempHashValue2))
					.ToArray();

				return new HashValue(hashValueBytes, 128);
			}

			private static void Mix(ref ulong k)
			{
				k ^= k >> 33;
				k *= 0xff51afd7ed558ccd;
				k ^= k >> 33;
				k *= 0xc4ceb9fe1a85ec53;
				k ^= k >> 33;
			}

			private static ulong RotateLeft(ulong operand, int shiftCount)
			{
				shiftCount &= 0x3f;

				return
					(operand << shiftCount) |
					(operand >> (64 - shiftCount));
			}
		}
	}
}
