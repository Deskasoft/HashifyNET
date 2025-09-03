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

namespace HashifyNet.Algorithms.XxHash3
{
	internal class XxHash3_Implementation
		: StreamableHashFunctionBase<IXxHash3Config>,
			IXxHash3
	{
		public override IXxHash3Config Config => _config.Clone();

		private readonly IXxHash3Config _config;
		public XxHash3_Implementation(IXxHash3Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits != 64 && _config.HashSizeInBits != 128)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 64 or 128 bits.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer3(_config.HashSizeInBits, _config.Seed);
		}

		private class BlockTransformer3 : BlockTransformerBase<BlockTransformer3>
		{
			public BlockTransformer3() : base(inputBlockSize: 64) { }

			private System.IO.Hashing.NonCryptographicHashAlgorithm _hash;
			public BlockTransformer3(int hashSize, long seed) : this()
			{
				switch (hashSize)
				{
					case 64:
						_hash = new System.IO.Hashing.XxHash3(seed);
						break;
					case 128:
						_hash = new System.IO.Hashing.XxHash128(seed);
						break;
					default:
						throw new NotImplementedException();
				}
			}

			protected override void CopyStateTo(BlockTransformer3 other)
			{
				other._hash = _hash;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				_hash.Append(data.AsSpan());
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				Span<byte> last = FinalizeInputBuffer == null ? Span<byte>.Empty : FinalizeInputBuffer.AsSpan();
				_hash.Append(last);

				byte[] output = _hash.GetCurrentHash();
				return new HashValue(output, _hash.HashLengthInBytes * 8);
			}
		}
	}

}
