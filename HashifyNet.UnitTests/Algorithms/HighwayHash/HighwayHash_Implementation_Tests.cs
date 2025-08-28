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

using HashifyNet.Algorithms.HighwayHash;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.HighwayHash
{
	public class HighwayHash_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HighwayHash_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_IsCloned()
		{
			var HighwayHashConfigMock = new Mock<IHighwayHashConfig>();
			{
				HighwayHashConfigMock.Setup(bc => bc.Clone())
					.Returns(new HighwayHashConfig());
			}

			GC.KeepAlive(
				new HighwayHash_Implementation(HighwayHashConfigMock.Object));

			HighwayHashConfigMock.Verify(bc => bc.Clone(), Times.Once);

			HighwayHashConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}


		#region HashSize

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 220, 255, 250, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var HighwayHashConfigMock = new Mock<IHighwayHashConfig>();
				{
					HighwayHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					HighwayHashConfigMock.Setup(bc => bc.Clone())
						.Returns(() => HighwayHashConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new HighwayHash_Implementation(
								HighwayHashConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			// 64, 128, 256
			var validHashSizes = new int[] { 64, 128, 256 };

			foreach (var validHashSize in validHashSizes)
			{
				var HighwayHashConfigMock = new Mock<IHighwayHashConfig>();
				{
					HighwayHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					HighwayHashConfigMock.SetupGet(bc => bc.Key)
						.Returns(new byte[32]);

					HighwayHashConfigMock.Setup(bc => bc.Clone())
						.Returns(() => HighwayHashConfigMock.Object);
				}

				GC.KeepAlive(
					new HighwayHash_Implementation(
						HighwayHashConfigMock.Object));
			}
		}

		#endregion

		#region Key

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_InvalidKeyLength_Throws()
		{
			var HighwayHashConfigMock = new Mock<IHighwayHashConfig>();
			{
				HighwayHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(64);

				HighwayHashConfigMock.SetupGet(bc => bc.Key)
					.Returns(new byte[31]);

				HighwayHashConfigMock.Setup(bc => bc.Clone())
					.Returns(() => HighwayHashConfigMock.Object);
			}

			Assert.Equal("config.Key",
				Assert.Throws<ArgumentOutOfRangeException>(() =>
						new HighwayHash_Implementation(
							HighwayHashConfigMock.Object))
					.ParamName);
		}

		[Fact]
		public void HighwayHash_Implementation_Constructor_Config_ValidKeyLength_Works()
		{
			var HighwayHashConfigMock = new Mock<IHighwayHashConfig>();
			{
				HighwayHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(64);

				HighwayHashConfigMock.SetupGet(bc => bc.Key)
					.Returns(new byte[32]);

				HighwayHashConfigMock.Setup(bc => bc.Clone())
					.Returns(() => HighwayHashConfigMock.Object);
			}

			GC.KeepAlive(
				new HighwayHash_Implementation(
					HighwayHashConfigMock.Object));
		}

		#endregion

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IHighwayHash, IHighwayHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "6944D5B975DA3570"),
					new KnownValue(128, TestConstants.Empty, "2F6380A491D0415AE514C834A55B3B00"),
					new KnownValue(256, TestConstants.Empty, "5415DE88E9EEAA62F54662C43DC403544BF5D4B1EC544EA07195B25D437ACD85"),

					new KnownValue(64, TestConstants.FooBar, "AC9C9F9BC3DB15C2"),
					new KnownValue(128, TestConstants.FooBar, "993CA4BF72D8D82892A5640F68C6B3E1"),
					new KnownValue(256, TestConstants.FooBar, "3811DBDEDC7E72DCC7ABE815F7E59FB11CE6C41C5D637FB92AD37222AF47EC5E"),

					new KnownValue(64, TestConstants.LoremIpsum, "3CF82519EAC83AD2"),
					new KnownValue(128, TestConstants.LoremIpsum, "995B62A2FE94F48CD508B11BD50A5816"),
					new KnownValue(256, TestConstants.LoremIpsum, "5EA88ADCF46810D2B6C4DE27F1A336DEA49DA4B6581694870AC3712E04AB09AC"),

					new KnownValue(64, new byte[4096], "0F00B84C7D49401E"),
					new KnownValue(128, new byte[4096], "5324BA0F6D3ABC2F98D259612D4E020F"),
					new KnownValue(256, new byte[4096], "7252E13D67863BA17FFB3886C987725D0CAA7A076B1D86A433E12A2AF3D1130F"),
					};

			protected override IHighwayHash CreateHashFunction(int hashSize) =>
				new HighwayHash_Implementation(
					new HighwayHashConfig()
					{
						HashSizeInBits = hashSize
					});
		}

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IHighwayHash, IHighwayHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
						new KnownValue(64, TestConstants.Empty, "6944D5B975DA3570"),
						new KnownValue(64, TestConstants.FooBar, "AC9C9F9BC3DB15C2"),
						new KnownValue(64, TestConstants.LoremIpsum, "3CF82519EAC83AD2"),
				};

			protected override IHighwayHash CreateHashFunction(int hashSize) =>
				new HighwayHash_Implementation(
					new HighwayHashConfig());
		}

		public class IStreamableHashFunction_Tests_WithHashSizeAndFoobarAsKey
			: IStreamableHashFunction_TestBase<IHighwayHash, IHighwayHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
						new KnownValue(64, TestConstants.Empty, "9E6435D277AEEF7E"),
						new KnownValue(128, TestConstants.Empty, "80F6C59A3F2D125AE7905CCB8E6D7DF4"),
						new KnownValue(256, TestConstants.Empty, "411CE0464C68E5E2C6948F10D657DEEE04456A29CA67BB9CD4043292F6014204"),
						new KnownValue(64, TestConstants.FooBar, "0B4DCBAA9B5E31CF"),
						new KnownValue(128, TestConstants.FooBar, "DE6AD620F1BB5019909FA89C56543A08"),
						new KnownValue(256, TestConstants.FooBar, "DBBC7B597308D2A3EB3268B04EA600FA42CFCB2489013121E0A112940BBEA0F9"),
						new KnownValue(64, TestConstants.LoremIpsum, "B03AA7D97A01C931"),
						new KnownValue(128, TestConstants.LoremIpsum, "C50F1A7660EA9F69CAAEB27430C6FEB2"),
						new KnownValue(256, TestConstants.LoremIpsum, "B7356BF01E7FC707B2AFAB938F014F8C615F881E92374522AB10B8CAD2F0C46B"),
						new KnownValue(64, new byte[4096], "D866F13B557EEE97"),
						new KnownValue(128, new byte[4096], "89934D146AC8AE80E13A1E0F560FCA21"),
						new KnownValue(256, new byte[4096], "FE34C41A096D4695A478E693756800801126AE9E020B0FC00812D5A94F45D8B4"),
				};

			protected override IHighwayHash CreateHashFunction(int hashSize) =>
				new HighwayHash_Implementation(
					new HighwayHashConfig()
					{
						HashSizeInBits = hashSize,
						Key = TestConstants.FooBar.Concat(new byte[26]).ToArray()
					});
		}
	}
}