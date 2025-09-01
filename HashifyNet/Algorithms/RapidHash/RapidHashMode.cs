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
	/// Specifies the hashing mode to use in the RapidHash algorithm.
	/// </summary>
	/// <remarks>The <see cref="RapidHashMode"/> enumeration defines different modes of operation for the RapidHash
	/// algorithm, allowing users to select the desired balance between performance and hash precision.</remarks>
	public enum RapidHashMode
	{
		/// <summary>
		/// General-purpose hashing, especially when you need maximum speed for large inputs (e.g., hashing files, large network packets).
		/// </summary>
		Original,
		/// <summary>
		/// High-performance computing (HPC) and server applications where you are hashing many medium-sized keys (roughly 16 to 512 bytes) and cache performance is important.
		/// </summary>
		Micro,
		/// <summary>
		/// Mobile and embedded applications (e.g., microcontrollers) where code size is the top priority. It's the fastest option for very small inputs (up to 48 bytes).
		/// </summary>
		Nano
	}
}
