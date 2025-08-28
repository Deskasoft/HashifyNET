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

namespace HashifyNet.UnitTests.Algorithms.Jenkins
{
	public class JenkinsLookup3Config_Tests
	{
		[Fact]
		public void JenkinsLookup3Config_Defaults_HaventChanged()
		{
			var jenkinsLookup3Config = new JenkinsLookup3Config();

			Assert.Equal(32, jenkinsLookup3Config.HashSizeInBits);
			Assert.Equal(0U, jenkinsLookup3Config.Seed);
			Assert.Equal(0U, jenkinsLookup3Config.Seed2);
		}

		[Fact]
		public void JenkinsLookup3Config_Clone_Works()
		{
			var jenkinsLookup3Config = new JenkinsLookup3Config()
			{
				HashSizeInBits = 64,
				Seed = 1337U,
				Seed2 = 7331U
			};

			var jenkinsLookup3ConfigClone = jenkinsLookup3Config.Clone();

			Assert.IsType<JenkinsLookup3Config>(jenkinsLookup3ConfigClone);

			Assert.Equal(jenkinsLookup3Config.HashSizeInBits, jenkinsLookup3ConfigClone.HashSizeInBits);
			Assert.Equal(jenkinsLookup3Config.Seed, jenkinsLookup3ConfigClone.Seed);
			Assert.Equal(jenkinsLookup3Config.Seed2, jenkinsLookup3ConfigClone.Seed2);
		}
	}
}