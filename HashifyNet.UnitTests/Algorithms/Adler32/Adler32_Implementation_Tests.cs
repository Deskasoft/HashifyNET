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

using HashifyNet.Algorithms.Adler32;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Adler32
{
	public class Adler32_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Adler32_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Adler32_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Adler32_Implementation_Constructor_Config_IsCloned()
		{
			var adler32ConfigMock = new Mock<IAdler32Config>();
			{
				adler32ConfigMock.Setup(bc => bc.Clone())
					.Returns(new Adler32Config());
			}

			GC.KeepAlive(
				new Adler32_Implementation(adler32ConfigMock.Object));

			adler32ConfigMock.Verify(bc => bc.Clone(), Times.Once);

			adler32ConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion

		#region Hash Sizes
		[Fact]
		public void Adler32_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { 31, 16, 64, 512, 256, 384, -1, 0, 3 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var adler32ConfigMock = new Mock<IAdler32Config>();
				{
					adler32ConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					adler32ConfigMock.Setup(bc => bc.Clone())
						.Returns(() => adler32ConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Adler32_Implementation(
								adler32ConfigMock.Object))
						.ParamName);
			}
		}
		#endregion

		#endregion

		#region Config

		[Fact]
		public void Adler32_Implementation_Config_IsCloneOfClone()
		{
			var adler32Config3 = Mock.Of<IAdler32Config>();
			var adler32Config2 = Mock.Of<IAdler32Config>(bc => bc.HashSizeInBits == 32 && bc.Clone() == adler32Config3);
			var adler32Config = Mock.Of<IAdler32Config>(bc => bc.Clone() == adler32Config2);

			var adler32Hash = new Adler32_Implementation(adler32Config);

			Assert.Equal(adler32Config3, adler32Hash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Adler32_Implementation_HashSizeInBits_IsFromConfig()
		{
			var adler32Config2 = Mock.Of<IAdler32Config>(bc => bc.HashSizeInBits == 32 && bc.Clone() == new Adler32Config());
			var adler32Config = Mock.Of<IAdler32Config>(bc => bc.Clone() == adler32Config2);

			var adler32Hash = new Adler32_Implementation(adler32Config);

			Assert.Equal(32, adler32Hash.Config.HashSizeInBits);
		}

		#endregion

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IAdler32, IAdler32Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.Empty, "00000001"),
					new KnownValue(32, TestConstants.FooBar, "08ab027a"),
					new KnownValue(32, TestConstants.LoremIpsum, "4fa43789"),
				};

			protected override IAdler32 CreateHashFunction(int hashSize) =>
				new Adler32_Implementation(
					new Adler32Config());
		}
	}
}