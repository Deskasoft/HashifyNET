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

namespace HashifyNet.Algorithms.ELF64
{
	/// <summary>
	/// Represents the configuration for the ELF64 hashing algorithm.
	/// </summary>
	/// <remarks>This class provides the configuration details for the ELF64 hashing algorithm, including the hash
	/// size and the ability to create a copy of the configuration. It implements the <see cref="IELF64Config"/>
	/// interface.</remarks>
	public class ELF64Config : IELF64Config
	{
		/// <summary>
		/// Gets the size of the hash, in bits, produced by the algorithm.
		/// </summary>
		public int HashSizeInBits => 32;

		/// <summary>
		/// Creates a new instance of the <see cref="IELF64Config"/> with the same configuration as the current instance.
		/// </summary>
		/// <returns>A new <see cref="IELF64Config"/> instance that is a copy of the current instance.</returns>
		public IELF64Config Clone() => new ELF64Config();
	}
}
