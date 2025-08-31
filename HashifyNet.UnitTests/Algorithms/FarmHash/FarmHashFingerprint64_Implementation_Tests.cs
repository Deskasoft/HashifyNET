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
	public class FarmHashFingerprint64_Implementation_Tests
	{
		#region Constructor

		[Fact]
		public void FarmHashFingerprint64_Implementation_Constructor_Works()
		{
			GC.KeepAlive(
				HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 64 }));
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void FarmHashFingerprint64_Implementation_HashSizeInBits_Is64()
		{
			var farmHash = HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 64 });

			Assert.Equal(64, farmHash.Config.HashSizeInBits);
		}

		#endregion

		public class IHashFunction_Tests
			: IHashFunction_TestBase<IFarmHashFingerprint, IFarmHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(64, TestConstants.Empty, 0x9ae16a3b2f90404f),
					new KnownValue(64, TestConstants.FooBar.Take(3), 0x555c6f602f9383e3),
					new KnownValue(64, TestConstants.FooBar, 0xc43fb29ab5effcfe),
					new KnownValue(64, TestConstants.LoremIpsum.Take(13), 0x54145170e3383fcc),
					new KnownValue(64, TestConstants.LoremIpsum.Take(17), 0xbb25bd7ca089d86),
					new KnownValue(64, TestConstants.LoremIpsum.Take(50), 0x5462bf74ef4729b1),
					new KnownValue(64, TestConstants.LoremIpsum, 0x7975a177275d65bf),
				};

			protected override IFarmHashFingerprint CreateHashFunction(int hashSize) =>
				HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Create(new FarmHashConfig() { HashSizeInBits = 64 });
		}
	}

}
