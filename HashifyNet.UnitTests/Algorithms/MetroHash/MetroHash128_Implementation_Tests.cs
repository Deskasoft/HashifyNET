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
	public class MetroHash128_Implementation_Tests
	{
		#region Constructor

		#region  Config

		[Fact]
		public void MetroHash128_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(null))
					.ParamName);
		}

		[Fact]
		public void MetroHash128_Implementation_Constructor_Config_IsCloned()
		{
			var metroHashConfigMock = new Mock<IMetroHashConfig>();
			{
				metroHashConfigMock.Setup(bc => bc.HashSizeInBits)
					.Returns(128);
				metroHashConfigMock.Setup(bc => bc.Clone())
					.Returns(new MetroHashConfig() { HashSizeInBits = 128 });
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
		public void MetroHash128_Implementation_Config_IsCloneOfClone()
		{
			var metroHashConfig3 = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 128);
			var metroHashConfig2 = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 128 && mhc.Clone() == metroHashConfig3);
			var metroHashConfig = Mock.Of<IMetroHashConfig>(mhc => mhc.HashSizeInBits == 128 && mhc.Clone() == metroHashConfig2);

			var metroHash = HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(metroHashConfig);

			Assert.Equal(metroHashConfig3, metroHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void MetroHash128_Implementation_HashSizeInBits_Is128()
		{
			var metroHashConfig = Mock.Of<IMetroHashConfig>(mhc => mhc.Clone() == mhc && mhc.HashSizeInBits == 128);
			var metroHash = HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(metroHashConfig);

			Assert.Equal(128, metroHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IMetroHash, IMetroHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, "012345678901234567890123456789012345678901234567890123456789012", "c77ce2bfa4ed9f9b0548b2ac5074a297"),

					new KnownValue(128, TestConstants.Empty,      "0x4606b14684c65fb60005f3ca3d41d1cb"),
					new KnownValue(128, TestConstants.FooBar,     "0xe5fe590c9b99c223859bf8992882a5e3"),
					new KnownValue(128, TestConstants.LoremIpsum, "0x52c5e338fb7a400666e9fbabaebcb790"),
				};

			protected override IMetroHash CreateHashFunction(int hashSize) =>
				HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(new MetroHashConfig() { HashSizeInBits = 128 });
		}

		public class IStreamableHashFunction_Tests_WithSeed
			: IStreamableHashFunction_TestBase<IMetroHash, IMetroHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, "012345678901234567890123456789012345678901234567890123456789012", "45a3cdb838199d7fbdd68d867a14ecef"),

					new KnownValue(128, TestConstants.Empty,      "0xf9a908797eef84017d036b44fbede600"),
					new KnownValue(128, TestConstants.FooBar,     "0x52ff94470b31e45dfcf0cc865889f0df"),
					new KnownValue(128, TestConstants.LoremIpsum, "0x7786153ea37fe00904733ec964eaeb7c"),
				};

			protected override IMetroHash CreateHashFunction(int hashSize) =>
				HashFactory<IMetroHash, IMetroHashConfig>.Instance.Create(new MetroHashConfig() { HashSizeInBits = 128, Seed = 1 });
		}
	}
}