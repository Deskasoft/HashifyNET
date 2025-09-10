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
using Moq;

namespace HashifyNet.UnitTests.Algorithms.CRC
{
	public class CRC_Implementation_Tests
	{
		#region Constructor

		[Fact]
		public void CRC_Implementation_Constructor_ValidInputs_Work()
		{
			var crcConfigMock = new Mock<ICRCConfig>();
			{
				crcConfigMock.SetupGet(cc => cc.HashSizeInBits)
					.Returns(64);

				crcConfigMock.Setup(cc => cc.Clone())
					.Returns(() => crcConfigMock.Object);
			}

			GC.KeepAlive(
				new CRC_Implementation(crcConfigMock.Object));
		}

		#region Config

		[Fact]
		public void CRC_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new CRC_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void CRC_Implementation_Constructor_Config_IsCloned()
		{
			var crcConfigMock = new Mock<ICRCConfig>();
			{
				crcConfigMock.Setup(cc => cc.Clone())
					.Returns(
						new CRCConfig()
						{
							HashSizeInBits = 64
						});
			}

			GC.KeepAlive(
				new CRC_Implementation(crcConfigMock.Object));


			crcConfigMock.Verify(cc => cc.Clone(), Times.Once);

			crcConfigMock.VerifyGet(cc => cc.HashSizeInBits, Times.Never);
			crcConfigMock.VerifyGet(cc => cc.InitialValue, Times.Never);
			crcConfigMock.VerifyGet(cc => cc.Polynomial, Times.Never);
			crcConfigMock.VerifyGet(cc => cc.ReflectIn, Times.Never);
			crcConfigMock.VerifyGet(cc => cc.ReflectOut, Times.Never);
			crcConfigMock.VerifyGet(cc => cc.XOrOut, Times.Never);
		}

		#region HashSizeInBits

		[Fact]
		public void CRC_Implementation_Constructor_Config_HashSizeInBits_IsInvalid_Throws()
		{
			var invalidLengths = new[] { -1, 0, 65, 128 };

			foreach (var length in invalidLengths)
			{
				var crcConfigMock = new Mock<ICRCConfig>();
				{
					crcConfigMock.SetupGet(cc => cc.HashSizeInBits)
						.Returns(length);

					crcConfigMock.Setup(cc => cc.Clone())
						.Returns(() => crcConfigMock.Object);
				}


				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => new CRC_Implementation(crcConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void CRC_Implementation_Constructor_Config_HashSizeInBits_IsValid_Works()
		{
			var validLengths = Enumerable.Range(1, 64);

			foreach (var length in validLengths)
			{
				var crcConfigMock = new Mock<ICRCConfig>();
				{
					crcConfigMock.SetupGet(cc => cc.HashSizeInBits)
						.Returns(length);

					crcConfigMock.Setup(cc => cc.Clone())
						.Returns(() => crcConfigMock.Object);
				}


				GC.KeepAlive(
					new CRC_Implementation(crcConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void CRC_Implementation_Config_IsCloneOfClone()
		{
			var crcConfig3 = Mock.Of<ICRCConfig>();
			var crcConfig2 = Mock.Of<ICRCConfig>(cc => cc.HashSizeInBits == 32 && cc.Clone() == crcConfig3);
			var crcConfig = Mock.Of<ICRCConfig>(cc => cc.Clone() == crcConfig2);

			var crcHash = new CRC_Implementation(crcConfig);

			Assert.Equal(crcConfig3, crcHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void CRC_Implementation_HashSizeInBits_MatchesConfig()
		{
			var validHashSizes = Enumerable.Range(1, 64);

			foreach (var hashSize in validHashSizes)
			{
				var crcConfigMock = new Mock<ICRCConfig>();
				{
					crcConfigMock.SetupGet(cc => cc.HashSizeInBits)
						.Returns(hashSize);

					crcConfigMock.Setup(cc => cc.Clone())
						.Returns(() => crcConfigMock.Object);
				}

				var crc = new CRC_Implementation(crcConfigMock.Object);

				Assert.Equal(hashSize, crc.Config.HashSizeInBits);
			}
		}

		#endregion

		public class IStreamableHashFunction_Tests_CRC3_ROHC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(3, "123456789", 0x6),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC3_ROHC());
		}

		public class IStreamableHashFunction_Tests_CRC4_ITU
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(4, "123456789", 0x7),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC4_ITU());
		}

		public class IStreamableHashFunction_Tests_CRC5_EPC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues =>
				new KnownValue[] {
					new KnownValue(5, "123456789", 0x00),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC5_EPC());
		}

		public class IStreamableHashFunction_Tests_CRC5_ITU
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues =>
				new KnownValue[] {
					new KnownValue(5, "123456789", 0x07),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC5_ITU());
		}

		public class IStreamableHashFunction_Tests_CRC5_USB
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(5, "123456789", 0x19),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC5_USB());
		}

		public class IStreamableHashFunction_Tests_CRC6_CDMA2000A
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(6, "123456789", 0x0d),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC6_CDMA2000A());
		}

		public class IStreamableHashFunction_Tests_CRC6_CDMA2000B
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(6, "123456789", 0x3b),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC6_CDMA2000B());
		}

		public class IStreamableHashFunction_Tests_CRC6_DARC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(6, "123456789", 0x26),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC6_DARC());
		}

		public class IStreamableHashFunction_Tests_CRC6_ITU
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(6, "123456789", 0x06),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC6_ITU());
		}

		public class IStreamableHashFunction_Tests_CRC7
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(7, "123456789", 0x75),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC7());
		}

		public class IStreamableHashFunction_Tests_CRC7_ROHC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(7, "123456789", 0x53),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC7_ROHC());
		}

		public class IStreamableHashFunction_Tests_CRC8
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xf4),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8());
		}

		public class IStreamableHashFunction_Tests_CRC8_CDMA2000
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xda),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_CDMA2000());
		}

		public class IStreamableHashFunction_Tests_CRC8_DARC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0x15),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_DARC());
		}

		public class IStreamableHashFunction_Tests_CRC8_DVBS2
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xbc),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_DVBS2());
		}

		public class IStreamableHashFunction_Tests_CRC8_EBU
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0x97),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_EBU());
		}

		public class IStreamableHashFunction_Tests_CRC8_ICODE
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0x7e),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_ICODE());
		}

		public class IStreamableHashFunction_Tests_CRC8_ITU
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xa1),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_ITU());
		}

		public class IStreamableHashFunction_Tests_CRC8_MAXIM
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xa1),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_MAXIM());
		}

		public class IStreamableHashFunction_Tests_CRC8_ROHC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0xd0),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_ROHC());
		}

		public class IStreamableHashFunction_Tests_CRC8_WCDMA
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(8, "123456789", 0x25),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC8_WCDMA());
		}

		public class IStreamableHashFunction_Tests_CRC10
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(10, "123456789", 0x199),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC10());
		}

		public class IStreamableHashFunction_Tests_CRC10_CDMA2000
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(10, "123456789", 0x233),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC10_CDMA2000());
		}

		public class IStreamableHashFunction_Tests_CRC11
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(11, "123456789", 0x5a3),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC11());
		}

		public class IStreamableHashFunction_Tests_CRC12_3GPP
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(12, "123456789", 0xdaf),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC12_3GPP());
		}

		public class IStreamableHashFunction_Tests_CRC12_CDMA2000
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(12, "123456789", 0xd4d),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC12_CDMA2000());
		}

		public class IStreamableHashFunction_Tests_CRC12_DECT
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(12, "123456789", 0xf5b),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC12_DECT());
		}

		public class IStreamableHashFunction_Tests_CRC13_BBC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(13, "123456789", 0x04fa),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC13_BBC());
		}

		public class IStreamableHashFunction_Tests_CRC14_DARC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(14, "123456789", 0x082d),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC14_DARC());
		}

		public class IStreamableHashFunction_Tests_CRC15
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(15, "123456789", 0x059e),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC15());
		}

		public class IStreamableHashFunction_Tests_CRC15_MPT1327
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(15, "123456789", 0x2566),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC15_MPT1327());
		}

		public class IStreamableHashFunction_Tests_ARC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xbb3d),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileARC());
		}

		public class IStreamableHashFunction_Tests_CRC16_AUGCCITT
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xe5cc),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_AUGCCITT());
		}

		public class IStreamableHashFunction_Tests_CRC16_BUYPASS
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xfee8),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_BUYPASS());
		}

		public class IStreamableHashFunction_Tests_CRC16_CCITTFALSE
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x29b1),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_CCITTFALSE());
		}

		public class IStreamableHashFunction_Tests_CRC16_CDMA2000
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x4c06),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_CDMA2000());
		}

		public class IStreamableHashFunction_Tests_CRC16_DDS110
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x9ecf),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_DDS110());
		}

		public class IStreamableHashFunction_Tests_CRC16_DECTR
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x007e),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_DECTR());
		}

		public class IStreamableHashFunction_Tests_CRC16_DECTX
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x007f),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_DECTX());
		}

		public class IStreamableHashFunction_Tests_CRC16_DNP
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xea82),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_DNP());
		}

		public class IStreamableHashFunction_Tests_CRC16_EN13757
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xc2b7),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_EN13757());
		}

		public class IStreamableHashFunction_Tests_CRC16_GENIBUS
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xd64e),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_GENIBUS());
		}

		public class IStreamableHashFunction_Tests_CRC16_MAXIM
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x44c2),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_MAXIM());
		}

		public class IStreamableHashFunction_Tests_CRC16_MCRF4XX
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x6f91),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_MCRF4XX());
		}

		public class IStreamableHashFunction_Tests_CRC16_RIELLO
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x63d0),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_RIELLO());
		}

		public class IStreamableHashFunction_Tests_CRC16_T10DIF
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xd0db),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_T10DIF());
		}

		public class IStreamableHashFunction_Tests_CRC16_TELEDISK
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x0fb3),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_TELEDISK());
		}

		public class IStreamableHashFunction_Tests_CRC16_TMS37157
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x26b1),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_TMS37157());
		}

		public class IStreamableHashFunction_Tests_CRC16_USB
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xb4c8),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC16_USB());
		}

		public class IStreamableHashFunction_Tests_CRCA
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0xbf05),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRCA());
		}

		public class IStreamableHashFunction_Tests_KERMIT
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x2189),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileKERMIT());
		}

		public class IStreamableHashFunction_Tests_MODBUS
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x4b37),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileMODBUS());
		}

		public class IStreamableHashFunction_Tests_X25
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x906e),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileX25());
		}

		public class IStreamableHashFunction_Tests_XMODEM
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(16, "123456789", 0x31c3),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileXMODEM());
		}

		public class IStreamableHashFunction_Tests_CRC24
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(24, "123456789", 0x21cf02),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC24());
		}

		public class IStreamableHashFunction_Tests_CRC24_FLEXRAYA
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(24, "123456789", 0x7979bd),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC24_FLEXRAYA());
		}

		public class IStreamableHashFunction_Tests_CRC24_FLEXRAYB
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(24, "123456789", 0x1f23b8),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC24_FLEXRAYB());
		}

		public class IStreamableHashFunction_Tests_CRC31_PHILIPS
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(31, "123456789", 0x0ce9e46c),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC31_PHILIPS());
		}

		public class IStreamableHashFunction_Tests_CRC32
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0xcbf43926),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32());
		}

		public class IStreamableHashFunction_Tests_CRC32_BZIP2
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0xfc891918),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32_BZIP2());
		}

		public class IStreamableHashFunction_Tests_CRC32C
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0xe3069283),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32C());
		}

		public class IStreamableHashFunction_Tests_CRC32D
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0x87315576),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32D());
		}

		public class IStreamableHashFunction_Tests_CRC32_MPEG2
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0x0376e6e7),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32_MPEG2());
		}

		public class IStreamableHashFunction_Tests_CRC32_POSIX
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0x765e7680),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32_POSIX());
		}

		public class IStreamableHashFunction_Tests_CRC32Q
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0x3010bf7f),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC32Q());
		}

		public class IStreamableHashFunction_Tests_JAMCRC
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0x340bc6d9),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileJAMCRC());
		}

		public class IStreamableHashFunction_Tests_XFER
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, "123456789", 0xbd0be338),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileXFER());
		}

		public class IStreamableHashFunction_Tests_CRC40_GSM
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(40, "123456789", 0xd4164fc646),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC40_GSM());
		}

		public class IStreamableHashFunction_Tests_CRC64
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, "123456789", 0x6c40df5f0b497347),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC64());
		}

		public class IStreamableHashFunction_Tests_CRC64_WE
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, "123456789", 0x62ec59e3f1a4f00a),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC64_WE());
		}

		public class IStreamableHashFunction_Tests_CRC64_XZ
			: IStreamableHashFunction_TestBase<ICRC, ICRCConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, "123456789", 0x995dc9bbdf1939fa),
				};

			protected override ICRC CreateHashFunction(int hashSize) =>
				new CRC_Implementation(new CRCConfigProfileCRC64_XZ());
		}
	}
}
