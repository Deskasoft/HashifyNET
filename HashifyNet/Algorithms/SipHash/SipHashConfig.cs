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

namespace HashifyNet.Algorithms.SipHash
{
	/// <summary>
	/// Concrete implementation of the SipHash configuration.
	/// </summary>
	public class SipHashConfig : ISipHashConfig
	{
		/// <summary>
		/// Gets the size of the hash, in bits, produced by the algorithm.
		/// </summary>
		public int HashSizeInBits => 64;
		/// <summary>
		/// Gets or sets the cryptographic key as a read-only list of bytes.
		/// </summary>
		public IReadOnlyList<byte> Key { get; set; } = new byte[16];
		/// <summary>
		/// Gets or sets the number of computation rounds to be performed.
		/// </summary>
		public int C_Rounds { get; set; } = 2;
		/// <summary>
		/// Gets or sets the number of rounds used in the operation.
		/// </summary>
		public int D_Rounds { get; set; } = 4;

		/// <summary>
		/// Initializes a new instance of the SipHashConfig class.
		/// </summary>
		/// <param name="key">The 16-byte secret key. Must not be null.</param>
		/// <param name="c_Rounds">Number of compression rounds. Defaults to 2 for SipHash-2-4.</param>
		/// <param name="d_Rounds">Number of finalization rounds. Defaults to 4 for SipHash-2-4.</param>
		public SipHashConfig(IReadOnlyList<byte> key, int c_Rounds = 2, int d_Rounds = 4)
		{
			Key = key;
			C_Rounds = c_Rounds;
			D_Rounds = d_Rounds;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SipHashConfig"/> class.
		/// </summary>
		/// <remarks>This constructor creates a default configuration for the SipHash algorithm. Use the properties
		/// of this class to customize the configuration as needed.</remarks>
		public SipHashConfig()
		{
		}

		/// <summary>
		/// Creates a new instance of <see cref="ISipHashConfig"/> with the same configuration values as the current instance.
		/// </summary>
		/// <returns>A new <see cref="ISipHashConfig"/> instance that is a copy of the current configuration.</returns>
		public ISipHashConfig Clone() => new SipHashConfig(Key.ToArray(), C_Rounds, D_Rounds);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			if (Key is byte[] keyArray)
			{
				ArrayHelpers.ZeroFill(keyArray);
			}

			Key = null;
		}
	}
}