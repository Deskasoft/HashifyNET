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
		/// Gets the length of the hash in bits.
		/// </summary>
		int BitLength { get; }

		/// <summary>
		/// Gets the endianness of the hash value.
		/// </summary>
		ValueEndianness Endianness { get; }

		/// <summary>
		/// Gets the hash value as an immutable array of bytes.
		/// </summary>
		ImmutableArray<byte> Hash { get; }

		/// <summary>
		/// Gets the hash value as a <see cref="Guid"/>. If the bit length is greater than 128, an exception is thrown.
		/// </summary>
		/// <returns>A Guid representation of the underlying hash value.</returns>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="BitLength"/> is greater than 128.</exception>
		Guid AsGuid();

		/// <summary>
		/// Gets the hash value as a <see cref="BigInteger"/>.
		/// </summary>
		/// <returns>The created <see cref="BigInteger"/> instance.</returns>
		BigInteger AsBigInteger();

		/// <summary>
		/// Gets the hash value as a signed 32-bit integer. If the bit length is greater than 32, an exception is thrown.
		/// </summary>
		/// <returns>A signed 32-bit integer.</returns>
		int AsInt32();

		/// <summary>
		/// Gets the hash value as a signed 64-bit integer. If the bit length is greater than 64, an exception is thrown.
		/// </summary>
		/// <returns>A signed 64-bit integer.</returns>
		long AsInt64();

		/// <summary>
		/// Gets the hash value as a signed 16-bit integer. If the bit length is greater than 16, an exception is thrown.
		/// </summary>
		/// <returns>A signed 16-bit integer.</returns>
		short AsInt16();

		/// <summary>
		/// Gets the hash value as a Unicode character. If the bit length is greater than 16, an exception is thrown.
		/// </summary>
		/// <returns>A unicode character.</returns>
		char AsChar();

		/// <summary>
		/// Gets the hash value as a single-precision floating point number. If the bit length is greater than 32, an exception is thrown.
		/// </summary>
		/// <returns>A single-precision floating point number.</returns>
		float AsSingle();

		/// <summary>
		/// Gets the hash value as a double-precision floating point number. If the bit length is greater than 64, an exception is thrown.
		/// </summary>
		/// <returns>A double-precision floating point number.</returns>
		double AsDouble();

		/// <summary>
		/// Gets the hash value as a decimal number. If the bit length is greater than 96, an exception is thrown.
		/// </summary>
		/// <returns>A decimal number.</returns>
		decimal AsDecimal();

		/// <summary>
		/// Gets the hash value as a <see cref="DateTime"/>. The hash value is interpreted as the number of ticks (100-nanosecond intervals) since 12:00:00 midnight, January 1, 0001.
		/// </summary>
		/// <returns>The generated <see cref="DateTime"/> instance.</returns>
		DateTime AsDateTime();

		/// <summary>
		/// Gets the hash value as a <see cref="DateTimeOffset"/>. The hash value is interpreted as the number of ticks (100-nanosecond intervals) since 12:00:00 midnight, January 1, 0001.
		/// </summary>
		/// <returns>The generated <see cref="DateTimeOffset"/> instance.</returns>
		DateTimeOffset AsDateTimeOffset();

		/// <summary>
		/// Gets the hash value as a <see cref="TimeSpan"/>. The hash value is interpreted as the number of ticks (100-nanosecond intervals).
		/// </summary>
		/// <returns>The generated <see cref="TimeSpan"/> instance.</returns>
		TimeSpan AsTimeSpan();

		/// <summary>
		/// Gets the hash value as a binary string. Each byte is represented by 8 bits, with leading zeros if necessary.
		/// </summary>
		/// <returns>The generated binary string.</returns>
		string AsBinaryString();

		/// <summary>
		/// Gets the hash value as a Base85-encoded string in the <seealso cref="Base85Variant.Ascii85"/> standard. If the bit length is not a multiple of 8, the last byte is padded with zeros.
		/// </summary>
		/// <returns>The generated base85 string.</returns>
		string AsBase85String();

		/// <summary>
		/// Encodes the current data as a Base85-encoded string using the specified variant.
		/// </summary>
		/// <param name="variant">The <see cref="Base85Variant"/> to use for encoding. This determines the character set and encoding rules applied.</param>
		/// <returns>A Base85-encoded string representation of the data.</returns>
		string AsBase85String(Base85Variant variant);

		/// <summary>
		/// Gets the hash value as a Base64-encoded string. If the bit length is not a multiple of 8, the last byte is padded with zeros.
		/// </summary>
		/// <param name="formattingOptions">The formatting options for generating base64 encoded string.</param>
		/// <returns>The generated base64 encoded string.</returns>
		string AsBase64String(Base64FormattingOptions formattingOptions = Base64FormattingOptions.None);

		/// <summary>
		/// Gets the hash value as a Base58-encoded string using the <seealso cref="Base58Variant.Bitcoin"/> variant. If the bit length is not a multiple of 8, the last byte is padded with zeros.
		/// </summary>
		/// <returns>The generated base58 string.</returns>
		string AsBase58String();

		/// <summary>
		/// Encodes the current data as a Base58-encoded string using the specified variant.
		/// </summary>
		/// <returns>The generated base58 string in the specified variant.</returns>
		string AsBase58String(Base58Variant variant);

		/// <summary>
		/// Gets the hash value as a Base32-encoded string in the <seealso cref="Base32Variant.Rfc4648"/> standard. If the bit length is not a multiple of 8, the last byte is padded with zeros.
		/// </summary>
		/// <returns>The generated base32 string.</returns>
		string AsBase32String();

		/// <summary>
		/// Gets the hash value as a Base32-encoded string in the specified standard. If the bit length is not a multiple of 8, the last byte is padded with zeros.
		/// </summary>
		/// <returns>The generated base32 string.</returns>
		string AsBase32String(Base32Variant variant);

		/// <summary>
		/// Gets the hash value as a hexadecimal string, using uppercase letters for 'a' to 'f'.
		/// </summary>
		/// <returns>The hexadecimal string.</returns>
		string AsHexString();

		/// <summary>
		/// Gets the hash value as a <see cref="BitArray"/>.
		/// </summary>
		/// <returns>The created <see cref="BitArray"/> instance.</returns>
		BitArray AsBitArray();

		/// <summary>
		/// Gets the hash value as a modifiable byte array.
		/// </summary>
		/// <returns>The modifiable byte array.</returns>
		byte[] AsByteArray();

		/// <summary>
		/// Gets the hash value as a read-only span of bytes.
		/// </summary>
		/// <param name="start">The zero-based byte index at which to begin the span. Must be greater than or equal to 0 and less than <see cref="Hash"/>.Length.</param>
		/// <param name="length">The number of bytes to include in the span. Must be greater than or equal to 0 and less than or equal to <see cref="Hash"/>.Length - <paramref name="start"/>.</param>
		/// <returns>The read-only span of bytes.</returns>
		ReadOnlySpan<byte> AsSpan(int start, int length);

		/// <summary>
		/// Gets the hash value as a read-only span of bytes of the specified length from the start.
		/// </summary>
		/// <param name="length">The number of bytes to include in the span. Must be greater than or equal to 0 and less than or equal to <see cref="Hash"/>.Length.</param>
		/// <returns>The read-only span of bytes.</returns>
		ReadOnlySpan<byte> AsSpan(int length);

		/// <summary>
		/// Gets the hash value as a read-only span of bytes.
		/// </summary>
		/// <returns>The read-only span of bytes.</returns>
		ReadOnlySpan<byte> AsSpan();

		/// <summary>
		/// Slices a portion of the hash value starting from the specified bit index and spanning the specified number of bits.
		/// </summary>
		/// <param name="start">The zero-based bit index at which to begin the slice. Must be greater than or equal to 0 and less than <see cref="BitLength"/>.</param>
		/// <param name="length">The number of bits to include in the slice. Must be greater than or equal to 1 and less than or equal to <see cref="BitLength"/> - <paramref name="start"/>.</param>
		/// <returns>The sliced <see cref="IHashValue"/> instance.</returns>
		IHashValue Slice(int start, int length);

		/// <summary>
		/// Slices the hash value to the specified length in bits, starting from the beginning of the hash.
		/// </summary>
		/// <param name="length">The number of bits to include in the slice. Must be greater than or equal to 1 and less than or equal to <see cref="BitLength"/>.</param>
		/// <returns>The sliced <see cref="IHashValue"/> instance.</returns>
		IHashValue Slice(int length);

		/// <summary>
		/// Converts the current hash value to a new representation with the specified bit length.
		/// </summary>
		/// <param name="bitLength">The desired bit length of the resulting hash value. Must be greater than or equal to 1.</param>
		/// <returns>An <see cref="IHashValue"/> instance representing the hash value coerced to the specified bit length.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bitLength"/> is less than 1.</exception>
		IHashValue Coerce(int bitLength);

		/// <summary>
		/// Converts the current hash value to a new representation with the specified endianness of little-endian.
		/// </summary>
		/// <returns>The new <see cref="IHashValue"/> instance with little-endian byte order. If the current instance is already in little-endian format, it returns the same instance.</returns>
		IHashValue AsLittleEndian();

		/// <summary>
		/// Converts the current hash value to a new representation with the specified endianness of big-endian.
		/// </summary>
		/// <returns>The new <see cref="IHashValue"/> instance with big-endian byte order. If the current instance is already in big-endian format, it returns the same instance.</returns>
		IHashValue AsBigEndian();

		/// <summary>
		/// Converts the current hash value to a new representation with the specified endianness.
		/// </summary>
		/// <param name="endianness">The desired <see cref="ValueEndianness"/> of the resulting hash value.</param>
		/// <returns>The new <see cref="IHashValue"/> instance with the specified byte order. If the current instance is already in the specified format, it returns the same instance.</returns>
		IHashValue ToEndianness(ValueEndianness endianness);

		/// <summary>
		/// Reverses the endianness of the current hash value. If the current endianness is <see cref="ValueEndianness.NotApplicable"/>, no changes are made.
		/// </summary>
		/// <returns>The new <see cref="IHashValue"/> instance with reversed byte order. If the current instance has <see cref="ValueEndianness.NotApplicable"/>, it returns the same instance.</returns>
		IHashValue ReverseEndianness();
	}
}
