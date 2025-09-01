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

using HashifyNet.Algorithms.RapidHash;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.RapidHash
{
	public class RapidHash_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void RapidHash_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new RapidHash_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void RapidHash_Implementation_Constructor_Config_IsCloned()
		{
			var rapidHashConfigMock = new Mock<IRapidHashConfig>();
			{
				rapidHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(64);

				rapidHashConfigMock.Setup(bc => bc.Clone())
					.Returns(new RapidHashConfig());
			}

			GC.KeepAlive(
				new RapidHash_Implementation(rapidHashConfigMock.Object));

			rapidHashConfigMock.Verify(bc => bc.Clone(), Times.Once);

			rapidHashConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void RapidHash_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 63, 32, 128, 800, 801, 900, 1024 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var rapidHashConfigMock = new Mock<IRapidHashConfig>();
				{
					rapidHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					rapidHashConfigMock.Setup(bc => bc.Clone())
						.Returns(() => rapidHashConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new RapidHash_Implementation(
								rapidHashConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void RapidHash_Implementation_HashSizeInBits_IsFromConfig()
		{
			var rapidHashConfig2 = Mock.Of<IRapidHashConfig>(bc => bc.HashSizeInBits == 64 && bc.Clone() == new RapidHashConfig());
			var rapidHashConfig = Mock.Of<IRapidHashConfig>(bc => bc.Clone() == rapidHashConfig2);

			var rapidHash = new RapidHash_Implementation(rapidHashConfig);

			Assert.Equal(64, rapidHash.Config.HashSizeInBits);
		}

		#endregion

		#endregion

		#endregion

		public class IHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IRapidHash, IRapidHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, 232177599295442350),

					new KnownValue(64, TestConstants.FooBar, 15523718635262960188),

					new KnownValue(64, TestConstants.LoremIpsum, 18040269197578596193),

					new KnownValue(64, new byte[1], 5702620981742189058),
					new KnownValue(64, new byte[7], 10883712246314272921),
					new KnownValue(64, new byte[8], 11456056956516475379),
					new KnownValue(64, new byte[16], 11820096523416114993),
					new KnownValue(64, new byte[17], 12879367262414962620),

					new KnownValue(64, new byte[4096], 11567716601755724648),

					new KnownValue(64, new byte[8192], 17362367839289931837),

					new KnownValue(64, new byte[10000], 5626523013684839545),
				};

			protected override IRapidHash CreateHashFunction(int hashSize) =>
				new RapidHash_Implementation(
					new RapidHashConfig());
		}

		public class IHashFunction_Tests_WithSeed
			: IStreamableHashFunction_TestBase<IRapidHash, IRapidHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, 7833074399267231953),

					new KnownValue(64, TestConstants.FooBar, 11416782446331650639),

					new KnownValue(64, TestConstants.LoremIpsum, 10744372729903230306),

					new KnownValue(64, new byte[4096], 18209065387139984514),

					new KnownValue(64, new byte[8192], 18419876097751971287),

					new KnownValue(64, new byte[10000], 16909027571307796084),
				};

			protected override IRapidHash CreateHashFunction(int hashSize) =>
				new RapidHash_Implementation(
					new RapidHashConfig()
					{
						Seed = 90_01
					});
		}

		public class IHashFunction_Tests_WithModeMicro
	: IStreamableHashFunction_TestBase<IRapidHash, IRapidHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, 232177599295442350),

					new KnownValue(64, TestConstants.FooBar, 15523718635262960188),

					new KnownValue(64, TestConstants.LoremIpsum, 8521930955689885920),

					new KnownValue(64, new byte[1], 5702620981742189058),
					new KnownValue(64, new byte[7], 10883712246314272921),
					new KnownValue(64, new byte[8], 11456056956516475379),
					new KnownValue(64, new byte[16], 11820096523416114993),
					new KnownValue(64, new byte[17], 12879367262414962620),

					new KnownValue(64, new byte[4096], 13516268575382731638),

					new KnownValue(64, new byte[8192], 5236988562116181528),

					new KnownValue(64, new byte[10000], 10980458045961532347),
				};

			protected override IRapidHash CreateHashFunction(int hashSize) =>
				new RapidHash_Implementation(
					new RapidHashConfig()
					{
						Mode = RapidHashMode.Micro
					});
		}

		public class IHashFunction_Tests_WithModeNano
: IStreamableHashFunction_TestBase<IRapidHash, IRapidHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, 232177599295442350),

					new KnownValue(64, TestConstants.FooBar, 15523718635262960188),

					new KnownValue(64, TestConstants.LoremIpsum, 9489726530920820974),

					new KnownValue(64, new byte[1], 5702620981742189058),
					new KnownValue(64, new byte[7], 10883712246314272921),
					new KnownValue(64, new byte[8], 11456056956516475379),
					new KnownValue(64, new byte[16], 11820096523416114993),
					new KnownValue(64, new byte[17], 12879367262414962620),

					new KnownValue(64, new byte[4096], 16506082494102252922),

					new KnownValue(64, new byte[8192], 15389422866100442776),

					new KnownValue(64, new byte[10000], 5782605844267771859),
				};

			protected override IRapidHash CreateHashFunction(int hashSize) =>
				new RapidHash_Implementation(
					new RapidHashConfig()
					{
						Mode = RapidHashMode.Nano
					});
		}
	}
}