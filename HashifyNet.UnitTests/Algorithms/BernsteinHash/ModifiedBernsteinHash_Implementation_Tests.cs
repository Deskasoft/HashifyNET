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

using HashifyNet.Algorithms.BernsteinHash;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests.Algorithms.BernsteinHash
{
	public class ModifiedBernsteinHash_Implementation_Tests
	{
		[Fact]
		public void ModifiedBernsteinHash_Implementation_HashSizeInBits_IsSet()
		{
			var bernsteinHash = HashFactory<IModifiedBernsteinHash, IBernsteinConfig>.Create();

			Assert.Equal(32, bernsteinHash.Config.HashSizeInBits);
		}

		public class IStreamableHashFunction_Tests
			: IStreamableHashFunction_TestBase<IModifiedBernsteinHash, IBernsteinConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.Empty, 0x00000000),
					new KnownValue(32, TestConstants.FooBar, 0xf030b397),
					new KnownValue(32, TestConstants.LoremIpsum, 0xfeceaf2a),
				};

			protected override IModifiedBernsteinHash CreateHashFunction(int hashSize)
			{
				return HashFactory<IModifiedBernsteinHash, IBernsteinConfig>.Create();
			}
		}
	}

}
