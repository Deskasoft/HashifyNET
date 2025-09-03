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

using HashifyNet.Algorithms.Jenkins;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Jenkins
{
	public class JenkinsLookup3_Implementation_Tests
	{

		#region Constructor

		[Fact]
		public void JenkinsLookup3_Implementation_Constructor_ValidInputs_Works()
		{
			var jenkinsLookupConfigMock = new Mock<IJenkinsLookup3Config>();
			{
				jenkinsLookupConfigMock.SetupGet(jlc => jlc.HashSizeInBits)
					.Returns(32);

				jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
					.Returns(() => jenkinsLookupConfigMock.Object);
			}

			GC.KeepAlive(
				new JenkinsLookup3_Implementation(jenkinsLookupConfigMock.Object));
		}

		#region Config

		[Fact]
		public void JenkinsLookup3_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new JenkinsLookup3_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void JenkinsLookup3_Implementation_Constructor_Config_IsCloned()
		{
			var jenkinsLookupConfigMock = new Mock<IJenkinsLookup3Config>();
			{
				jenkinsLookupConfigMock.SetupGet(jlc => jlc.HashSizeInBits)
					.Returns(32);

				jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
					.Returns(() => new JenkinsLookup3Config()
					{
						HashSizeInBits = 32
					});
			}

			GC.KeepAlive(
				new JenkinsLookup3_Implementation(jenkinsLookupConfigMock.Object));

			jenkinsLookupConfigMock.Verify(jlc => jlc.Clone(), Times.Once);

			jenkinsLookupConfigMock.VerifyGet(jlc => jlc.HashSizeInBits, Times.Never);
			jenkinsLookupConfigMock.VerifyGet(jlc => jlc.Seed, Times.Never);
			jenkinsLookupConfigMock.VerifyGet(jlc => jlc.Seed2, Times.Never);
		}

		#region HashSizeInBits


		[Fact]
		public void JenkinsLookup3_Implementation_Constructor_Config_HashSizeInBits_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 1, 31, 33, 63, 65 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var jenkinsLookupConfigMock = new Mock<IJenkinsLookup3Config>();
				{
					jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
						.Returns(() =>
							new JenkinsLookup3Config()
							{
								HashSizeInBits = invalidHashSize
							});
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => new JenkinsLookup3_Implementation(jenkinsLookupConfigMock.Object))
						.ParamName);
			}
		}

		[Fact]
		public void JenkinsLookup3_Implementation_Constructor_Config_HashSizeInBits_IsValid_Works()
		{
			var validHashSizes = new[] { 32, 64 };

			foreach (var validHashSize in validHashSizes)
			{
				var jenkinsLookupConfigMock = new Mock<IJenkinsLookup3Config>();
				{
					jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
						.Returns(() =>
							new JenkinsLookup3Config()
							{
								HashSizeInBits = validHashSize
							});
				}

				GC.KeepAlive(
					new JenkinsLookup3_Implementation(jenkinsLookupConfigMock.Object));
			}
		}

		#endregion

		#endregion

		#endregion

		#region ComputeHash

		[Fact]
		public void JenkinsLookup3_Implementation_ComputeHash_HashSizeInBits_MagicallyInvalid_Throws()
		{
			var jenkinsLookupConfigMock = new Mock<IJenkinsLookup3Config>();
			{
				var readCount = 0;

				jenkinsLookupConfigMock.SetupGet(jlc => jlc.HashSizeInBits)
					.Returns(() =>
					{
						readCount += 1;

						if (readCount == 1)
						{
							return 32;
						}

						return 33;
					});

				jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
					.Returns(() => jenkinsLookupConfigMock.Object);
			}

			var jenkinsLookup3 = new JenkinsLookup3_Implementation(jenkinsLookupConfigMock.Object);

			Assert.Throws<NotImplementedException>(
				() => jenkinsLookup3.ComputeHash(new byte[1]));
		}

		#endregion

		public class IHashFunction_Tests
			: IHashFunction_TestBase<IJenkinsLookup3, IJenkinsLookup3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.FooBar, 0xaeb72b0c),
					new KnownValue(64, TestConstants.FooBar, 0xacf2fd4caeb72b0c),
					new KnownValue(32, TestConstants.LoremIpsum.Take(12), 0x8e663aee),
					new KnownValue(64, TestConstants.LoremIpsum.Take(12), 0xb6d69c8c8e663aee),
					new KnownValue(32, TestConstants.LoremIpsum.Take(15), 0x9b484ed5),
					new KnownValue(64, TestConstants.LoremIpsum.Take(15), 0x14b71e4d9b484ed5),
					new KnownValue(32, TestConstants.LoremIpsum.Take(19), 0xad9cba6f),
					new KnownValue(64, TestConstants.LoremIpsum.Take(19), 0x2807920ead9cba6f),
					new KnownValue(32, TestConstants.LoremIpsum.Take(23), 0x57c46621),
					new KnownValue(64, TestConstants.LoremIpsum.Take(23), 0x01e860f357c46621),
					new KnownValue(32, TestConstants.LoremIpsum.Take(24), 0xf2b662ef),
					new KnownValue(64, TestConstants.LoremIpsum.Take(24), 0xcad6d781f2b662ef),
				};

			protected override IJenkinsLookup3 CreateHashFunction(int hashSize) =>
				new JenkinsLookup3_Implementation(
					new JenkinsLookup3Config()
					{
						HashSizeInBits = hashSize
					});
		}

		public class IHashFunction_Tests_DefaultConstructor
			: IHashFunction_TestBase<IJenkinsLookup3, IJenkinsLookup3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.FooBar, 0xaeb72b0c),
					new KnownValue(32, TestConstants.LoremIpsum.Take(12), 0x8e663aee),
					new KnownValue(32, TestConstants.LoremIpsum.Take(15), 0x9b484ed5),
					new KnownValue(32, TestConstants.LoremIpsum.Take(19), 0xad9cba6f),
					new KnownValue(32, TestConstants.LoremIpsum.Take(23), 0x57c46621),
					new KnownValue(32, TestConstants.LoremIpsum.Take(24), 0xf2b662ef),
				};

			protected override IJenkinsLookup3 CreateHashFunction(int hashSize) =>
				new JenkinsLookup3_Implementation(
					new JenkinsLookup3Config());
		}

		public class IHashFunction_Tests_WithInitVals
			: IHashFunction_TestBase<IJenkinsLookup3, IJenkinsLookup3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.FooBar, 0x7306b2fc),
					new KnownValue(64, TestConstants.FooBar, 0x28b9e020444d6de2),
				};

			protected override IJenkinsLookup3 CreateHashFunction(int hashSize) =>
				new JenkinsLookup3_Implementation(
					new JenkinsLookup3Config()
					{
						HashSizeInBits = hashSize,
						Seed = 0x7da236b9,
						Seed2 = unchecked((int)0x87930b75)
					});
		}

		public class IHashFunction_Tests_WithInitVals_DefaultHashSize
			: IHashFunction_TestBase<IJenkinsLookup3, IJenkinsLookup3Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.FooBar, 0x7306b2fc),
				};

			protected override IJenkinsLookup3 CreateHashFunction(int hashSize) =>
				new JenkinsLookup3_Implementation(
					new JenkinsLookup3Config()
					{
						Seed = 0x7da236b9,
						Seed2 = unchecked((int)0x87930b75)
					});
		}
	}
}
