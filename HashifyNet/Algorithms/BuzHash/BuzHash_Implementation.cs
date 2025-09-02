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

namespace HashifyNet.Algorithms.BuzHash
{
	/// <summary>
	/// Provides an implementation of the BuzHash algorithm, a rolling hash function that supports configurable hash sizes
	/// and circular shift directions.
	/// </summary>
	/// <remarks>This class is designed to compute rolling hash values efficiently for streams of data. It supports
	/// hash sizes of 8, 16, 32, and 64 bits, and allows customization of the circular shift direction and the
	/// randomization table (Rtab) used in the hashing process.  The configuration for the hash function is provided via an
	/// <see cref="IBuzHashConfig"/> object, which must specify a valid hash size, a non-null Rtab with exactly 256
	/// entries, and a valid circular shift direction. The configuration is cloned internally to ensure
	/// immutability.</remarks>
	[HashAlgorithmImplementation(typeof(IBuzHash), typeof(BuzHashConfig))]
	internal class BuzHash_Implementation
		: StreamableHashFunctionBase<IBuzHashConfig>,
			IBuzHash
	{
		public override IBuzHashConfig Config => _config.Clone();

		private readonly IBuzHashConfig _config;
		private static readonly IEnumerable<int> _validHashSizes =
			new HashSet<int>() { 8, 16, 32, 64 };

		public BuzHash_Implementation(IBuzHashConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.Rtab == null || _config.Rtab.Count != 256)
			{
				throw new ArgumentException($"{nameof(config.Rtab)} must be non-null list of 256 {nameof(UInt64)} values.", $"{nameof(config)}.{nameof(config.Rtab)}");
			}

			if (!_validHashSizes.Contains(_config.HashSizeInBits))
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within BuzHashBase.ValidHashSizes.");
			}

			if (_config.ShiftDirection != CircularShiftDirection.Left && _config.ShiftDirection != CircularShiftDirection.Right)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.ShiftDirection)}", _config.ShiftDirection, $"{nameof(config)}.{nameof(config.ShiftDirection)} must be a valid {nameof(CircularShiftDirection)} value.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			switch (_config.HashSizeInBits)
			{
				case 8:
					return new BlockTransformer_8Bit(_config);

				case 16:
					return new BlockTransformer_16Bit(_config);

				case 32:
					return new BlockTransformer_32Bit(_config);

				case 64:
					return new BlockTransformer_64Bit(_config);

				default:
					throw new NotImplementedException();
			}
		}

		private class BlockTransformer_8Bit
			: BlockTransformerBase<BlockTransformer_8Bit>
		{
			private IReadOnlyList<ulong> _rtab;
			private readonly CircularShiftDirection _shiftDirection;

			private byte _hashValue;
			public BlockTransformer_8Bit()
			{

			}

			public BlockTransformer_8Bit(IBuzHashConfig config)
				: this()
			{
				_rtab = config.Rtab;
				_shiftDirection = config.ShiftDirection;

				_hashValue = (byte)config.Seed;
			}

			protected override void CopyStateTo(BlockTransformer_8Bit other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
				other._rtab = _rtab;
			}
			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempRtab = _rtab;

				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue = (byte)(CShift(tempHashValue, 1, _shiftDirection) ^ (byte)tempRtab[dataArray[currentOffset]]);
				}

				_hashValue = tempHashValue;
			}
			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken) =>
				new HashValue(new byte[] { _hashValue }, 8);
		}

		private class BlockTransformer_16Bit
			: BlockTransformerBase<BlockTransformer_16Bit>
		{
			private IReadOnlyList<ulong> _rtab;
			private readonly CircularShiftDirection _shiftDirection;

			private ushort _hashValue;
			public BlockTransformer_16Bit()
			{

			}

			public BlockTransformer_16Bit(IBuzHashConfig config)
				: this()
			{
				_rtab = config.Rtab;
				_shiftDirection = config.ShiftDirection;

				_hashValue = (ushort)config.Seed;
			}

			protected override void CopyStateTo(BlockTransformer_16Bit other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
				other._rtab = _rtab;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempRtab = _rtab;

				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue = (ushort)(CShift(tempHashValue, 1, _shiftDirection) ^ (ushort)tempRtab[dataArray[currentOffset]]);
				}

				_hashValue = tempHashValue;
			}
			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken) =>
				new HashValue(Endianness.GetBytesLittleEndian(_hashValue), 16);
		}
		private class BlockTransformer_32Bit
		: BlockTransformerBase<BlockTransformer_32Bit>
		{
			private IReadOnlyList<ulong> _rtab;
			private readonly CircularShiftDirection _shiftDirection;

			private uint _hashValue;

			public BlockTransformer_32Bit()
			{
			}

			public BlockTransformer_32Bit(IBuzHashConfig config)
				: this()
			{
				_rtab = config.Rtab;
				_shiftDirection = config.ShiftDirection;

				_hashValue = (uint)config.Seed;
			}

			protected override void CopyStateTo(BlockTransformer_32Bit other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
				other._rtab = _rtab;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempRtab = _rtab;

				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue = CShift(tempHashValue, 1, _shiftDirection) ^ (uint)tempRtab[dataArray[currentOffset]];
				}

				_hashValue = tempHashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken) =>
				new HashValue(Endianness.GetBytesLittleEndian(_hashValue), 32);
		}

		private class BlockTransformer_64Bit
			: BlockTransformerBase<BlockTransformer_64Bit>
		{
			private IReadOnlyList<ulong> _rtab;
			private readonly CircularShiftDirection _shiftDirection;

			private ulong _hashValue;

			public BlockTransformer_64Bit()
			{
			}

			public BlockTransformer_64Bit(IBuzHashConfig config)
				: this()
			{
				_rtab = config.Rtab;
				_shiftDirection = config.ShiftDirection;

				_hashValue = config.Seed;
			}

			protected override void CopyStateTo(BlockTransformer_64Bit other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
				other._rtab = _rtab;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempRtab = _rtab;

				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue = CShift(tempHashValue, 1, _shiftDirection) ^ tempRtab[dataArray[currentOffset]];
				}

				_hashValue = tempHashValue;
			}
			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken) =>
				new HashValue(Endianness.GetBytesLittleEndian(_hashValue), 64);
		}

		#region CShift

		private static byte CShift(byte n, int shiftCount, CircularShiftDirection shiftDirection)
		{
			if (shiftDirection == CircularShiftDirection.Right)
			{
				return RotateRight(n, shiftCount);
			}

			return RotateLeft(n, shiftCount);
		}

		private static ushort CShift(ushort n, int shiftCount, CircularShiftDirection shiftDirection)
		{
			if (shiftDirection == CircularShiftDirection.Right)
			{
				return RotateRight(n, shiftCount);
			}

			return RotateLeft(n, shiftCount);
		}

		private static uint CShift(uint n, int shiftCount, CircularShiftDirection shiftDirection)
		{
			if (shiftDirection == CircularShiftDirection.Right)
			{
				return RotateRight(n, shiftCount);
			}

			return RotateLeft(n, shiftCount);
		}

		private static ulong CShift(ulong n, int shiftCount, CircularShiftDirection shiftDirection)
		{
			if (shiftDirection == CircularShiftDirection.Right)
			{
				return RotateRight(n, shiftCount);
			}

			return RotateLeft(n, shiftCount);
		}

		#endregion

		#region RotateLeft

		private static byte RotateLeft(byte operand, int shiftCount)
		{
			shiftCount &= 0x07;

			return (byte)(
				(operand << shiftCount) |
				(operand >> (8 - shiftCount)));
		}

		private static ushort RotateLeft(ushort operand, int shiftCount)
		{
			shiftCount &= 0x0f;

			return (ushort)(
				(operand << shiftCount) |
				(operand >> (16 - shiftCount)));
		}

		private static uint RotateLeft(uint operand, int shiftCount)
		{
			shiftCount &= 0x1f;

			return
				(operand << shiftCount) |
				(operand >> (32 - shiftCount));
		}

		private static ulong RotateLeft(ulong operand, int shiftCount)
		{
			shiftCount &= 0x3f;

			return
				(operand << shiftCount) |
				(operand >> (64 - shiftCount));
		}

		#endregion

		#region RotateRight

		private static byte RotateRight(byte operand, int shiftCount)
		{
			shiftCount &= 0x07;

			return (byte)(
				(operand >> shiftCount) |
				(operand << (8 - shiftCount)));
		}

		private static ushort RotateRight(ushort operand, int shiftCount)
		{
			shiftCount &= 0x0f;

			return (ushort)(
				(operand >> shiftCount) |
				(operand << (16 - shiftCount)));
		}

		private static uint RotateRight(uint operand, int shiftCount)
		{
			shiftCount &= 0x1f;

			return
				(operand >> shiftCount) |
				(operand << (32 - shiftCount));
		}

		private static ulong RotateRight(ulong operand, int shiftCount)
		{
			shiftCount &= 0x3f;

			return
				(operand >> shiftCount) |
				(operand << (64 - shiftCount));
		}

		#endregion
	}
}

