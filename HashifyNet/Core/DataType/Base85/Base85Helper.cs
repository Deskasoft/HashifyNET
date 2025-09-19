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

using System;
using System.Text;

namespace HashifyNet
{
	internal static class Base85Helper
	{
		private const string Ascii85Alphabet = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstu";
		private const string Z85Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";
		private const string Rfc1924Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~";

		private static readonly uint[] Powers = { 52200625, 614125, 7225, 85, 1 };

		public static string AsBase85String(byte[] data, Base85Variant variant)
		{
			if (data == null || data.Length == 0)
			{
				return variant == Base85Variant.AdobeAscii85 ? "<~~>" : "";
			}

			var builder = new StringBuilder();
			string alphabet = GetAlphabet(variant);
			int dataIndex = 0;

			while (dataIndex + 3 < data.Length)
			{
				uint value = ((uint)data[dataIndex++] << 24) |
							 ((uint)data[dataIndex++] << 16) |
							 ((uint)data[dataIndex++] << 8) |
							 data[dataIndex++];

				if (value == 0 && (variant == Base85Variant.Ascii85 || variant == Base85Variant.AdobeAscii85))
				{
					builder.Append('z');
					continue;
				}

				for (int i = 0; i < 5; i++)
				{
					builder.Append(alphabet[(int)(value / Powers[i] % 85)]);
				}
			}

			int remainingBytes = data.Length - dataIndex;
			if (remainingBytes > 0)
			{
				uint value = 0;
				for (int i = 0; i < remainingBytes; i++)
				{
					value |= (uint)data[dataIndex + i] << (24 - (i * 8));
				}

				var finalChars = new char[5];
				for (int i = 0; i < 5; i++)
				{
					finalChars[i] = alphabet[(int)(value / Powers[i] % 85)];
				}

				for (int i = 0; i < remainingBytes + 1; i++)
				{
					builder.Append(finalChars[i]);
				}
			}

			if (variant == Base85Variant.AdobeAscii85)
			{
				return $"<~{builder.ToString()}~>";
			}

			return builder.ToString();
		}

		private static string GetAlphabet(Base85Variant variant)
		{
			switch (variant)
			{
				case Base85Variant.Ascii85:
				case Base85Variant.AdobeAscii85:
					return Ascii85Alphabet;
				case Base85Variant.Z85:
					return Z85Alphabet;
				case Base85Variant.Rfc1924:
					return Rfc1924Alphabet;
				default:
					throw new ArgumentException("Unsupported Base85 variant.", nameof(variant));
			}
		}
	}
}
