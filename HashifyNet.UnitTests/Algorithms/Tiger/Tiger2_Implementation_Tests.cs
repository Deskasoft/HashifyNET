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
using HashifyNet.Algorithms.Tiger2;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Tiger2
{
	public class Tiger2_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Tiger2_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Tiger2_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Tiger2_Implementation_Constructor_Config_IsCloned()
		{
			var tigerConfigMock = new Mock<ITiger2Config>();
			{
				tigerConfigMock.Setup(bc => bc.Clone())
					.Returns(new Tiger2Config());
			}

			GC.KeepAlive(
				new Tiger2_Implementation(tigerConfigMock.Object));

			tigerConfigMock.Verify(bc => bc.Clone(), Times.Once);

			tigerConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void Tiger2_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var TigerConfigMock = new Mock<ITiger2Config>();
				{
					TigerConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					TigerConfigMock.Setup(bc => bc.Clone())
						.Returns(() => TigerConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Tiger2_Implementation(
								TigerConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void Tiger2_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			var validHashSizes = new int[] { 128, 160, 192 };

			foreach (var validHashSize in validHashSizes)
			{
				var TigerConfigMock = new Mock<ITiger2Config>();
				{
					TigerConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					TigerConfigMock.SetupGet(bc => bc.Passes)
						.Returns(4);

					TigerConfigMock.Setup(bc => bc.Clone())
						.Returns(() => TigerConfigMock.Object);
				}

				GC.KeepAlive(
					new Tiger2_Implementation(
						TigerConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void Tiger2_Implementation_Config_IsCloneOfClone()
		{
			var tigerConfig3 = Mock.Of<ITiger2Config>();
			var tigerConfig2 = Mock.Of<ITiger2Config>(bc => bc.HashSizeInBits == 192 && bc.Passes == 4 && bc.Clone() == tigerConfig3);
			var tigerConfig = Mock.Of<ITiger2Config>(bc => bc.Clone() == tigerConfig2);

			var tigerHash = new Tiger2_Implementation(tigerConfig);

			Assert.Equal(tigerConfig3, tigerHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Tiger2_Implementation_HashSizeInBits_IsFromConfig()
		{
			var tigerConfig2 = Mock.Of<ITiger2Config>(bc => bc.HashSizeInBits == 192 && bc.Passes == 4 && bc.Clone() == new Tiger2Config() { HashSizeInBits = 192, Passes = 4 });
			var tigerConfig = Mock.Of<ITiger2Config>(bc => bc.Clone() == tigerConfig2);

			var tigerHash = new Tiger2_Implementation(tigerConfig);

			Assert.Equal(192, tigerHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<ITiger2, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "6A7201A47AAC2065913811175553489A"),
					new KnownValue(160, TestConstants.Empty, "6A7201A47AAC2065913811175553489ADD0F8B99"),
					new KnownValue(192, TestConstants.Empty, "6A7201A47AAC2065913811175553489ADD0F8B99E65A0955"),

					new KnownValue(128, TestConstants.FooBar, "FEA3EDC11AA95F18A80E8B0382CDA707"),
					new KnownValue(160, TestConstants.FooBar, "FEA3EDC11AA95F18A80E8B0382CDA707883B7D76"),
					new KnownValue(192, TestConstants.FooBar, "FEA3EDC11AA95F18A80E8B0382CDA707883B7D7681F47331"),

					new KnownValue(128, TestConstants.LoremIpsum, "0E120D1465055C264E62ED817E7F410C"),
					new KnownValue(160, TestConstants.LoremIpsum, "0E120D1465055C264E62ED817E7F410C2112DFB5"),
					new KnownValue(192, TestConstants.LoremIpsum, "0E120D1465055C264E62ED817E7F410C2112DFB58903E444"),

					new KnownValue(128, new byte[4096], "D8CBFD1E1F93EB29E57D41279BB23BD9"),
					new KnownValue(160, new byte[4096], "D8CBFD1E1F93EB29E57D41279BB23BD9AB3BBAD3"),
					new KnownValue(192, new byte[4096], "D8CBFD1E1F93EB29E57D41279BB23BD9AB3BBAD31CB8C1CE"),
					};

			protected override ITiger2 CreateHashFunction(int hashSize) =>
				new Tiger2_Implementation(
					new Tiger2Config()
					{
						HashSizeInBits = hashSize
					});
		}

		public class IStreamableHashFunction_Tests_With3Passes
	: IStreamableHashFunction_TestBase<ITiger2, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "4441BE75F6018773C206C22745374B92"),
					new KnownValue(160, TestConstants.Empty, "4441BE75F6018773C206C22745374B924AA8313F"),
					new KnownValue(192, TestConstants.Empty, "4441BE75F6018773C206C22745374B924AA8313FEF919F41"),

					new KnownValue(128, TestConstants.FooBar, "42D583C25FABB198F3770A1E1D52B2BF"),
					new KnownValue(160, TestConstants.FooBar, "42D583C25FABB198F3770A1E1D52B2BF40C51A8A"),
					new KnownValue(192, TestConstants.FooBar, "42D583C25FABB198F3770A1E1D52B2BF40C51A8A98913F0C"),

					new KnownValue(128, TestConstants.LoremIpsum, "B6F0C2A50C2190B833AAB9B8C5008E1C"),
					new KnownValue(160, TestConstants.LoremIpsum, "B6F0C2A50C2190B833AAB9B8C5008E1C6DF6E06D"),
					new KnownValue(192, TestConstants.LoremIpsum, "B6F0C2A50C2190B833AAB9B8C5008E1C6DF6E06D4B87E1E5"),

					new KnownValue(128, new byte[4096], "08B7786E158FE397E62D8191A12CA3A0"),
					new KnownValue(160, new byte[4096], "08B7786E158FE397E62D8191A12CA3A0793A8B8D"),
					new KnownValue(192, new byte[4096], "08B7786E158FE397E62D8191A12CA3A0793A8B8D811F35FF"),
					};

			protected override ITiger2 CreateHashFunction(int hashSize) =>
				new Tiger2_Implementation(
					new Tiger2Config()
					{
						HashSizeInBits = hashSize,
						Passes = 3,
					});
		}

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<ITiger2, ITigerConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(192, TestConstants.Empty, "6A7201A47AAC2065913811175553489ADD0F8B99E65A0955"),
					new KnownValue(192, TestConstants.FooBar, "FEA3EDC11AA95F18A80E8B0382CDA707883B7D7681F47331"),
					new KnownValue(192, TestConstants.LoremIpsum, "0E120D1465055C264E62ED817E7F410C2112DFB58903E444"),
				};

			protected override ITiger2 CreateHashFunction(int hashSize) =>
				new Tiger2_Implementation(
					new Tiger2Config());
		}
	}
}