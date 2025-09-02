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

namespace HashifyNet.Algorithms.MurmurHash
{
	/// <summary>
	/// Provides an implementation of the MurmurHash1 algorithm, a non-cryptographic hash function  designed for high
	/// performance and uniform distribution of hash values.
	/// </summary>
	/// <remarks>This class implements the MurmurHash1 algorithm, which is suitable for scenarios where  fast
	/// hashing is required, such as hash-based data structures or checksums. It is not  intended for cryptographic
	/// purposes and should not be used in security-sensitive contexts.  The hash computation is based on the seed value
	/// specified in the configuration, and the  algorithm processes input data in 4-byte chunks, with special handling for
	/// any remaining bytes.</remarks>
	[HashAlgorithmImplementation(typeof(IMurmurHash1), typeof(MurmurHash1Config))]
	internal class MurmurHash1_Implementation
		: HashFunctionBase<IMurmurHash1Config>,
			IMurmurHash1
	{
		public override IMurmurHash1Config Config => _config.Clone();

		private const uint _m = 0XC6A4A793;
		private readonly IMurmurHash1Config _config;

		public MurmurHash1_Implementation(IMurmurHash1Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();
		}

		protected override IHashValue ComputeHashInternal(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			var dataArray = data.Array;
			var dataOffset = data.Offset;
			var dataCount = data.Count;

			var endOffset = dataOffset + dataCount;
			var remainderCount = dataCount % 4;

			uint hashValue = _config.Seed ^ ((uint)dataCount * _m);

			// Process 4-byte groups
			{
				var groupEndOffset = endOffset - remainderCount;

				for (var currentOffset = dataOffset; currentOffset < groupEndOffset; currentOffset += 4)
				{
					hashValue += Endianness.ToUInt32LittleEndian(dataArray, currentOffset);
					hashValue *= _m;
					hashValue ^= hashValue >> 16;
				}
			}

			// Process remainder
			if (remainderCount > 0)
			{
				var remainderOffset = endOffset - remainderCount;

				switch (remainderCount)
				{
					case 3: hashValue += (uint)dataArray[remainderOffset + 2] << 16; goto case 2;
					case 2: hashValue += (uint)dataArray[remainderOffset + 1] << 8; goto case 1;
					case 1:
						hashValue += dataArray[remainderOffset];
						break;
				}
				;

				hashValue *= _m;
				hashValue ^= hashValue >> 16;
			}


			hashValue *= _m;
			hashValue ^= hashValue >> 10;
			hashValue *= _m;
			hashValue ^= hashValue >> 17;

			return new HashValue(
				Endianness.GetBytesLittleEndian(hashValue),
				32);
		}
	}
}
