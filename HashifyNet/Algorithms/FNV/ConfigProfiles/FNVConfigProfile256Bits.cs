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
	/// Represents a predefined configuration profile for the 256-bit variant of the Fowler-Noll-Vo (FNV-1) hash
	/// algorithm.
	/// </summary>
	/// <remarks>This configuration profile is specifically tailored for computing 256-bit FNV-1 hashes. It defines
	/// the hash size, prime, and offset values according to the standard constants for the 256-bit FNV-1 algorithm. Use
	/// this class to simplify the setup of hash computations requiring a 256-bit hash size.</remarks>
	[DefineHashConfigProfile("256Bits", "Represents a predefined configuration profile for the 256-bit variant of the Fowler-Noll-Vo (FNV-1) hash algorithm.")]
	public sealed class FNVConfigProfile256Bits : FNVConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FNVConfigProfile256Bits"/> class with predefined settings for a
		/// 256-bit FNV-1 hash configuration.
		/// </summary>
		/// <remarks>This constructor sets the hash size to 256 bits and initializes the prime and offset values to
		/// the constants defined for the 256-bit variant of the Fowler-Noll-Vo (FNV-1) hash algorithm. These values are used
		/// to configure the hash computation.</remarks>
		public FNVConfigProfile256Bits()
		{
			HashSizeInBits = 256;
			Prime = BigInteger.Parse("374144419156711147060143317175368453031918731002211");
			Offset = BigInteger.Parse("100029257958052580907070968620625704837092796014241193945225284501741471925557");
		}
	}
}
