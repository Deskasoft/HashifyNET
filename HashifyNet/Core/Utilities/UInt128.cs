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

#if NETSTANDARD2_0
namespace HashifyNet.Core.Utilities
{
	internal readonly struct UInt128
	{
		public ulong Lower { get; }
		public ulong Upper { get; }

		public UInt128(ulong upper, ulong lower)
		{
			Lower = lower;
			Upper = upper;
		}

		public static UInt128 operator +(UInt128 a, UInt128 b)
		{
			var carryOver = 0UL;
			var lowResult = unchecked(a.Lower + b.Lower);

			if (lowResult < a.Lower)
			{
				carryOver = 1UL;
			}

			return new UInt128(a.Upper + b.Upper + carryOver, lowResult);
		}
	}
}
#endif // NETSTANDARD2_0