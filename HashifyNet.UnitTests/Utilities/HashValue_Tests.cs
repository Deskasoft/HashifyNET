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

using HashifyNet.Algorithms.SHA1;
using HashifyNet.Algorithms.SHA256;
using HashifyNet.Algorithms.SHA384;
using HashifyNet.Algorithms.SHA512;
using HashifyNet.Core.Utilities;
using Moq;
using System.Collections.Immutable;
using System.Text;

namespace HashifyNet.UnitTests.Utilities
{
	public class HashValue_Tests
	{
		#region Constructor

		[Fact]
		public void HashValue_Constructor_ValidParameters_Works()
		{
			// Enumerable
			GC.KeepAlive(
				new HashValue(ValueEndianness.NotApplicable, Enumerable.Range(0, 1).Select(i => (byte)i), 8));

			// Non-enumerable
			GC.KeepAlive(
				new HashValue(ValueEndianness.NotApplicable, new byte[1], 8));
		}

		[Fact]
		public void HashValue_Constructor_CoerceToArray_WorksAsExpected()
		{
			var underlyingHashValues = new IEnumerable<byte>[] {
				Enumerable.Range(1, 2).Select(i => (byte) i),
				Enumerable.Range(1, 2).Concat(new[] { 0 }).Select(i => (byte) i),
				new List<byte>() { 1, 2 },
				new List<byte>() { 1, 2, 0 },
				new byte[] { 1, 2 },
				new byte[] { 1, 2, 0 }
			};

			foreach (var underlyingHashValue in underlyingHashValues)
			{
				var hashValues = new[] {
					new HashValue(ValueEndianness.NotApplicable, ArrayHelpers.CoerceToArray(underlyingHashValue, 8), 8),
					new HashValue(ValueEndianness.NotApplicable, ArrayHelpers.CoerceToArray(underlyingHashValue, 9), 9),
					new HashValue(ValueEndianness.NotApplicable, ArrayHelpers.CoerceToArray(underlyingHashValue, 10), 10),
					new HashValue(ValueEndianness.NotApplicable, ArrayHelpers.CoerceToArray(underlyingHashValue, 16), 16),
					new HashValue(ValueEndianness.NotApplicable, ArrayHelpers.CoerceToArray(underlyingHashValue, 24), 24)
				};

				Assert.Equal(new byte[] { 1 }, hashValues[0].Hash);
				Assert.Equal(new byte[] { 1, 0 }, hashValues[1].Hash);
				Assert.Equal(new byte[] { 1, 2 }, hashValues[2].Hash);
				Assert.Equal(new byte[] { 1, 2 }, hashValues[3].Hash);
				Assert.Equal(new byte[] { 1, 2, 0 }, hashValues[4].Hash);
			}
		}

		#region Hash

		[Fact]
		public void HashValue_Constructor_Hash_IsNull_Throws()
		{
			Assert.Equal(
				"hash",
				Assert.Throws<ArgumentException>(() =>
						new HashValue(ValueEndianness.NotApplicable, null, 8))
					.ParamName);
		}

		#endregion

		#region BitLength

		[Fact]
		public void HashValue_Constructor_BitLength_IsInvalid_Throws()
		{
			var invalidBitLengths = new[] { int.MinValue, -1, 0 };

			foreach (var invalidBitLength in invalidBitLengths)
			{
				Assert.Equal(
					"bitLength",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new HashValue(ValueEndianness.NotApplicable, new byte[1], invalidBitLength))
						.ParamName);
			}
		}

		#endregion

		#endregion

		#region Hash

		[Fact]
		public void HashValue_Hash_IsCopyOfConstructorValue()
		{
			// Underlying Enumerable
			{
				var enumerableValue = Enumerable.Range(2, 1).Select(i => (byte)i);

				var hashValue = new HashValue(ValueEndianness.NotApplicable, enumerableValue, 8);

				Assert.NotStrictEqual(enumerableValue, hashValue.AsByteArray());
				Assert.Equal(enumerableValue, hashValue.AsByteArray());
			}

			// Underlying Array
			{
				var arrayValue = new byte[] { 2 };

				var hashValue = new HashValue(ValueEndianness.NotApplicable, arrayValue, 8);

				Assert.NotStrictEqual(arrayValue, hashValue.AsByteArray());
				Assert.Equal(arrayValue, hashValue.AsByteArray());
			}
		}

		#endregion

		#region BitLength

		[Fact]
		public void HashValue_BitLength_IsSameAsConstructorValue()
		{
			var validBitLengths = Enumerable.Range(1, 16);

			foreach (var validBitLength in validBitLengths)
			{
				byte[] orig = new byte[2];
				byte[] coerced = ArrayHelpers.CoerceToArray(orig, validBitLength);
				var hashValue = new HashValue(ValueEndianness.NotApplicable, coerced, validBitLength);

				Assert.Equal(validBitLength, hashValue.BitLength);
			}
		}

		#endregion

		#region ByteLength
		[Fact]
		public void HashValue_ByteLength_IsSameAsConstructorValue()
		{
			var validByteLengths = Enumerable.Range(1, 16);

			foreach (var validByteLength in validByteLengths)
			{
				var hv = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer((validByteLength + 7) / 8), validByteLength);
				Assert.Equal(hv.Hash.Length, hv.ByteLength);
			}
		}
		#endregion

		#region AsBase64String

		[Fact]
		public void HashValue_AsBase64String_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("f"), 8), Base64String = "Zg==" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("fo"), 16), Base64String = "Zm8=" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("foo"), 24), Base64String = "Zm9v" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("foob"), 32), Base64String = "Zm9vYg==" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("fooba"), 40), Base64String = "Zm9vYmE=" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, Encoding.ASCII.GetBytes("foobar"), 48), Base64String = "Zm9vYmFy" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base64String,
					knownValue.HashValue.AsBase64String());
			}
		}

		#endregion

		#region AsBitArray

		[Fact]
		public void HashValue_AsBitArray_ExpectedValues()
		{
			var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173 }, 8);
			var bitArray = hashValue.AsBitArray();

			Assert.True(bitArray[0]);
			Assert.False(bitArray[1]);
			Assert.True(bitArray[2]);
			Assert.True(bitArray[3]);
			Assert.False(bitArray[4]);
			Assert.True(bitArray[5]);
			Assert.False(bitArray[6]);
			Assert.True(bitArray[7]);
		}

		#endregion

		#region AsHexString

		[Fact]
		public void HashValue_AsHexString_ExpectedValue()
		{
			var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 255 }, 24);

			Assert.Equal(
				"AD00FF",
				hashValue.AsHexString());
		}
		#endregion

		#region AsByteArray
		[Fact]
		public void HashValue_AsByteArray_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0 }, 8), ByteArray = new byte[] { 0 } },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0, 0 }, 16), ByteArray = new byte[] { 0, 0 } },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 255 }, 24), ByteArray = new byte[] { 173, 0, 255 } },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.ByteArray,
					knownValue.HashValue.AsByteArray());
			}
		}
		#endregion

		#region CalculateEntropy
		[Fact]
		public void HashValue_CalculateEntropy_ExpectedValues()
		{
			var knownValues = new[] {
				new { EntropyValue = HashFactory<ISHA512>.Create().ComputeHash(TestConstants.Empty).CalculateEntropy(), ExpectedMinimum = 5.0d },
				new { EntropyValue = HashFactory<ISHA384>.Create().ComputeHash(TestConstants.Empty).CalculateEntropy(), ExpectedMinimum = 5.0d },
				new { EntropyValue = HashFactory<ISHA256>.Create().ComputeHash(TestConstants.Empty).CalculateEntropy(), ExpectedMinimum = 4.4d },
				new { EntropyValue = HashFactory<ISHA1>.Create().ComputeHash(TestConstants.Empty).CalculateEntropy(), ExpectedMinimum = 4.3d },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.InRange(knownValue.EntropyValue, knownValue.ExpectedMinimum, 8.0d);
			}
		}
		#endregion

		#region CalculateEntropyPercentage
		[Fact]
		public void HashValue_CalculateEntropyPercentage_ExpectedValues()
		{
			var knownValues = new[] {
				new { EntropyPercentage = HashFactory<ISHA512>.Create().ComputeHash(TestConstants.Empty).CalculateEntropyPercentage(), ExpectedMinimum = 96.0d },
				new { EntropyPercentage = HashFactory<ISHA384>.Create().ComputeHash(TestConstants.Empty).CalculateEntropyPercentage(), ExpectedMinimum = 97.0d},
				new { EntropyPercentage = HashFactory<ISHA256>.Create().ComputeHash(TestConstants.Empty).CalculateEntropyPercentage(), ExpectedMinimum = 98.0d },
				new { EntropyPercentage = HashFactory<ISHA1>.Create().ComputeHash(TestConstants.Empty).CalculateEntropyPercentage(), ExpectedMinimum = 99.0d },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.InRange(knownValue.EntropyPercentage, knownValue.ExpectedMinimum, 100.0d);
			}
		}
		#endregion

		#region CopyTo
		[Fact]
		public void HashValue_CopyTo_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64) }
			};

			foreach (var knownValue in knownValues)
			{
				byte[] output1 = new byte[8];

				Span<byte> output2 = new Span<byte>(new byte[8]);

				ImmutableArray<byte>.Builder output3 = ImmutableArray.CreateBuilder<byte>(8);
				output3.Count = 8;

				MemoryStream output4 = new MemoryStream(8);

				knownValue.HashValue.CopyTo(output1, 0);
				knownValue.HashValue.CopyTo(output2);
				knownValue.HashValue.CopyTo(output3, 0);
				knownValue.HashValue.CopyTo(output4);

				byte[] original = knownValue.HashValue.AsByteArray();
				Assert.Equal(original, output1);
				Assert.Equal(original, output2);
				Assert.Equal(original, output3);
				Assert.Equal(original, output4.ToArray());
			}
		}
		#endregion

		#region Endianness
		[Fact]
		public void HashValue_Endianness_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64);
			var hv2 = new HashValue(ValueEndianness.BigEndian, TestConstants.CreateRandomBuffer(8), 64);
			var hv3 = new HashValue(ValueEndianness.LittleEndian, TestConstants.CreateRandomBuffer(8), 64);

			// Endianness shouldn't touch the buffer if NotApplicable:
			Assert.Equal(hv1.WithEndianness(ValueEndianness.BigEndian), hv1);
			Assert.Equal(hv1.WithEndianness(ValueEndianness.LittleEndian), hv1);
			Assert.Equal(hv1.WithEndianness(ValueEndianness.NotApplicable), hv1);

			// Endianness must not touch if the desired endianness is the same as the current:
			Assert.Equal(hv1.WithEndianness(ValueEndianness.NotApplicable), hv1);
			Assert.Equal(hv2.WithEndianness(ValueEndianness.BigEndian), hv2);
			Assert.Equal(hv3.WithEndianness(ValueEndianness.LittleEndian), hv3);

			// Endianness must convert Big Endian to Little Endian and Little Endian to Big Endian:
			Assert.Equal(hv2.WithEndianness(ValueEndianness.LittleEndian), hv2.Reverse());
			Assert.Equal(hv3.WithEndianness(ValueEndianness.BigEndian), hv3.Reverse());

			// Revert Endianness must convert Big Endian to Little Endian and Little Endian to Big Endian but not touch NotApplicable:
			Assert.Equal(hv1.ReverseEndianness(), hv1);
			Assert.Equal(hv2.ReverseEndianness(), hv2.Reverse());
			Assert.Equal(hv3.ReverseEndianness(), hv3.Reverse());

			// AsBigEndian must return Big Endian and NotApplicable as-is but convert Little Endian to Big Endian:
			Assert.Equal(hv1.AsBigEndian(), hv1);
			Assert.Equal(hv2.AsBigEndian(), hv2);
			Assert.Equal(hv3.AsBigEndian(), hv3.Reverse());

			// AsLittleEndian must return Little Endian and NotApplicable as-is but convert Big Endian to Little Endian:
			Assert.Equal(hv1.AsLittleEndian(), hv1);
			Assert.Equal(hv2.AsLittleEndian(), hv2.Reverse());
			Assert.Equal(hv3.AsLittleEndian(), hv3);
		}
		#endregion

		#region Integrals
		[Fact]
		public void HashValue_AsDecimal_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv3 = new HashValue(ValueEndianness.NotApplicable, new byte[12], 96);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsDecimal());
			Assert.Equal(0, hv2.AsDecimal());
			Assert.Equal(0, hv3.AsDecimal());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsDecimal());
		}

		[Fact]
		public void HashValue_AsDouble_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsDouble());
			Assert.Equal(0, hv2.AsDouble());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsDouble());
		}

		[Fact]
		public void HashValue_AsSingle_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[2], 16);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsSingle());
			Assert.Equal(0, hv2.AsSingle());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsSingle());
		}

		[Fact]
		public void HashValue_AsInt64_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsInt64());
			Assert.Equal(0, hv2.AsInt64());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsInt64());
		}

		[Fact]
		public void HashValue_AsInt32_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[2], 16);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsInt32());
			Assert.Equal(0, hv2.AsInt32());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsInt32());
		}

		[Fact]
		public void HashValue_AsInt16_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[1], 8);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[2], 16);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(0, hv1.AsInt16());
			Assert.Equal(0, hv2.AsInt16());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsInt16());
		}

		[Fact]
		public void HashValue_AsBigInteger_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[1], 8);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[2], 16);
			var hv3 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv4 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv5 = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);
			var hv6 = new HashValue(ValueEndianness.NotApplicable, new byte[32], 256);
			var hv7 = new HashValue(ValueEndianness.NotApplicable, new byte[64], 512);
			var hv8 = new HashValue(ValueEndianness.NotApplicable, new byte[128], 1024);

			Assert.Equal(0, hv1.AsBigInteger());
			Assert.Equal(0, hv2.AsBigInteger());
			Assert.Equal(0, hv3.AsBigInteger());
			Assert.Equal(0, hv4.AsBigInteger());
			Assert.Equal(0, hv5.AsBigInteger());
			Assert.Equal(0, hv6.AsBigInteger());
			Assert.Equal(0, hv7.AsBigInteger());
			Assert.Equal(0, hv8.AsBigInteger());
		}

		#endregion

		#region AsChar
		[Fact]
		public void HashValue_AsChar_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[2], 16);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[1], 8);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);

			Assert.Equal('\0', hv1.AsChar());
			Assert.Equal('\0', hv2.AsChar());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsChar());
		}
		#endregion

		#region Time
		[Fact]
		public void HashValue_AsDateTime_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(new DateTime(0), hv1.AsDateTime());
			Assert.Equal(new DateTime(0), hv2.AsDateTime());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsDateTime());
		}

		[Fact]
		public void HashValue_AsDateTimeOffset_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(new DateTimeOffset(0, TimeSpan.Zero), hv1.AsDateTimeOffset());
			Assert.Equal(new DateTimeOffset(0, TimeSpan.Zero), hv2.AsDateTimeOffset());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsDateTimeOffset());
		}

		[Fact]
		public void HashValue_AsTimeSpan_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[4], 32);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal(new TimeSpan(0), hv1.AsTimeSpan());
			Assert.Equal(new TimeSpan(0), hv2.AsTimeSpan());

			Assert.Throws<NotSupportedException>(() => hv_invalid.AsTimeSpan());
		}
		#endregion

		#region AsBinaryString
		[Fact]
		public void HashValue_AsBinaryString_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);

			Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", hv1.AsBinaryString());
			Assert.Equal("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", hv2.AsBinaryString());
		}
		#endregion

		#region AsGuid
		[Fact]
		public void HashValue_AsGuid_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, new byte[8], 64);
			var hv2 = new HashValue(ValueEndianness.NotApplicable, new byte[16], 128);
			var hv_invalid = new HashValue(ValueEndianness.NotApplicable, new byte[32], 256);

			Assert.Equal(new Guid(new byte[16]), hv1.AsGuid());
			Assert.Equal(new Guid(new byte[16]), hv2.AsGuid());
			Assert.Throws<InvalidOperationException>(() => hv_invalid.AsGuid());
		}
		#endregion

		#region AsSpan
		[Fact]
		public void HashValue_AsSpan_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64);

			var span1 = hv1.AsSpan();
			var span2 = hv1.AsSpan(4);
			var span3 = hv1.AsSpan(4, 4);

			Assert.Equal(span1, hv1.Hash.AsSpan());
			Assert.Equal(span2, hv1.Hash.AsSpan(0, 4));
			Assert.Equal(span3, hv1.Hash.AsSpan(4, 4));
		}
		#endregion

		#region AsMemory
		[Fact]
		public void HashValue_AsMemory_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64);

			var mem1 = hv1.AsMemory();

			Assert.Equal(mem1, hv1.Hash.AsMemory());
		}
		#endregion

		#region Slice
		[Fact]
		public void HashValue_Slice_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64);

			var slice1 = hv1.Slice(4);
			var slice2 = hv1.Slice(4, 4);

			Assert.Equal(slice1.AsSpan(), hv1.AsSpan(4));
			Assert.Equal(slice2.AsSpan(), hv1.AsSpan(4, 4));
		}
		#endregion

		#region AsStream
		[Fact]
		public void HashValue_AsStream_Works()
		{
			var hv1 = new HashValue(ValueEndianness.NotApplicable, TestConstants.CreateRandomBuffer(8), 64);

			Stream ns = hv1.AsStream();
			ns.Position = 0;
			byte[] ns_array = new byte[8];
			int c = ns.Read(ns_array, 0, 8);
			Assert.Equal(8, c);

			MemoryStream ms = new MemoryStream(8);
			hv1.AsStream(ms);

			byte[] original = hv1.AsByteArray();
			Assert.Equal(original, ns_array);
			Assert.Equal(original, ms.ToArray());
		}
		#endregion

		#region AsBase32String
		[Fact]
		public void HashValue_AsBase32String_Rfc4648_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base32String = "MZXW6YTBOI======" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base32String = "KRUGKIDROVUWG2ZAMJZG653OEBTG66BANJ2W24DTEBXXMZLSEB2GQZJANRQXU6JAMRXWO===" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base32String = "JRXXEZLNEBUXA43VNUQGI33MN5ZCA43JOQQGC3LFOQWCAY3PNZZWKY3UMV2HK4RAMFSGS4DJONRWS3THEBSWY2LUFYQCAVLUEBXXE3TBOJSSAYLMNFYXKYLNEBWWC5LSNFZSYIDBOQQHM33MOV2HAYLUEBWWC43TMEXCAICQNBQXGZLMNR2XGIDQOVWHM2LOMFZCA4DVOJ2XGIDFOUQHMZLOMVXGC5DJOMQGG33NNVXWI3ZO" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base32String,
					knownValue.HashValue.AsBase32String());
			}
		}

		[Fact]
		public void HashValue_AsBase32String_Crockford_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base32String = "CSQPYRK1E8" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base32String = "AHM6A83HENMP6TS0C9S6YXVE41K6YY10D9TPTW3K41QQCSBJ41T6GS90DHGQMY90CHQPE" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base32String = "9HQQ4SBD41MQ0WVNDMG68VVCDXS20WV9EGG62VB5EGP20RVFDSSPARVMCNT7AWH0C5J6JW39EDHPJVK741JPRTBM5RG20NBM41QQ4VK1E9JJ0RBCD5RQARBD41PP2XBJD5SJR831EGG7CVVCENT70RBM41PP2WVKC4Q2082GD1GQ6SBCDHTQ683GENP7CTBEC5S20W3NE9TQ6835EMG7CSBECNQ62X39ECG66VVDDNQP8VSE" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base32String,
					knownValue.HashValue.AsBase32String(Base32Variant.Crockford));
			}
		}
		#endregion

		#region AsBase58String
		[Fact]
		public void HashValue_AsBase58String_Bitcoin_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base58String = "t1Zv2yaZ" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base58String = "7DdiPPYtxLjCD3wA1po2rvZHTDYjkZYiEtazrfiwJcwnKCizhGFhBGHeRdx" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base58String = "ANTbBh3nk9K3wfs1J43pynEbvAQiZP4k1uVXJhQZdBpEv9MncAngn1BFz3sCykR9GDwBfvLXJgds29L8r3ogWWPpxEBZeHnsF7pVXdfkQwTueyLc8Rf5FKZe7fFYmLwsTnYTS8jsAYWGqKnL3QsJTiuZgUAF9NKRyyG3Tz4Fn2MHrtTXyMYPq9q4msyo35fxivnkf9WXzRrzH" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base58String,
					knownValue.HashValue.AsBase58String()); // Base58Variant.Bitcoin
			}
		}

		[Fact]
		public void HashValue_AsBase58String_Flickr_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base58String = "T1yV2Yzy" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base58String = "7dCHooxTXkJcd3Wa1PN2RVyhsdxJKyxHeTzZREHWiBWMjcHZGgfGbghDqCX" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base58String = "ansAbG3MK9j3WES1i43PYMeAVapHyo4K1UuwiGpyCbPeV9mMBaMFM1bfZ3ScYKq9gdWbEVkwiFCS29k8R3NFvvoPXebyDhMSf7PuwCEKpWsUDYkB8qE5fjyD7EfxLkWSsMxsr8JSaxvgQjMk3pSisHUyFtaf9njqYYg3sZ4fM2mhRTswYmxoQ9Q4LSYN35EXHVMKE9vwZqRZh" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base58String,
					knownValue.HashValue.AsBase58String(Base58Variant.Flickr));
			}
		}

		[Fact]
		public void HashValue_AsBase58String_Ripple_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base58String = "trZvpy2Z" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base58String = "fDd5PPYtxLjUDsAwrFopivZHTDYjkZY5Nt2ziC5AJcA8KU5z6GE6BGHeRdx" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base58String = "w4TbB6s8k9KsAC1rJhsFy8NbvwQ5ZPhkruVXJ6QZdBFNv9M8cw8g8rBEzs1UykR9GDABCvLXJgd1p9L3isogWWPFxNBZeH81EfFVXdCkQATueyLc3RCnEKZefCEYmLA1T8YTS3j1wYWGqK8LsQ1JT5uZg7wE94KRyyGsTzhE8pMHitTXyMYPq9qhm1yosnCx5v8kC9WXzRizH" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base58String,
					knownValue.HashValue.AsBase58String(Base58Variant.Ripple));
			}
		}
		#endregion

		#region AsBase85String
		[Fact]
		public void HashValue_AsBase85String_Ascii85_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base85String = "AoDTs@<)" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base85String = "<+ohcEHPu*CER),Dg-(AAoDo:C3=B4F!,CEATAo8BOr<&@=!2AA8c)" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base85String = "9Q+r_D'3P3F*2=BA8c:&EZfF;F<G\"/ATTIG@rH7+ARfgnFEMUH@:X(kBldcuDJ()'Ch[uD+<X[++E):<@<,p%@;KXtF^],0D..R-BlbgJ@<<W8DesQ<E+*i2D..L,@4iZF:hX9YASc1*F!,FECj'N1@<*K0F`MVG+D#[<G%GQ&DIIX$F!+t2D/F3%D_;" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base85String,
					knownValue.HashValue.AsBase85String());
			}
		}

		[Fact]
		public void HashValue_AsBase85String_AdobeAscii85_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base85String = "<~AoDTs@<)~>" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base85String = "<~<+ohcEHPu*CER),Dg-(AAoDo:C3=B4F!,CEATAo8BOr<&@=!2AA8c)~>" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base85String = "<~9Q+r_D'3P3F*2=BA8c:&EZfF;F<G\"/ATTIG@rH7+ARfgnFEMUH@:X(kBldcuDJ()'Ch[uD+<X[++E):<@<,p%@;KXtF^],0D..R-BlbgJ@<<W8DesQ<E+*i2D..L,@4iZF:hX9YASc1*F!,FECj'N1@<*K0F`MVG+D#[<G%GQ&DIIX$F!+t2D/F3%D_;~>" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base85String,
					knownValue.HashValue.AsBase85String(Base85Variant.AdobeAscii85));
			}
		}

		[Fact]
		public void HashValue_AsBase85String_Z85_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base85String = "w]zP%vr8" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base85String = "ra]?=ADL#9yAN8bz*c7ww]z]pyisxjB0byAwPw]nxK@r5vs0hwwn=8" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base85String = "oMa@.z6iLiB9hsxwn=p5AV/BqBrC1ewPPECv@DmawN/*[BAIQDvpT7>x(^=#zF786y?W#zarTWaaA8prvrb{4vqGT$BZYbfzddNcx(+*FvrrSnz!%MrAa9&hzddHbvj&VBp?ToUwO=g9B0bBAy<6Jgvr9GfB-IRCaz2WrC4CM5zEET3B0a$hzeBi4z.q" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base85String,
					knownValue.HashValue.AsBase85String(Base85Variant.Z85));
			}
		}

		[Fact]
		public void HashValue_AsBase85String_Rfc1924_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.FooBar, TestConstants.FooBar.Length * 8), Base85String = "W^Zp|VR8" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.QuickBrownFox, TestConstants.QuickBrownFox.Length * 8), Base85String = "RA^-&adl~9Yan8BZ+C7WW^Z^PYISXJb0BYaWpW^NXk{R5VS0HWWN&8" },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, TestConstants.LoremIpsum, TestConstants.LoremIpsum.Length * 8), Base85String = "OmA{!Z6IlIb9HSXWN&P5av*bQbRc1EWppecV{dMAWn*+@baiqdVPt7=X>(&~Zf786Y-w~ZARtwAAa8PRVRB_4VQgt}bzyBFZDDnCX>%+fVRRsNZ)|mRaA9;HZDDhBVJ;vbP-tOuWo&G9b0BbaY<6jGVR9gFb#ircAZ2wRc4cm5Zeet3b0A}HZEbI4Z!Q" },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.Base85String,
					knownValue.HashValue.AsBase85String(Base85Variant.Rfc1924));
			}
		}
		#endregion

		#region GetHashCode

		[Fact]
		public void HashValue_GetHashCode_ExpectedValues()
		{
			var knownValues = new[] {
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0 }, 4), HashCode = 16213 },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0 }, 8), HashCode = 16089 },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0, 0 }, 12), HashCode = 494915 },
				new { HashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 0, 0 }, 16), HashCode = 521823 },
			};

			foreach (var knownValue in knownValues)
			{
				Assert.Equal(
					knownValue.HashCode,
					knownValue.HashValue.GetHashCode());
			}
		}

		#endregion

		#region Equals

		[Fact]
		public void HashValue_Equals_Works()
		{
			{
				var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 255 }, 24);

				Assert.False(hashValue.Equals(null));
				Assert.False(hashValue.Equals((object)null));
				Assert.False(hashValue.Equals("abc"));

				var mockValue = Mock.Of<IHashValue>(hv => hv.BitLength == 24 && hv.Hash == new byte[] { 173, 0, 255 }.ToImmutableArray());

				Assert.True(hashValue.Equals(mockValue));
				Assert.True(hashValue.Equals((object)mockValue));
			}

			{
				var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 254 }, 24);
				var mockValue = Mock.Of<IHashValue>(hv => hv.BitLength == 24 && hv.Hash == new byte[] { 173, 0, 255 }.ToImmutableArray());

				Assert.False(hashValue.Equals(mockValue));
			}

			{
				var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 254 }, 23);
				var mockValue = Mock.Of<IHashValue>(hv => hv.BitLength == 24 && hv.Hash == new byte[] { 173, 0, 254 }.ToImmutableArray());

				Assert.False(hashValue.Equals(mockValue));
			}

			{
				var hashValue = new HashValue(ValueEndianness.NotApplicable, new byte[] { 173, 0, 255 }, 24);
				var mockValue = Mock.Of<IHashValue>(hv => hv.BitLength == 23 && hv.Hash == new byte[] { 173, 0, 127 }.ToImmutableArray());

				Assert.False(hashValue.Equals(mockValue));
			}
		}

		#endregion
	}
}
