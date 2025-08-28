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

using HashifyNet.Algorithms.Blake2;

namespace HashifyNet.UnitTests.Algorithms.Blake2
{
	public class Blake2BConfig_Tests
	{
		[Fact]
		public void Blake2BConfig_Defaults_HaventChanged()
		{
			var blake2BConfig = new Blake2BConfig();

			Assert.Equal(512, blake2BConfig.HashSizeInBits);

			Assert.Null(blake2BConfig.Key);
			Assert.Null(blake2BConfig.Salt);
			Assert.Null(blake2BConfig.Personalization);
		}

		[Fact]
		public void Blake2BConfig_Clone_Works()
		{
			var blake2BConfig = new Blake2BConfig()
			{
				HashSizeInBits = 256,
				Key = new byte[64],
				Salt = new byte[16],
				Personalization = new byte[16],
			};

			var blake2BConfigClone = blake2BConfig.Clone();

			Assert.IsType<Blake2BConfig>(blake2BConfigClone);

			Assert.Equal(blake2BConfig.HashSizeInBits, blake2BConfigClone.HashSizeInBits);
			Assert.Equal(blake2BConfig.Key, blake2BConfigClone.Key);
			Assert.Equal(blake2BConfig.Salt, blake2BConfigClone.Salt);
			Assert.Equal(blake2BConfig.Personalization, blake2BConfigClone.Personalization);
		}

		[Fact]
		public void Blake2BConfig_Clone_WithNullArrays_Works()
		{
			var blake2BConfig = new Blake2BConfig()
			{
				Key = null,
				Salt = null,
				Personalization = null,
			};

			var blake2BConfigClone = blake2BConfig.Clone();

			Assert.IsType<Blake2BConfig>(blake2BConfigClone);

			Assert.Null(blake2BConfigClone.Key);
			Assert.Null(blake2BConfigClone.Salt);
			Assert.Null(blake2BConfigClone.Personalization);
		}
	}
}