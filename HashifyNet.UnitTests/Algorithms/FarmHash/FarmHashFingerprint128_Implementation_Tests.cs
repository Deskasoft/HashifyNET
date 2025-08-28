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
	public class FarmHashFingerprint128_Implementation_Tests
	{

		#region Constructor

		[Fact]
		public void FarmHashFingerprint128_Implementation_Constructor_Works()
		{
			GC.KeepAlive(
				HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Instance.Create(new FarmHashConfig() { HashSizeInBits = 128 }));
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void FarmHashFingerprint128_Implementation_HashSizeInBits_Is128()
		{
			var farmHash = HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Instance.Create(new FarmHashConfig() { HashSizeInBits = 128 });

			Assert.Equal(128, farmHash.Config.HashSizeInBits);
		}

		#endregion

		public class IHashFunction_Tests
			: IHashFunction_TestBase<IFarmHashFingerprint, IFarmHashConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.Empty, "0x3cb540c392e51e293df09dfc64c09a2b"),
					new KnownValue(128, TestConstants.FooBar.Take(3), "0xb39b3f148460e4ac14be06e50b80b82e"),
					new KnownValue(128, TestConstants.FooBar, "0x988cb7153af1a1da72162ccf17c06450"),
					new KnownValue(128, TestConstants.LoremIpsum.Take(13), "0x2e8ba24e20ecfd83a0adc6c41584ca33"),
					new KnownValue(128, TestConstants.LoremIpsum.Take(17), "0x5f74542e89cb32d3fb12ca78d96817cb"),
					new KnownValue(128, TestConstants.LoremIpsum.Take(31), "0x4a98ff6acfe7dc9ec455b7f9b98865b7"),
					new KnownValue(128, TestConstants.LoremIpsum.Take(50), "0xbfbb5a9fa793e2b857fff2a86ba83dfd"),
					new KnownValue(128, TestConstants.LoremIpsum, "0xa4f60ceb5e562608dc296c7ab55cdd31"),
				};

			protected override IFarmHashFingerprint CreateHashFunction(int hashSize) =>
				HashFactory<IFarmHashFingerprint, IFarmHashConfig>.Instance.Create(new FarmHashConfig() { HashSizeInBits = 128 });
		}
	}
}