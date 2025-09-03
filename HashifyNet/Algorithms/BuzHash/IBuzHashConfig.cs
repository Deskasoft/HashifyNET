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

namespace HashifyNet.Algorithms.BuzHash
{
	/// <summary>
	/// Defines a configuration for a <see cref="IBuzHash"/> implementation.
	/// </summary>
	public interface IBuzHashConfig : IHashConfig<IBuzHashConfig>
	{
		/// <summary>
		/// <inheritdoc cref="IHashConfigBase.HashSizeInBits"/>
		/// <para>Implementation expects the value to be <c>8</c>, <c>16</c>, <c>32</c>, and <c>64</c>.</para>
		/// </summary>
		new int HashSizeInBits { get; }

		/// <summary>
		/// Gets a list of <c>256</c> (preferably random and distinct) <see cref="long"/> values.
		/// </summary>
		/// <value>
		/// List of 256 <see cref="long"/> values.
		/// </value>
		IReadOnlyList<long> Rtab { get; }

		/// <summary>
		/// Gets the seed value.
		/// </summary>
		/// <value>
		/// The seed value.
		/// </value>
		/// <remarks>
		/// Only the bottom <see cref="IHashConfigBase.HashSizeInBits"/> bits should be used for a given configuration.
		/// </remarks>
		long Seed { get; }

		/// <summary>
		/// Gets the shift direction.
		/// </summary>
		/// <value>
		/// The shift direction.
		/// </value>
		CircularShiftDirection ShiftDirection { get; }
	}
}
