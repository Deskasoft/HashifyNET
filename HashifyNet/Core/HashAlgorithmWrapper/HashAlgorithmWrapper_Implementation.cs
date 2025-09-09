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

using HashifyNet.Core.Utilities;
using System;
using System.Threading;

namespace HashifyNet.Core.HashAlgorithm
{
	internal class HashAlgorithmWrapper_Implementation
		: CryptographicStreamableHashFunctionBase<IHashAlgorithmWrapperConfig>,
			IHashAlgorithmWrapper
	{
		public override IHashAlgorithmWrapperConfig Config => _config.Clone();

		private readonly IHashAlgorithmWrapperConfig _config;
		private readonly System.Security.Cryptography.HashAlgorithm hashAlgorithm;
		public HashAlgorithmWrapper_Implementation(IHashAlgorithmWrapperConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();
			if (_config.InstanceFactory == null)
			{
				throw new ArgumentException($"{nameof(_config)}.{nameof(_config.InstanceFactory)} has not been set.", $"{nameof(_config)}.{nameof(_config.InstanceFactory)}");
			}

			hashAlgorithm = _config.InstanceFactory() ?? throw new InvalidOperationException("The hash algorithm factory returned null.");

			if (_config.HashSizeInBits != hashAlgorithm.HashSize)
			{
				throw new InvalidOperationException($"{nameof(_config)}.{nameof(_config.HashSizeInBits)} does not match the underlying {nameof(_config.InstanceFactory)} hash algorithm's size. Expected {_config.HashSizeInBits} bits but got {hashAlgorithm.HashSize} bits.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(hashAlgorithm);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				hashAlgorithm?.Dispose();
			}

			base.Dispose(disposing);
		}

		private class BlockTransformer : BlockTransformerBase<BlockTransformer>
		{
			public BlockTransformer()
			{
			}

			private System.Security.Cryptography.HashAlgorithm _algo;
			public BlockTransformer(System.Security.Cryptography.HashAlgorithm algorithm) : this()
			{
				_algo = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				other._algo = _algo;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				if (_algo.TransformBlock(data.Array, data.Offset, data.Count, null, 0) != data.Count)
				{
					throw new Exception("Hash algorithm did not process all input data.");
				}
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				byte[] lastBytes = FinalizeInputBuffer ?? Array.Empty<byte>();
				byte[] lastResult = _algo.TransformFinalBlock(lastBytes, 0, lastBytes.Length);
				if (lastResult == null || lastResult.Length != lastBytes.Length)
				{
					throw new Exception("Hash algorithm did not process all input data.");
				}

				return new HashValue(_algo.Hash, _algo.HashSize);
			}
		}
	}
}
