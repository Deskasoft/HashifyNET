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

using HashifyNet.Algorithms.BuzHash;

namespace HashifyNet.UnitTests.Algorithms.BuzHash
{
	public class BuzHashConfig_Tests
	{
		[Fact]
		public void BuzHashConfig_Defaults_HaventChanged()
		{
			var buzHashConfig = new BuzHashConfig();

			Assert.Null(buzHashConfig.Rtab);

			Assert.Equal(64, buzHashConfig.HashSizeInBits);
			Assert.Equal(0UL, buzHashConfig.Seed);
			Assert.Equal(CircularShiftDirection.Left, buzHashConfig.ShiftDirection);
		}

		[Fact]
		public void BuzHashConfig_Clone_Works()
		{
			var buzHashConfig = new BuzHashConfig()
			{
				Rtab = new UInt64[256],
				HashSizeInBits = 32,
				Seed = 1337UL,
				ShiftDirection = CircularShiftDirection.Right
			};

			var buzHashConfigClone = buzHashConfig.Clone();

			Assert.IsType<BuzHashConfig>(buzHashConfigClone);

			Assert.Equal(buzHashConfig.Rtab, buzHashConfigClone.Rtab);
			Assert.Equal(buzHashConfig.HashSizeInBits, buzHashConfigClone.HashSizeInBits);
			Assert.Equal(buzHashConfig.Seed, buzHashConfigClone.Seed);
			Assert.Equal(buzHashConfig.ShiftDirection, buzHashConfigClone.ShiftDirection);
		}

		[Fact]
		public void BuzHashConfig_Clone_WithNullArrays_Works()
		{
			var buzHashConfig = new BuzHashConfig()
			{
				Rtab = null,
				HashSizeInBits = 32,
				Seed = 1337UL,
				ShiftDirection = CircularShiftDirection.Right
			};

			var buzHashConfigClone = buzHashConfig.Clone();

			Assert.IsType<BuzHashConfig>(buzHashConfigClone);

			Assert.Equal(buzHashConfig.Rtab, buzHashConfigClone.Rtab);
			Assert.Equal(buzHashConfig.HashSizeInBits, buzHashConfigClone.HashSizeInBits);
			Assert.Equal(buzHashConfig.Seed, buzHashConfigClone.Seed);
			Assert.Equal(buzHashConfig.ShiftDirection, buzHashConfigClone.ShiftDirection);
		}
	}
}