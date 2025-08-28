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

using HashifyNet.Core.Utilities;
using System;

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Provides functionality to verify data using the Argon2id hashing algorithm.
	/// </summary>
	/// <remarks>This class is designed to securely verify whether a given attempt matches an original value using
	/// the Argon2id hashing algorithm. It requires a secret key for verification, which is provided during construction.
	/// The class implements <see cref="IDisposable"/> to ensure that sensitive data, such as the secret key, is securely
	/// cleared from memory when the object is disposed.</remarks>
	public sealed class Argon2idVerifier : IDisposable
	{
		private bool disposedValue;
		private readonly byte[] _secret;

		/// <summary>
		/// Initializes a new instance of the <see cref="Argon2idVerifier"/> class with the specified secret.
		/// </summary>
		/// <param name="secret">A byte array representing the secret key used for verification. This value cannot be null.</param>
		public Argon2idVerifier(byte[] secret)
		{
			_secret = secret;
		}

		/// <summary>
		/// Verifies whether the provided attempt matches the original value using Argon2id hashing.
		/// </summary>
		/// <param name="attempt">The byte array representing the value to verify.</param>
		/// <param name="original">The byte array representing the original hashed value to compare against.</param>
		/// <returns><see langword="true"/> if the attempt matches the original value; otherwise, <see langword="false"/>.</returns>
		public bool Verify(byte[] attempt, byte[] original)
		{
			return Argon2idVerification.Verify(attempt, original, _secret);
		}

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					ArrayHelpers.ZeroFill(_secret);
				}

				disposedValue = true;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
