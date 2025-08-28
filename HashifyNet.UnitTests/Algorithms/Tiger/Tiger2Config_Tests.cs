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

using HashifyNet.Algorithms.Tiger2;

namespace HashifyNet.UnitTests.Algorithms.Tiger2
{
	public class Tiger2Config_Tests
	{
		[Fact]
		public void Tiger2Config_Defaults_HaventChanged()
		{
			var tigerConfig = new Tiger2Config();

			Assert.Equal(192, tigerConfig.HashSizeInBits);
		}

		[Fact]
		public void Tiger2Config_Clone_Works()
		{
			var tigerConfig = new Tiger2Config()
			{
				HashSizeInBits = 160,
			};

			var tigerConfigClone = tigerConfig.Clone();

			Assert.IsType<Tiger2Config>(tigerConfigClone);

			Assert.Equal(tigerConfig.HashSizeInBits, tigerConfigClone.HashSizeInBits);
		}
	}
}