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

using HashifyNet.Algorithms.CRC;

namespace HashifyNet.UnitTests.Algorithms.CRC
{
	public class CRCConfig_Tests
	{
		[Fact]
		public void CRCConfig_Defaults_HaventChanged()
		{
			var crcConfig = new CRCConfig();


			Assert.Equal(0, crcConfig.HashSizeInBits);
			Assert.Equal(0L, crcConfig.InitialValue);
			Assert.Equal(0L, crcConfig.Polynomial);
			Assert.False(crcConfig.ReflectIn);
			Assert.False(crcConfig.ReflectOut);
			Assert.Equal(0L, crcConfig.XOrOut);
		}

		[Fact]
		public void CRCConfig_Standards_HaventChanged()
		{
			foreach (var crcStandard in _expectedCrcStandards)
			{
				var crcConfig = crcStandard.Key();
				var expectedCrcConig = crcStandard.Value;

				Assert.Equal(expectedCrcConig.HashSizeInBits, crcConfig.HashSizeInBits);
				Assert.Equal(expectedCrcConig.InitialValue, crcConfig.InitialValue);
				Assert.Equal(expectedCrcConig.Polynomial, crcConfig.Polynomial);
				Assert.Equal(expectedCrcConig.ReflectIn, crcConfig.ReflectIn);
				Assert.Equal(expectedCrcConig.ReflectOut, crcConfig.ReflectOut);
				Assert.Equal(expectedCrcConig.XOrOut, crcConfig.XOrOut);
			}
		}

		[Fact]
		public void CRCConfig_Clone_Works()
		{
			var crcConfig = new CRCConfigProfileCRC64();

			var crcConfigClone = crcConfig.Clone();

			Assert.IsType<CRCConfig>(crcConfigClone);

			Assert.Equal(crcConfig.HashSizeInBits, crcConfigClone.HashSizeInBits);
			Assert.Equal(crcConfig.InitialValue, crcConfigClone.InitialValue);
			Assert.Equal(crcConfig.Polynomial, crcConfigClone.Polynomial);
			Assert.Equal(crcConfig.ReflectIn, crcConfigClone.ReflectIn);
			Assert.Equal(crcConfig.ReflectOut, crcConfigClone.ReflectOut);
			Assert.Equal(crcConfig.XOrOut, crcConfigClone.XOrOut);
		}

		private readonly IEnumerable<KeyValuePair<Func<ICRCConfig>, ICRCConfig>> _expectedCrcStandards =
			new[] {
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC3_ROHC(),
					new CRCConfig {
						HashSizeInBits = 3,
						Polynomial = 0x3,
						InitialValue = 0x7,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC4_ITU(),
					new CRCConfig {
						HashSizeInBits = 4,
						Polynomial = 0x3,
						InitialValue = 0x0,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC5_EPC(),
					new CRCConfig {
						HashSizeInBits = 5,
						Polynomial = 0x09,
						InitialValue = 0x09,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC5_ITU(),
					new CRCConfig {
						HashSizeInBits = 5,
						Polynomial = 0x15,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC5_USB(),
					new CRCConfig {
						HashSizeInBits = 5,
						Polynomial = 0x05,
						InitialValue = 0x1f,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x1f,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC6_CDMA2000A(),
					new CRCConfig {
						HashSizeInBits = 6,
						Polynomial = 0x27,
						InitialValue = 0x3f,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC6_CDMA2000B(),
					new CRCConfig {
						HashSizeInBits = 6,
						Polynomial = 0x07,
						InitialValue = 0x3f,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC6_DARC(),
					new CRCConfig {
						HashSizeInBits = 6,
						Polynomial = 0x19,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC6_ITU(),
					new CRCConfig {
						HashSizeInBits = 6,
						Polynomial = 0x03,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC7(),
					new CRCConfig {
						HashSizeInBits = 7,
						Polynomial = 0x09,
						InitialValue = 0x00,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC7_ROHC(),
					new CRCConfig {
						HashSizeInBits = 7,
						Polynomial = 0x4f,
						InitialValue = 0x7f,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x07,
						InitialValue = 0x00,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_CDMA2000(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x9b,
						InitialValue = 0xff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_DARC(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x39,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_DVBS2(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0xd5,
						InitialValue = 0x00,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_EBU(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x1d,
						InitialValue = 0xff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_ICODE(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x1d,
						InitialValue = 0xfd,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_ITU(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x07,
						InitialValue = 0x00,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x55,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_MAXIM(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x31,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_ROHC(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x07,
						InitialValue = 0xff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC8_WCDMA(),
					new CRCConfig {
						HashSizeInBits = 8,
						Polynomial = 0x9b,
						InitialValue = 0x00,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC10(),
					new CRCConfig {
						HashSizeInBits = 10,
						Polynomial = 0x233,
						InitialValue = 0x000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC10_CDMA2000(),
					new CRCConfig {
						HashSizeInBits = 10,
						Polynomial = 0x3d9,
						InitialValue = 0x3ff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC11(),
					new CRCConfig {
						HashSizeInBits = 11,
						Polynomial = 0x385,
						InitialValue = 0x01a,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC12_3GPP(),
					new CRCConfig {
						HashSizeInBits = 12,
						Polynomial = 0x80f,
						InitialValue = 0x000,
						ReflectIn = false,
						ReflectOut = true,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC12_CDMA2000(),
					new CRCConfig {
						HashSizeInBits = 12,
						Polynomial = 0xf13,
						InitialValue = 0xfff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC12_DECT(),
					new CRCConfig {
						HashSizeInBits = 12,
						Polynomial = 0x80f,
						InitialValue = 0x000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC13_BBC(),
					new CRCConfig {
						HashSizeInBits = 13,
						Polynomial = 0x1cf5,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC14_DARC(),
					new CRCConfig {
						HashSizeInBits = 14,
						Polynomial = 0x0805,
						InitialValue = 0x0000,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC15(),
					new CRCConfig {
						HashSizeInBits = 15,
						Polynomial = 0x4599,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC15_MPT1327(),
					new CRCConfig {
						HashSizeInBits = 15,
						Polynomial = 0x6815,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0001,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileARC(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0x0000,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_AUGCCITT(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0x1d0f,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_BUYPASS(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_CCITTFALSE(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_CDMA2000(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0xc867,
						InitialValue = 0xffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_DDS110(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0x800d,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_DECTR(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x0589,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0001,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_DECTX(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x0589,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_DNP(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x3d65,
						InitialValue = 0x0000,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_EN13757(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x3d65,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_GENIBUS(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_MAXIM(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0x0000,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_MCRF4XX(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_RIELLO(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xb2aa,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_T10DIF(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8bb7,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_TELEDISK(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0xa097,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_TMS37157(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0x89ec,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC16_USB(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0xffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRCA(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xc6c6,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileKERMIT(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0x0000,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileMODBUS(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x8005,
						InitialValue = 0xffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileX25(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0xffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileXMODEM(),
					new CRCConfig {
						HashSizeInBits = 16,
						Polynomial = 0x1021,
						InitialValue = 0x0000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC24(),
					new CRCConfig {
						HashSizeInBits = 24,
						Polynomial = 0x864cfb,
						InitialValue = 0xb704ce,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC24_FLEXRAYA(),
					new CRCConfig {
						HashSizeInBits = 24,
						Polynomial = 0x5d6dcb,
						InitialValue = 0xfedcba,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC24_FLEXRAYB(),
					new CRCConfig {
						HashSizeInBits = 24,
						Polynomial = 0x5d6dcb,
						InitialValue = 0xabcdef,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC31_PHILIPS(),
					new CRCConfig {
						HashSizeInBits = 31,
						Polynomial = 0x04c11db7,
						InitialValue = 0x7fffffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x7fffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x04c11db7,
						InitialValue = 0xffffffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32_BZIP2(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x04c11db7,
						InitialValue = 0xffffffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0xffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32C(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x1edc6f41,
						InitialValue = 0xffffffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32D(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0xa833982b,
						InitialValue = 0xffffffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0xffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32_MPEG2(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x04c11db7,
						InitialValue = 0xffffffff,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32_POSIX(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x04c11db7,
						InitialValue = 0x00000000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0xffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC32Q(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x814141ab,
						InitialValue = 0x00000000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileJAMCRC(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x04c11db7,
						InitialValue = 0xffffffff,
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = 0x00000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileXFER(),
					new CRCConfig {
						HashSizeInBits = 32,
						Polynomial = 0x000000af,
						InitialValue = 0x00000000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x00000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC40_GSM(),
					new CRCConfig {
						HashSizeInBits = 40,
						Polynomial = 0x0004820009,
						InitialValue = 0x0000000000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0xffffffffff,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC64(),
					new CRCConfig {
						HashSizeInBits = 64,
						Polynomial = 0x42f0e1eba9ea3693,
						InitialValue = 0x0000000000000000,
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = 0x0000000000000000,
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC64_WE(),
					new CRCConfig {
						HashSizeInBits = 64,
						Polynomial = 0x42f0e1eba9ea3693,
						InitialValue = unchecked ((long)0xffffffffffffffff),
						ReflectIn = false,
						ReflectOut = false,
						XOrOut = unchecked ((long)0xffffffffffffffff),
					}
				),
				new KeyValuePair<Func<ICRCConfig>, ICRCConfig>(
					() => new CRCConfigProfileCRC64_XZ(),
					new CRCConfig {
						HashSizeInBits = 64,
						Polynomial = 0x42f0e1eba9ea3693,
						InitialValue = unchecked ((long)0xffffffffffffffff),
						ReflectIn = true,
						ReflectOut = true,
						XOrOut = unchecked ((long)0xffffffffffffffff),
					}
				),
			};
	}
}
