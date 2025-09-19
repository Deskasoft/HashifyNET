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
using System.Threading;

namespace HashifyNet.Algorithms.MetroHash
{
	/// <summary>
	/// Provides an implementation of the MetroHash algorithm, a high-performance, non-cryptographic hash function.
	/// </summary>
	/// <remarks>This class supports both 64-bit and 128-bit hash sizes, as specified in the configuration. It is
	/// designed for scenarios where fast hashing is required, such as checksums, hash-based data structures, or data
	/// deduplication. The hash size must be either 64 or 128 bits, and an exception will be thrown if an unsupported size
	/// is specified.</remarks>
	[HashAlgorithmImplementation(typeof(IMetroHash), typeof(MetroHashConfig))]
	internal class MetroHash_Implementation
		: StreamableHashFunctionBase<IMetroHashConfig>,
			IMetroHash
	{
		public override IMetroHashConfig Config => _config.Clone();

		private readonly IMetroHashConfig _config;
		public MetroHash_Implementation(IMetroHashConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();
			if (_config.HashSizeInBits != 128 && _config.HashSizeInBits != 64)
			{
				throw new ArgumentOutOfRangeException(
		nameof(config.HashSizeInBits),
		"Hash size for MetroHash must be 128 or 64 bits.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			switch (_config.HashSizeInBits)
			{
				case 128:
					return new BlockTransformer128((ulong)_config.Seed);
				case 64:
					return new BlockTransformer64((ulong)_config.Seed);
				default:
					throw new NotSupportedException($"Hash size of {_config.HashSizeInBits} bits is not supported by MetroHash.");
			}
		}

		private class BlockTransformer128
			: BlockTransformerBase<BlockTransformer128>
		{
			private const ulong _k0 = 0xC83A91E1;
			private const ulong _k1 = 0x8648DBDB;
			private const ulong _k2 = 0x7BDEC03B;
			private const ulong _k3 = 0x2F5870A5;

			private ulong _a;
			private ulong _b;
			private ulong _c;
			private ulong _d;
			private ulong _bytesProcessed;

			public BlockTransformer128()
				: base(inputBlockSize: 32)
			{
			}

			public BlockTransformer128(ulong seed)
				: this()
			{
				_a = (seed - _k0) * _k3;
				_b = (seed + _k1) * _k2;
				_c = (seed + _k0) * _k2;
				_d = (seed - _k1) * _k3;

				_bytesProcessed = 0;
			}

			protected override void CopyStateTo(BlockTransformer128 other)
			{
				base.CopyStateTo(other);

				other._a = _a;
				other._b = _b;
				other._c = _c;
				other._d = _d;

				other._bytesProcessed = _bytesProcessed;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;

				for (var currentOffset = 0; currentOffset < dataCount; currentOffset += 32)
				{
					tempA += Endianness.ToUInt64LittleEndian(data, currentOffset) * _k0;
					tempA = RotateRight(tempA, 29) + tempC;

					tempB += Endianness.ToUInt64LittleEndian(data, currentOffset + 8) * _k1;
					tempB = RotateRight(tempB, 29) + tempD;

					tempC += Endianness.ToUInt64LittleEndian(data, currentOffset + 16) * _k2;
					tempC = RotateRight(tempC, 29) + tempA;

					tempD += Endianness.ToUInt64LittleEndian(data, currentOffset + 24) * _k3;
					tempD = RotateRight(tempD, 29) + tempB;
				}

				_a = tempA;
				_b = tempB;
				_c = tempC;
				_d = tempD;

				_bytesProcessed += (ulong)dataCount;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;

				if (_bytesProcessed > 0)
				{
					tempC ^= RotateRight(((tempA + tempD) * _k0) + tempB, 21) * _k1;
					tempD ^= RotateRight(((tempB + tempC) * _k1) + tempA, 21) * _k0;
					tempA ^= RotateRight(((tempA + tempC) * _k0) + tempD, 21) * _k1;
					tempB ^= RotateRight(((tempB + tempD) * _k1) + tempC, 21) * _k0;
				}

				var remainderOffset = 0;
				var remainderCount = leftover.Length;
				if (remainderCount >= 16)
				{
					tempA += Endianness.ToUInt64LittleEndian(leftover, remainderOffset) * _k2;
					tempA = RotateRight(tempA, 33) * _k3;

					tempB += Endianness.ToUInt64LittleEndian(leftover, remainderOffset + 8) * _k2;
					tempB = RotateRight(tempB, 33) * _k3;

					tempA ^= RotateRight((tempA * _k2) + tempB, 45) * _k1;
					tempB ^= RotateRight((tempB * _k3) + tempA, 45) * _k0;

					remainderOffset += 16;
					remainderCount -= 16;
				}

				if (remainderCount >= 8)
				{
					tempA += Endianness.ToUInt64LittleEndian(leftover, remainderOffset) * _k2;
					tempA = RotateRight(tempA, 33) * _k3;
					tempA ^= RotateRight((tempA * _k2) + tempB, 27) * _k1;

					remainderOffset += 8;
					remainderCount -= 8;
				}

				if (remainderCount >= 4)
				{
					tempB += Endianness.ToUInt32LittleEndian(leftover, remainderOffset) * _k2;
					tempB = RotateRight(tempB, 33) * _k3;
					tempB ^= RotateRight((tempB * _k3) + tempA, 46) * _k0;

					remainderOffset += 4;
					remainderCount -= 4;
				}

				if (remainderCount >= 2)
				{
					tempA += Endianness.ToUInt16LittleEndian(leftover, remainderOffset) * _k2;
					tempA = RotateRight(tempA, 33) * _k3;
					tempA ^= RotateRight((tempA * _k2) + tempB, 22) * _k1;

					remainderOffset += 2;
					remainderCount -= 2;
				}

				if (remainderCount >= 1)
				{
					tempB += leftover[remainderOffset] * _k2;
					tempB = RotateRight(tempB, 33) * _k3;
					tempB ^= RotateRight((tempB * _k3) + tempA, 58) * _k0;
				}

				tempA += RotateRight((tempA * _k0) + tempB, 13);
				tempB += RotateRight((tempB * _k1) + tempA, 37);
				tempA += RotateRight((tempA * _k2) + tempB, 13);
				tempB += RotateRight((tempB * _k3) + tempA, 37);

				var hashValueBytes = new byte[16];

				Buffer.BlockCopy(Endianness.GetBytesLittleEndian(tempA), 0, hashValueBytes, 0, 8);
				Buffer.BlockCopy(Endianness.GetBytesLittleEndian(tempB), 0, hashValueBytes, 8, 8);

				return new HashValue(ValueEndianness.LittleEndian, hashValueBytes, 128);
			}

			private static ulong RotateRight(ulong operand, int shiftCount)
			{
				shiftCount &= 0x3f;

				return
					(operand >> shiftCount) |
					(operand << (64 - shiftCount));
			}
		}

		private class BlockTransformer64
	: BlockTransformerBase<BlockTransformer64>
		{
			private const ulong _k0 = 0xD6D018F5;
			private const ulong _k1 = 0xA2AA033B;
			private const ulong _k2 = 0x62992FC1;
			private const ulong _k3 = 0x30BC5B29;

			private ulong _initialValue;

			private ulong _a;
			private ulong _b;
			private ulong _c;
			private ulong _d;

			private ulong _bytesProcessed;

			public BlockTransformer64()
				: base(inputBlockSize: 32)
			{
			}

			public BlockTransformer64(ulong seed)
				: this()
			{
				_initialValue = (seed + _k2) * _k0;

				_a = _initialValue;
				_b = _initialValue;
				_c = _initialValue;
				_d = _initialValue;

				_bytesProcessed = 0;
			}

			protected override void CopyStateTo(BlockTransformer64 other)
			{
				base.CopyStateTo(other);

				other._initialValue = _initialValue;

				other._a = _a;
				other._b = _b;
				other._c = _c;
				other._d = _d;

				other._bytesProcessed = _bytesProcessed;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				var dataCount = data.Length;

				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;

				for (var currentOffset = 0; currentOffset < data.Length; currentOffset += 32)
				{
					tempA += Endianness.ToUInt64LittleEndian(data, currentOffset) * _k0;
					tempA = RotateRight(tempA, 29) + tempC;

					tempB += Endianness.ToUInt64LittleEndian(data, currentOffset + 8) * _k1;
					tempB = RotateRight(tempB, 29) + tempD;

					tempC += Endianness.ToUInt64LittleEndian(data, currentOffset + 16) * _k2;
					tempC = RotateRight(tempC, 29) + tempA;

					tempD += Endianness.ToUInt64LittleEndian(data, currentOffset + 24) * _k3;
					tempD = RotateRight(tempD, 29) + tempB;
				}

				_a = tempA;
				_b = tempB;
				_c = tempC;
				_d = tempD;

				_bytesProcessed += (ulong)dataCount;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;

				if (_bytesProcessed > 0)
				{
					tempC ^= RotateRight(((tempA + tempD) * _k0) + tempB, 37) * _k1;
					tempD ^= RotateRight(((tempB + tempC) * _k1) + tempA, 37) * _k0;
					tempA ^= RotateRight(((tempA + tempC) * _k0) + tempD, 37) * _k1;
					tempB ^= RotateRight(((tempB + tempD) * _k1) + tempC, 37) * _k0;


					tempA = _initialValue + (tempA ^ tempB);
				}

				var remainderOffset = 0;
				var remainderCount = leftover.Length;

				if (remainderCount >= 16)
				{
					tempB = tempA + (Endianness.ToUInt64LittleEndian(leftover, remainderOffset) * _k2);
					tempB = RotateRight(tempB, 29) * _k3;

					tempC = tempA + (Endianness.ToUInt64LittleEndian(leftover, remainderOffset + 8) * _k2);
					tempC = RotateRight(tempC, 29) * _k3;

					tempB ^= RotateRight(tempB * _k0, 21) + tempC;
					tempC ^= RotateRight(tempC * _k3, 21) + tempB;
					tempA += tempC;


					remainderOffset += 16;
					remainderCount -= 16;
				}

				if (remainderCount >= 8)
				{
					tempA += Endianness.ToUInt64LittleEndian(leftover, remainderOffset) * _k3;
					tempA ^= RotateRight(tempA, 55) * _k1;

					remainderOffset += 8;
					remainderCount -= 8;
				}

				if (remainderCount >= 4)
				{
					tempA += Endianness.ToUInt32LittleEndian(leftover, remainderOffset) * _k3;
					tempA ^= RotateRight(tempA, 26) * _k1;

					remainderOffset += 4;
					remainderCount -= 4;
				}

				if (remainderCount >= 2)
				{
					tempA += Endianness.ToUInt16LittleEndian(leftover, remainderOffset) * _k3;
					tempA ^= RotateRight(tempA, 48) * _k1;

					remainderOffset += 2;
					remainderCount -= 2;
				}

				if (remainderCount >= 1)
				{
					tempA += leftover[remainderOffset] * _k3;
					tempA ^= RotateRight(tempA, 37) * _k1;
				}

				tempA ^= RotateRight(tempA, 28);
				tempA *= _k0;
				tempA ^= RotateRight(tempA, 29);

				return new HashValue(
					ValueEndianness.LittleEndian,
					Endianness.GetBytesLittleEndian(tempA),
					64);
			}
			private static ulong RotateRight(ulong operand, int shiftCount)
			{
				shiftCount &= 0x3f;

				return
					(operand >> shiftCount) |
					(operand << (64 - shiftCount));
			}
		}
	}
}
