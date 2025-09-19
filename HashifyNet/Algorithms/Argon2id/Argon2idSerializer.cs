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

using System;
using System.Text;

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Provides methods for serializing and deserializing Argon2id strings to and from byte arrays.
	/// </summary>
	/// <remarks>This class is designed to handle the conversion of Argon2id strings to byte arrays for storage or
	/// transmission, and to convert byte arrays back into their original string representation. The serialization uses
	/// UTF-8 encoding.</remarks>
	public static class Argon2idSerializer
	{
		/// <summary>
		/// Converts the specified string to a UTF-8 encoded byte array.
		/// </summary>
		/// <param name="argon2idstring">The string to be serialized into a byte array. Cannot be <see langword="null"/>.</param>
		/// <returns>A byte array containing the UTF-8 encoded representation of the input string.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="argon2idstring"/> is <see langword="null"/>.</exception>
		public static byte[] Serialize(string argon2idstring)
		{
			if (argon2idstring == null)
			{
				throw new ArgumentNullException(nameof(argon2idstring));
			}

			return Encoding.UTF8.GetBytes(argon2idstring);
		}

		/// <summary>
		/// Converts a byte array to its equivalent string representation using UTF-8 encoding.
		/// </summary>
		/// <param name="data">The byte array to convert. Cannot be <see langword="null"/>.</param>
		/// <returns>The string representation of the byte array.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null"/>.</exception>
		public static string Deserialize(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			return Encoding.UTF8.GetString(data);
		}

#if NET8_0_OR_GREATER
		/// <summary>
		/// Converts the specified string to a UTF-8 encoded byte array.
		/// </summary>
		/// <param name="data">The read-only span of bytes to convert.</param>
		/// <returns>The string representation of the byte array.</returns>
		public static string Deserialize(ReadOnlySpan<byte> data)
		{
			return Encoding.UTF8.GetString(data);
		}
#endif
	}
}
