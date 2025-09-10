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
	/// Represents the configuration profile for the 512-bit FNV-1 hash function.
	/// </summary>
	/// <remarks>This class defines the parameters specific to the 512-bit variant of the Fowler-Noll-Vo (FNV-1)
	/// hash algorithm, including the hash size, prime, and offset values. These parameters are based on the standard
	/// specification for 512-bit FNV-1 hashes and are used to configure the hash computation.</remarks>
	[DefineHashConfigProfile("512Bits", "Represents the configuration profile for the 512-bit FNV-1 hash function.")]
	public sealed class FNVConfigProfile512Bits : FNVConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FNVConfigProfile512Bits"/> class, configuring the parameters for a
		/// 512-bit FNV-1 hash function.
		/// </summary>
		/// <remarks>This constructor sets the hash size to 512 bits and initializes the prime and offset values
		/// specific to the 512-bit FNV-1 hash algorithm. These values are used in the computation of the hash and are based
		/// on the standard FNV-1 specification for 512-bit hashes.</remarks>
		public FNVConfigProfile512Bits()
		{
			HashSizeInBits = 512;
			Prime = BigInteger.Parse("35835915874844867368919076489095108449946327955754392558399825615420669938882575126094039892345713852759");
			Offset = BigInteger.Parse("9659303129496669498009435400716310466090418745672637896108374329434462657994582932197716438449813051892206539805784495328239340083876191928701583869517785");
		}
	}
}
