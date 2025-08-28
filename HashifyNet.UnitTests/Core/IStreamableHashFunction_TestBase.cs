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
using HashifyNet.UnitTests.Mocks;

namespace HashifyNet.UnitTests
{
	public abstract class IStreamableHashFunction_TestBase<TStreamableHashFunction, CName>
		: IHashFunction_TestBase<TStreamableHashFunction, CName>
		where TStreamableHashFunction : IStreamableHashFunction<CName> where CName : IHashConfig<CName>
	{
		[Fact]
		public async Task IStreamableHashFunction_ComputeHashAsync_Stream_Seekable_MatchesKnownValues()
		{
			foreach (var knownValue in KnownValues)
			{
				var hf = CreateHashFunction(knownValue.HashSize);

				IHashConfig<CName> config = hf.Config;

				using (var ms = new SlowAsyncStream(new MemoryStream(knownValue.TestValue)))
				{
					var hashResults = await hf.ComputeHashAsync(ms);

					Assert.Equal(
						new HashValue(knownValue.ExpectedValue.Take((config.HashSizeInBits + 7) / 8), config.HashSizeInBits),
						hashResults);
				}
			}
		}

		[Fact]
		public async Task IStreamableHashFunction_ComputeHashAsync_Stream_Seekable_MatchesKnownValues_SlowStream()
		{
			foreach (var knownValue in KnownValues)
			{
				var hf = CreateHashFunction(knownValue.HashSize);

				IHashConfig<CName> config = hf.Config;

				using (var ms = new SlowAsyncStream(new MemoryStream(knownValue.TestValue)))
				{
					var hashResults = await hf.ComputeHashAsync(ms);

					Assert.Equal(
						new HashValue(knownValue.ExpectedValue.Take((config.HashSizeInBits + 7) / 8), config.HashSizeInBits),
						hashResults);
				}
			}
		}
	}
}