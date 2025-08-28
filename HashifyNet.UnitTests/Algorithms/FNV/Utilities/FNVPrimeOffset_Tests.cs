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

using HashifyNet.Algorithms.FNV.Utilities;
using System.Numerics;

namespace HashifyNet.UnitTests.Algorithms.FNV.Utilities
{
	public class FNVPrimeOffset_Tests
	{
		[Fact]
		public void FNVPrimeOffset_Create_InvalidBitSize_Throws()
		{
			var invalidBitSizes = new[] { -1, 0, 8, 16, 31, 33, 63, 65, 255 };

			foreach (var invalidBitSize in invalidBitSizes)
			{
				Assert.Equal(
					"bitSize",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => FNVPrimeOffset.Create(invalidBitSize, BigInteger.One, BigInteger.One))
						.ParamName);
			}
		}

		[Fact]
		public void FNVPrimeOffset_Create_ValidBitSize_Throws()
		{
			var validBitSizes = new[] { 32, 64, 96, 128, 256, 512, 1024, 65536 };

			foreach (var validBitSize in validBitSizes)
			{
				var fnvPrimeOffset = FNVPrimeOffset.Create(validBitSize, BigInteger.One, BigInteger.One);

				Assert.NotNull(fnvPrimeOffset);
			}
		}

		[Fact]
		public void FNVPrimeOffset_Create_InvalidPrime_Throws()
		{
			var invalidPrimes = new[] { new BigInteger(-65536), BigInteger.Zero, BigInteger.MinusOne };

			foreach (var invalidPrime in invalidPrimes)
			{
				Assert.Equal(
					"prime",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => FNVPrimeOffset.Create(32, invalidPrime, BigInteger.One))
						.ParamName);
			}
		}

		[Fact]
		public void FNVPrimeOffset_Create_InvalidOffset_Throws()
		{
			var invalidOffsets = new[] { new BigInteger(-65536), BigInteger.Zero, BigInteger.MinusOne };

			foreach (var invalidOffset in invalidOffsets)
			{
				Assert.Equal(
					"offset",
					Assert.Throws<ArgumentOutOfRangeException>(
							() => FNVPrimeOffset.Create(32, BigInteger.One, invalidOffset))
						.ParamName);
			}
		}

		[Fact]
		public void FNVPrimeOffset_Create_Works()
		{
			Assert.NotNull(
				FNVPrimeOffset.Create(32, BigInteger.One, BigInteger.One));
		}

		[Fact]
		public void FNVPrimeOffset_PrimeAndOffset_Works()
		{
			{
				var fnvPrimeOffset = FNVPrimeOffset.Create(32, BigInteger.One, new BigInteger(UInt64.MaxValue));

				Assert.Equal(
					new UInt32[] { 1 },
					fnvPrimeOffset.Prime);

				Assert.Equal(
					new UInt32[] { UInt32.MaxValue },
					fnvPrimeOffset.Offset);
			}

			{
				var fnvPrimeOffset = FNVPrimeOffset.Create(96, new BigInteger(UInt64.MaxValue), BigInteger.One);


				Assert.Equal(
					new UInt32[] { UInt32.MaxValue, UInt32.MaxValue, 0 },
					fnvPrimeOffset.Prime);

				Assert.Equal(
					new UInt32[] { 1, 0, 0 },
					fnvPrimeOffset.Offset);
			}
		}

		[Fact]
		public void FNVPrimeOffset_PrimeAndOffset_ResultsAreCached()
		{
			var fnvPrimeOffset1 = FNVPrimeOffset.Create(32, BigInteger.One, BigInteger.One);
			var fnvPrimeOffset2 = FNVPrimeOffset.Create(32, BigInteger.One, BigInteger.One);

			Assert.Equal(
				fnvPrimeOffset1.Prime,
				fnvPrimeOffset2.Prime);

			Assert.Equal(
				fnvPrimeOffset1.Prime,
				fnvPrimeOffset2.Offset);

			Assert.Equal(
				fnvPrimeOffset1.Prime,
				fnvPrimeOffset2.Prime);

			Assert.Equal(
				fnvPrimeOffset1.Prime,
				fnvPrimeOffset2.Offset);
		}
	}
}