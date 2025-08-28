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

using HashifyNet.Core.Utilities;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests
{
	public abstract class IHashFunction_TestBase<THashFunction, CName>
		where THashFunction : IHashFunction<CName> where CName : IHashConfig<CName>
	{
		protected abstract IEnumerable<KnownValue> KnownValues { get; }

		[Fact]
		public void IHashFunction_ComputeHash_ByteArray_MatchesKnownValues()
		{
			foreach (var knownValue in KnownValues)
			{
				var hf = CreateHashFunction(knownValue.HashSize);
				var hashResults = ComputeHash(hf, knownValue.TestValue);

				IHashConfig<CName> config = hf.Config;

				Assert.Equal(
					new HashValue(knownValue.ExpectedValue.Take((config.HashSizeInBits + 7) / 8), config.HashSizeInBits),
					hashResults);
			}
		}

		protected class KnownValue
		{
			public readonly int HashSize;
			public readonly byte[] TestValue;
			public readonly byte[] ExpectedValue;
			public KnownValue(int hashSize, IEnumerable<byte> testValue, IEnumerable<byte> expectedValue)
			{
				TestValue = testValue.ToArray();
				ExpectedValue = expectedValue.ToArray();
				HashSize = hashSize;
			}

			public KnownValue(int hashSize, string utf8Value, string expectedValue)
				: this(hashSize, utf8Value.ToBytes(), expectedValue.HexToBytes()) { }

			public KnownValue(int hashSize, string utf8Value, uint expectedValue)
				: this(hashSize, utf8Value.ToBytes(), ToBytes(expectedValue, 32)) { }

			public KnownValue(int hashSize, string utf8Value, ulong expectedValue)
				: this(hashSize, utf8Value.ToBytes(), ToBytes(expectedValue, 64)) { }

			public KnownValue(int hashSize, IEnumerable<byte> value, string expectedValue)
				: this(hashSize, value, expectedValue.HexToBytes()) { }

			public KnownValue(int hashSize, IEnumerable<byte> value, uint expectedValue)
				: this(hashSize, value, ToBytes(expectedValue, 32)) { }

			public KnownValue(int hashSize, IEnumerable<byte> value, ulong expectedValue)
				: this(hashSize, value, ToBytes(expectedValue, 64)) { }
		}

		protected abstract THashFunction CreateHashFunction(int hashSize);

		protected virtual IHashValue ComputeHash(THashFunction hf, byte[] data)
		{
			return hf.ComputeHash(data);
		}

		private static byte[] ToBytes(ulong value, int bitLength)
		{
			if (bitLength <= 0 || bitLength > 64)
			{
				throw new ArgumentOutOfRangeException("bitLength", "bitLength but be in the range [1, 64].");
			}

			value &= ulong.MaxValue >> (64 - bitLength);

			var valueBytes = new byte[(bitLength + 7) / 8];

			for (int x = 0; x < valueBytes.Length; ++x)
			{
				valueBytes[x] = (byte)value;
				value >>= 8;
			}

			return valueBytes;
		}
	}
}