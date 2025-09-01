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

using Isopoh.Cryptography.Argon2;
using System;
using System.Linq;

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Provides methods for verifying the integrity or authenticity of data hashed using the Argon2id algorithm.
	/// </summary>
	/// <remarks>This class includes static methods to compare hashed data segments or byte arrays, ensuring that
	/// the provided data matches the expected original data. It is designed for use cases where Argon2id hashing is
	/// employed for secure password storage or data integrity verification.</remarks>
	public static class Argon2idVerification
	{
		/// <summary>
		/// Verifies the integrity or authenticity of the specified data.
		/// </summary>
		/// <param name="attempt">The attempted non-hashed password. Cannot be <see langword="null"/>.</param>
		/// <param name="original">The original password hashed as Argon2id string. Cannot be <see langword="null"/>.</param>
		/// <param name="secret">The original secret hashed with the original password. Can be empty or <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the data is successfully verified; otherwise, <see langword="false"/>.</returns>
		public static bool Verify(byte[] attempt, byte[] original, byte[] secret = null)
		{
			if (attempt == null)
			{
				throw new ArgumentNullException(nameof(attempt));
			}

			if (original == null)
			{
				throw new ArgumentNullException(nameof(original));
			}

			string originalString = Argon2idSerializer.Deserialize(original);

			Argon2Config originalConfig = new Argon2Config();
			DecodeExtension.DecodeString(originalConfig, originalString, out var originalBuffer);

			originalConfig.Password = attempt;
			originalConfig.Secret = secret;
			string attemptString = Argon2.Hash(originalConfig);

			Argon2Config attemptConfig = new Argon2Config();
			DecodeExtension.DecodeString(attemptConfig, attemptString, out var attemptBuffer);

			try
			{
				return Argon2.FixedTimeEquals(attemptBuffer, originalBuffer);
			}
			finally
			{
				attemptBuffer.Dispose();
				originalBuffer.Dispose();
			}
		}

		/// <summary>
		/// Verifies whether the provided hash attempt matches the original hash using the Argon2id algorithm.
		/// </summary>
		/// <param name="original">The original hash value to compare against. Cannot be <see langword="null"/>.</param>
		/// <param name="attempt">The attempted non-hashed password. Cannot be <see langword="null"/>.</param>
		/// <param name="secret">The original secret hashed with the original password. Can be empty or <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the hash attempt matches the original hash; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="attempt"/> or <paramref name="original"/> is <see langword="null"/>.</exception>
		public static bool VerifyArgon2id(this IHashValue original, byte[] attempt, byte[] secret = null)
		{
			if (attempt == null)
			{
				throw new ArgumentNullException(nameof(attempt));
			}

			if (original == null)
			{
				throw new ArgumentNullException(nameof(original));
			}
			
			if (!(original is IEncodedHashValue encodedHashValue))
			{
				throw new ArgumentException($"The provided {nameof(original)} must implement {nameof(IEncodedHashValue)}. The given hash value is likely not computed by Argon2id.", nameof(original));
			}

			return Verify(attempt, encodedHashValue.EncodedHash.ToArray(), secret);
		}
	}
}
