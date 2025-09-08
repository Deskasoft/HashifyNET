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
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HashifyNet
{
	/// <summary>
	/// Defines the base functionality for a streamable hash function, allowing iterative or asynchronous processing of
	/// data streams to compute hash values.
	/// </summary>
	/// <remarks>This interface provides methods for creating block transformers for iterative hashing, as well as
	/// synchronous and asynchronous methods for computing hash values from data streams. Implementations may impose
	/// specific requirements on the input streams, such as requiring them to be readable or seekable.</remarks>
	public interface IStreamableHashFunctionBase
	{
		/// <summary>
		/// Creates a new transformer that will process data and hold the internal state using this hash function's algorithm.
		/// </summary>
		/// <returns>A new instance of <see cref="IBlockTransformer"/> that can process input iteratively and produce a final <see cref="IHashValue"/> for that input.</returns>
		IBlockTransformer CreateBlockTransformer();

		/// <summary>
		/// Computes hash value for given stream.
		/// </summary>
		/// <param name="data">Stream of data to hash.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be readable.;<paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be seekable for this type of hash function.;<paramref name="data"/></exception>
		IHashValue ComputeHash(Stream data);

		/// <summary>
		/// Computes hash value for given stream.
		/// </summary>
		/// <param name="data">Stream of data to hash.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while calculating the hash value.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be readable.;<paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be seekable for this type of hash function.;<paramref name="data"/></exception>
		/// <exception cref="TaskCanceledException">The <paramref name="cancellationToken"/> was canceled.</exception>
		IHashValue ComputeHash(Stream data, CancellationToken cancellationToken);

		/// <summary>
		/// Computes hash value for given stream asynchronously.
		/// </summary>
		/// <param name="data">Stream of data to hash.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <remarks>
		/// All stream IO is done via ReadAsync.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be readable.;<paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be seekable for this type of hash function.;<paramref name="data"/></exception>
		Task<IHashValue> ComputeHashAsync(Stream data);

		/// <summary>
		/// Computes hash value for given stream asynchronously.
		/// </summary>
		/// <param name="data">Stream of data to hash.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while calculating the hash value.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <remarks>
		/// All stream IO is done via ReadAsync.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be readable.;<paramref name="data"/></exception>
		/// <exception cref="ArgumentException">Stream must be seekable for this type of hash function.;<paramref name="data"/></exception>
		/// <exception cref="TaskCanceledException">The <paramref name="cancellationToken"/> was canceled.</exception>
		Task<IHashValue> ComputeHashAsync(Stream data, CancellationToken cancellationToken);
	}
}
