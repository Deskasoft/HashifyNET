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

using HashifyNet.Algorithms.MetroHash;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.MetroHash
{
	public class MetroHash64_Implementation_Tests
	{
		#region Constructor

		#region  Config

		[Fact]
		public void MetroHash64_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(null))
					.ParamName);
		}

		[Fact]
		public void MetroHash64_Implementation_Constructor_Config_IsCloned()
		{
			var metroHashConfigMock = new Mock<IMetroHashConfig>();
			{
				metroHashConfigMock.Setup(bc => bc.HashSizeInBits)
					.Returns(64);
				metroHashConfigMock.Setup(bc => bc.Clone())
					.Returns(new MetroHashConfig());
			}

			GC.KeepAlive(
			   HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(metroHashConfigMock.Object));

			metroHashConfigMock.Verify(bc => bc.Clone(), Times.Once);

			metroHashConfigMock.VerifyGet(bc => bc.Seed, Times.Never);
		}

		#endregion

		#endregion

		#region Config

		[Fact]
		public void MetroHash64_Implementation_Config_IsCloneOfClone()
		{
			var metroHashConfig3 = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 64);
			var metroHashConfig2 = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 64 && mhc.Clone() == metroHashConfig3);
			var metroHashConfig = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 64 && mhc.Clone() == metroHashConfig2);

			var metroHash = HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(metroHashConfig);

			Assert.Equal(metroHashConfig3, metroHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void MetroHash64_Implementation_HashSizeInBits_Is64()
		{
			var metroHashConfig = Mock.Of<IMetroHashConfig>(mhc => mhc.Clone() == mhc && mhc.HashSizeInBits == 64);
			var metroHash = HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(metroHashConfig);

			Assert.Equal(64, metroHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IMetroHash, IMetroHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, "012345678901234567890123456789012345678901234567890123456789012", "6b753dae06704bad"),

					new KnownValue(64, TestConstants.Empty, 0x705fb008071e967d),
					new KnownValue(64, TestConstants.FooBar, 0xafdc1105b8a90a61),
					new KnownValue(64, TestConstants.LoremIpsum, 0xf2083d32ac311dab),
				};

			protected override IMetroHash CreateHashFunction(int hashSize) =>
				HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(new MetroHashConfig() { HashSizeInBits = 64 });
		}

		public class IStreamableHashFunction_Tests_WithSeed
			: IStreamableHashFunction_TestBase<IMetroHash, IMetroHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, "012345678901234567890123456789012345678901234567890123456789012", "3b0d481cf4b9b8df"),

					new KnownValue(64, TestConstants.Empty, 0xe6f660fe36b85a05),
					new KnownValue(64, TestConstants.FooBar, 0xa4e1647f495bd189),
					new KnownValue(64, TestConstants.LoremIpsum, 0x74f8c5ccdd69b4b3),
				};

			protected override IMetroHash CreateHashFunction(int hashSize) =>
				HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(new MetroHashConfig() { HashSizeInBits = 64, Seed = 1 });
		}
	}
}