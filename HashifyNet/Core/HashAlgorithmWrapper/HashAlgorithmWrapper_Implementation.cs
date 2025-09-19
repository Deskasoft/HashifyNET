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
using System.Security.Cryptography;
using System.Threading;

namespace HashifyNet.Core.HashAlgorithm
{
	internal class HashAlgorithmWrapper_Implementation
		: CryptographicStreamableHashFunctionBase<IHashAlgorithmWrapperConfig>,
			IHashAlgorithmWrapper
	{
		public override IHashAlgorithmWrapperConfig Config => _config.Clone();

		private readonly IHashAlgorithmWrapperConfig _config;
		private readonly IncrementalHash hashAlgorithm;
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

			if (_config.HashSizeInBits < 1)
			{
				throw new InvalidOperationException($"{nameof(_config)}.{nameof(_config.HashSizeInBits)} must be greater than 0.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(hashAlgorithm, _config.HashSizeInBits);
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

			private IncrementalHash _algo;
			private readonly int _hashSizeInBits;
			public BlockTransformer(IncrementalHash algorithm, int hashSizeInBits) : this()
			{
				_hashSizeInBits = hashSizeInBits;
				_algo = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				other._algo = _algo;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
#if NET8_0_OR_GREATER
				_algo.AppendData(data);
#else
				byte[] dataArray = data.ToArray();
				_algo.AppendData(dataArray);
#endif
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				if (leftover.Length > 0)
				{
					TransformByteGroupsInternal(leftover);
				}

				byte[] result = _algo.GetHashAndReset();
				return new HashValue(_algo.AlgorithmName == HashAlgorithmName.MD5 ? ValueEndianness.LittleEndian : ValueEndianness.BigEndian, result, _hashSizeInBits);
			}
		}
	}
}