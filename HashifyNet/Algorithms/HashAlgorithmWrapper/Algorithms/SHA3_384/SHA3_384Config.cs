#if NET8_0_OR_GREATER

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

namespace HashifyNet.Algorithms.SHA3_384
{
	/// <summary>
	/// Represents the configuration settings for the SHA3_384 hashing algorithm.
	/// </summary>
	/// <remarks>This class provides a fixed configuration for the SHA3_384 algorithm, which always produces a 384-bit
	/// hash. It includes the hash size and supports cloning to create independent copies of the configuration.</remarks>
	public class SHA3_384Config : ISHA3_384Config
	{
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="SHA3_384Config"/> class with default settings.
		/// </summary>
		public SHA3_384Config()
		{
			HashSizeInBits = 384; // SHA3_384 produces a fixed 384-bit hash
		}

		/// <summary>
		/// Gets the size of the hash in bits. For SHA3_384, this is always fixed at 384 bits.
		/// </summary>
		public int HashSizeInBits { get; private set; }

		/// <summary>
		/// Gets or sets the secret key for the hash algorithm.
		/// </summary>
#if NET8_0_OR_GREATER
#nullable enable
		public byte[]?
#nullable restore
#else
		public byte[]
#endif // NET8_0_OR_GREATER
		Key
		{ get; set; } = null;

		/// <summary>
		/// Creates a deep copy of the current <see cref="SHA3_384Config"/> instance.
		/// </summary>
		/// <returns>A new <see cref="SHA3_384Config"/> instance with the same settings.</returns>
		public ISHA3_384Config Clone()
		{
			return new SHA3_384Config()
			{
				Key = (byte[])Key?.Clone(),
			};
		}

		/// <summary>
		/// Releases the resources used by the current instance of the class.
		/// </summary>
		/// <remarks>This method is called to release both managed and unmanaged resources. Override this method in a
		/// derived class to provide custom cleanup logic.</remarks>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only
		/// unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					if (Key != null)
					{
						ArrayHelpers.ZeroFill(Key);
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~SHA3_384Config()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			System.GC.SuppressFinalize(this);
		}
	}
}

#endif // NET8_0_OR_GREATER
