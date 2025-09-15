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

using HashifyNet.Core;

namespace HashifyNet.Algorithms.Keccak
{
	/// <summary>
	/// Represents a configuration profile for Keccak computations that uses SHA-3 padding.
	/// </summary>
	[DefineHashConfigProfile("Sha3Padding", "Keccak computation defaulting to Sha3 Padding.")]
	public sealed class KeccakConfigProfileSha3Padding : KeccakConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeccakConfigProfileSha3Padding"/> class with default settings for
		/// SHA-3 padding.
		/// </summary>
		public KeccakConfigProfileSha3Padding()
		{
			UseSha3Padding = true;
		}
	}
}
