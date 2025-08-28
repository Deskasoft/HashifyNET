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

using System.Text;

namespace HashifyNet.UnitTests.Utilities
{
	public static class UtilityExtensions
	{
		/// <summary>Converts a hex string to byte array.</summary>
		/// <param name="hexString">String containing a hexadecimal value, [0-9a-fA-F _-] allowed.</param>
		/// <returns>Byte array of the binary representation of the hexString.</returns>
		public static byte[] HexToBytes(this string hexString)
		{
			bool reverse = hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase);

			if (reverse)
			{
				hexString = hexString.Substring(2);
			}

			var chars = hexString
				.Replace(" ", "")
				.Replace("-", "")
				.Replace("_", "")
				.ToCharArray();

			if (chars.Length % 2 == 1)
			{
				throw new ArgumentException("hexString's length must be divisible by 2 after removing spaces, underscores, and dashes.", "hexString");
			}

			var bytes = new byte[chars.Length / 2];

			for (int x = 0; x < chars.Length; ++x)
			{
				if (x % 2 == 0)
				{
					bytes[x / 2] = 0;
				}
				else
				{
					bytes[x / 2] <<= 4;
				}

				if (chars[x] >= '0' && chars[x] <= '9')
				{
					bytes[x / 2] |= (byte)(chars[x] - '0');
				}
				else if (chars[x] >= 'a' && chars[x] <= 'f')
				{
					bytes[x / 2] |= (byte)(chars[x] - 'a' + 10);
				}
				else if (chars[x] >= 'A' && chars[x] <= 'F')
				{
					bytes[x / 2] |= (byte)(chars[x] - 'A' + 10);
				}
				else
				{
					throw new ArgumentException("hexString contains an invalid character, only [0-9a-fA-F _-] expected", "hexString");
				}
			}

			if (reverse)
			{
				bytes = bytes.Reverse().ToArray();
			}

			return bytes;
		}

		/// <summary>Converts string to byte array.</summary>
		/// <param name="value">String to encode into bytes.</param>
		/// <returns>UTF-8 encoding of the string as a byte array.</returns>
		public static byte[] ToBytes(this string value)
		{
			return Encoding.UTF8.GetBytes(value);
		}
	}
}