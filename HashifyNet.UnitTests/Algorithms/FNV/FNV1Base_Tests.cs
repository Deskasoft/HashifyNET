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

using HashifyNet.Algorithms.FNV;

namespace HashifyNet.UnitTests.Algorithms.FNV
{
	public class FNV1Base_Tests
	{
		private abstract class FNV1Impl
			: FNV1Base
		{
			private FNV1Impl(IFNVConfig config)
				: base(config)
			{
			}

			public static uint[] ExtendedMultiplyTest(IReadOnlyList<uint> operand1, IReadOnlyList<uint> operand2, int hashSizeInBytes) =>
				ExtendedMultiply(operand1, operand2, hashSizeInBytes);
		}

		[Fact]
		public void FNV1Base_ExtendedMultiply_WorksConversly()
		{
			var x = new uint[] { 65536, 1024 };
			var y = new uint[] { 524288, 65536, 1024, 8 };

			var expectedValue = new uint[] { 0, 536870920, 134217729, 1572864 };

			Assert.Equal(expectedValue, FNV1Impl.ExtendedMultiplyTest(x, y, 16));
			Assert.Equal(expectedValue, FNV1Impl.ExtendedMultiplyTest(y, x, 16));
		}
	}
}
