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
using System.IO;
using System.Threading;

namespace HashifyNet.Core.HashAlgorithm
{
	[HashAlgorithmImplementation(typeof(IHashAlgorithmWrapper), typeof(HashAlgorithmWrapperConfig))]
	internal class HashAlgorithmWrapper_Implementation
		: HashFunctionBase<IHashAlgorithmWrapperConfig>,
			IHashAlgorithmWrapper
	{
		public override IHashAlgorithmWrapperConfig Config => _config.Clone();

		private readonly IHashAlgorithmWrapperConfig _config;
		public HashAlgorithmWrapper_Implementation(IHashAlgorithmWrapperConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();
			if (_config.InstanceFactory == null)
			{
				throw new ArgumentException($"{nameof(config)}.{nameof(config.InstanceFactory)} has not been set.", $"{nameof(config)}.{nameof(config.InstanceFactory)}");
			}

			using (var hashAlgorithm = _config.InstanceFactory())
			{
				if (_config.HashSizeInBits != hashAlgorithm.HashSize)
				{
					throw new InvalidOperationException($"{nameof(config)}.{nameof(config.HashSizeInBits)} does not match the underlying {nameof(config.InstanceFactory)} hash algorithm's size. Expected {config.HashSizeInBits} bits but got {hashAlgorithm.HashSize} bits.");
				}
			}
		}

		public IHashValue ComputeHash(Stream data)
		{
			using (var hashAlgorithm = _config.InstanceFactory())
			{
				return new HashValue(
					hashAlgorithm.ComputeHash(data),
					_config.HashSizeInBits);
			}
		}

		protected override IHashValue ComputeHashInternal(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			using (var hashAlgorithm = _config.InstanceFactory())
			{
				return new HashValue(
					hashAlgorithm.ComputeHash(data.Array, data.Offset, data.Count),
					_config.HashSizeInBits);
			}
		}
	}
}