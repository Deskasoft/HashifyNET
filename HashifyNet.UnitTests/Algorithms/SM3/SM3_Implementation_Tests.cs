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

using HashifyNet.Algorithms.SM3;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.SM3
{
	public class SM3_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void SM3_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SM3_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SM3_Implementation_Constructor_Config_IsCloned()
		{
			var SM3ConfigMock = new Mock<ISM3Config>();
			{
				SM3ConfigMock.Setup(bc => bc.Clone())
					.Returns(new SM3Config());
			}

			GC.KeepAlive(
				new SM3_Implementation(SM3ConfigMock.Object));

			SM3ConfigMock.Verify(bc => bc.Clone(), Times.Once);

			SM3ConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void SM3_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var SM3ConfigMock = new Mock<ISM3Config>();
				{
					SM3ConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					SM3ConfigMock.Setup(bc => bc.Clone())
						.Returns(() => SM3ConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new SM3_Implementation(
								SM3ConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<ISM3, ISM3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(256, TestConstants.Empty, "1ab21d8355cfa17f8e61194831e81a8f22bec8c728fefb747ed035eb5082aa2b"),
					new KnownValue(256, TestConstants.FooBar, "adac3a2e85990453d191fd60633e44a4bed4e5785a7a023e6ad908eba21069e6"),
					new KnownValue(256, TestConstants.LoremIpsum, "6c902b5a4bcf740954678ea4f3c0f787def7e762486ce26f42b5ef6ba5b90bdd"),
					new KnownValue(256, new byte[4096], "996d9ccd1272a25d574ed05aaa72c6cfd9736d3cdd0ff72a45031f6a1c4092ba"),
				};

			protected override ISM3 CreateHashFunction(int hashSize) =>
				new SM3_Implementation(
					new SM3Config());
		}
	}
}