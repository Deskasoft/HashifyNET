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

using System.Collections.Generic;
using System.Linq;

namespace HashifyNet.Algorithms.HighwayHash
{
	/// <summary>
	/// Concrete implementation of the HighwayHash configuration.
	/// </summary>
	public class HighwayHashConfig : IHighwayHashConfig
	{
		/// <summary>
		/// Gets or sets the size of the hash, in bits. The default is 64 bits.
		/// </summary>
		public int HashSizeInBits { get; set; } = 64;
		/// <summary>
		/// Gets or sets the cryptographic key as a read-only list of bytes. The default is a 32-byte array of zeros.
		/// </summary>
		public IReadOnlyList<byte> Key { get; set; } = new byte[32];

		/// <summary>
		/// Initializes a new instance of the <see cref="HighwayHashConfig"/> class.
		/// </summary>
		/// <remarks>This constructor creates a default configuration for the HighwayHash algorithm. Use this class
		/// to specify settings or parameters for HighwayHash operations.</remarks>
		public HighwayHashConfig()
		{
		}

		/// <summary>
		/// Creates a new instance of <see cref="HighwayHashConfig"/> with the same configuration as the current instance.
		/// </summary>
		/// <returns>A new <see cref="IHighwayHashConfig"/> instance that is a copy of the current configuration.</returns>
		public IHighwayHashConfig Clone() => new HighwayHashConfig()
		{
			HashSizeInBits = this.HashSizeInBits,
			Key = this.Key.ToArray()
		};
	}
}