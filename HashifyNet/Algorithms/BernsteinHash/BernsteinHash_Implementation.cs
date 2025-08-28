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

namespace HashifyNet.Algorithms.BernsteinHash
{
	/// <summary>
	/// Provides an implementation of the Bernstein hash algorithm, a simple and fast non-cryptographic hash function.
	/// </summary>
	/// <remarks>This implementation supports a fixed hash size of 32 bits, as required by the Bernstein hash
	/// algorithm. The hash function processes input data in a streaming manner, making it suitable for scenarios where
	/// data is processed incrementally.</remarks>
	[HashAlgorithmImplementation(typeof(IBernsteinHash), typeof(BernsteinConfig))]
	internal class BernsteinHash_Implementation
		: StreamableHashFunctionBase<IBernsteinConfig>,
			IBernsteinHash
	{
		private readonly IBernsteinConfig _config;
		public override IBernsteinConfig Config => _config.Clone();
		public BernsteinHash_Implementation(IBernsteinConfig config)
		{
			_config = config?.Clone() ?? throw new ArgumentNullException(nameof(config));
			if (config.HashSizeInBits != 32)
			{
				throw new ArgumentOutOfRangeException(
					nameof(config.HashSizeInBits),
					"Hash size for Bernstein hash must be 32 bits.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer() =>
			new BlockTransformer();

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private uint _hashValue;

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var endOffset = data.Offset + data.Count;

				var tempHashValue = _hashValue;

				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					tempHashValue = (33 * tempHashValue) + dataArray[currentOffset];
				}

				_hashValue = tempHashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				return new HashValue(
					BitConverter.GetBytes(_hashValue),
					32);
			}
		}
	}
}