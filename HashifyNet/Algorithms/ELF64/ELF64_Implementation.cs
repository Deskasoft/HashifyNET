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

namespace HashifyNet.Algorithms.ELF64
{
	/// <summary>
	/// Provides an implementation of the ELF64 hash function, a 32-bit non-cryptographic hash algorithm.
	/// </summary>
	/// <remarks>This class implements the ELF64 hash function, which is commonly used for hashing purposes in
	/// non-cryptographic scenarios. The hash size is fixed at 32 bits, and attempting to configure a different hash size
	/// will result in an exception.</remarks>
	[HashAlgorithmImplementation(typeof(IELF64), typeof(ELF64Config))]
	internal class ELF64_Implementation
		: StreamableHashFunctionBase<IELF64Config>,
			IELF64
	{
		public override IELF64Config Config => _config.Clone();

		private readonly IELF64Config _config;
		public ELF64_Implementation(IELF64Config config)
		{
			_config = config?.Clone() ?? throw new ArgumentNullException(nameof(config));
			if (config.HashSizeInBits != 32)
			{
				throw new ArgumentException($"Invalid hash size {config.HashSizeInBits}. Only 32 bits is supported.", nameof(config));
			}
		}

		public override IBlockTransformer CreateBlockTransformer() =>
			new BlockTransformer();

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private uint _hashValue;

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._hashValue = _hashValue;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				var tempHashValue = _hashValue;

				for (var currentOffset = 0; currentOffset < data.Length; ++currentOffset)
				{
					tempHashValue <<= 4;
					tempHashValue += data[currentOffset];

					var tmp = tempHashValue & 0xF0000000;

					if (tmp != 0)
					{
						tempHashValue ^= tmp >> 24;
					}

					tempHashValue &= 0x0FFFFFFF;
				}

				_hashValue = tempHashValue;
			}
			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				return new HashValue(
					ValueEndianness.LittleEndian,
					Endianness.GetBytesLittleEndian(_hashValue),
					32);
			}
		}
	}
}