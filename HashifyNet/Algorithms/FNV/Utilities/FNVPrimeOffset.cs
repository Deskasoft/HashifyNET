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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace HashifyNet.Algorithms.FNV.Utilities
{
	internal sealed class FNVPrimeOffset
	{
		public IReadOnlyList<uint> Prime { get; }
		public IReadOnlyList<uint> Offset { get; }

		private static readonly ConcurrentDictionary<(BigInteger, int), IReadOnlyList<uint>> _calculatedUintArrays =
			new ConcurrentDictionary<(BigInteger, int), IReadOnlyList<uint>>();

		private FNVPrimeOffset(IReadOnlyList<uint> prime, IReadOnlyList<uint> offset)
		{
			Debug.Assert(prime != null);
			Debug.Assert(offset != null);

			Prime = prime;
			Offset = offset;
		}

		public static FNVPrimeOffset Create(int bitSize, BigInteger prime, BigInteger offset)
		{
			if (bitSize <= 0 || bitSize % 32 != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(bitSize), $"{nameof(bitSize)} must be a positive a multiple of 32.");
			}

			if (prime <= BigInteger.Zero)
			{
				throw new ArgumentOutOfRangeException(nameof(prime), $"{nameof(prime)} must greater than zero.");
			}

			if (offset <= BigInteger.Zero)
			{
				throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must greater than zero.");
			}

			return new FNVPrimeOffset(
				_calculatedUintArrays.GetOrAdd((prime, bitSize), ToUInt32Array),
				_calculatedUintArrays.GetOrAdd((offset, bitSize), ToUInt32Array));
		}

		private static IReadOnlyList<uint> ToUInt32Array((BigInteger, int) tuple) =>
			ToUInt32Array(tuple.Item1, tuple.Item2);

		private static IReadOnlyList<uint> ToUInt32Array(BigInteger value, int bitSize)
		{
			Debug.Assert(bitSize > 0);
			Debug.Assert(bitSize % 32 == 0);

			var uint32Values = new uint[bitSize / 32];
			var bigIntegerBytes = value.ToByteArray();

			var copyLength = uint32Values.Length * 4;

			if (bigIntegerBytes.Length < copyLength)
			{
				copyLength = bigIntegerBytes.Length;
			}

			Buffer.BlockCopy(
				bigIntegerBytes, 0,
				uint32Values, 0,
				copyLength);

			return uint32Values;
		}
	}
}
