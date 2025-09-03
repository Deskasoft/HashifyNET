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

using HashifyNet.Algorithms.XxHash3;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.XxHash3
{
	public class XxHash3_Implementation_Tests
	{
		#region Constructor

		[Fact]
		public void XxHash3_Implementation_Constructor_ValidInputs_Works()
		{
			var xxHashConfigMock = new Mock<IXxHash3Config>();
			{
				xxHashConfigMock.SetupGet(xhc => xhc.HashSizeInBits)
					.Returns(64);

				xxHashConfigMock.SetupGet(xhc => xhc.Seed)
					.Returns(0L);

				xxHashConfigMock.Setup(xhc => xhc.Clone())
					.Returns(() => xxHashConfigMock.Object);
			}

			GC.KeepAlive(
				new XxHash3_Implementation(xxHashConfigMock.Object));
		}

		#region Config

		[Fact]
		public void XxHash3_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new XxHash3_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void XxHash3_Implementation_Constructor_Config_IsCloned()
		{
			var xxHashConfigMock = new Mock<IXxHash3Config>();
			{
				xxHashConfigMock.Setup(xhc => xhc.Clone())
					.Returns(() => new XxHash3Config()
					{
						HashSizeInBits = 64,
					});
			}

			GC.KeepAlive(
				new XxHash3_Implementation(xxHashConfigMock.Object));

			xxHashConfigMock.Verify(xhc => xhc.Clone(), Times.Once);

			xxHashConfigMock.VerifyGet(xhc => xhc.HashSizeInBits, Times.Never);
			xxHashConfigMock.VerifyGet(xhc => xhc.Seed, Times.Never);
		}

		#region HashSizeInBits

		[Fact]
		public void XxHash3_Implementation_Constructor_Config_HashSizeInBits_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 31, 33, 63, 65, 127, 129, 65535 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var xxHashConfigMock = new Mock<IXxHash3Config>();
				{
					xxHashConfigMock.SetupGet(pc => pc.HashSizeInBits)
						.Returns(invalidHashSize);

					xxHashConfigMock.Setup(pc => pc.Clone())
						.Returns(() => xxHashConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => new XxHash3_Implementation(xxHashConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void XxHash3_Implementation_Constructor_Config_HashSizeInBits_IsValid_Works()
		{
			var validHashSizes = new[] { 64, 128 };

			foreach (var validHashSize in validHashSizes)
			{
				var xxHashConfigMock = new Mock<IXxHash3Config>();
				{
					xxHashConfigMock.SetupGet(pc => pc.HashSizeInBits)
						.Returns(validHashSize);

					xxHashConfigMock.Setup(pc => pc.Clone())
						.Returns(() => xxHashConfigMock.Object);
				}

				GC.KeepAlive(
					new XxHash3_Implementation(xxHashConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests_xxHash3_DefaultConstructor
			: IStreamableHashFunction_TestBase<IXxHash3, IXxHash3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "2d06800538d394c2"),
					new KnownValue(64, TestConstants.FooBar, "d78fda63144c5c84"),
					new KnownValue(64, TestConstants.LoremIpsum, "060828d267c7d477"),
				};

			protected override IXxHash3 CreateHashFunction(int hashSize) =>
				new XxHash3_Implementation(
					new XxHash3Config());
		}

		public class IStreamableHashFunction_Tests_xxHash3
		: IStreamableHashFunction_TestBase<IXxHash3, IXxHash3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "2d06800538d394c2"),
					new KnownValue(64, TestConstants.FooBar, "d78fda63144c5c84"),
					new KnownValue(64, TestConstants.LoremIpsum, "060828d267c7d477"),

					new KnownValue(128, TestConstants.Empty, "99aa06d3014798d86001c324468d497f"),
					new KnownValue(128, TestConstants.FooBar, "3c9e102628997f44ac87b0b131c6992d"),
					new KnownValue(128, TestConstants.LoremIpsum, "6af7a84c00767a0d77d1b81bbaa39705"),
				};

			protected override IXxHash3 CreateHashFunction(int hashSize) =>
				new XxHash3_Implementation(
					new XxHash3Config()
					{
						HashSizeInBits = hashSize
					});
		}
	}

}
