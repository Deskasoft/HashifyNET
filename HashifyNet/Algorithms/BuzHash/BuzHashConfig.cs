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
using System.Collections.Generic;
using System.Linq;

namespace HashifyNet.Algorithms.BuzHash
{
	/// <summary>
	/// Defines a configuration for a <see cref="IBuzHash"/> implementation.
	/// </summary>
	[DeclareHashConfigProfile(typeof(DefaultBuzHashConfig))]
	public class BuzHashConfig
		: IBuzHashConfig
	{
		/// <summary>
		/// Gets a list of <c>256</c> (preferably random and distinct) <see cref="long"/> values.
		/// </summary>
		/// <value>
		/// List of 256 <see cref="long"/> values.
		/// </value>
		public IReadOnlyList<long> Rtab { get; set; } = null;

		/// <summary>
		/// Gets the desired hash size, in bits.
		/// </summary>
		/// <value>
		/// The desired hash size, in bits.
		/// </value>
		/// <remarks>
		/// Defaults to <c>64</c>.
		/// </remarks>
		public int HashSizeInBits { get; set; } = 64;

		/// <summary>
		/// Gets the seed value.
		/// </summary>
		/// <value>
		/// The seed value.
		/// </value>
		/// <remarks>
		/// Defaults to <c>0L</c>
		/// </remarks>
		public long Seed { get; set; }

		/// <summary>
		/// Gets the shift direction.
		/// </summary>
		/// <value>
		/// The shift direction.
		/// </value>
		/// <remarks>
		/// Defaults to <see cref="CircularShiftDirection.Left"/>.
		/// </remarks>
		public CircularShiftDirection ShiftDirection { get; set; } = CircularShiftDirection.Left;

		/// <summary>
		/// Makes a deep clone of the current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public IBuzHashConfig Clone() =>
			new BuzHashConfig()
			{
				Rtab = Rtab?.ToArray(),
				HashSizeInBits = HashSizeInBits,
				Seed = Seed,
				ShiftDirection = ShiftDirection
			};
	}
}
