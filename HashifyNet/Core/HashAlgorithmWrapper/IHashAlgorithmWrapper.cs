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
using System.IO;

namespace HashifyNet.Core.HashAlgorithm
{
	/// <summary>
	/// Implementation of <see cref="IHashFunction{FName}" /> that wraps cryptographic hash functions known as <see cref="System.Security.Cryptography.HashAlgorithm" />.
	/// </summary>
	public interface IHashAlgorithmWrapper
		: IHashFunction<IHashAlgorithmWrapperConfig>
	{
		/// <summary>
		/// Computes hash value for the given stream.
		/// </summary>
		/// <param name="data">Stream of data to hash.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be readable.;<paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be seekable for this type of hash function.;<paramref name="data"/></exception>
		IHashValue ComputeHash(Stream data);
	}
}