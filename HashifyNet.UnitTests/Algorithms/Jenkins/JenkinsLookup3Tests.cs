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

namespace HashifyNet.UnitTests.Algorithms.Jenkins
{
	public class JenkinsLookup3Tests
	{
		[Fact]
		public void JenkinsLookup3_32bit_ComputeHash_ExtremelyLongStream_Works()
		{
			byte[] knownValue;

			{
				var loremIpsumRepeatCount = 800;
				var loremIpsumLength = TestConstants.LoremIpsum.Length;

				knownValue = new byte[loremIpsumLength * loremIpsumRepeatCount];

				for (var x = 0; x < loremIpsumRepeatCount; ++x)
				{
					Array.Copy(TestConstants.LoremIpsum, 0, knownValue, loremIpsumLength * x, loremIpsumLength);
				}
			}

			var jenkinsLookup3 = new JenkinsLookup3_Implementation(
				new JenkinsLookup3Config()
				{
					HashSizeInBits = 32
				});

			var resultBytes = jenkinsLookup3.ComputeHash(knownValue);

			Assert.Equal(
				0x85c64fdU,
				BitConverter.ToUInt32(resultBytes.Hash, 0));
		}
	}
}