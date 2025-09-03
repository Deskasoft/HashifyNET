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

using System;
using System.Collections;
using System.Collections.Immutable;
using System.Numerics;

namespace HashifyNet
{
	/// <summary>
	/// Common interface to represent a hash value. The hash value is guaranteed to be immutable.
	/// </summary>
	public interface IHashValue
		: IEquatable<IHashValue>, IComparable<IHashValue>
	{
		/// <summary>
		/// Gets the length of the hash value in bits.
		/// </summary>
		/// <value>
		/// The length of the hash value in bits.
		/// </value>
		int BitLength { get; }

		/// <summary>
		/// Gets resulting byte array.
		/// </summary>
		/// <value>
		/// The hash value.
		/// </value>
		/// <remarks>
		/// Implementations should coerce the input hash value to be <see cref="BitLength"/> size in bits.
		/// </remarks>
		ImmutableArray<byte> Hash { get; }

		/// <summary>
		/// Converts the hash value to a bit array.
		/// </summary>
		/// <returns>A <see cref="BitArray"/> instance to represent this hash value.</returns>
		BitArray AsBitArray();

		/// <summary>
		/// Converts the immutable array to a byte array representation.
		/// </summary>
		/// <returns>A byte array that represents the current immutable array.</returns>
		byte[] AsByteArray();

		/// <summary>
		/// Converts the hash value to a hexadecimal string.
		/// </summary>
		/// <returns>A hex string representing this hash value.</returns>
		string AsHexString();

		/// <summary>
		/// Converts the hash value to a hexadecimal string.
		/// </summary>
		/// <param name="uppercase"><c>true</c> if the result should use uppercase hex values; otherwise <c>false</c>.</param>
		/// <returns>A hex string representing this hash value.</returns>
		string AsHexString(bool uppercase);

		/// <summary>
		/// Converts the hash value to a the base64 string.
		/// </summary>
		/// <param name="formattingOptions">Formatting options for the generated base64 string.</param>
		/// <returns>A base64 string representing this hash value.</returns>
		string AsBase64String(Base64FormattingOptions formattingOptions = Base64FormattingOptions.None);

		/// <summary>
		/// Converts the hash value to a representation with the specified bit length.
		/// </summary>
		/// <remarks>This method allows the caller to adjust the bit length of the hash value to meet specific
		/// requirements. The resulting hash value may be truncated or padded, depending on the specified bit
		/// length.</remarks>
		/// <param name="bitLength">The desired bit length of the resulting hash value. Must be a positive integer.</param>
		/// <returns>An <see cref="IHashValue"/> instance representing the hash value with the specified bit length.</returns>
		IHashValue Coerce(int bitLength);

		/// <summary>
		/// Converts the current hash to its equivalent <see cref="BigInteger"/> representation.
		/// </summary>
		/// <remarks>This method uses the byte array representation of the hash to construct the <see
		/// cref="BigInteger"/>.</remarks>
		/// <returns>A <see cref="BigInteger"/> that represents the current hash.</returns>
		BigInteger AsBigInteger();

		/// <summary>
		/// Converts the hash value to a 64-bit unsigned integer.
		/// </summary>
		/// <remarks>This method interprets the hash value as a little-endian byte array and converts it to a <see
		/// cref="ulong"/>. If the hash value exceeds 8 bytes, an exception is thrown.</remarks>
		/// <returns>A 64-bit unsigned integer representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value is larger than 8 bytes.</exception>
		[CLSCompliant(false)]
		ulong AsUInt64();

		/// <summary>
		/// Converts the hash value to a <see cref="Guid"/>.
		/// </summary>
		/// <remarks>The hash must be exactly 16 bytes in length to be converted to a <see cref="Guid"/>. If the hash
		/// length is not 16 bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="Guid"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 16 bytes in length.</exception>
		Guid AsGuid();

		/// <summary>
		/// Converts the hash value to a 32-bit integer representation.
		/// </summary>
		/// <remarks>This method interprets the hash value as a little-endian byte array and converts it to an <see
		/// cref="int"/>. If the hash value exceeds 4 bytes, an exception is thrown.</remarks>
		/// <returns>A 32-bit integer representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value exceeds 4 bytes in length.</exception>
		int AsInt32();

		/// <summary>
		/// Converts the hash value to a 32-bit unsigned integer.
		/// </summary>
		/// <remarks>The method interprets the hash value as a little-endian byte array and converts it to a <see
		/// cref="uint"/>. If the hash value exceeds 4 bytes, an exception is thrown.</remarks>
		/// <returns>A 32-bit unsigned integer representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value is larger than 4 bytes.</exception>
		[CLSCompliant(false)]
		uint AsUInt32();

		/// <summary>
		/// Converts the hash value to a 64-bit signed integer.
		/// </summary>
		/// <remarks>This method interprets the hash value as a little-endian byte array and converts it to a 64-bit
		/// signed integer. If the hash value exceeds 8 bytes, an exception is thrown.</remarks>
		/// <returns>A 64-bit signed integer representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value is larger than 8 bytes.</exception>
		long AsInt64();

		/// <summary>
		/// Converts the hash value to a 16-bit signed integer.
		/// </summary>
		/// <remarks>This method interprets the hash value as a little-endian byte array and converts it to a 16-bit
		/// signed integer. If the hash value exceeds 2 bytes in length, an exception is thrown.</remarks>
		/// <returns>A 16-bit signed integer representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value exceeds 2 bytes in length.</exception>
		[CLSCompliant(false)]
		short AsInt16();

		/// <summary>
		/// Converts the hash value to a 16-bit unsigned integer.
		/// </summary>
		/// <remarks>This method interprets the hash value as a little-endian byte array and converts it to a  <see
		/// cref="ushort"/>. If the hash value exceeds 2 bytes, an exception is thrown.</remarks>
		/// <returns>A 16-bit unsigned integer representation of the hash value. If the hash value is less than 2 bytes, the result is
		/// padded with zeros.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value exceeds 2 bytes, as it cannot be represented as a <see cref="ushort"/>.</exception>
		ushort AsUInt16();

		/// <summary>
		/// Converts the hash value to a <see cref="char"/> representation.
		/// </summary>
		/// <remarks>This method interprets the hash as a sequence of bytes and converts it into a <see cref="char"/>.
		/// If the hash contains more than two bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="char"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash contains more than two bytes, as it cannot be represented as a <see cref="char"/>.</exception>
		char AsChar();

		/// <summary>
		/// Converts the hash value to a single-precision floating-point number.
		/// </summary>
		/// <remarks>The hash must be exactly 4 bytes in length to perform the conversion. If the hash length is not 4
		/// bytes, an <see cref="InvalidOperationException"/> is thrown.</remarks>
		/// <returns>A <see cref="float"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 4 bytes in length.</exception>
		float AsSingle();

		/// <summary>
		/// Converts the hash value to a <see cref="double"/> representation.
		/// </summary>
		/// <remarks>The hash must be exactly 8 bytes in length to perform the conversion. If the hash length is not
		/// 8 bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="double"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 8 bytes in length.</exception>
		double AsDouble();

		/// <summary>
		/// Converts the hash value to a <see cref="decimal"/> representation.
		/// </summary>
		/// <remarks>The hash must be exactly 16 bytes in length to be converted to a <see cref="decimal"/>. If the
		/// hash length is not 16 bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="decimal"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 16 bytes in length.</exception>
		decimal AsDecimal();

		/// <summary>
		/// Converts the hash value to a <see cref="DateTime"/> instance.
		/// </summary>
		/// <remarks>The hash must be exactly 8 bytes long to represent a valid <see cref="DateTime"/>. If the hash
		/// length is not 8 bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="DateTime"/> instance created from the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 8 bytes long.</exception>
		DateTime AsDateTime();

		/// <summary>
		/// Converts the hash value to a <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <remarks>The hash must be exactly 8 bytes long to perform the conversion. The resulting <see
		/// cref="DateTimeOffset"/> is created with a UTC offset of <see cref="TimeSpan.Zero"/>.</remarks>
		/// <returns>A <see cref="DateTimeOffset"/> representation of the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash is not exactly 8 bytes long.</exception>
		DateTimeOffset AsDateTimeOffset();

		/// <summary>
		/// Converts the hash value to a <see cref="TimeSpan"/> instance.
		/// </summary>
		/// <remarks>The hash value must be exactly 8 bytes long to be converted to a <see cref="TimeSpan"/>. If the
		/// hash length is not 8 bytes, an exception is thrown.</remarks>
		/// <returns>A <see cref="TimeSpan"/> representing the hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the hash value is not exactly 8 bytes long.</exception>
		TimeSpan AsTimeSpan();

		/// <summary>
		/// Converts the current hash to its binary string representation.
		/// </summary>
		/// <remarks>This method generates a binary string where each byte of the hash is represented  as an
		/// 8-character binary value, padded with leading zeros if necessary. The resulting string concatenates the binary
		/// representations of all bytes in sequence.</remarks>
		/// <returns>A string containing the binary representation of the hash.</returns>
		string AsBinaryString();

		/// <summary>
		/// Encodes the current hash as a Base85-encoded string.
		/// </summary>
		/// <remarks>This method converts the hash's data, represented as a byte array, into a Base85-encoded string
		/// using the ASCII85 encoding scheme. Base85 encoding is a compact representation of binary data that uses a set of
		/// 85 printable ASCII characters.</remarks>
		/// <returns>A Base85-encoded string representation of the hash's data. Returns an empty string if the byte array is empty.</returns>
		string AsBase85();

		/// <summary>
		/// Converts the current hash to its Base58-encoded string representation.
		/// </summary>
		/// <remarks>This method uses the Base58 encoding scheme, which is commonly used in applications such as
		/// cryptocurrency addresses. The encoding excludes visually ambiguous characters such as '0', 'O', 'I', and
		/// 'l'.</remarks>
		/// <returns>A Base58-encoded string representation of the current hash.</returns>
		string AsBase58();

		/// <summary>
		/// Converts the current hash to a Base32-encoded string representation.
		/// </summary>
		/// <remarks>The Base32 encoding uses the alphabet "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567" and appends padding 
		/// characters ('=') to ensure the output length is a multiple of 8. This method processes the hash's data as a
		/// byte array and encodes it into Base32 format.</remarks>
		/// <returns>A Base32-encoded string representation of the hash's data. Returns an empty string if the hash's data is
		/// empty.</returns>
		string AsBase32();
	}
}
