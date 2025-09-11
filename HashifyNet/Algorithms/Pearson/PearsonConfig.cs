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
using HashifyNet.Core;

namespace HashifyNet.Algorithms.Pearson
{
	/// <summary>
	/// Defines a configuration for a <see cref="IPearson"/> implementation.
	/// </summary>
	[DeclareHashConfigProfile(typeof(PearsonConfigProfileWikipedia))]
	public class PearsonConfig
		: IPearsonConfig
	{
		/// <summary>
		/// A 256-length lookup table of a permutation of [0, 255].
		/// </summary>
		/// <value>
		/// The table.
		/// </value>
		public IReadOnlyList<byte> Table { get; set; } = null;

		/// <summary>
		/// Gets the desired hash size, in bits.
		/// </summary>
		/// <value>
		/// The desired hash size, in bits.
		/// </value>
		public int HashSizeInBits { get; set; } = 8;

		/// <summary>
		/// Makes a deep clone of current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public IPearsonConfig Clone() =>
			new PearsonConfig()
			{
				Table = Table?.ToArray(),
				HashSizeInBits = HashSizeInBits
			};
	}
}
