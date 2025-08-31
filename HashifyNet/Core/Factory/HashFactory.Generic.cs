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

using System;

namespace HashifyNet
{
	/// <summary>
	/// Provides a factory for creating instances of hash algorithms that implement the specified algorithm interface and
	/// configuration type.
	/// </summary>
	/// <remarks>This factory is designed to create instances of hash algorithms that conform to the specified
	/// interface and configuration type. It supports creating algorithms with default configurations or with a
	/// user-specified configuration.</remarks>
	/// <typeparam name="TAlgorithmInterface">The interface type that the hash algorithm implements. This must be a type that implements <see
	/// cref="IHashFunction{TConfig}"/>.</typeparam>
	/// <typeparam name="TConfig">The type of the configuration object used to configure the hash algorithm. This must be a type that implements <see
	/// cref="IHashConfig{TConfig}"/>.</typeparam>
	public sealed partial class HashFactory<TAlgorithmInterface, TConfig> : IGenericHashFactory<TAlgorithmInterface, TConfig> where TAlgorithmInterface : IHashFunction<TConfig>
		where TConfig : IHashConfig<TConfig>
	{
		private static HashFactory<TAlgorithmInterface, TConfig> _singleton = null;
		private static HashFactory<TAlgorithmInterface, TConfig> Singleton
		{
			get
			{
				if (_singleton == null)
				{
					_singleton = new HashFactory<TAlgorithmInterface, TConfig>();
				}

				return _singleton;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public TAlgorithmInterface CreateInstance()
		{
			return (TAlgorithmInterface)HashFactory.Create(typeof(TAlgorithmInterface));
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="config"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public TAlgorithmInterface CreateInstance(TConfig config)
		{
			return (TAlgorithmInterface)HashFactory.Create(typeof(TAlgorithmInterface), config);
		}
	}
}
