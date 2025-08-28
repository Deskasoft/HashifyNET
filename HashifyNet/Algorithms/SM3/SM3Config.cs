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

namespace HashifyNet.Algorithms.SM3
{
	/// <summary>
	/// Represents the configuration settings for the SM3 cryptographic hash algorithm.
	/// </summary>
	/// <remarks>This class provides the default configuration for the SM3 hash algorithm, including the hash size.
	/// It implements the <see cref="ISM3Config"/> interface, allowing for cloning of the configuration.</remarks>
	public class SM3Config : ISM3Config
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int HashSizeInBits => 256;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public ISM3Config Clone()
		{
			return new SM3Config();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// No resources to dispose
		}
	}
}
