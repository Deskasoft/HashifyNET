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
using System.Numerics;

namespace HashifyNet.Algorithms.FNV
{
	/// <summary>
	/// Represents the configuration profile for the 128-bit Fowler-Noll-Vo (FNV) hash algorithm.
	/// </summary>
	/// <remarks>This class defines the parameters specific to the 128-bit variant of the FNV-1 and FNV-1a hash
	/// algorithms, including the hash size, prime, and offset values. These parameters are based on the standard
	/// constants for the 128-bit FNV hash function. <para> Use this class to configure and initialize a hash function
	/// that operates with a 128-bit hash size.</para></remarks>
	[DefineHashConfigProfile("128Bits", "Represents the configuration profile for the 128-bit Fowler-Noll-Vo (FNV) hash algorithm.")]
	public sealed class FNVConfigProfile128Bits : FNVConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FNVConfigProfile128Bits"/> class, configuring the parameters for a
		/// 128-bit Fowler-Noll-Vo hash function.
		/// </summary>
		/// <remarks>This constructor sets the hash size to 128 bits and initializes the prime and offset values
		/// specific to the 128-bit FNV-1/FNV-1a hash algorithm. These values are based on the standard constants defined for
		/// the 128-bit variant of the algorithm.</remarks>
		public FNVConfigProfile128Bits()
		{
			HashSizeInBits = 128;
			Prime = BigInteger.Parse("309485009821345068724781371");
			Offset = BigInteger.Parse("144066263297769815596495629667062367629");
		}
	}
}
