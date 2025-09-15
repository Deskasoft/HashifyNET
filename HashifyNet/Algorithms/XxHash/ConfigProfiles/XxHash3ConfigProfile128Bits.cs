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

namespace HashifyNet.Algorithms.XxHash3
{
	/// <summary>
	/// Represents a predefined configuration profile for XxHash3 with a 128-bit hash size.
	/// </summary>
	/// <remarks>This configuration profile is designed to provide a default setup for generating 128-bit hashes
	/// using the XxHash3 algorithm. It sets the <see cref="XxHash3Config.HashSizeInBits"/> property to 128.</remarks>
	[DefineHashConfigProfile("128Bits", "Provides a default 128 bit config for XxHash3.")]
	public sealed class XxHash3ConfigProfile128Bits : XxHash3Config
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XxHash3ConfigProfile128Bits"/> class with a default hash size of 128
		/// bits.
		/// </summary>
		/// <remarks>This constructor sets the <see cref="XxHash3Config.HashSizeInBits"/> property to 128, which is the standard
		/// hash size for this configuration profile.</remarks>
		public XxHash3ConfigProfile128Bits()
		{
			HashSizeInBits = 128;
		}
	}
}
