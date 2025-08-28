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

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Specifies the version of the Argon2id hashing algorithm to use.
	/// </summary>
	/// <remarks>The Argon2id algorithm supports multiple versions, which may differ in their internal behavior or
	/// compatibility. Use <see cref="Version10"/> for the original version 1.0 specification, or <see cref="Version13"/>
	/// for the updated version 1.3 specification.</remarks>
	public enum Argon2idVersion
	{
		/// <summary>
		/// Represents version 1.0 in the versioning of Argon2id.
		/// </summary>
		Version10 = 0x10,
		/// <summary>
		/// Represents version 13 in the versioning of Argon2id. This is the default and most commonly used version.
		/// </summary>
		Version13 = 0x13
	}
}
