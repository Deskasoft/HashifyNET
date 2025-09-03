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

using System.Collections.Immutable;
using System.Linq;

namespace HashifyNet
{
	/// <summary>
	/// Provides methods for securely comparing byte arrays to prevent timing attacks.
	/// </summary>
	/// <remarks>The <see cref="HashComparer"/> class is designed to perform cryptographic-safe comparisons of byte
	/// arrays. It ensures that comparisons are resistant to timing attacks, which can exploit differences in execution
	/// time to infer information about the data being compared. This class is particularly useful in scenarios where
	/// sensitive data, such as cryptographic hashes or authentication tokens, must be compared securely.</remarks>
	public static class HashComparer
	{
		/// <summary>
		/// Compares two byte arrays for equality in a way that is resistant to timing attacks.
		/// </summary>
		/// <remarks>This method performs a constant-time comparison to prevent timing attacks, which can occur when
		/// the time taken to compare two values leaks information about their contents. The method ensures that the
		/// comparison time depends only on the length of the arrays, not their contents. <para> If the arrays have different
		/// lengths, the method returns <see langword="false"/> immediately. </para></remarks>
		/// <param name="left">The first byte array to compare. Cannot be <see langword="null"/>.</param>
		/// <param name="right">The second byte array to compare. Cannot be <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the two byte arrays are equal; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if either <paramref name="left"/> or <paramref name="right"/> is <see langword="null"/>.</exception>
		public static bool FixedTimeEquals(byte[] left, byte[] right)
		{
			if (left == null)
			{
				throw new System.ArgumentNullException(nameof(left));
			}

			if (right == null)
			{
				throw new System.ArgumentNullException(nameof(right));
			}

#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETSTANDARD2_1 || NET5_0_OR_GREATER
            return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(left, right);
#else
			if (left.Length != right.Length)
			{
				return false;
			}

			int length = left.Length;
			int accumulator = 0;

			for (int i = 0; i < length; i++)
			{
				accumulator |= left[i] - right[i];
			}

			return accumulator == 0;
#endif
		}

		/// <summary>
		/// Compares two immutable byte arrays for equality in a way that is resistant to timing attacks.
		/// </summary>
		/// <remarks>This method performs a constant-time comparison to prevent timing attacks, which can occur when
		/// the time taken to compare two values leaks information about their contents. The method ensures that the
		/// comparison time depends only on the length of the immutable  arrays, not their contents. <para> If the immutable arrays have different
		/// lengths, the method returns <see langword="false"/> immediately. </para></remarks>
		/// <param name="left">The first byte array to compare. Cannot be <see langword="null"/>.</param>
		/// <param name="right">The second byte array to compare. Cannot be <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the two immutable byte arrays are equal; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if either <paramref name="left"/> or <paramref name="right"/> is <see langword="null"/>.</exception>
		public static bool FixedTimeEquals(ImmutableArray<byte> left, ImmutableArray<byte> right)
		{
			return FixedTimeEquals(left.ToArray(), right.ToArray());
		}
	}
}




