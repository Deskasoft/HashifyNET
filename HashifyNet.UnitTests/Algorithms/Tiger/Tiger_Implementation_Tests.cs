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

using HashifyNet.Algorithms.Tiger;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Tiger
{
	public class Tiger_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Tiger_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Tiger_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Tiger_Implementation_Constructor_Config_IsCloned()
		{
			var tigerConfigMock = new Mock<ITigerConfig>();
			{
				tigerConfigMock.Setup(bc => bc.Clone())
					.Returns(new TigerConfig());
			}

			GC.KeepAlive(
				new Tiger_Implementation(tigerConfigMock.Object));

			tigerConfigMock.Verify(bc => bc.Clone(), Times.Once);

			tigerConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void Tiger_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var TigerConfigMock = new Mock<ITigerConfig>();
				{
					TigerConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					TigerConfigMock.Setup(bc => bc.Clone())
						.Returns(() => TigerConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Tiger_Implementation(
								TigerConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void Tiger_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			var validHashSizes = new int[] { 128, 160, 192 };

			foreach (var validHashSize in validHashSizes)
			{
				var TigerConfigMock = new Mock<ITigerConfig>();
				{
					TigerConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					TigerConfigMock.SetupGet(bc => bc.Passes)
						.Returns(4);

					TigerConfigMock.Setup(bc => bc.Clone())
						.Returns(() => TigerConfigMock.Object);
				}

				GC.KeepAlive(
					new Tiger_Implementation(
						TigerConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void Tiger_Implementation_Config_IsCloneOfClone()
		{
			var tigerConfig3 = Mock.Of<ITigerConfig>();
			var tigerConfig2 = Mock.Of<ITigerConfig>(bc => bc.HashSizeInBits == 192 && bc.Passes == 4 && bc.Clone() == tigerConfig3);
			var tigerConfig = Mock.Of<ITigerConfig>(bc => bc.Clone() == tigerConfig2);

			var tigerHash = new Tiger_Implementation(tigerConfig);

			Assert.Equal(tigerConfig3, tigerHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Tiger_Implementation_HashSizeInBits_IsFromConfig()
		{
			var tigerConfig2 = Mock.Of<ITigerConfig>(bc => bc.HashSizeInBits == 192 && bc.Passes == 4 && bc.Clone() == new TigerConfig() { HashSizeInBits = 192, Passes = 4 });
			var tigerConfig = Mock.Of<ITigerConfig>(bc => bc.Clone() == tigerConfig2);

			var tigerHash = new Tiger_Implementation(tigerConfig);

			Assert.Equal(192, tigerHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<ITiger, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "24cc78a7f6ff3546e7984e59695ca13d"),
					new KnownValue(160, TestConstants.Empty, "24cc78a7f6ff3546e7984e59695ca13d804e0b68"),
					new KnownValue(192, TestConstants.Empty, "24cc78a7f6ff3546e7984e59695ca13d804e0b686e255194"),

					new KnownValue(128, TestConstants.FooBar, "e9595994dc40836af53b0f2b0465a7ec"),
					new KnownValue(160, TestConstants.FooBar, "e9595994dc40836af53b0f2b0465a7ecbc673f1c"),
					new KnownValue(192, TestConstants.FooBar, "e9595994dc40836af53b0f2b0465a7ecbc673f1c32bef51a"),

					new KnownValue(128, TestConstants.LoremIpsum, "7a8b005064f20a85ec465202c71a62be"),
					new KnownValue(160, TestConstants.LoremIpsum, "7a8b005064f20a85ec465202c71a62bef99bdd5c"),
					new KnownValue(192, TestConstants.LoremIpsum, "7a8b005064f20a85ec465202c71a62bef99bdd5c38b33e09"),

					new KnownValue(128, new byte[4096], "7870D27F029D0B84F9D7CBCC22C8DCC4"),
					new KnownValue(160, new byte[4096], "7870D27F029D0B84F9D7CBCC22C8DCC47C6C6E04"),
					new KnownValue(192, new byte[4096], "7870D27F029D0B84F9D7CBCC22C8DCC47C6C6E045C78B0CC"),
					};

			protected override ITiger CreateHashFunction(int hashSize) =>
				new Tiger_Implementation(
					new TigerConfig()
					{
						HashSizeInBits = hashSize
					});
		}

		public class IStreamableHashFunction_Tests_With3Passes
	: IStreamableHashFunction_TestBase<ITiger, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "3293AC630C13F0245F92BBB1766E1616"),
					new KnownValue(160, TestConstants.Empty, "3293AC630C13F0245F92BBB1766E16167A4E5849"),
					new KnownValue(192, TestConstants.Empty, "3293AC630C13F0245F92BBB1766E16167A4E58492DDE73F3"),

					new KnownValue(128, TestConstants.FooBar, "C8CD4BF6959A162866C37FB6745F372C"),
					new KnownValue(160, TestConstants.FooBar, "C8CD4BF6959A162866C37FB6745F372CC919FFAA"),
					new KnownValue(192, TestConstants.FooBar, "C8CD4BF6959A162866C37FB6745F372CC919FFAA05560EEF"),

					new KnownValue(128, TestConstants.LoremIpsum, "782146B49CBC16AAE955C528AA4EF168"),
					new KnownValue(160, TestConstants.LoremIpsum, "782146B49CBC16AAE955C528AA4EF168D8651776"),
					new KnownValue(192, TestConstants.LoremIpsum, "782146B49CBC16AAE955C528AA4EF168D865177615F922C5"),

					new KnownValue(128, new byte[4096], "4FA90A71FA600BC5E394A1D4B415563E"),
					new KnownValue(160, new byte[4096], "4FA90A71FA600BC5E394A1D4B415563E6A3B25E0"),
					new KnownValue(192, new byte[4096], "4FA90A71FA600BC5E394A1D4B415563E6A3B25E095CFFACD"),
					};

			protected override ITiger CreateHashFunction(int hashSize) =>
				new Tiger_Implementation(
					new TigerConfig()
					{
						HashSizeInBits = hashSize,
						Passes = 3,
					});
		}

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<ITiger, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(192, TestConstants.Empty, "24cc78a7f6ff3546e7984e59695ca13d804e0b686e255194"),
					new KnownValue(192, TestConstants.FooBar, "e9595994dc40836af53b0f2b0465a7ecbc673f1c32bef51a"),
					new KnownValue(192, TestConstants.LoremIpsum, "7a8b005064f20a85ec465202c71a62bef99bdd5c38b33e09"),
				};

			protected override ITiger CreateHashFunction(int hashSize) =>
				new Tiger_Implementation(
					new TigerConfig());
		}
	}
}