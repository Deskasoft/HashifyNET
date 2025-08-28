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
using System.Threading;
using System.Threading.Tasks;

namespace HashifyNet
{
	/// <summary>
	/// Defines the base functionality for hash computation operations.
	/// </summary>
	/// <remarks>This interface provides methods to compute hash values for byte arrays and array segments,  with
	/// support for specifying offsets, counts, and cancellation tokens. Implementations of this  interface are expected to
	/// produce consistent hash values for the same input data.</remarks>
	public interface IHashFunctionBase
	{
		/// <summary>
		/// Computes hash value for given byte array.
		/// </summary>
		/// <param name="data">Array of data to hash.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		IHashValue ComputeHash(byte[] data);

		/// <summary>
		/// Computes hash value for given byte array.
		/// </summary>
		/// <param name="data">Array of data to hash.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while calculating the hash value.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="TaskCanceledException">The <paramref name="cancellationToken"/> was canceled.</exception>
		IHashValue ComputeHash(byte[] data, CancellationToken cancellationToken);

		/// <summary>
		/// Computes hash value for given byte array.
		/// </summary>
		/// <param name="data">Array of data to hash.</param>
		/// <param name="offset">The offset from which to begin using the data.</param>
		/// <param name="count">The number of bytes to use as data.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/>;Offset must be a value greater than or equal to zero and less than or equal to the length of the array.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/>;Count must be a value greater than or equal to zero and less than the the remaining length of the array after the offset value.</exception>
		IHashValue ComputeHash(byte[] data, int offset, int count);

		/// <summary>
		/// Computes hash value for given byte array.
		/// </summary>
		/// <param name="data">Array of data to hash.</param>
		/// <param name="offset">The offset from which to begin using the data.</param>
		/// <param name="count">The number of bytes to use as data.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while calculating the hash value.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/></exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/>;Offset must be a value greater than or equal to zero and less than or equal to the length of the array.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/>;Count must be a value greater than or equal to zero and less than the the remaining length of the array after the offset value.</exception>
		/// <exception cref="TaskCanceledException">The <paramref name="cancellationToken"/> was canceled.</exception>
		IHashValue ComputeHash(byte[] data, int offset, int count, CancellationToken cancellationToken);

		/// <summary>
		/// Computes hash value for given array segment.
		/// </summary>
		/// <param name="data">Array segment of data to hash.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		IHashValue ComputeHash(ArraySegment<byte> data);

		/// <summary>
		/// Computes hash value for given array segment.
		/// </summary>
		/// <param name="data">Array segment of data to hash.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while calculating the hash value.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		/// <exception cref="TaskCanceledException">The <paramref name="cancellationToken"/> was canceled.</exception>
		IHashValue ComputeHash(ArraySegment<byte> data, CancellationToken cancellationToken);
	}
}
