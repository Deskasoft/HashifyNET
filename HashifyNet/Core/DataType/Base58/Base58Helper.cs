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

using HashifyNet.Algorithms.Blake2;
using HashifyNet.Algorithms.SHA256;
using HashifyNet.Core.Utilities;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace HashifyNet
{
	internal static class Base58Helper
	{
		public static string AsBase58String(byte[] data, Base58Variant variant)
		{
			_ = data ?? throw new ArgumentNullException(nameof(data));

			string alphabet = GetAlphabet(variant);
			char leadingZeroChar = alphabet[0];

			byte[] lilEndianData = data.Reverse().ToArray();
			byte[] dataForBigInt = lilEndianData;
			if (dataForBigInt.Length > 0 && (dataForBigInt[dataForBigInt.Length - 1] & 0x80) != 0)
			{
				dataForBigInt = new byte[lilEndianData.Length + 1];
				Array.Copy(lilEndianData, 0, dataForBigInt, 0, lilEndianData.Length);
				dataForBigInt[dataForBigInt.Length - 1] = 0;
			}

			var intData = new BigInteger(dataForBigInt);
			var builder = new StringBuilder();

			if (intData == 0)
			{
				builder.Append(leadingZeroChar);
			}

			while (intData > 0)
			{
				intData = BigInteger.DivRem(intData, 58, out BigInteger remainder);
				builder.Insert(0, alphabet[(int)remainder]);
			}

			int leadingZeros = 0;
			for (int i = 0; i < data.Length && data[i] == 0; i++)
			{
				leadingZeros++;
			}

			for (int i = 0; i < leadingZeros; i++)
			{
				builder.Insert(0, leadingZeroChar);
			}

			return builder.ToString();
		}

		private static string GetAlphabet(Base58Variant variant)
		{
			switch (variant)
			{
				case Base58Variant.Bitcoin:
					return "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
				case Base58Variant.Flickr:
					return "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
				case Base58Variant.Ripple:
					return "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";
				default:
					throw new NotSupportedException($"Unsupported base58 variant '{variant}'.");
			}
		}
	}
}
