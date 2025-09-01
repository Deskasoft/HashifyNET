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
using System.Collections.Generic;
using System.Linq;

namespace HashifyNet.Core.Utilities
{
	internal static class ArrayHelpers
	{
		public static void ZeroFill(byte[] array)
		{
			if (array == null)
			{
				return;
			}

			Array.Clear(array, 0, array.Length);
		}

		public static void ZeroFill(ArraySegment<byte> array)
		{
			ZeroFill(array.Array);
		}
		
		/// <summary>
		/// Coerces the given <paramref name="hash"/> to a byte array with <paramref name="bitLength"/> significant bits.
		/// </summary>
		/// <param name="hash">The hash.</param>
		/// <param name="bitLength">Length of the hash, in bits.</param>
		/// <returns>A byte array that has been coerced to the proper length.</returns>
		public static byte[] CoerceToArray(IEnumerable<byte> hash, int bitLength)
		{
			var byteLength = (bitLength + 7) / 8;

			if ((bitLength % 8) == 0)
			{
				if (hash is IReadOnlyCollection<byte> hashByteCollection)
				{
					if (hashByteCollection.Count == byteLength)
					{
						return hash.ToArray();
					}
				}

				if (hash is byte[] hashByteArray)
				{
					var newHashArray = new byte[byteLength];
					{
						Array.Copy(hashByteArray, newHashArray, Math.Min(byteLength, hashByteArray.Length));
					}

					return newHashArray;
				}
			}

			byte finalByteMask = (byte)((1 << (bitLength % 8)) - 1);
			{
				if (finalByteMask == 0)
				{
					finalByteMask = 255;
				}
			}

			var coercedArray = new byte[byteLength];

			var currentIndex = 0;
			var hashEnumerator = hash.GetEnumerator();

			while (currentIndex < byteLength && hashEnumerator.MoveNext())
			{
				if (currentIndex == (byteLength - 1))
				{
					coercedArray[currentIndex] = (byte)(hashEnumerator.Current & finalByteMask);
				}
				else
				{
					coercedArray[currentIndex] = hashEnumerator.Current;
				}

				currentIndex += 1;
			}

			return coercedArray;
		}
	}
}


