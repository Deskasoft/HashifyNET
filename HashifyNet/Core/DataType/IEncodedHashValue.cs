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

using System.Collections.Immutable;

namespace HashifyNet
{
	/// <summary>
	/// Represents a hash value that is encoded and provides functionality to access the encoded data as well as decode it
	/// into a string representation.
	/// </summary>
	/// <remarks>This interface extends <see cref="IHashValue"/> to include operations specific to encoded hash
	/// values. It provides access to the encoded hash as an immutable byte array and the ability to decode the hash into a
	/// human-readable string.</remarks>
	public interface IEncodedHashValue : IHashValue
	{
		/// <summary>
		/// Gets the encoded hash as an immutable array of bytes.
		/// </summary>
		ImmutableArray<byte> EncodedHash { get; }

		/// <summary>
		/// Decodes the current encoded hash and returns the resulting string.
		/// </summary>
		/// <remarks>This method processes the encoded hash using the internal decoding operation and returns the
		/// decoded result.</remarks>
		/// <returns>A <see cref="string"/> representing the decoded value of the encoded hash.</returns>
		string Decode();
	}
}
