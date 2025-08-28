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

using HashifyNet.Algorithms.Whirlpool;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Whirlpool
{
	public class Whirlpool_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Whirlpool_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Whirlpool_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Whirlpool_Implementation_Constructor_Config_IsCloned()
		{
			var whirlpoolConfigMock = new Mock<IWhirlpoolConfig>();
			{
				whirlpoolConfigMock.Setup(bc => bc.Clone())
					.Returns(new WhirlpoolConfig());
			}

			GC.KeepAlive(
				new Whirlpool_Implementation(whirlpoolConfigMock.Object));

			whirlpoolConfigMock.Verify(bc => bc.Clone(), Times.Once);

			whirlpoolConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}


		#region HashSize

		[Fact]
		public void Whirlpool_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var WhirlpoolConfigMock = new Mock<IWhirlpoolConfig>();
				{
					WhirlpoolConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					WhirlpoolConfigMock.Setup(bc => bc.Clone())
						.Returns(() => WhirlpoolConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Whirlpool_Implementation(
								WhirlpoolConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void Whirlpool_Implementation_Config_IsCloneOfClone()
		{
			var whirlpoolConfig3 = Mock.Of<IWhirlpoolConfig>();
			var whirlpoolConfig2 = Mock.Of<IWhirlpoolConfig>(bc => bc.HashSizeInBits == 512 && bc.Clone() == whirlpoolConfig3);
			var whirlpoolConfig = Mock.Of<IWhirlpoolConfig>(bc => bc.Clone() == whirlpoolConfig2);

			var whirlpoolHash = new Whirlpool_Implementation(whirlpoolConfig);

			Assert.Equal(whirlpoolConfig3, whirlpoolHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Whirlpool_Implementation_HashSizeInBits_IsFromConfig()
		{
			var whirlpoolConfig2 = Mock.Of<IWhirlpoolConfig>(bc => bc.HashSizeInBits == 512 && bc.Clone() == new WhirlpoolConfig());
			var whirlpoolConfig = Mock.Of<IWhirlpoolConfig>(bc => bc.Clone() == whirlpoolConfig2);

			var whirlpoolHash = new Whirlpool_Implementation(whirlpoolConfig);

			Assert.Equal(512, whirlpoolHash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IWhirlpool, IWhirlpoolConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(512, TestConstants.Empty, "19fa61d75522a4669b44e39c1d2e1726c530232130d407f89afee0964997f7a73e83be698b288febcf88e3e03c4f0757ea8964e59b63d93708b138cc42a66eb3"),

					new KnownValue(512, TestConstants.FooBar, "9923afaec3a86f865bb231a588f453f84e8151a2deb4109aebc6de4284be5bebcff4fab82a7e51d920237340a043736e9d13bab196006dcca0fe65314d68eab9"),

					new KnownValue(512, TestConstants.LoremIpsum, "b152ec5e36b8806f9bbcd85b775714102cd94851c83d8a46e2f5f0b95e84e8bafc4419cc5a76a2e3a54f6c61d543fc2e6646d86baaad2cc22a42538c8b39ffcc"),

					new KnownValue(512, new byte[4096], "e2cf924e929eb33c6127846096aaa14a92e8e6c87636999713e70887f1b3e9df426a8a68804f9821ef32e2ee8767ef31bfca8771cea45919486f02cb41526190"),
					};

			protected override IWhirlpool CreateHashFunction(int hashSize) =>
				new Whirlpool_Implementation(
					new WhirlpoolConfig());
		}

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IWhirlpool, IWhirlpoolConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(512, TestConstants.Empty, "19fa61d75522a4669b44e39c1d2e1726c530232130d407f89afee0964997f7a73e83be698b288febcf88e3e03c4f0757ea8964e59b63d93708b138cc42a66eb3"),
					new KnownValue(512, TestConstants.FooBar, "9923afaec3a86f865bb231a588f453f84e8151a2deb4109aebc6de4284be5bebcff4fab82a7e51d920237340a043736e9d13bab196006dcca0fe65314d68eab9"),
					new KnownValue(512, TestConstants.LoremIpsum, "b152ec5e36b8806f9bbcd85b775714102cd94851c83d8a46e2f5f0b95e84e8bafc4419cc5a76a2e3a54f6c61d543fc2e6646d86baaad2cc22a42538c8b39ffcc"),
				};

			protected override IWhirlpool CreateHashFunction(int hashSize) =>
				new Whirlpool_Implementation(
					new WhirlpoolConfig());
		}
	}
}