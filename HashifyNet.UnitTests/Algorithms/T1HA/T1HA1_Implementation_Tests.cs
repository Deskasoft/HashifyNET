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

using HashifyNet.Algorithms.T1HA;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.T1HA
{
	public class T1HA1_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void T1HA1_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new T1HA1_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void T1HA1_Implementation_Constructor_Config_IsCloned()
		{
			var tigerConfigMock = new Mock<IT1HA1Config>();
			{
				tigerConfigMock.Setup(bc => bc.Clone())
					.Returns(new T1HA1Config());
			}

			GC.KeepAlive(
				new T1HA1_Implementation(tigerConfigMock.Object));

			tigerConfigMock.Verify(bc => bc.Clone(), Times.Once);

			tigerConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void T1HA1_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 7, 9, 10, 11, 12, 13, 14, 15, 513, 520 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var t1HAConfigMock = new Mock<IT1HA1Config>();
				{
					t1HAConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					t1HAConfigMock.Setup(bc => bc.Clone())
						.Returns(() => t1HAConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new T1HA1_Implementation(
								t1HAConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void T1HA1_Implementation_Constructor_Config_ValidHashSize_Works()
		{
			var validHashSizes = new int[] { 64 };

			foreach (var validHashSize in validHashSizes)
			{
				var t1HAConfigMock = new Mock<IT1HA1Config>();
				{
					t1HAConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(validHashSize);

					t1HAConfigMock.Setup(bc => bc.Clone())
						.Returns(() => t1HAConfigMock.Object);
				}

				GC.KeepAlive(
					new T1HA1_Implementation(
						t1HAConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region Config

		[Fact]
		public void T1HA1_Implementation_Config_IsCloneOfClone()
		{
			var t1HAConfig3 = Mock.Of<IT1HA1Config>();
			var t1HAConfig2 = Mock.Of<IT1HA1Config>(bc => bc.HashSizeInBits == 64 && bc.Clone() == t1HAConfig3);
			var t1HAConfig = Mock.Of<IT1HA1Config>(bc => bc.Clone() == t1HAConfig2);

			var t1HAHash = new T1HA1_Implementation(t1HAConfig);

			Assert.Equal(t1HAConfig3, t1HAHash.Config);
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void T1HA1_Implementation_HashSizeInBits_IsFromConfig()
		{
			var t1HAConfig2 = Mock.Of<IT1HA1Config>(bc => bc.HashSizeInBits == 64 && bc.Clone() == new T1HA1Config());
			var t1HAConfig = Mock.Of<IT1HA1Config>(bc => bc.Clone() == t1HAConfig2);

			var tigerHash = new T1HA1_Implementation(t1HAConfig);

			Assert.Equal(64, tigerHash.Config.HashSizeInBits);
		}

		#endregion

		public class IHashFunction_Tests_DefaultConstructor
			: IHashFunction_TestBase<IT1HA1, IT1HA1Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "0x0000000000000000"),
					new KnownValue(64, TestConstants.FooBar, "0x54758117B7094EEA"),
					new KnownValue(64, TestConstants.LoremIpsum, "0x67CEF9DD7C4FFA8F"),
				};

			protected override IT1HA1 CreateHashFunction(int hashSize) =>
				new T1HA1_Implementation(
					new T1HA1Config());
		}

		public class IHashFunction_Tests_WithSeed
	: IHashFunction_TestBase<IT1HA1, IT1HA1Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, "0x9AC379DED4CFF850"),
					new KnownValue(64, TestConstants.FooBar, "0xFC6CE33F6E9100E9"),
					new KnownValue(64, TestConstants.LoremIpsum, "0xF98A2B714E25D68A"),
				};

			protected override IT1HA1 CreateHashFunction(int hashSize) =>
				new T1HA1_Implementation(
					new T1HA1Config()
					{
						Seed = 1990L
					});
		}
	}
}