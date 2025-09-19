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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace HashifyNet.Algorithms.CRC
{
	/// <summary>
	/// Provides an implementation of a cyclic redundancy check (CRC) hash function based on the specified configuration.
	/// </summary>
	/// <remarks>This class supports CRC hash functions with configurable parameters such as polynomial, initial
	/// value, reflection settings, and XOR output. It is designed to handle CRC calculations for streams of data
	/// efficiently and supports hash sizes between 1 and 64 bits.</remarks>
	[HashAlgorithmImplementation(typeof(ICRC), typeof(CRCConfig))]
	internal class CRC_Implementation
		: StreamableHashFunctionBase<ICRCConfig>,
			ICRC
	{
		public override ICRCConfig Config => _config.Clone();

		private readonly ICRCConfig _config;
		private static readonly ConcurrentDictionary<(int, ulong, bool), IReadOnlyList<ulong>> _dataDivisionTableCache =
			new ConcurrentDictionary<(int, ulong, bool), IReadOnlyList<ulong>>();

		public CRC_Implementation(ICRCConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits <= 0 || _config.HashSizeInBits > 64)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be >= 1 and <= 64");
			}
		}

		public override IBlockTransformer CreateBlockTransformer() =>
			new BlockTransformer(_config);

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private int _hashSizeInBits;
			private IReadOnlyList<ulong> _crcTable;
			private int _mostSignificantShift;
			private bool _reflectIn;
			private bool _reflectOut;
			private ulong _xOrOut;
			private ulong _hashValue;

			public BlockTransformer()
				: base()
			{
			}

			public BlockTransformer(ICRCConfig config)
				: this()
			{
				_hashSizeInBits = config.HashSizeInBits;
				_crcTable = GetDataDivisionTable(_hashSizeInBits, (ulong)config.Polynomial, config.ReflectIn);

				// _mostSignificantShift
				{
					// How much hash must be right-shifted to get the most significant byte (HashSize >= 8) or bit (HashSize < 8)
					if (_hashSizeInBits < 8)
					{
						_mostSignificantShift = _hashSizeInBits - 1;
					}
					else
					{
						_mostSignificantShift = _hashSizeInBits - 8;
					}
				}

				_reflectIn = config.ReflectIn;
				_reflectOut = config.ReflectOut;
				_xOrOut = (ulong)config.XOrOut;

				// _hashValue
				{
					var initialValue = (ulong)config.InitialValue;

					if (config.ReflectIn)
					{
						initialValue = ReflectBits(initialValue, _hashSizeInBits);
					}

					_hashValue = initialValue;
				}
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._hashSizeInBits = _hashSizeInBits;
				other._crcTable = _crcTable;
				other._mostSignificantShift = _mostSignificantShift;
				other._reflectIn = _reflectIn;
				other._reflectOut = _reflectOut;
				other._xOrOut = _xOrOut;

				other._hashValue = _hashValue;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				var tempHashValue = _hashValue;

				var tempHashSizeInBits = _hashSizeInBits;
				var tempReflectIn = _reflectIn;
				var tempCrcTable = _crcTable;
				var tempMostSignificantShift = _mostSignificantShift;

				for (var currentOffset = 0; currentOffset < data.Length; ++currentOffset)
				{
					if (tempHashSizeInBits >= 8)
					{
						// Process per byte, treating hash differently based on input endianness
						if (tempReflectIn)
						{
							tempHashValue = (tempHashValue >> 8) ^ tempCrcTable[(byte)tempHashValue ^ data[currentOffset]];
						}
						else
						{
							tempHashValue = (tempHashValue << 8) ^ tempCrcTable[((byte)(tempHashValue >> tempMostSignificantShift)) ^ data[currentOffset]];
						}
					}
					else
					{
						// Process per bit, treating hash differently based on input endianness
						for (var currentBit = 0; currentBit < 8; ++currentBit)
						{
							if (tempReflectIn)
							{
								tempHashValue = (tempHashValue >> 1) ^ tempCrcTable[(byte)(tempHashValue & 1) ^ ((byte)(data[currentOffset] >> currentBit) & 1)];
							}
							else
							{
								tempHashValue = (tempHashValue << 1) ^ tempCrcTable[(byte)((tempHashValue >> tempMostSignificantShift) & 1) ^ ((byte)(data[currentOffset] >> (7 - currentBit)) & 1)];
							}
						}
					}
				}

				_hashValue = tempHashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				var finalHashValue = _hashValue;
				if (_reflectIn ^ _reflectOut)
				{
					finalHashValue = ReflectBits(finalHashValue, _hashSizeInBits);
				}

				finalHashValue ^= _xOrOut;

				ValueEndianness resultEndianness = _reflectOut ? ValueEndianness.LittleEndian : ValueEndianness.BigEndian;

				return new HashValue(
					resultEndianness,
					ToBytes(finalHashValue, _hashSizeInBits, resultEndianness),
					_hashSizeInBits);
			}

			/// <summary>
			/// Calculates the data-division table for the CRC parameters provided.
			/// </summary>
			/// <param name="hashSizeInBits">Length of the produced CRC value, in bits.</param>
			/// <param name="polynomial">Divisor to use when calculating the CRC.</param>
			/// <param name="reflectIn">If true, the CRC calculation processes input as big endian bit order.</param>
			/// <returns>
			/// Array of UInt64 values that allows a CRC implementation to look up the result
			/// of dividing the index (data) by the polynomial.
			/// </returns>
			/// <remarks>
			/// Resulting array contains 256 items if settings.Bits &gt;= 8, or 2 items if settings.Bits &lt; 8.
			/// The table accounts for reflecting the index bits to fix the input endianness,
			/// but it is not possible completely account for the output endianness if the CRC is mixed-endianness.
			/// </remarks>
			private static IReadOnlyList<ulong> GetDataDivisionTable(int hashSizeInBits, ulong polynomial, bool reflectIn)
			{
				return _dataDivisionTableCache.GetOrAdd(
					(hashSizeInBits, polynomial, reflectIn),
					GetDataDivisionTableInternal);
			}

			private static IReadOnlyList<ulong> GetDataDivisionTableInternal((int, ulong, bool) cacheKey)
			{
				var hashSizeInBits = cacheKey.Item1;
				var polynomial = cacheKey.Item2;
				var reflectIn = cacheKey.Item3;
				var perBitCount = 8;

				if (hashSizeInBits < 8)
				{
					perBitCount = 1;
				}

				var crcTable = new ulong[1 << perBitCount];
				var mostSignificantBit = 1UL << (hashSizeInBits - 1);

				for (uint x = 0; x < crcTable.Length; ++x)
				{
					ulong curValue = x;

					if (perBitCount > 1 && reflectIn)
					{
						curValue = ReflectBits(curValue, perBitCount);
					}

					curValue <<= hashSizeInBits - perBitCount;

					for (int y = 0; y < perBitCount; ++y)
					{
						if ((curValue & mostSignificantBit) > 0UL)
						{
							curValue = (curValue << 1) ^ polynomial;
						}
						else
						{
							curValue <<= 1;
						}
					}

					if (reflectIn)
					{
						curValue = ReflectBits(curValue, hashSizeInBits);
					}

					curValue &= ulong.MaxValue >> (64 - hashSizeInBits);

					crcTable[x] = curValue;
				}

				return crcTable;
			}

			private static byte[] ToBytes(ulong value, int bitLength, ValueEndianness endianness)
			{
				value &= ulong.MaxValue >> (64 - bitLength);

				var byteCount = (bitLength + 7) / 8;
				var valueBytes = new byte[byteCount];

				if (endianness == ValueEndianness.LittleEndian)
				{
					for (int x = 0; x < valueBytes.Length; ++x)
					{
						valueBytes[x] = (byte)value;
						value >>= 8;
					}
				}
				else // BigEndian
				{
					for (int x = valueBytes.Length - 1; x >= 0; --x)
					{
						valueBytes[x] = (byte)value;
						value >>= 8;
					}
				}

				if (bitLength % 8 != 0)
				{
					return valueBytes;
				}

				return valueBytes;
			}

			private static ulong ReflectBits(ulong value, int bitLength)
			{
				ulong reflectedValue = 0UL;

				for (int x = 0; x < bitLength; ++x)
				{
					reflectedValue <<= 1;

					reflectedValue |= value & 1;

					value >>= 1;
				}

				return reflectedValue;
			}
		}
	}
}