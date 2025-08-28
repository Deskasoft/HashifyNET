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

namespace HashifyNet.Algorithms.SipHash
{
	/// <summary>
	/// Configuration for the SipHash algorithm.
	/// </summary>
	public interface ISipHashConfig : ICryptographicHashConfig<ISipHashConfig>
	{
		/// <summary>
		/// <inheritdoc cref="IHashConfigBase.HashSizeInBits"/>
		/// <para>For SipHash, this is fixed at 64 bits.</para>
		/// </summary>
		new int HashSizeInBits { get; }

		/// <summary>
		/// The 128-bit (16-byte) secret key. This is mandatory.
		/// </summary>
		IReadOnlyList<byte> Key { get; }

		/// <summary>
		/// The number of compression rounds per message block.
		/// </summary>
		int C_Rounds { get; }

		/// <summary>
		/// The number of finalization rounds.
		/// </summary>
		int D_Rounds { get; }
	}
}