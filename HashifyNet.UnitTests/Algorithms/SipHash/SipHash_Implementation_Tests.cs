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

using HashifyNet.Algorithms.SipHash;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.SipHash
{
	public class SipHash_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void SipHash_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SipHash_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SipHash_Implementation_Constructor_Config_IsCloned()
		{
			var SipHashConfigMock = new Mock<ISipHashConfig>();
			{
				SipHashConfigMock.Setup(bc => bc.Clone())
					.Returns(new SipHashConfig());
			}

			GC.KeepAlive(
				new SipHash_Implementation(SipHashConfigMock.Object));

			SipHashConfigMock.Verify(bc => bc.Clone(), Times.Once);

			SipHashConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}


		#region HashSize

		[Fact]
		public void SipHash_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var SipHashConfigMock = new Mock<ISipHashConfig>();
				{
					SipHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					SipHashConfigMock.SetupGet(bc => bc.Key)
						.Returns(new byte[16]);

					SipHashConfigMock.Setup(bc => bc.Clone())
						.Returns(() => SipHashConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new SipHash_Implementation(
								SipHashConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#region Key

		[Fact]
		public void SipHash_Implementation_Constructor_Config_InvalidKeyLength_Throws()
		{
			var SipHashConfigMock = new Mock<ISipHashConfig>();
			{
				SipHashConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(64);

				SipHashConfigMock.SetupGet(bc => bc.Key)
					.Returns(new byte[15]);

				SipHashConfigMock.Setup(bc => bc.Clone())
					.Returns(() => SipHashConfigMock.Object);
			}


			Assert.Equal("config.Key",
				Assert.Throws<ArgumentOutOfRangeException>(() =>
						new SipHash_Implementation(
							SipHashConfigMock.Object))
					.ParamName);
		}

		#endregion

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<ISipHash, ISipHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "D70077739D4B921E"),
					new KnownValue(64, TestConstants.FooBar, "EB0D539DB97CAC0A"),
					new KnownValue(64, TestConstants.LoremIpsum, "7AF4C1B9EDE7B3E7"),
					new KnownValue(64, new byte[4096], "E89E6B6CC9B19731"),
				};

			protected override ISipHash CreateHashFunction(int hashSize) =>
				new SipHash_Implementation(
					new SipHashConfig());
		}

		public class IStreamableHashFunction_Tests_WithCDRounds
	: IStreamableHashFunction_TestBase<ISipHash, ISipHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
						new KnownValue(64, TestConstants.Empty, "2C530C1562A7FBD1"),
						new KnownValue(64, TestConstants.FooBar, "349BF05D0AAE3877"),
						new KnownValue(64, TestConstants.LoremIpsum, "93469F32F1E95D2E"),
						new KnownValue(64, new byte[4096], "77785FE2325E7031"),
				};

			protected override ISipHash CreateHashFunction(int hashSize) =>
				new SipHash_Implementation(
					new SipHashConfig()
					{
						C_Rounds = 1,
						D_Rounds = 3,
					});
		}

		public class IStreamableHashFunction_Tests_WithCDRoundsAndFoobarAsKey
: IStreamableHashFunction_TestBase<ISipHash, ISipHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
						new KnownValue(64, TestConstants.Empty, "97F2D49506F607D0"),
						new KnownValue(64, TestConstants.FooBar, "29BFDF8F79328CD9"),
						new KnownValue(64, TestConstants.LoremIpsum, "CEE0C73AA4996814"),
						new KnownValue(64, new byte[4096], "214DB7DBEBFE1116"),
				};

			protected override ISipHash CreateHashFunction(int hashSize) =>
				new SipHash_Implementation(
					new SipHashConfig()
					{
						Key = TestConstants.FooBar.Concat(new byte[10]).ToArray(),
						C_Rounds = 1,
						D_Rounds = 3,
					});
		}

		public class IStreamableHashFunction_Tests_WithFoobarAsKey
			: IStreamableHashFunction_TestBase<ISipHash, ISipHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "82FA81C71AB68CD2"),
					new KnownValue(64, TestConstants.FooBar, "A79BD4926BE711A2"),
					new KnownValue(64, TestConstants.LoremIpsum, "876310F92265BF9E"),
					new KnownValue(64, new byte[4096], "91168C9004F64E2C"),
				};

			protected override ISipHash CreateHashFunction(int hashSize) =>
				new SipHash_Implementation(
					new SipHashConfig()
					{
						Key = TestConstants.FooBar.Concat(new byte[10]).ToArray()
					});
		}
	}
}