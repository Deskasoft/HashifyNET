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

namespace HashifyNet.UnitTests.Utilities
{
	internal sealed class TestConstants
	{
		// Constant values available for KnownValues to use.
		public static readonly byte[] Empty = new byte[0];
		public static readonly byte[] FooBar = "foobar".ToBytes();

		public static readonly byte[] LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.  Ut ornare aliquam mauris, at volutpat massa.  Phasellus pulvinar purus eu venenatis commodo.".ToBytes();

		public static readonly byte[] QuickBrownFox = "The quick brown fox jumps over the lazy dog".ToBytes();

		public static readonly byte[] RandomShort = "55d0e01ec669dc69".HexToBytes();
		public static readonly byte[] RandomLong = "1122eeba86d52989b26b0efd2be8d091d3ad307b771ff8d1208104f9aa40b12ab057a0d78656ba037e475178c159bf3ee64dcd279610d64bb7888a97211884c7a894378263135124720ef6ef560da6c85fb491cb732b331e89bcb00e7daef271e127483e91b189ceeaf2f6711394e2eca07fb4db62c5a8fd8195ae3b39da63".HexToBytes();
	}

}
