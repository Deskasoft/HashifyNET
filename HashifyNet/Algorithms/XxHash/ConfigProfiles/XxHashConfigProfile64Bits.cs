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

namespace HashifyNet.Algorithms.XxHash
{
	/// <summary>
	/// Represents a configuration profile for XXHash operations with a default hash size of 64 bits.
	/// </summary>
	/// <remarks>This class provides a predefined configuration for XXHash algorithms, specifically tailored for
	/// 64-bit hash computations. It is intended to simplify the setup of XXHash operations by providing default settings
	/// optimized for 64-bit hashing.</remarks>
	[DefineHashConfigProfile("64Bits", "Provides a default 64 bit config for XxHash.")]
	public sealed class XxHashConfigProfile64Bits : XxHashConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XxHashConfigProfile64Bits"/> class with a default hash size of 64
		/// bits.
		/// </summary>
		/// <remarks>This class is designed to configure settings for 64-bit XXHash operations.</remarks>
		public XxHashConfigProfile64Bits()
		{
			HashSizeInBits = 64;
		}
	}
}
