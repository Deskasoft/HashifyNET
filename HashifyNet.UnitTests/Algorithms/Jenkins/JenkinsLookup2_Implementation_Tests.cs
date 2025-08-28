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
	public class JenkinsLookup2_Implementation_Tests
	{
		#region Constructor

		[Fact]
		public void JenkinsLookup2_Implementation_Constructor_ValidInputs_Works()
		{
			var jenkinsLookupConfigMock = new Mock<IJenkinsLookup2Config>();
			{
				jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
					.Returns(() => jenkinsLookupConfigMock.Object);
			}

			GC.KeepAlive(
				new JenkinsLookup2_Implementation(jenkinsLookupConfigMock.Object));
		}

		#region Config

		[Fact]
		public void JenkinsLookup2_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new JenkinsLookup2_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void JenkinsLookup2_Implementation_Constructor_Config_IsCloned()
		{
			var jenkinsLookupConfigMock = new Mock<IJenkinsLookup2Config>();
			{
				jenkinsLookupConfigMock.Setup(jlc => jlc.Clone())
					.Returns(() => new JenkinsLookup2Config());
			}

			GC.KeepAlive(
				new JenkinsLookup2_Implementation(jenkinsLookupConfigMock.Object));

			jenkinsLookupConfigMock.Verify(jlc => jlc.Clone(), Times.Once);

			jenkinsLookupConfigMock.VerifyGet(jlc => jlc.Seed, Times.Never);
		}

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IJenkinsLookup2, IJenkinsLookup2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.Empty, 0xbd49d10d),
					new KnownValue(32, TestConstants.FooBar, 0x9d3ffa02),
					new KnownValue(32, TestConstants.LoremIpsum.Take(15), 0x0c39787e),
					new KnownValue(32, TestConstants.LoremIpsum.Take(19), 0x2a06cf89),
					new KnownValue(32, TestConstants.LoremIpsum.Take(23), 0xa4fd862c),
				};

			protected override IJenkinsLookup2 CreateHashFunction(int hashSize) =>
				new JenkinsLookup2_Implementation(
					new JenkinsLookup2Config());
		}

		public class IStreamableHashFunction_Tests_WithInitVal
			: IStreamableHashFunction_TestBase<IJenkinsLookup2, IJenkinsLookup2Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.FooBar, 0x6117ff85),
				};

			protected override IJenkinsLookup2 CreateHashFunction(int hashSize) =>
				new JenkinsLookup2_Implementation(
					new JenkinsLookup2Config()
					{
						Seed = 0x7da236b9U
					});
		}
	}
}