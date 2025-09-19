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

using Isopoh.Cryptography.Argon2;
using System;

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Provides methods for decoding Argon2id hash values and byte arrays into their original forms.
	/// </summary>
	/// <remarks>This class includes static methods to decode Argon2id hash values and byte arrays, supporting both
	/// direct decoding of byte arrays and decoding from hash representations. The decoding process assumes that the input
	/// data is properly formatted and adheres to the Argon2id specification.</remarks>
	public static class Argon2idDecoder
	{
		/// <summary>
		/// Decodes the specified byte array and returns the resulting data.
		/// </summary>
		/// <param name="data">The byte array to decode. Cannot be <see langword="null"/>.</param>
		/// <returns>The decoded byte array, or <see langword="null"/> if decoding fails.</returns>
		public static byte[] Decode(byte[] data)
		{
			Argon2Config config = new Argon2Config();
			if (!DecodeExtension.DecodeString(config, Argon2idSerializer.Deserialize(data), out var buffer))
			{
				return null;
			}

			return buffer.Buffer;
		}

#if NET8_0_OR_GREATER
		/// <summary>
		/// Decodes the specified read-only span of bytes and returns the resulting data.
		/// </summary>
		/// <param name="data">The read-only span of bytes to decode.</param>
		/// <returns>The decoded byte array, or <see langword="null"/> if decoding fails.</returns>
		public static byte[] Decode(ReadOnlySpan<byte> data)
		{
			Argon2Config config = new Argon2Config();
			if (!DecodeExtension.DecodeString(config, Argon2idSerializer.Deserialize(data), out var buffer))
			{
				return null;
			}

			return buffer.Buffer;
		}
#endif

		/// <summary>
		/// Decodes the hash value of an Argon2id hash into its raw byte representation.
		/// </summary>
		/// <param name="val">The hash value to decode. Must not be <see langword="null"/>.</param>
		/// <returns>A byte array containing the raw byte representation of the Argon2id hash.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="val"/> is <see langword="null"/>.</exception>
		public static byte[] DecodeArgon2id(this IHashValue val)
		{
			if (val == null)
			{
				throw new ArgumentNullException(nameof(val));
			}

			return Decode(val.AsByteArray());
		}
	}
}
