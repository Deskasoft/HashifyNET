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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace HashifyNet.Core.Utilities
{
	/// <summary>
	/// Implementation of <see cref="IHashValue"/>
	/// </summary>
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
		public int BitLength { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="HashValue"/>.
		/// </summary>
		/// <param name="hash">The hash.</param>
		/// <param name="bitLength">Length of the hash, in bits.</param>
		/// <exception cref="ArgumentNullException"><paramref name="hash"/></exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bitLength"/>;bitLength must be greater than or equal to 1.</exception>
		public HashValue(IEnumerable<byte> hash, int bitLength)
		{
			if (hash == null)
			{
				throw new ArgumentNullException(nameof(hash));
			}

			if (bitLength < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(bitLength), $"{nameof(bitLength)} must be greater than or equal to 1.");
			}

			ImmutableArray<byte> immutableHash = hash.ToImmutableArray();
			if (immutableHash.Length != (bitLength + 7) / 8)
			{
				throw new ArgumentOutOfRangeException(nameof(hash), $"The length of {nameof(hash)} in bits must be equal to {nameof(bitLength)}. Bytes: {immutableHash.Length} Expected: {immutableHash.Length * 8}, Got: {bitLength}");
			}

			Hash = immutableHash;
			BitLength = bitLength;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="formattingOptions"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public string AsBase64String(Base64FormattingOptions formattingOptions = Base64FormattingOptions.None)
		{
			return Convert.ToBase64String(Hash.ToArray(), formattingOptions);
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
		[CLSCompliant(false)]
		public ulong AsUInt64()
		{
			byte[] data = AsByteArray();
			if (data.Length > 8)
			{
				throw new InvalidOperationException("Hash is too large to fit in a UInt64.");
			}
			ulong result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (ulong)data[i] << (8 * i);
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public Guid AsGuid()
		{
			byte[] data = AsByteArray();
			if (data.Length != 16)
			{
				throw new InvalidOperationException("Hash must be exactly 16 bytes to convert to a GUID.");
			}

			return new Guid(data);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public int AsInt32()
		{
			byte[] data = AsByteArray();
			if (data.Length > 4)
			{
				throw new InvalidOperationException("Hash is too large to fit in an Int32.");
			}
			int result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= data[i] << (8 * i);
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		[CLSCompliant(false)]
		public uint AsUInt32()
		{
			byte[] data = AsByteArray();
			if (data.Length > 4)
			{
				throw new InvalidOperationException("Hash is too large to fit in a UInt32.");
			}
			uint result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (uint)data[i] << (8 * i);
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public long AsInt64()
		{
			byte[] data = AsByteArray();
			if (data.Length > 8)
			{
				throw new InvalidOperationException("Hash is too large to fit in an Int64.");
			}
			long result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (long)data[i] << (8 * i);
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public short AsInt16()
		{
			byte[] data = AsByteArray();
			if (data.Length > 2)
			{
				throw new InvalidOperationException("Hash is too large to fit in an Int16.");
			}
			short result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (short)(data[i] << (8 * i));
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		[CLSCompliant(false)]
		public ushort AsUInt16()
		{
			byte[] data = AsByteArray();
			if (data.Length > 2)
			{
				throw new InvalidOperationException("Hash is too large to fit in a UInt16.");
			}
			ushort result = 0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (ushort)(data[i] << (8 * i));
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public char AsChar()
		{
			byte[] data = AsByteArray();
			if (data.Length > 2)
			{
				throw new InvalidOperationException("Hash is too large to fit in a Char.");
			}
			char result = (char)0;
			for (int i = 0; i < data.Length; i++)
			{
				result |= (char)(data[i] << (8 * i));
			}
			return result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public float AsSingle()
		{
			byte[] data = AsByteArray();
			if (data.Length != 4)
			{
				throw new InvalidOperationException("Hash must be exactly 4 bytes to convert to a Single.");
			}
			return BitConverter.ToSingle(data, 0);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public double AsDouble()
		{
			byte[] data = AsByteArray();
			if (data.Length != 8)
			{
				throw new InvalidOperationException("Hash must be exactly 8 bytes to convert to a Double.");
			}
			return BitConverter.ToDouble(data, 0);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public decimal AsDecimal()
		{
			byte[] data = AsByteArray();
			if (data.Length != 16)
			{
				throw new InvalidOperationException("Hash must be exactly 16 bytes to convert to a Decimal.");
			}
			int[] bits = new int[4];
			for (int i = 0; i < 4; i++)
			{
				bits[i] = BitConverter.ToInt32(data, i * 4);
			}
			return new decimal(bits);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public DateTime AsDateTime()
		{
			byte[] data = AsByteArray();
			if (data.Length != 8)
			{
				throw new InvalidOperationException("Hash must be exactly 8 bytes to convert to a DateTime.");
			}
			long ticks = BitConverter.ToInt64(data, 0);
			return new DateTime(ticks);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public DateTimeOffset AsDateTimeOffset()
		{
			byte[] data = AsByteArray();
			if (data.Length != 8)
			{
				throw new InvalidOperationException("Hash must be exactly 8 bytes to convert to a DateTimeOffset.");
			}
			long ticks = BitConverter.ToInt64(data, 0);
			return new DateTimeOffset(ticks, TimeSpan.Zero);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="InvalidOperationException"><inheritdoc/></exception>
		public TimeSpan AsTimeSpan()
		{
			byte[] data = AsByteArray();
			if (data.Length != 8)
			{
				throw new InvalidOperationException("Hash must be exactly 8 bytes to convert to a TimeSpan.");
			}
			long ticks = BitConverter.ToInt64(data, 0);
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
		public string AsBase85()
		{
			const string Base85Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~";
			byte[] data = AsByteArray();
			if (data.Length == 0) return "";
			var builder = new StringBuilder();
			int value = 0;
			int count = 0;
			foreach (byte b in data)
			{
				value = (value << 8) | b;
				count++;
				if (count == 4)
				{
					for (int i = 4; i >= 0; i--)
					{
						builder.Append(Base85Alphabet[(value / (int)Math.Pow(85, i)) % 85]);
					}
					value = 0;
					count = 0;
				}
			}
			if (count > 0)
			{
				value <<= (4 - count) * 8; // Pad with zeros
				for (int i = 4; i >= 5 - count; i--)
				{
					builder.Append(Base85Alphabet[(value / (int)Math.Pow(85, i)) % 85]);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBase58()
		{
			const string Base58Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
			byte[] data = AsByteArray();

			byte[] dataPadded = new byte[data.Length + 1];
			Array.Copy(data, dataPadded, data.Length);
			var intData = new BigInteger(dataPadded);

			var builder = new StringBuilder();
			while (intData > 0)
			{
				intData = BigInteger.DivRem(intData, 58, out BigInteger remainder);
				builder.Insert(0, Base58Alphabet[(int)remainder]);
			}

			for (int i = data.Length - 1; i >= 0 && data[i] == 0; i--)
			{
				builder.Insert(0, '1');
			}

			return builder.ToString();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string AsBase32()
		{
			const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
			byte[] data = AsByteArray();

			if (data.Length == 0) return "";

			var builder = new StringBuilder();
			int bitsRead = 0;
			int buffer = 0;

			foreach (byte b in data)
			{
				buffer = (buffer << 8) | b;
				bitsRead += 8;

				while (bitsRead >= 5)
				{
					int index = (buffer >> (bitsRead - 5)) & 31;
					builder.Append(Base32Alphabet[index]);
					bitsRead -= 5;
				}
			}

			if (bitsRead > 0)
			{
				int index = (buffer << (5 - bitsRead)) & 31;
				builder.Append(Base32Alphabet[index]);
			}

			int padding = (8 - (builder.Length % 8)) % 8;
			builder.Append('=', padding);

			return builder.ToString();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns>
		/// <inheritdoc/>
		/// </returns>
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
		/// <returns>
		/// <inheritdoc/>
		/// </returns>
		public string AsHexString() => AsHexString(false);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="uppercase"><inheritdoc/></param>
		/// <returns>
		/// <inheritdoc/>
		/// </returns>
		public string AsHexString(bool uppercase)
		{
			var stringBuilder = new StringBuilder(Hash.Length);
			var formatString = uppercase ? "X2" : "x2";

			foreach (var byteValue in Hash)
			{
				stringBuilder.Append(byteValue.ToString(formatString));
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
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
		/// Determines whether the specified <see cref="Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as IHashValue);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(IHashValue other)
		{
			if (other == null || other.BitLength != BitLength)
			{
				return false;
			}

			return Hash.SequenceEqual(other.Hash);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
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
		/// <param name="bitLength"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public virtual IHashValue Coerce(int bitLength)
		{
			if (bitLength < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(bitLength), $"{nameof(bitLength)} must be greater than or equal to 1.");
			}

			return new HashValue(ArrayHelpers.CoerceToArray(Hash.ToArray(), bitLength), bitLength);
		}
	}
}
