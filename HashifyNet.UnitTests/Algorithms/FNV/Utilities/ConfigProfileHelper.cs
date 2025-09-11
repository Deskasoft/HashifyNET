// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the ""Software""), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
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

namespace HashifyNet.UnitTests.Algorithms.FNV.Utilities
{
	internal static class ConfigProfileHelper
	{
		public static IFNVConfig GetProfile(int hashSizeInBits)
		{
			switch (hashSizeInBits)
			{
				case 32:
					return new FNVConfigProfile32Bits();
				case 64:
					return new FNVConfigProfile64Bits();
				case 128:
					return new FNVConfigProfile128Bits();
				case 256:
					return new FNVConfigProfile256Bits();
				case 512:
					return new FNVConfigProfile512Bits();
				case 1024:
					return new FNVConfigProfile1024Bits();
				default:
					throw new ArgumentOutOfRangeException(nameof(hashSizeInBits), "Only 32, 64, 128, 256, 512, and 1024 are valid hash sizes for FNV.");
			}
		}
	}
}
