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

namespace HashifyNet.Algorithms.MD5
{
	/// <summary>
	/// Represents the configuration settings for the MD5 hashing algorithm.
	/// </summary>
	/// <remarks>This class provides a fixed configuration for the MD5 algorithm, which always produces a 128-bit
	/// hash. It includes the hash size and supports cloning to create independent copies of the configuration.</remarks>
	public class MD5Config : IMD5Config
	{
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="MD5Config"/> class with default settings.
		/// </summary>
		public MD5Config()
		{
			HashSizeInBits = 128; // MD5 produces a fixed 128-bit hash
		}

		/// <summary>
		/// Gets the size of the hash in bits. For MD5, this is always fixed at 128 bits.
		/// </summary>
		public int HashSizeInBits { get; private set; }

		/// <summary>
		/// Creates a deep copy of the current <see cref="MD5Config"/> instance.
		/// </summary>
		/// <returns>A new <see cref="MD5Config"/> instance with the same settings.</returns>
		public IMD5Config Clone()
		{
			return new MD5Config();
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
					// Nothing to dispose.
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~MD5Config()
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
