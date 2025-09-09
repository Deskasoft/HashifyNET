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

namespace HashifyNet.Core
{
	/// <summary>
	/// Common interface to non-cryptographic hash functions that can be computed over a stream of data without buffering.
	/// </summary>
	public abstract class CryptographicStreamableHashFunctionBase<CName>
		: StreamableHashFunctionBase<CName>,
			IStreamableHashFunction<CName>, ICryptographicStreamableHashFunction<CName> where CName : ICryptographicHashConfig<CName>
	{
		private bool disposedValue;

		/// <summary>
		/// Releases the resources used by the current instance of the class.
		/// </summary>
		/// <remarks>This method releases both managed and unmanaged resources. Call this method when you are 
		/// finished using the object to ensure that all resources are properly released. If overriding  this method, ensure
		/// that all resources are disposed of and the base class implementation is called.</remarks>
		/// <param name="disposing">A value indicating whether to release both managed and unmanaged resources (<see langword="true"/>) or only
		/// unmanaged resources (<see langword="false"/>).</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					Config?.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
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
			System.GC.SuppressFinalize(this);
		}
	}

}
