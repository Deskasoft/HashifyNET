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

using HashifyNet.Core;

namespace HashifyNet.Algorithms.Gost
{
	/// <summary>
	/// Concrete implementation of the GOST R 34.11-2012 (Streebog) configuration.
	/// </summary>
	[DeclareHashConfigProfile(typeof(GostConfigProfile256Bits))]
	public class GostConfig : IGostConfig
	{
		/// <summary>
		/// GOST R 34.11-2012 (Streebog) supports 256 or 512 bits hash output size. It either has to be 256 bits or 512 bits.
		/// The default is 512 bits.
		/// </summary>
		public int HashSizeInBits { get; set; } = 512;
		/// <summary>
		/// Creates a new instance of <see cref="GostConfig"/> with the same configuration as the current instance.
		/// </summary>
		/// <returns>A new <see cref="IGostConfig"/> instance with the same <see cref="HashSizeInBits"/> value as the current instance.</returns>
		public IGostConfig Clone() => new GostConfig() { HashSizeInBits = HashSizeInBits };

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// Nothing to dispose
		}
	}
}
