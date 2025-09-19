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
using HashifyNet.Core.HashAlgorithm;
using System;
using System.Security.Cryptography;

namespace HashifyNet
{
	internal abstract class HashAlgorithmWrapperBase<TConfig> : CryptographicStreamableHashFunctionBase<TConfig> where TConfig : ICryptographicHashConfig<TConfig>
	{
		public override TConfig Config => _config.Clone();

		private readonly TConfig _config;
		private readonly IHashAlgorithmWrapper _wrapper;

		public HashAlgorithmWrapperBase(TConfig config, Func<IncrementalHash> factory)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			if (factory == null)
			{
				throw new ArgumentNullException(nameof(factory));
			}

			_config = config.Clone();

			_wrapper = new HashAlgorithmWrapper_Implementation(new HashAlgorithmWrapperConfig(factory, _config.HashSizeInBits));
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return _wrapper.CreateBlockTransformer();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_wrapper?.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
