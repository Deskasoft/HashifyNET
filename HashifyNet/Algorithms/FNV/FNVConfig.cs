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
using System.Numerics;

namespace HashifyNet.Algorithms.FNV
{
	/// <summary>
	/// Defines a configuration for a <see cref="IFNV"/> implementation.
	/// </summary>
	[DeclareHashConfigProfile(typeof(FNVConfigProfile32Bits))]
	[DeclareHashConfigProfile(typeof(FNVConfigProfile64Bits))]
	[DeclareHashConfigProfile(typeof(FNVConfigProfile128Bits))]
	[DeclareHashConfigProfile(typeof(FNVConfigProfile256Bits))]
	[DeclareHashConfigProfile(typeof(FNVConfigProfile512Bits))]
	[DeclareHashConfigProfile(typeof(FNVConfigProfile1024Bits))]
	public class FNVConfig
		: IFNVConfig
	{
		/// <summary>
		/// Length of the produced hash, in bits.
		/// </summary>
		/// <value>
		/// The length of the produced hash, in bits
		/// </value>
		public int HashSizeInBits { get; set; }

		/// <summary>
		/// The prime integer to use when calculating the FNV value.
		/// </summary>
		/// <value>
		/// The prime value.
		/// </value>
		public BigInteger Prime { get; set; }

		/// <summary>
		/// The offset integer to use when calculating the FNV value.
		/// </summary>
		/// <value>
		/// The offset value.
		/// </value>
		public BigInteger Offset { get; set; }

		/// <summary>
		/// Makes a deep clone of current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public IFNVConfig Clone() =>
			new FNVConfig()
			{
				HashSizeInBits = HashSizeInBits,
				Prime = Prime,
				Offset = Offset
			};
	}
}
