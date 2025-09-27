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

using HashifyNet.Algorithms.T1HA;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.T1HA
{
	public class T1HA2_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void T1HA2_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new T1HA2_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void T1HA2_Implementation_Constructor_Config_IsCloned()
		{
			var tigerConfigMock = new Mock<IT1HA2Config>();
			{
				tigerConfigMock.Setup(bc => bc.Clone())
					.Returns(new T1HA2Config());
			}

			GC.KeepAlive(
				new T1HA2_Implementation(tigerConfigMock.Object));

			tigerConfigMock.Verify(bc => bc.Clone(), Times.Once);

			tigerConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void T1HA2_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var t1HAConfigMock = new Mock<IT1HA2Config>();
				{
					t1HAConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					t1HAConfigMock.Setup(bc => bc.Clone())
						.Returns(() => t1HAConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new T1HA2_Implementation(
								t1HAConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void T1HA2_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			var validHashSizes = new int[] { 64 };

			foreach (var validHashSize in validHashSizes)
			{
				var t1HAConfigMock = new Mock<IT1HA2Config>();
				{
					t1HAConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					t1HAConfigMock.Setup(bc => bc.Clone())
						.Returns(() => t1HAConfigMock.Object);
				}

				GC.KeepAlive(
					new T1HA2_Implementation(
						t1HAConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void T1HA2_Implementation_Config_IsCloneOfClone()
		{
			var t1HAConfig3 = Mock.Of<IT1HA2Config>();
			var t1HAConfig2 = Mock.Of<IT1HA2Config>(bc => bc.HashSizeInBits == 64 && bc.Clone() == t1HAConfig3);
			var t1HAConfig = Mock.Of<IT1HA2Config>(bc => bc.Clone() == t1HAConfig2);

			var t1HAHash = new T1HA2_Implementation(t1HAConfig);

			Assert.Equal(t1HAConfig3, t1HAHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void T1HA2_Implementation_HashSizeInBits_IsFromConfig()
		{
			var t1HAConfig2 = Mock.Of<IT1HA2Config>(bc => bc.HashSizeInBits == 64 && bc.Clone() == new T1HA2Config() { HashSizeInBits = 64 });
			var t1HAConfig = Mock.Of<IT1HA2Config>(bc => bc.Clone() == t1HAConfig2);

			var tigerHash = new T1HA2_Implementation(t1HAConfig);

			Assert.Equal(64, tigerHash.Config.HashSizeInBits);
		}

		#endregion

		public class IHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "0x3C8426E33CB41606"),
					new KnownValue(64, TestConstants.FooBar, "0x1A07D1FF9AD31E1C"),
					new KnownValue(64, TestConstants.LoremIpsum, "0x40097D9FE1A7886F"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config());
		}

		public class IHashFunction_Tests_128Bits
	: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "0xA86CEEAEABBFCDE1CD2801D3B92237D6"),
					new KnownValue(128, TestConstants.FooBar, "0x216CF5D9778329E4CD3E4B6901078F2D"),
					new KnownValue(128, TestConstants.LoremIpsum, "0xDF9AA8354F09F357813B9C1B673D56CB"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config()
					{
						HashSizeInBits = 128
					});
		}

		public class IHashFunction_Tests_64Bits_WithOneSeed
: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "0xE8904B621723EA1A"),
					new KnownValue(64, TestConstants.FooBar, "0xB32BD261B3A0D449"),
					new KnownValue(64, TestConstants.LoremIpsum, "0x5B58A023833EB973"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config()
					{
						HashSizeInBits = 64,
						Seed = 1990L
					});
		}

		public class IHashFunction_Tests_64Bits_WithTwoSeeds
: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "0x35A9F860C6D244F8"),
					new KnownValue(64, TestConstants.FooBar, "0x72F08251CF527FE4"),
					new KnownValue(64, TestConstants.LoremIpsum, "0x1500AA6696EE88C0"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config()
					{
						HashSizeInBits = 64,
						Seed = 1990L,
						Seed2 = 1990L
					});
		}

		public class IHashFunction_Tests_128Bits_WithOneSeed
: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "0x4A1BD3DEE8A0B0FB82B7BD75A89DE099"),
					new KnownValue(128, TestConstants.FooBar, "0x5F7F7EBD6E29D51D17AF43394829087F"),
					new KnownValue(128, TestConstants.LoremIpsum, "0x2523168143B7EC9CE3643E12FC9396FC"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config()
					{
						HashSizeInBits = 128,
						Seed = 1990L
					});
		}

		public class IHashFunction_Tests_128Bits_WithTwoSeeds
: IStreamableHashFunction_TestBase<IT1HA2, IT1HA2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "0x82AEE2239D8A17F12249181E6A6315A1"),
					new KnownValue(128, TestConstants.FooBar, "0xD888024C43FF3598B5AE61C821311FAB"),
					new KnownValue(128, TestConstants.LoremIpsum, "0x6DF7F9C6CB5526347C3E6B1D8EB6FD06"),
				};

			protected override IT1HA2 CreateHashFunction(int hashSize) =>
				new T1HA2_Implementation(
					new T1HA2Config()
					{
						HashSizeInBits = 128,
						Seed = 1990L,
						Seed2 = 1990L
					});
		}
	}
}