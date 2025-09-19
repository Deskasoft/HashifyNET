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

using HashifyNet.Algorithms.FNV.Utilities;
using HashifyNet.Core;
using HashifyNet.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace HashifyNet.Algorithms.FNV
{
	internal abstract class FNV1Base
		: StreamableHashFunctionBase<IFNVConfig>,
			IFNV
	{
		public override IFNVConfig Config => _config.Clone();

		protected readonly IFNVConfig _config;
		protected readonly FNVPrimeOffset _fnvPrimeOffset;

		protected FNV1Base(IFNVConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits <= 0 || _config.HashSizeInBits % 32 != 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be a positive a multiple of 32.");
			}

			if (_config.Prime <= BigInteger.Zero)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Prime)}", _config.Prime, $"{nameof(config)}.{nameof(config.Prime)} must be greater than zero.");
			}

			if (_config.Offset <= BigInteger.Zero)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Offset)}", _config.Offset, $"{nameof(config)}.{nameof(config.Offset)} must be greater than zero.");
			}

			_fnvPrimeOffset = FNVPrimeOffset.Create(_config.HashSizeInBits, _config.Prime, _config.Offset);
		}

		protected abstract class BlockTransformer_32BitBase<TSelf>
			: BlockTransformerBase<TSelf>
			where TSelf : BlockTransformer_32BitBase<TSelf>, new()
		{
			protected uint _prime;
			protected uint _hashValue;

			public BlockTransformer_32BitBase()
			{
			}

			public BlockTransformer_32BitBase(FNVPrimeOffset fnvPrimeOffset)
				: this()
			{
				_prime = fnvPrimeOffset.Prime[0];
				_hashValue = fnvPrimeOffset.Offset[0];
			}

			protected override void CopyStateTo(TSelf other)
			{
				base.CopyStateTo(other);
				other._prime = _prime;
				other._hashValue = _hashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken) =>
				new HashValue(ValueEndianness.LittleEndian, Endianness.GetBytesLittleEndian(_hashValue), 32);
		}

		protected abstract class BlockTransformer_64BitBase<TSelf>
			: BlockTransformerBase<TSelf>
			where TSelf : BlockTransformer_64BitBase<TSelf>, new()
		{
			protected ulong _prime;
			protected ulong _hashValue;
			public BlockTransformer_64BitBase()
			{
			}

			public BlockTransformer_64BitBase(FNVPrimeOffset fnvPrimeOffset)
				: this()
			{
				_prime = ((ulong)fnvPrimeOffset.Prime[1] << 32) | fnvPrimeOffset.Prime[0];
				_hashValue = ((ulong)fnvPrimeOffset.Offset[1] << 32) | fnvPrimeOffset.Offset[0];
			}

			protected override void CopyStateTo(TSelf other)
			{
				base.CopyStateTo(other);
				other._prime = _prime;
				other._hashValue = _hashValue;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken) =>
				new HashValue(ValueEndianness.LittleEndian, Endianness.GetBytesLittleEndian(_hashValue), 64);
		}

		protected abstract class BlockTransformer_ExtendedBase<TSelf>
			: BlockTransformerBase<TSelf>
			where TSelf : BlockTransformer_ExtendedBase<TSelf>, new()
		{
			protected uint[] _prime;
			protected uint[] _hashValue;
			protected int _hashSizeInBytes;

			public BlockTransformer_ExtendedBase()
			{
			}

			public BlockTransformer_ExtendedBase(FNVPrimeOffset fnvPrimeOffset)
				: this()
			{
				_prime = fnvPrimeOffset.Prime.ToArray();
				_hashValue = fnvPrimeOffset.Offset.ToArray();
				_hashSizeInBytes = _hashValue.Length * 4;
			}

			protected override void CopyStateTo(TSelf other)
			{
				base.CopyStateTo(other);

				other._prime = _prime;
				other._hashValue = _hashValue;
				other._hashSizeInBytes = _hashSizeInBytes;
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken) =>
				new HashValue(ValueEndianness.LittleEndian, UInt32ArrayToBytes(_hashValue), _hashSizeInBytes * 8);
		}

		/// <summary>
		/// Multiplies operand1 by operand2 as if both operand1 and operand2 were single large integers.
		/// </summary>
		/// <param name="operand1">Array of <see cref="uint"/> values to be multiplied.</param>
		/// <param name="operand2">Array of <see cref="uint"/> values to multiply by.</param>
		/// <param name="hashSizeInBytes">Hash size, in bytes, to truncate products at.</param>
		/// <returns></returns>
		protected static uint[] ExtendedMultiply(IReadOnlyList<uint> operand1, IReadOnlyList<uint> operand2, int hashSizeInBytes)
		{
			Debug.Assert(hashSizeInBytes % 4 == 0);

			// Temporary array to hold the results of 32-bit multiplication.
			var product = new uint[hashSizeInBytes / 4];

			// Bottom of equation
			for (int y = 0; y < operand2.Count; ++y)
			{
				// Skip multiplying things by zero
				if (operand2[y] == 0)
				{
					continue;
				}

				uint carryOver = 0;

				// Top of equation
				for (int x = 0; x < operand1.Count; ++x)
				{
					if (x + y >= product.Length)
					{
						break;
					}

					var productResult = product[x + y] + (((ulong)operand2[y]) * operand1[x]) + carryOver;
					product[x + y] = (uint)productResult;

					carryOver = (uint)(productResult >> 32);
				}
			}

			return product;
		}

		private static IEnumerable<byte> UInt32ArrayToBytes(IEnumerable<uint> values)
		{
			return values.SelectMany(v => Endianness.GetBytesLittleEndian(v));
		}
	}
}