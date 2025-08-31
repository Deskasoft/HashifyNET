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

using HashifyNet.Algorithms.FarmHash;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests.Algorithms.FarmHash
{
	public class FarmHashFingerprint32_Implementation_Tests
	{
		#region Constructor

		[Fact]
		public void FarmHashFingerprint32_Implementation_Constructor_Works()
		{
			GC.KeepAlive(
				HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 32 }));
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void FarmHashFingerprint32_Implementation_HashSizeInBits_Is32()
		{
			var farmHash = HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 32 });

			Assert.Equal(32, farmHash.Config.HashSizeInBits);
		}

		#endregion

		public class IHashFunction_Tests
			: IHashFunction_TestBase<IFarmHashFingerprint, IFarmHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(32, TestConstants.Empty, 0xdc56d17a),
					new KnownValue(32, TestConstants.FooBar.Take(3), 0x6b5025e3),
					new KnownValue(32, TestConstants.FooBar, 0xe2f34cdf),
					new KnownValue(32, TestConstants.LoremIpsum.Take(17), 0xe3e27892),
					new KnownValue(32, TestConstants.LoremIpsum, 0x6482ed0d),
				};

			protected override IFarmHashFingerprint CreateHashFunction(int hashSize) =>
			   HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 32 });
		}
	}

}
