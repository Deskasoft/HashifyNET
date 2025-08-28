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
using System.Threading;

namespace HashifyNet.Algorithms.Adler32
{
	/// <summary>
	/// Implementation of the Adler32 checksum algorithm.
	/// Adler32 is a fast checksum algorithm, often used in conjunction with zlib.
	/// It is not a cryptographically secure hash function.
	/// </summary>
	[HashAlgorithmImplementation(typeof(IAdler32), typeof(Adler32Config))]
	internal partial class Adler32_Implementation
		: StreamableHashFunctionBase<IAdler32Config>,
		  IAdler32
	{
		public override IAdler32Config Config => _config.Clone();

		private readonly IAdler32Config _config;

		public Adler32_Implementation(IAdler32Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits != 32)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 32.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer();
		}

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			// The largest prime number smaller than 65536
			private const uint MOD_ADLER = 65521;
			// NMAX is the largest number of bytes that can be processed without s2 overflowing.
			private const int NMAX = 5552;

			private uint _s1;
			private uint _s2;

			public BlockTransformer()
				: base(inputBlockSize: NMAX) // Process data in chunks to prevent overflow
			{
				// Initial values
				_s1 = 1;
				_s2 = 0;
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);
				other._s1 = _s1;
				other._s2 = _s2;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				uint s1 = _s1;
				uint s2 = _s2;

				var dataArray = data.Array;
				var endOffset = data.Offset + data.Count;

				for (int currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					s1 = (s1 + dataArray[currentOffset]) % MOD_ADLER;
					s2 = (s2 + s1) % MOD_ADLER;
				}

				_s1 = s1;
				_s2 = s2;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var remainder = FinalizeInputBuffer;
				if (remainder != null && remainder.Length > 0)
				{
					// Process the final remaining bytes
					TransformByteGroupsInternal(new ArraySegment<byte>(remainder));
				}

				cancellationToken.ThrowIfCancellationRequested();

				// Combine s2 and s1 into the final 32-bit checksum
				uint finalValue = (_s2 << 16) | _s1;

				// Convert to a big-endian byte array
				var hashValueBytes = new byte[4];
				hashValueBytes[0] = (byte)(finalValue >> 24);
				hashValueBytes[1] = (byte)(finalValue >> 16);
				hashValueBytes[2] = (byte)(finalValue >> 8);
				hashValueBytes[3] = (byte)finalValue;

				return new HashValue(hashValueBytes, 32);
			}
		}
	}
}