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

#if NET8_0_OR_GREATER
using System;
#endif

using System.Runtime.CompilerServices;

namespace HashifyNet.Algorithms.T1HA
{
	internal static class T1HAGlobals
	{
		public const uint PRIME32_0 = 0x92D78269U;
		public const uint PRIME32_1 = 0xCA9B4735U;
		public const uint PRIME32_2 = 0xA4ABA1C3U;
		public const uint PRIME32_3 = 0xF6499843U;
		public const uint PRIME32_4 = 0x86F0FD61U;
		public const uint PRIME32_5 = 0xCA2DA6FBU;
		public const uint PRIME32_6 = 0xC4BB3575U;

		public const ulong PRIME_0 = 0xEC99BF0D8372CAABUL;
		public const ulong PRIME_1 = 0x82434FE90EDCEF39UL;
		public const ulong PRIME_2 = 0xD4F06DB99D67BE4BUL;
		public const ulong PRIME_3 = 0xBD9CACC22C6E9571UL;
		public const ulong PRIME_4 = 0x9C06FAF4D023E3ABUL;
		public const ulong PRIME_5 = 0xC060724A8424F345UL;
		public const ulong PRIME_6 = 0xCB5AF53AE3AAAC31UL;


#if true
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong BigMul_ref(ulong a, ulong b, out ulong low)
		{
			unchecked
			{
				uint al = (uint)a;
				uint ah = (uint)(a >> 32);
				uint bl = (uint)b;
				uint bh = (uint)(b >> 32);

				ulong mull = ((ulong)al) * bl;
				ulong t = ((ulong)ah) * bl + (mull >> 32);
				ulong tl = ((ulong)al) * bh + (uint)t;

				low = tl << 32 | (uint)mull;

				return ((ulong)ah) * bh + (t >> 32) + (tl >> 32);
			}
		}
#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BigMul128(ulong x, ulong y, out ulong lo, out ulong hi)
		{
#if NET8_0_OR_GREATER
			hi = Math.BigMul(x, y, out lo);
#else
			hi = BigMul_ref(x, y, out lo);
#endif
		}
	}
}
