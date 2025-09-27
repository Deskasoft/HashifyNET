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

namespace HashifyNet.UnitTests.Algorithms.T1HA
{
	public class T1HA2Config_Tests
	{
		[Fact]
		public void T1HA2Config_Defaults_HaventChanged()
		{
			var t1HAConfig = new T1HA2Config();
			Assert.Equal(64, t1HAConfig.HashSizeInBits);
		}

		[Fact]
		public void T1HA2Config_Clone_Works()
		{
			var t1HAConfig = new T1HA2Config()
			{
				HashSizeInBits = 128,
				Seed = 1990L,
				Seed2 = 2001L,
			};

			var t1haConfigClone = t1HAConfig.Clone();

			Assert.IsType<T1HA2Config>(t1haConfigClone);

			Assert.Equal(t1HAConfig.HashSizeInBits, t1haConfigClone.HashSizeInBits);
			Assert.Equal(t1HAConfig.Seed, t1haConfigClone.Seed);
			Assert.Equal(t1HAConfig.Seed2, t1haConfigClone.Seed2);
		}
	}
}