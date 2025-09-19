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

namespace HashifyNet.Algorithms.SHA256
{
	/// <summary>
	/// Represents the configuration settings for the SHA256 cryptographic hash algorithm.
	/// </summary>
	/// <remarks>This interface extends <see cref="ICryptographicHashConfig{T}"/> to provide configuration specific
	/// to the SHA256 algorithm. The <see cref="HashSizeInBits"/> property is fixed at <c>256</c> bits, as defined by the SHA256
	/// standard.</remarks>
	public interface ISHA256Config : ICryptographicHashConfig<ISHA256Config>
	{
		/// <summary>
		/// <inheritdoc cref="IHashConfigBase.HashSizeInBits"/>
		/// <para>For SHA256, this is always fixed at <c>256</c> bits.</para>
		/// </summary>
		new int HashSizeInBits { get; }

		/// <summary>
		/// Gets the secret key for the hash algorithm.
		/// </summary>
		byte[] Key { get; }
	}
}
