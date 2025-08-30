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

using System;
using SystemHashAlgorithm = System.Security.Cryptography.HashAlgorithm;

namespace HashifyNet.Core.HashAlgorithm
{
	/// <summary>
	/// Defines a configuration for a <see cref="IHashAlgorithmWrapper"/> implementation.
	/// </summary>
	public class HashAlgorithmWrapperConfig
		: IHashAlgorithmWrapperConfig
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int HashSizeInBits { get; }

		/// <summary>
		/// A delegate that produces <see cref="SystemHashAlgorithm"/> instances.
		/// </summary>
		/// <value>
		/// The delegate.
		/// </value>
		public Func<SystemHashAlgorithm> InstanceFactory { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HashAlgorithmWrapperConfig"/> class with the specified hash algorithm
		/// factory and expected hash size.
		/// </summary>
		/// <param name="instanceFactory">A factory method that creates instances of the hash algorithm.</param>
		/// <param name="expectedHashSize">The expected size of the hash, in bits. This value is used to validate the hash size produced by the algorithm.</param>
		public HashAlgorithmWrapperConfig(Func<SystemHashAlgorithm> instanceFactory, int expectedHashSize)
		{
			HashSizeInBits = expectedHashSize;
			InstanceFactory = instanceFactory;
		}

		/// <summary>
		/// Makes a deep clone of the current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public IHashAlgorithmWrapperConfig Clone() =>
			new HashAlgorithmWrapperConfig(InstanceFactory, HashSizeInBits);

   		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			InstanceFactory = null;
		}
	}
}
