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
using System.Linq;
using System.Threading;

namespace HashifyNet.Algorithms.Pearson
{
	/// <summary>
	/// Provides an implementation of the Pearson hashing algorithm, a simple and fast non-cryptographic hash function.
	/// </summary>
	/// <remarks>This class is designed to compute hash values based on the Pearson hashing algorithm, which uses a
	/// permutation table and supports configurable hash sizes. It is intended for use in scenarios where a lightweight,
	/// non-cryptographic hash function is sufficient. The implementation ensures that the configuration provided is valid,
	/// including constraints on the permutation table and hash size.</remarks>
	[HashAlgorithmImplementation(typeof(IPearson), typeof(PearsonConfig))]
	internal class Pearson_Implementation
		: StreamableHashFunctionBase<IPearsonConfig>,
			IPearson
	{
		public override IPearsonConfig Config => _config.Clone();

		private readonly IPearsonConfig _config;
		public Pearson_Implementation(IPearsonConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();


			if (_config.Table == null)
			{
				throw new ArgumentException($"{nameof(config)}.{nameof(config.Table)} must be non-null.", $"{nameof(config)}.{nameof(config.Table)}");
			}

			if (_config.Table.Count != 256 || _config.Table.Distinct().Count() != 256)
			{
				throw new ArgumentException($"{nameof(config)}.{nameof(config.Table)} must be a permutation of [0, 255].", $"{nameof(config)}.{nameof(config.Table)}");
			}

			if (_config.HashSizeInBits <= 0 || _config.HashSizeInBits % 8 != 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be a positive integer that is divisible by 8.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(_config);
		}

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private IReadOnlyList<byte> _table;

			private bool _anyBytesProcessed;
			private byte[] _hashValue;
			public BlockTransformer()
			{

			}

			public BlockTransformer(IPearsonConfig config)
				: this()
			{
				_table = config.Table;

				_anyBytesProcessed = false;
				_hashValue = new byte[config.HashSizeInBits / 8];
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._table = _table;

				other._anyBytesProcessed = false;

				// _hashValue
				{
					other._hashValue = new byte[_hashValue.Length];

					Array.Copy(_hashValue, other._hashValue, _hashValue.Length);
				}
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataCount = data.Count;
				var endOffset = data.Offset + dataCount;

				var tempHashValue = _hashValue;
				var tempAnyBytesProcessed = _anyBytesProcessed;

				var tempTable = _table;
				var tempHashValueLength = tempHashValue.Length;


				for (var currentOffset = data.Offset; currentOffset < endOffset; ++currentOffset)
				{
					for (int y = 0; y < tempHashValueLength; ++y)
					{
						if (tempAnyBytesProcessed)
						{
							tempHashValue[y] = tempTable[tempHashValue[y] ^ dataArray[currentOffset]];
						}
						else
						{
							tempHashValue[y] = tempTable[(dataArray[currentOffset] + y) & 0xff];
						}
					}

					tempAnyBytesProcessed = true;
				}

				_anyBytesProcessed = tempAnyBytesProcessed;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				return new HashValue(_hashValue, _hashValue.Length * 8);
			}
		}
	}
}