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
using System.Diagnostics;
using System.Threading;

namespace HashifyNet.Algorithms.Jenkins
{
	/// <summary>
	/// Provides an implementation of the Jenkins Lookup2 hash function, a non-cryptographic hash function designed for
	/// general-purpose hashing of byte sequences.
	/// </summary>
	/// <remarks>This class implements the Jenkins Lookup2 algorithm, which is optimized for speed and uniform
	/// distribution of hash values. It is suitable for use cases such as hash-based data structures (e.g., hash tables) or
	/// checksum calculations, but it is not intended for cryptographic purposes. The hash function processes input data
	/// in 12-byte blocks and uses a configurable seed value to initialize its internal state. The seed can be set via the
	/// <see cref="IJenkinsLookup2Config"/>  configuration object passed to the constructor.</remarks>
	[HashAlgorithmImplementation(typeof(IJenkinsLookup2), typeof(JenkinsLookup2Config))]
	internal class JenkinsLookup2_Implementation
		: StreamableHashFunctionBase<IJenkinsLookup2Config>,
			IJenkinsLookup2
	{
		public override IJenkinsLookup2Config Config => _config.Clone();

		private readonly IJenkinsLookup2Config _config;

		public JenkinsLookup2_Implementation(IJenkinsLookup2Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();
		}

		public override IBlockTransformer CreateBlockTransformer() =>
			new BlockTransformer(_config);

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private uint _a;
			private uint _b;
			private uint _c;
			private uint _bytesProcessed;

			public BlockTransformer()
				: base(inputBlockSize: 12)
			{
			}

			public BlockTransformer(IJenkinsLookup2Config config)
				: this()
			{
				_a = 0x9e3779b9;
				_b = 0x9e3779b9;
				_c = config.Seed;

				_bytesProcessed = 0;
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				other._a = _a;
				other._b = _b;
				other._c = _c;

				other._bytesProcessed = _bytesProcessed;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				Debug.Assert(data.Count % 12 == 0);

				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempA = _a;
				var tempB = _b;
				var tempC = _c;

				for (var currentOffset = data.Offset; currentOffset < endOffset; currentOffset += 12)
				{
					tempA += BitConverter.ToUInt32(dataArray, currentOffset);
					tempB += BitConverter.ToUInt32(dataArray, currentOffset + 4);
					tempC += BitConverter.ToUInt32(dataArray, currentOffset + 8);

					Mix(ref tempA, ref tempB, ref tempC);
				}

				_a = tempA;
				_b = tempB;
				_c = tempC;

				_bytesProcessed += (uint)dataCount;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var remainder = FinalizeInputBuffer;
				var remainderLength = (remainder?.Length).GetValueOrDefault();

				Debug.Assert(remainderLength >= 0);
				Debug.Assert(remainderLength < 12);

				var finalA = _a;
				var finalB = _b;
				var finalC = _c;

				// All the case statements fall through on purpose
				switch (remainderLength)
				{
					case 11: finalC += (uint)remainder[10] << 24; goto case 10;
					case 10: finalC += (uint)remainder[9] << 16; goto case 9;
					case 9: finalC += (uint)remainder[8] << 8; goto case 8;
					// the first byte of c is reserved for the length

					case 8:
						finalB += BitConverter.ToUInt32(remainder, 4);
						goto case 4;

					case 7: finalB += (uint)remainder[6] << 16; goto case 6;
					case 6: finalB += (uint)remainder[5] << 8; goto case 5;
					case 5: finalB += remainder[4]; goto case 4;

					case 4:
						finalA += BitConverter.ToUInt32(remainder, 0);
						break;

					case 3: finalA += (uint)remainder[2] << 16; goto case 2;
					case 2: finalA += (uint)remainder[1] << 8; goto case 1;
					case 1:
						finalA += remainder[0];
						break;
				}

				finalC += _bytesProcessed + (uint)remainderLength;

				Mix(ref finalA, ref finalB, ref finalC);

				return new HashValue(
					BitConverter.GetBytes(finalC),
					32);
			}

			private static void Mix(ref uint a, ref uint b, ref uint c)
			{
				a -= b; a -= c; a ^= c >> 13;
				b -= c; b -= a; b ^= a << 8;
				c -= a; c -= b; c ^= b >> 13;

				a -= b; a -= c; a ^= c >> 12;
				b -= c; b -= a; b ^= a << 16;
				c -= a; c -= b; c ^= b >> 5;

				a -= b; a -= c; a ^= c >> 3;
				b -= c; b -= a; b ^= a << 10;
				c -= a; c -= b; c ^= b >> 15;
			}
		}
	}
}