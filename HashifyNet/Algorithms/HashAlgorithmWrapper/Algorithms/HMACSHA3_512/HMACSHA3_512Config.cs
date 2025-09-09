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

namespace HashifyNet.Algorithms.HMACSHA3_512
{
	/// <summary>
	/// Represents the configuration settings for the HMACSHA3_512 hashing algorithm.
	/// </summary>
	/// <remarks>This class provides a fixed configuration for the HMACSHA3_512 algorithm, which always produces a 512-bit
	/// hash. It includes the hash size and supports cloning to create independent copies of the configuration.</remarks>
	public class HMACSHA3_512Config : IHMACSHA3_512Config
	{
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="HMACSHA3_512Config"/> class with default settings.
		/// </summary>
		public HMACSHA3_512Config()
		{
			HashSizeInBits = 512; // HMACSHA3_512 produces a fixed 512-bit hash
		}

		/// <summary>
		/// Gets the size of the hash in bits. For HMACSHA3_512, this is always fixed at 512 bits.
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
		/// Creates a deep copy of the current <see cref="HMACSHA3_512Config"/> instance.
		/// </summary>
		/// <returns>A new <see cref="HMACSHA3_512Config"/> instance with the same settings.</returns>
		public IHMACSHA3_512Config Clone()
		{
			return new HMACSHA3_512Config()
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
		// ~HMACSHA3_512Config()
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
