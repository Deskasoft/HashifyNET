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

namespace HashifyNet.Algorithms.MurmurHash
{
	/// <summary>
	/// Provides an implementation of the MurmurHash2 algorithm, a non-cryptographic hash function designed for high
	/// performance and uniform distribution. This implementation supports both  32-bit and 64-bit hash sizes.
	/// </summary>
	/// <remarks>MurmurHash2 is a fast, non-cryptographic hash function suitable for general-purpose hashing. It is
	/// not intended for cryptographic use cases. This implementation supports configurable hash sizes of 32 or 64 bits,
	/// as specified in the <see cref="IMurmurHash2Config"/> provided  during construction. The hash computation is based
	/// on the seed and hash size defined in the  configuration. <para> This class is internal and intended for use within
	/// the library. It validates the configuration  during initialization and ensures that only supported hash sizes are
	/// used. </para> <para> Thread Safety: This implementation is thread-safe as long as the provided configuration is 
	/// not modified externally after being passed to the constructor.</para></remarks>
	[HashAlgorithmImplementation(typeof(IMurmurHash2), typeof(MurmurHash2Config))]
	internal class MurmurHash2_Implementation
		: HashFunctionBase<IMurmurHash2Config>,
			IMurmurHash2
	{
		public override IMurmurHash2Config Config => _config.Clone();

		private const uint _mixConstant32 = 0x5bd1e995;
		private const ulong _mixConstant64 = 0xc6a4a7935bd1e995;
		private readonly IMurmurHash2Config _config;
		private static readonly IEnumerable<int> _validHashSizes = new HashSet<int>() { 32, 64 };

		public MurmurHash2_Implementation(IMurmurHash2Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (!_validHashSizes.Contains(_config.HashSizeInBits))
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be contained within MurmurHash2.ValidHashSizes.");
			}
		}

		protected override IHashValue ComputeHashInternal(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			switch (_config.HashSizeInBits)
			{
				case 32:
					return ComputeHash32(data, cancellationToken);

				case 64:
					return ComputeHash64(data, cancellationToken);

				default:
					throw new NotImplementedException();
			}
		}

		protected IHashValue ComputeHash32(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			var dataCount = data.Length;
			var remainderCount = dataCount % 4;

			uint hashValue = (uint)_config.Seed ^ (uint)dataCount;

			// Process 4-byte groups
			{
				var groupEndOffset = dataCount - remainderCount;

				for (var currentOffset = 0; currentOffset < groupEndOffset; currentOffset += 4)
				{
					uint k = Endianness.ToUInt32LittleEndian(data, currentOffset);

					k *= _mixConstant32;
					k ^= k >> 24;
					k *= _mixConstant32;

					hashValue *= _mixConstant32;
					hashValue ^= k;
				}
			}

			// Process remainder
			if (remainderCount > 0)
			{
				var remainderOffset = dataCount - remainderCount;

				switch (remainderCount)
				{
					case 3: hashValue ^= (uint)data[remainderOffset + 2] << 16; goto case 2;
					case 2: hashValue ^= (uint)data[remainderOffset + 1] << 8; goto case 1;
					case 1:
						hashValue ^= data[remainderOffset];
						break;
				}
				;

				hashValue *= _mixConstant32;
			}

			hashValue ^= hashValue >> 13;
			hashValue *= _mixConstant32;
			hashValue ^= hashValue >> 15;

			return new HashValue(
				ValueEndianness.LittleEndian,
				Endianness.GetBytesLittleEndian(hashValue),
				32);
		}

		protected IHashValue ComputeHash64(ReadOnlySpan<byte> data, CancellationToken cancellationToken)
		{
			var dataCount = data.Length;
			var remainderCount = dataCount % 8;

			ulong hashValue = (ulong)_config.Seed ^ ((ulong)dataCount * _mixConstant64);

			// Process 8-byte groups
			{
				var groupEndOffset = dataCount - remainderCount;

				for (var currentOffset = 0; currentOffset < groupEndOffset; currentOffset += 8)
				{
					ulong k = Endianness.ToUInt64LittleEndian(data, currentOffset);

					k *= _mixConstant64;
					k ^= k >> 47;
					k *= _mixConstant64;

					hashValue ^= k;
					hashValue *= _mixConstant64;
				}
			}

			// Process remainder
			if (remainderCount > 0)
			{
				var remainderOffset = dataCount - remainderCount;

				switch (remainderCount)
				{
					case 7: hashValue ^= (ulong)data[remainderOffset + 6] << 48; goto case 6;
					case 6: hashValue ^= (ulong)data[remainderOffset + 5] << 40; goto case 5;
					case 5: hashValue ^= (ulong)data[remainderOffset + 4] << 32; goto case 4;
					case 4:
						hashValue ^= Endianness.ToUInt32LittleEndian(data, remainderOffset);
						break;

					case 3: hashValue ^= (ulong)data[remainderOffset + 2] << 16; goto case 2;
					case 2: hashValue ^= (ulong)data[remainderOffset + 1] << 8; goto case 1;
					case 1:
						hashValue ^= data[remainderOffset];
						break;
				}
				;

				hashValue *= _mixConstant64;
			}

			hashValue ^= hashValue >> 47;
			hashValue *= _mixConstant64;
			hashValue ^= hashValue >> 47;

			return new HashValue(
				ValueEndianness.LittleEndian,
				Endianness.GetBytesLittleEndian(hashValue),
				64);
		}
	}
}