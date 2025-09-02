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

namespace HashifyNet.Algorithms.Jenkins
{
	/// <summary>
	/// Provides an implementation of the Jenkins One-at-a-Time hash algorithm, a simple and fast non-cryptographic hash
	/// function.
	/// </summary>
	/// <remarks>This class implements the Jenkins One-at-a-Time hash algorithm, which produces a 32-bit hash value.
	/// It is designed for use in scenarios where a lightweight, non-cryptographic hash function is sufficient,  such as
	/// hash tables or checksum calculations. The algorithm processes input data in blocks and supports  streaming of data
	/// for incremental hashing. <para> The hash size is fixed at 32 bits, and attempting to configure a different hash
	/// size will result in an exception. </para> <para> This implementation is not thread-safe. If multiple threads need
	/// to use the same instance, external synchronization is required. </para></remarks>
	[HashAlgorithmImplementation(typeof(IJenkinsOneAtATime), typeof(JenkinsOneAtATimeConfig))]
	internal class JenkinsOneAtATime_Implementation
		: StreamableHashFunctionBase<IJenkinsOneAtATimeConfig>,
			IJenkinsOneAtATime
	{
		private readonly IJenkinsOneAtATimeConfig _config;
		public override IJenkinsOneAtATimeConfig Config => _config.Clone();
		public JenkinsOneAtATime_Implementation(IJenkinsOneAtATimeConfig config)
		{
			_config = config?.Clone() ?? throw new ArgumentNullException(nameof(config));
			if (config.HashSizeInBits != 32)
			{
				throw new ArgumentOutOfRangeException(
					nameof(config.HashSizeInBits),
					"Hash size for Jenkins One-at-a-Time hash must be 32 bits.");
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
					tempHashValue += dataArray[currentOffset];
					tempHashValue += tempHashValue << 10;
					tempHashValue ^= tempHashValue >> 6;
				}

				_hashValue = tempHashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var finalHashValue = _hashValue;
				finalHashValue += finalHashValue << 3;
				finalHashValue ^= finalHashValue >> 11;
				finalHashValue += finalHashValue << 15;

				return new HashValue(
					Endianness.GetBytesLittleEndian(finalHashValue),
					32);
			}
		}
	}

}
