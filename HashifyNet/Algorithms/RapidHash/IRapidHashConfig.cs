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

namespace HashifyNet.Algorithms.RapidHash
{
	/// <summary>
	/// Defines the configuration for the RapidHash algorithm, including fixed hash size and seed customization.
	/// </summary>
	/// <remarks>This interface extends <see cref="IHashConfig{T}"/> to provide configuration specific to the
	/// RapidHash algorithm. The hash size is fixed at 64 bits, and a seed value can be specified to customize the
	/// underlying key used in the hashing process.</remarks>
	public interface IRapidHashConfig : IHashConfig<IRapidHashConfig>
	{
		/// <summary>
		/// <inheritdoc cref="IHashConfigBase.HashSizeInBits"/>
		/// <para>For RapidHash, this is fixed at 64 bits.</para>
		/// </summary>
		new int HashSizeInBits { get; }

		/// <summary>
		/// Gets the seed value used to customize the underlying key.
		/// </summary>
		ulong Seed { get; }

		/// <summary>
		/// Gets the mode of operation for the RapidHash algorithm.
		/// </summary>
		RapidHashMode Mode { get; }
	}
}
