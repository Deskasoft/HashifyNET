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
	/// Represents the configuration profile for a 32-bit FNV (Fowler–Noll–Vo) hash function.
	/// </summary>
	/// <remarks>This profile defines the parameters for the 32-bit FNV-1 and FNV-1a hash algorithms,  including the
	/// hash size, prime, and offset values. It is designed for use in scenarios where a 32-bit hash is required, such as
	/// checksum calculations or hash-based data structures.</remarks>
	[DefineHashConfigProfile("32Bits", "Represents the configuration profile for a 32-bit FNV (Fowler–Noll–Vo) hash function.")]
	public sealed class FNVConfigProfile32Bits : FNVConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FNVConfigProfile32Bits"/> class, configuring the parameters for a
		/// 32-bit FNV (Fowler–Noll–Vo) hash function.
		/// </summary>
		/// <remarks>This constructor sets the hash size to 32 bits and initializes the prime and offset  values
		/// commonly used for the 32-bit FNV-1 and FNV-1a hash algorithms.</remarks>
		public FNVConfigProfile32Bits()
		{
			HashSizeInBits = 32;
			Prime = new BigInteger(16777619);
			Offset = new BigInteger(2166136261);
		}
	}
}
