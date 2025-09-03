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

using HashifyNet.Core;
using HashifyNet.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace HashifyNet.Algorithms.Jenkins
{
	/// <summary>
	/// Provides an implementation of the Jenkins Lookup3 hash algorithm, supporting both 32-bit and 64-bit hash sizes.
	/// </summary>
	/// <remarks>This class implements the Jenkins Lookup3 hash algorithm, which is designed for fast and efficient
	/// hashing of byte arrays. It supports configurable hash sizes of 32 or 64 bits, and allows for the use of seed values
	/// to produce different hash outputs. The implementation is optimized for performance and is suitable for scenarios
	/// such as hash-based data structures or checksum calculations.</remarks>
	[HashAlgorithmImplementation(typeof(IJenkinsLookup3), typeof(JenkinsLookup3Config))]
	internal class JenkinsLookup3_Implementation
		: HashFunctionBase<IJenkinsLookup3Config>,
			IJenkinsLookup3
	{
		public override IJenkinsLookup3Config Config => _config.Clone();

		private readonly IJenkinsLookup3Config _config;
		private static readonly IEnumerable<int> _validHashSizes = new HashSet<int>() { 32, 64 };

		public JenkinsLookup3_Implementation(IJenkinsLookup3Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (!_validHashSizes.Contains(_config.HashSizeInBits))
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within JenkinsLookup3.ValidHashSizes.");
			}
		}

		protected override IHashValue ComputeHashInternal(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			uint a = 0xdeadbeef + (uint)data.Count + (uint)_config.Seed;
			uint b = a;
			uint c = a;

			if (_config.HashSizeInBits == 64)
			{
				c += (uint)_config.Seed2;
			}

			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var remainderCount = dataCount % 12;
			{
				if (remainderCount == 0 && dataCount > 0)
				{
					remainderCount = 12;
				}
			}

			var remainderOffset = dataOffset + dataCount - remainderCount;

			// Main group processing
			int currentOffset = dataOffset;
			{
				while (currentOffset < remainderOffset)
				{
					a += Endianness.ToUInt32LittleEndian(dataArray, currentOffset);
					b += Endianness.ToUInt32LittleEndian(dataArray, currentOffset + 4);
					c += Endianness.ToUInt32LittleEndian(dataArray, currentOffset + 8);

					Mix(ref a, ref b, ref c);

					currentOffset += 12;
				}
			}

			// Remainder processing
			{
				Debug.Assert(remainderCount >= 0);
				Debug.Assert(remainderCount <= 12);

				switch (remainderCount)
				{
					case 12:
						c += Endianness.ToUInt32LittleEndian(dataArray, currentOffset + 8);
						goto case 8;

					case 11: c += (uint)dataArray[currentOffset + 10] << 16; goto case 10;
					case 10: c += (uint)dataArray[currentOffset + 9] << 8; goto case 9;
					case 9: c += dataArray[currentOffset + 8]; goto case 8;

					case 8:
						b += Endianness.ToUInt32LittleEndian(dataArray, currentOffset + 4);
						goto case 4;

					case 7: b += (uint)dataArray[currentOffset + 6] << 16; goto case 6;
					case 6: b += (uint)dataArray[currentOffset + 5] << 8; goto case 5;
					case 5: b += dataArray[currentOffset + 4]; goto case 4;

					case 4:
						a += Endianness.ToUInt32LittleEndian(dataArray, currentOffset);

						Final(ref a, ref b, ref c);
						break;

					case 3: a += (uint)dataArray[currentOffset + 2] << 16; goto case 2;
					case 2: a += (uint)dataArray[currentOffset + 1] << 8; goto case 1;
					case 1:
						a += dataArray[currentOffset];

						Final(ref a, ref b, ref c);
						break;
				}
			}

			byte[] hash;

			switch (_config.HashSizeInBits)
			{
				case 32:
					hash = Endianness.GetBytesLittleEndian(c);
					break;

				case 64:
					hash = Endianness.GetBytesLittleEndian((((ulong)b) << 32) | c);
					break;

				default:
					throw new NotImplementedException();
			}

			return new HashValue(hash, _config.HashSizeInBits);
		}

		private void Mix(ref uint a, ref uint b, ref uint c)
		{
			a -= c; a ^= RotateLeft(c, 4); c += b;
			b -= a; b ^= RotateLeft(a, 6); a += c;
			c -= b; c ^= RotateLeft(b, 8); b += a;

			a -= c; a ^= RotateLeft(c, 16); c += b;
			b -= a; b ^= RotateLeft(a, 19); a += c;
			c -= b; c ^= RotateLeft(b, 4); b += a;
		}

		private void Final(ref uint a, ref uint b, ref uint c)
		{
			c ^= b; c -= RotateLeft(b, 14);
			a ^= c; a -= RotateLeft(c, 11);
			b ^= a; b -= RotateLeft(a, 25);

			c ^= b; c -= RotateLeft(b, 16);
			a ^= c; a -= RotateLeft(c, 4);
			b ^= a; b -= RotateLeft(a, 14);

			c ^= b; c -= RotateLeft(b, 24);
		}
		private static uint RotateLeft(uint operand, int shiftCount)
		{
			shiftCount &= 0x1f;

			return
				(operand << shiftCount) |
				(operand >> (32 - shiftCount));
		}
	}
}
