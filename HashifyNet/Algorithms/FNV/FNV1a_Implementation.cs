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

using HashifyNet.Algorithms.FNV.Utilities;
using HashifyNet.Core;
using System;

namespace HashifyNet.Algorithms.FNV
{
	/// <summary>
	/// Provides an implementation of the FNV-1a hash algorithm, supporting multiple hash sizes.
	/// </summary>
	/// <remarks>This class implements the FNV-1a hash algorithm, a variant of the Fowler-Noll-Vo hash function. It
	/// supports 32-bit, 64-bit, and extended hash sizes, as determined by the configuration provided during
	/// initialization. The algorithm is optimized for fast, non-cryptographic hashing of data.</remarks>
	[HashAlgorithmImplementation(typeof(IFNV1a), typeof(FNVConfig))]
	internal class FNV1a_Implementation
		: FNV1Base,
			IFNV1a
	{
		public FNV1a_Implementation(IFNVConfig config)
			: base(config)
		{
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			switch (_config.HashSizeInBits)
			{
				case 32:
					return new BlockTransformer_32Bit(_fnvPrimeOffset);

				case 64:
					return new BlockTransformer_64Bit(_fnvPrimeOffset);

				default:
					return new BlockTransformer_Extended(_fnvPrimeOffset);
			}
		}

		private class BlockTransformer_32Bit
			: BlockTransformer_32BitBase<BlockTransformer_32Bit>
		{
			public BlockTransformer_32Bit()
				: base()
			{
			}

			public BlockTransformer_32Bit(FNVPrimeOffset fnvPrimeOffset)
				: base(fnvPrimeOffset)
			{
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempPrime = _prime;

				for (int currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue ^= dataArray[currentOffset];
					tempHashValue *= tempPrime;
				}

				_hashValue = tempHashValue;
			}
		}

		private class BlockTransformer_64Bit
			: BlockTransformer_64BitBase<BlockTransformer_64Bit>
		{
			public BlockTransformer_64Bit()
				: base()
			{
			}

			public BlockTransformer_64Bit(FNVPrimeOffset fnvPrimeOffset)
				: base(fnvPrimeOffset)
			{
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempPrime = _prime;

				for (int currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue ^= dataArray[currentOffset];
					tempHashValue *= tempPrime;
				}

				_hashValue = tempHashValue;
			}
		}

		private class BlockTransformer_Extended
			: BlockTransformer_ExtendedBase<BlockTransformer_Extended>
		{
			public BlockTransformer_Extended()
				: base()
			{
			}

			public BlockTransformer_Extended(FNVPrimeOffset fnvPrimeOffset)
				: base(fnvPrimeOffset)
			{
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempPrime = _prime;

				var tempHashSizeInBytes = _hashSizeInBytes;

				for (int currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{

					tempHashValue[0] ^= dataArray[currentOffset];
					tempHashValue = ExtendedMultiply(tempHashValue, tempPrime, tempHashSizeInBytes);
				}

				_hashValue = tempHashValue;
			}
		}
	}
}