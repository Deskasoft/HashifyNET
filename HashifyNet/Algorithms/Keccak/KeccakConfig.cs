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

namespace HashifyNet.Algorithms.Keccak
{
	/// <summary>
	/// Represents the configuration settings for the Keccak cryptographic hash function.
	/// </summary>
	/// <remarks>This configuration allows customization of the hash size and padding scheme used by the Keccak
	/// algorithm.</remarks>
	[DeclareHashConfigProfile(typeof(KeccakConfigProfileSha3Padding))]
	public class KeccakConfig : IKeccakConfig
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool UseSha3Padding { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int HashSizeInBits { get; set; } = 512;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IKeccakConfig Clone() => new KeccakConfig() { HashSizeInBits = this.HashSizeInBits, UseSha3Padding = this.UseSha3Padding };

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// Nothing to dispose
		}
	}
}
