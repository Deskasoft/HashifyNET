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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using EndianHelper = HashifyNet.Core.Utilities.Endianness;

namespace HashifyNet.Core.Utilities
{
	/// <summary>
	/// Represents a hash value and its associated bit length.
	/// </summary>
	/// <remarks>This class provides a representation of a hash value as an immutable sequence of bytes, along with
	/// its bit length. It includes methods to convert the hash value into various formats, such as Base64, hexadecimal,
	/// and numeric types. The hash value is immutable and ensures that the provided bit length matches the actual length
	/// of the hash in bits.</remarks>
	[DebuggerDisplay("Hex: {AsHexString()}, Base64: {AsBase64String()}")]
	public class HashValue
		: IHashValue
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public ImmutableArray<byte> Hash { get; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public ValueEndianness Endianness { get; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int BitLength { get; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int ByteLength { get; }

		private HashValue(ValueEndianness endianness, ImmutableArray<byte> hash, int bitLength)
		{
			if (hash.IsDefaultOrEmpty)
			{
				throw new ArgumentException("Expected a valid populated immutable array containing the hash result, got an empty or default ImmutableArray instance.", nameof(hash));
			}

			if (bitLength < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(bitLength), $"{nameof(bitLength)} must be greater than or equal to 1.");
			}

			if (hash.Length != (bitLength + 7) / 8)
			{
				throw new ArgumentOutOfRangeException(nameof(hash), $"The length of {nameof(hash)} in bits must be equal to {nameof(bitLength)}. Bytes: {hash.Length} Expected: {hash.Length * 8}, Got: {bitLength}");
			}

			if (endianness != ValueEndianness.NotApplicable)
			{
				if (HashValueOptions.FixedEndianness.HasValue)
				{
					ValueEndianness fixedEndianness = HashValueOptions.FixedEndianness.Value;
					if (endianness != fixedEndianness)
					{
						hash = hash.Reverse().ToImmutableArray();
						endianness = fixedEndianness;
					}
				}
			}

			Hash = hash;
			BitLength = bitLength;
			Endianness = endianness;
			ByteLength = (BitLength + 7) / 8;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="HashValue"/> class that is a representation of a hash value and its bit length computed by a hasher.
		/// </summary>
		/// <param name="endianness">The endianness of the given <paramref name="hash"/>.</param>
		/// <param name="hash">The hash computed by a hasher.</param>
		/// <param name="bitLength">The expected bit length of the given <paramref name="hash"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if the given hash is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="bitLength"/> parameter is smaller than 1.</exception>
		public HashValue(ValueEndianness endianness, IEnumerable<byte> hash, int bitLength) : this(endianness, hash?.ToImmutableArray() ?? ImmutableArray<byte>.Empty, bitLength)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="HashValue"/> class that is a representation of a hash value and its bit length computed by a hasher.
		/// </summary>
		/// <param name="endianness">The endianness of the given <paramref name="hash"/>.</param>
		/// <param name="hash">The hash computed by a hasher.</param>
		/// <param name="bitLength">The expected bit length of the given <paramref name="hash"/>.</param>
		/// <returns>A new instance of the <see cref="HashValue"/> class.</returns>
		/// <exception cref="ArgumentNullException">Thrown if the given hash is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="bitLength"/> parameter is smaller than 1.</exception>
		public static HashValue FromSpan(ValueEndianness endianness, ReadOnlySpan<byte> hash, int bitLength)
		{
			return new HashValue(endianness, hash.ToImmutableArray(), bitLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public BigInteger AsBigInteger()
		{
			return new BigInteger(AsByteArray());
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public Guid AsGuid()
		{
			if (BitLength > 128)
			{
				throw new InvalidOperationException("Hash must be smaller than or equal to 16 bytes.");
			}

			byte[] data = AsByteArray();
			if (data.Length < 16)
			{
				byte[] paddedData = new byte[16];
				Array.Copy(data, paddedData, data.Length);
				data = paddedData;
			}

			return new Guid(data);
		}

		#region Non-CLS-Compliant API
		/// <summary>
		/// Gets the hash value as an unsigned 64-bit integer. If the bit length is greater than 64, an exception is thrown.
		/// <para>This API is not CLS-Compliant.</para>
		/// </summary>
		/// <returns>An unsigned 64-bit integer value.</returns>
		[CLSCompliant(false)]
		public ulong AsUInt64()
		{
			return (ulong)AsNumber64();
		}

		/// <summary>
		/// Gets the hash value as an unsigned 32-bit integer. If the bit length is greater than 32, an exception is thrown.
		/// <para>This API is not CLS-Compliant.</para>
		/// </summary>
		/// <returns>An unsigned 32-bit integer.</returns>
		[CLSCompliant(false)]
		public uint AsUInt32()
		{
			return (uint)AsNumber32();
		}

		/// <summary>
		/// Gets the hash value as a unsigned 16-bit integer. If the bit length is greater than 16, an exception is thrown.
		/// <para>This API is not CLS-Compliant.</para>
		/// </summary>
		/// <returns>An unsigned 16-bit integer.</returns>
		[CLSCompliant(false)]
		public ushort AsUInt16()
		{
			return (ushort)AsNumber16();
		}
		#endregion

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public int AsInt32()
		{
			return AsNumber32();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public long AsInt64()
		{
			return AsNumber64();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public short AsInt16()
		{
			return AsNumber16();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public char AsChar()
		{
			return (char)AsNumber16();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public float AsSingle()
		{
			int l = AsNumber32();
			unsafe
			{
				return *(float*)&l;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public double AsDouble()
		{
			long l = AsNumber64();
			unsafe
			{
				return *(double*)&l;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public decimal AsDecimal()
		{
			return AsNumber128_96();
		}

		private decimal AsNumber128_96()
		{
			if (BitLength > 96)
			{
				throw new NotSupportedException("Bit Length greater than 96 is not supported.");
			}

			byte[] data = AsByteArray();

			byte[] dataPadded = new byte[12];
			Array.Copy(data, dataPadded, data.Length);

			int lo = (int)EndianHelper.ToUInt32LittleEndian(dataPadded, 0);
			int mid = (int)EndianHelper.ToUInt32LittleEndian(dataPadded, 4);
			int hi = (int)EndianHelper.ToUInt32LittleEndian(dataPadded, 8);
			return new decimal(lo, mid, hi, false, 0);
		}

		private long AsNumber64()
		{
			if (BitLength > 64)
			{
				throw new NotSupportedException("Bit Length greater than 64 is not supported.");
			}

			byte[] data = AsByteArray();

			long num = 0;
			for (int i = 0; i < data.Length; ++i)
			{
				num |= (long)data[i] << (8 * i);
			}

			return num;
		}

		private int AsNumber32()
		{
			if (BitLength > 32)
			{
				throw new NotSupportedException("Bit Length greater than 32 is not supported.");
			}

			return (int)AsNumber64();
		}

		private short AsNumber16()
		{
			if (BitLength > 16)
			{
				throw new NotSupportedException("Bit Length greater than 16 is not supported.");
			}

			return (short)AsNumber64();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public DateTime AsDateTime()
		{
			long ticks = AsNumber64();
			if (ticks < 0)
			{
				ticks &= long.MaxValue;
			}

			if (BitLength < 64)
			{
				double maxHashValue = 1L << BitLength;
				ticks = (long)(ticks / maxHashValue * DateTime.MaxValue.Ticks);
			}

			if (ticks < DateTime.MinValue.Ticks)
			{
				ticks = DateTime.MinValue.Ticks;
			}

			if (ticks > DateTime.MaxValue.Ticks)
			{
				ticks = DateTime.MaxValue.Ticks;
			}

			return new DateTime(ticks);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public DateTimeOffset AsDateTimeOffset()
		{
			long ticks = AsNumber64();
			if (ticks < 0)
			{
				ticks &= long.MaxValue;
			}

			if (BitLength < 64)
			{
				double maxHashValue = 1L << BitLength;
				ticks = (long)(ticks / maxHashValue * DateTimeOffset.MaxValue.Ticks);
			}

			if (ticks < DateTimeOffset.MinValue.Ticks)
			{
				ticks = DateTimeOffset.MinValue.Ticks;
			}

			if (ticks > DateTimeOffset.MaxValue.Ticks)
			{
				ticks = DateTimeOffset.MaxValue.Ticks;
			}

			return new DateTimeOffset(ticks, TimeSpan.Zero);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public TimeSpan AsTimeSpan()
		{
			long ticks = AsNumber64();
			if (ticks < 0)
			{
				ticks &= long.MaxValue;
			}

			if (BitLength < 64)
			{
				double maxHashValue = 1L << BitLength;
				ticks = (long)(ticks / maxHashValue * TimeSpan.MaxValue.Ticks);
			}

			if (ticks < TimeSpan.MinValue.Ticks)
			{
				ticks = TimeSpan.MinValue.Ticks;
			}

			if (ticks > TimeSpan.MaxValue.Ticks)
			{
				ticks = TimeSpan.MaxValue.Ticks;
			}

			return new TimeSpan(ticks);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBinaryString()
		{
			byte[] data = AsByteArray();
			var stringBuilder = new StringBuilder(data.Length * 8);
			foreach (var byteValue in data)
			{
				stringBuilder.Append(Convert.ToString(byteValue, 2).PadLeft(8, '0'));
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBase85String()
		{
			return AsBase85String(Base85Variant.Ascii85);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="variant"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public string AsBase85String(Base85Variant variant)
		{
			return Base85Helper.AsBase85String(AsByteArray(), variant);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="formattingOptions"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public string AsBase64String(Base64FormattingOptions formattingOptions = Base64FormattingOptions.None)
		{
			return Convert.ToBase64String(AsBigEndian().AsByteArray(), formattingOptions);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBase58String()
		{
			return AsBase58String(Base58Variant.Bitcoin);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="variant"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public string AsBase58String(Base58Variant variant)
		{
			return Base58Helper.AsBase58String(AsBigEndian().AsByteArray(), variant);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBase32String()
		{
			return AsBase32String(Base32Variant.Rfc4648);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="variant"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public string AsBase32String(Base32Variant variant)
		{
			return Base32Helper.AsBase32String(AsBigEndian().AsByteArray(), variant);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsHexString()
		{
			ImmutableArray<byte> hash = AsBigEndian().Hash;

			var stringBuilder = new StringBuilder(ByteLength);
			var formatString = "X2";

			foreach (var byteValue in hash)
			{
				stringBuilder.Append(byteValue.ToString(formatString));
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public BitArray AsBitArray()
		{
			return new BitArray(Hash.ToArray())
			{
				Length = BitLength
			};
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public byte[] AsByteArray()
		{
			return Hash.ToArray();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="start"><inheritdoc/></param>
		/// <param name="length"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public ReadOnlySpan<byte> AsSpan(int start, int length)
		{
			return Hash.AsSpan(start, length);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="length"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public ReadOnlySpan<byte> AsSpan(int length)
		{
			return AsSpan(0, length);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public ReadOnlySpan<byte> AsSpan()
		{
			return AsSpan(0, ByteLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="start"><inheritdoc/></param>
		/// <param name="length"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public IHashValue Slice(int start, int length)
		{
			return new HashValue(Endianness, AsSpan(start, length).ToImmutableArray(), length * 8);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="length"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public IHashValue Slice(int length)
		{
			return Slice(0, length);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="bitLength"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentOutOfRangeException"><inheritdoc/></exception>
		public virtual IHashValue Coerce(int bitLength)
		{
			if (BitLength == bitLength)
				return this;

			return new HashValue(Endianness, ArrayHelpers.CoerceToArray(Hash.ToArray(), bitLength), bitLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IHashValue AsLittleEndian()
		{
			if (Endianness != ValueEndianness.BigEndian)
				return this;

			return new HashValue(ValueEndianness.LittleEndian, Hash.Reverse().ToImmutableArray(), BitLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IHashValue AsBigEndian()
		{
			if (Endianness != ValueEndianness.LittleEndian)
				return this;

			return new HashValue(ValueEndianness.BigEndian, Hash.Reverse().ToImmutableArray(), BitLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="endianness"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public IHashValue WithEndianness(ValueEndianness endianness)
		{
			if (Endianness == ValueEndianness.NotApplicable || endianness == ValueEndianness.NotApplicable || Endianness == endianness)
				return this;

			return endianness == ValueEndianness.BigEndian ? AsBigEndian() : AsLittleEndian();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IHashValue ReverseEndianness()
		{
			if (Endianness == ValueEndianness.NotApplicable)
				return this;

			return new HashValue(Endianness == ValueEndianness.BigEndian ? ValueEndianness.LittleEndian : ValueEndianness.BigEndian, Hash.Reverse().ToImmutableArray(), BitLength);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="inputStream"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public Stream AsStream(Stream inputStream)
		{
			Stream stream = inputStream ?? new MemoryStream();
			if (!stream.CanWrite)
			{
				throw new ArgumentException("The given stream must be writable.", nameof(inputStream));
			}

			byte[] buffer = AsByteArray();
			stream.Write(buffer, 0, buffer.Length);

			return stream;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public Stream AsStream()
		{
			return AsStream(new MemoryStream());
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public ReadOnlyMemory<byte> AsMemory()
		{
			return Hash.AsMemory();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="array"><inheritdoc/></param>
		/// <param name="arrayIndex"><inheritdoc/></param>
		public void CopyTo(byte[] array, int arrayIndex)
		{
			_ = array ?? throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0 || arrayIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(arrayIndex), "The given array index is out of range.");
			}

			if (array.Length - arrayIndex < ByteLength)
			{
				throw new ArgumentException("The given array is too small to hold the hash value starting from the given index.", nameof(array));
			}

			Hash.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="destination"><inheritdoc/></param>
		public void CopyTo(Span<byte> destination)
		{
			if (destination.Length < ByteLength)
			{
				throw new ArgumentException("The given destination span is too small to hold the hash value.", nameof(destination));
			}

			AsSpan().CopyTo(destination);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="destination"><inheritdoc/></param>
		public void CopyTo(Memory<byte> destination)
		{
			if (destination.Length < ByteLength)
			{
				throw new ArgumentException("The given destination memory is too small to hold the hash value.", nameof(destination));
			}

			AsMemory().CopyTo(destination);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="destination"><inheritdoc/></param>
		/// <param name="destinationIndex"><inheritdoc/></param>
		public void CopyTo(ImmutableArray<byte>.Builder destination, int destinationIndex)
		{
			_ = destination ?? throw new ArgumentNullException(nameof(destination));
			if (destinationIndex < 0 || destinationIndex >= destination.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(destinationIndex), "The given destination index is out of range.");
			}

			if (destination.Count - destinationIndex < ByteLength)
			{
				throw new ArgumentException("The given destination is too small to hold the hash value starting from the given index.", nameof(destination));
			}

			for (int i = 0; i < ByteLength; i++)
			{
				destination[destinationIndex + i] = Hash[i];
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="destination"><inheritdoc/></param>
		public void CopyTo(Stream destination)
		{
			AsStream(destination);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public double CalculateEntropy()
		{
			int byteLength = ByteLength;
			if (byteLength == 0)
			{
				return 0;
			}

			var counts = new int[256];
			for (int i = 0; i < byteLength; ++i)
			{
				counts[Hash[i]]++;
			}

			double totalEntropy = 0.0;
			for (int i = 0; i < counts.Length; i++)
			{
				int count = counts[i];
				if (count > 0)
				{
					double probability = (double)count / byteLength;
					totalEntropy -= probability *
#if NET8_0_OR_GREATER
						Math.Log2(probability)
#else
						(Math.Log(probability) / Math.Log(2))
#endif
						;
				}
			}

			return totalEntropy;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public double CalculateEntropyPercentage()
		{
			int byteLength = ByteLength;
			if (byteLength <= 1)
			{
				return 100.0;
			}

			double actualEntropy = this.CalculateEntropy();
			double maxPossibleEntropy =

#if NET8_0_OR_GREATER
				Math.Log2(byteLength)
#else
				(Math.Log(byteLength) / Math.Log(2))
#endif
				;

			return (actualEntropy / maxPossibleEntropy) * 100.0;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IEnumerator<byte> GetEnumerator()
		{
			return ((IEnumerable<byte>)Hash).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Computes a hash code for the current object.
		/// </summary>
		/// <remarks>The hash code is calculated based on the values of the <see cref="BitLength"/> property and the
		/// elements of the <see cref="Hash"/> collection. This ensures that objects with  the same data produce the same hash
		/// code.</remarks>
		/// <returns>An integer representing the hash code for the current object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = 17;

				hashCode = (hashCode * 31) ^ BitLength.GetHashCode();

				foreach (var value in Hash)
				{
					hashCode = (hashCode * 31) ^ value.GetHashCode();
				}

				return hashCode;
			}
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object. This can be <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the specified object is an <see cref="IHashValue"/> and is equal to the current object;
		/// otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as IHashValue);
		}

		/// <summary>
		/// Determines whether the current hash value is equal to the specified <see cref="IHashValue"/> instance.
		/// </summary>
		/// <remarks>This method performs a fixed-time comparison of the hash values to mitigate timing attacks.  The
		/// comparison takes into account the bit length of the hash and the hash data itself.</remarks>
		/// <param name="other">The <see cref="IHashValue"/> instance to compare with the current instance.</param>
		/// <returns><see langword="true"/> if the current hash value is equal to <paramref name="other"/>; otherwise, <see
		/// langword="false"/>.</returns>
		public bool Equals(IHashValue other)
		{
			if (other == null || other.BitLength != BitLength)
			{
				return false;
			}

			// Since we currently do not know if this hash value belongs to a cryptographic hasher or a non-cryptographic hasher,
			// we should always check equality with fixed time equality checker.
			//
			// TODO: Maybe we can add an owner parameter to .ctor for a way to know if this is a cryptographic or non-cryptographic hash.
			return HashComparer.FixedTimeEquals(Hash, other.Hash);
		}

		/// <summary>
		/// Compares the current hash value to another hash value and determines their relative order.
		/// </summary>
		/// <remarks>The comparison is performed based on the bit length of the hash values and their byte-by-byte
		/// content. If the bit lengths differ, the hash with the smaller bit length is considered less. If the bit lengths
		/// are equal, the comparison proceeds byte by byte, and any remaining bits in the final byte are compared using a bit
		/// mask.</remarks>
		/// <param name="other">The hash value to compare with the current instance. Cannot be <see langword="null"/>.</param>
		/// <returns>A signed integer that indicates the relative order of the hash values: <list type="bullet">
		/// <item><description>Less than zero if the current instance is less than <paramref
		/// name="other"/>.</description></item> <item><description>Zero if the current instance is equal to <paramref
		/// name="other"/>.</description></item> <item><description>Greater than zero if the current instance is greater than
		/// <paramref name="other"/>.</description></item> </list></returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <see langword="null"/>.</exception>
		public int CompareTo(IHashValue other)
		{
			_ = other ?? throw new ArgumentNullException(nameof(other));

			if (BitLength != other.BitLength)
			{
				return BitLength.CompareTo(other.BitLength);
			}

			ImmutableArray<byte> currentHash = Hash;
			ImmutableArray<byte> otherHash = other.Hash;
			int fullBytes = BitLength / 8;

			for (int i = 0; i < fullBytes; i++)
			{
				int comparison = currentHash[i].CompareTo(otherHash[i]);
				if (comparison != 0)
				{
					return comparison;
				}
			}

			int remainingBits = BitLength % 8;
			if (remainingBits > 0)
			{
				// Create a mask to isolate only the bits we care about.
				// For 2 remaining bits (as in a 10-bit hash), the mask is 11000000.
				int mask = 0xFF << (8 - remainingBits);

				byte currentLastByte = (byte)(currentHash[fullBytes] & mask);
				byte otherLastByte = (byte)(otherHash[fullBytes] & mask);

				return currentLastByte.CompareTo(otherLastByte);
			}

			return 0;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public override string ToString()
		{
			return $"Hex: {AsHexString()}, Base64: {AsBase64String()}";
		}

		/// <summary>
		/// The equality operator for comparing two <see cref="HashValue"/> instances.
		/// </summary>
		/// <param name="left">The left <see cref="HashValue"/> instance to compare.</param>
		/// <param name="right">The right <see cref="HashValue"/> instance to compare.</param>
		/// <returns>The result of the equality comparison.</returns>
		public static bool operator ==(HashValue left, HashValue right)
		{
			if (left is null)
			{
				return right is null;
			}

			return left.Equals(right);
		}

		/// <summary>
		/// The inequality operator for comparing two <see cref="HashValue"/> instances.
		/// </summary>
		/// <param name="left">The left <see cref="HashValue"/> instance to compare.</param>
		/// <param name="right">The right <see cref="HashValue"/> instance to compare.</param>
		/// <returns>The result of the inequality comparison.</returns>
		public static bool operator !=(HashValue left, HashValue right)
		{
			return !(left == right);
		}
	}
}
