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

using HashifyNet.Algorithms.HighwayHash;

namespace HashifyNet.UnitTests.Algorithms.HighwayHash
{
	public class HighwayHashConfig_Tests
	{
		[Fact]
		public void HighwayHashConfig_Defaults_HaventChanged()
		{
			var HighwayHashConfig = new HighwayHashConfig();

			Assert.Equal(64, HighwayHashConfig.HashSizeInBits);
			Assert.NotNull(HighwayHashConfig.Key);
			Assert.Equal(32, HighwayHashConfig.Key.Count);
		}

		[Fact]
		public void HighwayHashConfig_Clone_Works()
		{
			var HighwayHashConfig = new HighwayHashConfig()
			{
				HashSizeInBits = 256,
				Key = new byte[32],
			};

			var HighwayHashConfigClone = HighwayHashConfig.Clone();

			Assert.IsType<HighwayHashConfig>(HighwayHashConfigClone);

			Assert.Equal(HighwayHashConfig.HashSizeInBits, HighwayHashConfigClone.HashSizeInBits);
			Assert.Equal(HighwayHashConfig.Key, HighwayHashConfigClone.Key);
		}
	}
}