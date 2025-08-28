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

using HashifyNet.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace HashifyNet.Algorithms.Blake2
{
	/// <summary>
	/// Defines a configuration for a Blake2B hash function implementation.
	/// </summary>
	public class Blake2BConfig
		: IBlake2BConfig
	{
		/// <summary>
		/// Gets the desired hash size, in bits.
		/// </summary>
		/// <value>
		/// The desired hash size, in bits.
		/// </value>
		/// <remarks>
		/// Defaults to <c>512</c>.
		/// </remarks>
		public int HashSizeInBits { get; set; } = 512;

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Key { get; set; } = null;

		/// <summary>
		/// Gets the salt.
		/// </summary>
		/// <value>
		/// The salt.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Salt { get; set; } = null;

		/// <summary>
		/// Gets the personalization sequence.
		/// </summary>
		/// <value>
		/// The personalization sequence.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Personalization { get; set; } = null;

		/// <summary>
		/// Makes a deep clone of current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public IBlake2BConfig Clone() =>
			new Blake2BConfig()
			{
				HashSizeInBits = HashSizeInBits,
				Key = Key?.ToArray(),
				Salt = Salt?.ToArray(),
				Personalization = Personalization?.ToArray(),
			};

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			if (Key != null && Key is byte[] keyArray)
			{
				ArrayHelpers.ZeroFill(keyArray);
			}
			if (Salt != null && Salt is byte[] saltArray)
			{
				ArrayHelpers.ZeroFill(saltArray);
			}
			if (Personalization != null && Personalization is byte[] personalizationArray)
			{
				ArrayHelpers.ZeroFill(personalizationArray);
			}

			Key = null;
			Salt = null;
			Personalization = null;
		}
	}
}