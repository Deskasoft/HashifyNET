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
	/// Represents the configuration profile for the Fowler-Noll-Vo (FNV) hash algorithm with a 1024-bit hash size.
	/// </summary>
	/// <remarks>This class defines the parameters specific to the 1024-bit variant of the FNV hash algorithm,
	/// including the hash size, prime, and offset values. These parameters are pre-defined constants tailored for the
	/// 1024-bit configuration. Use this profile when a 1024-bit hash size is required for hashing operations.</remarks>
	[DefineHashConfigProfile("1024Bits", "Represents the configuration profile for the Fowler-Noll-Vo (FNV) hash algorithm with a 1024-bit hash size.")]
	public sealed class FNVConfigProfile1024Bits : FNVConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FNVConfigProfile1024Bits"/> class, configuring the Fowler-Noll-Vo
		/// (FNV) hash algorithm for 1024-bit hash size.
		/// </summary>
		/// <remarks>This constructor sets the hash size to 1024 bits and initializes the prime and offset values used
		/// in the FNV-1 hash algorithm. The prime and offset values are pre-defined constants specific to the 1024-bit
		/// variant of the algorithm.</remarks>
		public FNVConfigProfile1024Bits()
		{
			HashSizeInBits = 1024;
			Prime = BigInteger.Parse("5016456510113118655434598811035278955030765345404790744303017523831112055108147451509157692220295382716162651878526895249385292291816524375083746691371804094271873160484737966720260389217684476157468082573");
			Offset = BigInteger.Parse("14197795064947621068722070641403218320880622795441933960878474914617582723252296732303717722150864096521202355549365628174669108571814760471015076148029755969804077320157692458563003215304957150157403644460363550505412711285966361610267868082893823963790439336411086884584107735010676915");
		}
	}
}
