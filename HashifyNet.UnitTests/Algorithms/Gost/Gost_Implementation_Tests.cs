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

using HashifyNet.Algorithms.Gost;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Gost
{
	public class Gost_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Gost_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Gost_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Gost_Implementation_Constructor_Config_IsCloned()
		{
			var gostConfigMock = new Mock<IGostConfig>();
			{
				gostConfigMock.Setup(bc => bc.Clone())
					.Returns(new GostConfig());
			}

			GC.KeepAlive(
				new Gost_Implementation(gostConfigMock.Object));

			gostConfigMock.Verify(bc => bc.Clone(), Times.Once);
			gostConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void Gost_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var GostConfigMock = new Mock<IGostConfig>();
				{
					GostConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					GostConfigMock.Setup(bc => bc.Clone())
						.Returns(() => GostConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Gost_Implementation(
								GostConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void Gost_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			// 256, 512
			var validHashSizes = new int[] { 256, 512 };

			foreach (var validHashSize in validHashSizes)
			{
				var GostConfigMock = new Mock<IGostConfig>();
				{
					GostConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					GostConfigMock.Setup(bc => bc.Clone())
						.Returns(() => GostConfigMock.Object);
				}

				GC.KeepAlive(
					new Gost_Implementation(
						GostConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void Gost_Implementation_Config_IsCloneOfClone()
		{
			var gostConfig3 = Mock.Of<IGostConfig>();
			var gostConfig2 = Mock.Of<IGostConfig>(bc => bc.HashSizeInBits == 512 && bc.Clone() == gostConfig3);
			var gostConfig = Mock.Of<IGostConfig>(bc => bc.Clone() == gostConfig2);

			var gostHash = new Gost_Implementation(gostConfig);

			Assert.Equal(gostConfig3, gostHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Gost_Implementation_HashSizeInBits_IsFromConfig()
		{
			var gostConfig2 = Mock.Of<IGostConfig>(bc => bc.HashSizeInBits == 512 && bc.Clone() == new GostConfig());
			var gostConfig = Mock.Of<IGostConfig>(bc => bc.Clone() == gostConfig2);

			var gostHash = new Gost_Implementation(gostConfig);

			Assert.Equal(512, gostHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IGost, IGostConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(256, TestConstants.Empty, "3f539a213e97c802cc229d474c6aa32a825a360b2a933a949fd925208d9ce1bb"),
					new KnownValue(512, TestConstants.Empty, "8E945DA209AA869F0455928529BCAE4679E9873AB707B55315F56CEB98BEF0A7362F715528356EE83CDA5F2AAC4C6AD2BA3A715C1BCD81CB8E9F90BF4C1C1A8A"),
					new KnownValue(256, TestConstants.FooBar, "e3c9fd89226d93b489a9fe27d686806e24a514e3787bca053c698ec4616ceb78"),
					new KnownValue(512, TestConstants.FooBar, "4B218ED4F6238EB9FDC42884D5902E644B85EEC1BBD82E628935FF3838ECD03BA145F3ADBD9C06CA8C3D8C726F90CA18F865293E1A41707D1A4E53D45343B385"),
					new KnownValue(256, TestConstants.LoremIpsum, "9122f47daa2e32a39969b66bc79c76aa60de83c7d89ba7ad8242d365691be410"),
					new KnownValue(512, TestConstants.LoremIpsum, "D04741DC9660002DD93C44BC13B10657D63CA8EA301A7C653B332A73C0201A8EDF418ECDB2814CD972755847847F057A593790AFCC25AFC9F51944956ECDCC86"),
					new KnownValue(256, new byte[4096], "138556e7e708123361e41adf3c7981f24c73915c4d180dcbfaab8b2563d3093b"),
					new KnownValue(512, new byte[4096], "df7e33ac1eac9b25b38edf093762093c1efd700d2e0450d19177866cdeba6af70755d33c862f2eb486b592008788206ae66fdc134a4d7a71b61eaae18c7a8c04"),
					};

			protected override IGost CreateHashFunction(int hashSize) =>
				new Gost_Implementation(
					new GostConfig()
					{
						HashSizeInBits = hashSize,
					});
		}

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IGost, IGostConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
							new KnownValue(512, TestConstants.Empty, "8e945da209aa869f0455928529bcae4679e9873ab707b55315f56ceb98bef0a7362f715528356ee83cda5f2aac4c6ad2ba3a715c1bcd81cb8e9f90bf4c1c1a8a"),
							new KnownValue(512, TestConstants.FooBar, "4b218ed4f6238eb9fdc42884d5902e644b85eec1bbd82e628935ff3838ecd03ba145f3adbd9c06ca8c3d8c726f90ca18f865293e1a41707d1a4e53d45343b385"),
							new KnownValue(512, TestConstants.LoremIpsum, "d04741dc9660002dd93c44bc13b10657d63ca8ea301a7c653b332a73c0201a8edf418ecdb2814cd972755847847f057a593790afcc25afc9f51944956ecdcc86"),
				};

			protected override IGost CreateHashFunction(int hashSize) =>
				new Gost_Implementation(
					new GostConfig());
		}
	}
}