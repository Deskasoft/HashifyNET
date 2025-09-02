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
using System.Linq;
using System.Threading;

namespace HashifyNet.Algorithms.Blake2
{
	/// <summary>
	/// Provides an implementation of the BLAKE2b cryptographic hash function, a high-performance and secure hashing
	/// algorithm.
	/// </summary>
	/// <remarks>This class implements the BLAKE2b hash function, which is optimized for 64-bit platforms and
	/// supports configurable hash sizes, keys, salts, and personalization strings. It is suitable for a wide range of
	/// cryptographic applications, including message authentication and data integrity verification.  The hash size can be
	/// configured between 8 and 512 bits, in multiples of 8. A key of up to 64 bytes can be used for keyed hashing, and
	/// optional salt and personalization values can be provided to further customize the hash output.  This implementation
	/// is not thread-safe. If multiple threads need to compute hashes concurrently, each thread should use its own
	/// instance of this class.</remarks>
	[HashAlgorithmImplementation(typeof(IBlake2B), typeof(Blake2BConfig))]
	internal partial class Blake2B_Implementation
		: CryptographicStreamableHashFunctionBase<IBlake2BConfig>,
			IBlake2B
	{
		public override IBlake2BConfig Config => _config.Clone();

		private readonly IBlake2BConfig _config;

		private readonly uint _originalKeyLength;
		private readonly byte[] _key;
		private readonly byte[] _salt;
		private readonly byte[] _personalization;

		private const int MinHashSizeBits = 8;
		private const int MaxHashSizeBits = 512;

		private const int MaxKeySizeBytes = 64;
		private const int SaltSizeBytes = 16;
		private const int PersonalizationSizeBytes = 16;

		private const int BlockSizeBytes = 128;

		private const ulong IV1 = 0x6A09E667F3BCC908UL;
		private const ulong IV2 = 0xBB67AE8584CAA73BUL;
		private const ulong IV3 = 0x3C6EF372FE94F82BUL;
		private const ulong IV4 = 0xA54FF53A5F1D36F1UL;
		private const ulong IV5 = 0x510E527FADE682D1UL;
		private const ulong IV6 = 0x9B05688C2B3E6C1FUL;
		private const ulong IV7 = 0x1F83D9ABFB41BD6BUL;
		private const ulong IV8 = 0x5BE0CD19137E2179UL;

		public Blake2B_Implementation(IBlake2BConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits < MinHashSizeBits || _config.HashSizeInBits > MaxHashSizeBits)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"Expected: {MinHashSizeBits} >= {nameof(config)}.{nameof(config.HashSizeInBits)} <= {MaxHashSizeBits}");
			}

			if (_config.HashSizeInBits % 8 != 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be a multiple of 8.");
			}

			var keyArray = (_config.Key ?? new byte[0]).ToArray();
			var saltArray = (_config.Salt ?? new byte[16]).ToArray();
			var personalizationArray = (_config.Personalization ?? new byte[16]).ToArray();

			if (keyArray.Length > MaxKeySizeBytes)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _config.Key, $"Expected: {nameof(config)}.{nameof(config.Key)}.Count <= {MaxKeySizeBytes}");
			}

			if (saltArray.Length != SaltSizeBytes)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Salt)}", _config.Salt, $"Expected: {nameof(config)}.{nameof(config.Salt)}.Count == {SaltSizeBytes}");
			}

			if (personalizationArray.Length != PersonalizationSizeBytes)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Personalization)}", _config.Personalization, $"Expected: {nameof(config)}.{nameof(config.Personalization)}.Count == {PersonalizationSizeBytes}");
			}

			_originalKeyLength = (uint)keyArray.Length;

			_key = new byte[128];
			Array.Copy(keyArray, _key, keyArray.Length);

			_salt = saltArray;
			_personalization = personalizationArray;
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			var blockTransformer = new BlockTransformer(_config.HashSizeInBits, _originalKeyLength, _salt, _personalization);

			if (_originalKeyLength > 0)
			{
				blockTransformer.TransformBytes(_key);
			}

			return blockTransformer;
		}

		private class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private int _hashSizeInBits;
			private uint _originalKeyLength;

			private ulong _a;
			private ulong _b;
			private ulong _c;
			private ulong _d;
			private ulong _e;
			private ulong _f;
			private ulong _g;
			private ulong _h;

			private UInt128 _counter = new UInt128(0, 0);
			private byte[] _delayedInputBuffer = null;

			public BlockTransformer()
				: base(inputBlockSize: 128)
			{
			}

			public BlockTransformer(int hashSizeInBits, uint originalKeyLength, byte[] salt, byte[] personalization)
				: this()
			{
				_hashSizeInBits = hashSizeInBits;
				_originalKeyLength = originalKeyLength;

				_a = IV1;
				_b = IV2;
				_c = IV3;
				_d = IV4;
				_e = IV5;
				_f = IV6;
				_g = IV7;
				_h = IV8;

				_a ^= 0x01010000U |
					((uint)hashSizeInBits / 8) |
					(originalKeyLength << 8);

				_e ^= Endianness.ToUInt64LittleEndian(salt, 0);
				_f ^= Endianness.ToUInt64LittleEndian(salt, 8);

				_g ^= Endianness.ToUInt64LittleEndian(personalization, 0);
				_h ^= Endianness.ToUInt64LittleEndian(personalization, 8);
			}
			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._hashSizeInBits = _hashSizeInBits;
				other._originalKeyLength = _originalKeyLength;

				other._a = _a;
				other._b = _b;
				other._c = _c;
				other._d = _d;
				other._e = _e;
				other._f = _f;
				other._g = _g;
				other._h = _h;

				other._counter = _counter;
				other._delayedInputBuffer = _delayedInputBuffer;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataOffset = data.Offset;
				var dataCount = data.Count;

				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;
				var tempE = _e;
				var tempF = _f;
				var tempG = _g;
				var tempH = _h;

				var tempCounter = _counter;

				// Process _delayedInputBuffer
				if (_delayedInputBuffer != null)
				{
					tempCounter += new UInt128(0, 128);

					Compress(
						ref tempA, ref tempB, ref tempC, ref tempD, ref tempE, ref tempF, ref tempG, ref tempH,
						tempCounter,
						_delayedInputBuffer, 0,
						false);
				}

				// Delay the last 128 bytes of input
				{
					_delayedInputBuffer = new byte[128];
					Array.Copy(dataArray, dataOffset + dataCount - 128, _delayedInputBuffer, 0, 128);

					dataCount -= 128;
				}

				// Process all non-delayed input
				if (dataCount > 0)
				{
					var endOffset = dataOffset + dataCount;

					for (var currentOffset = dataOffset; currentOffset < endOffset; currentOffset += 128)
					{
						tempCounter += new UInt128(0, 128);

						Compress(
							ref tempA, ref tempB, ref tempC, ref tempD, ref tempE, ref tempF, ref tempG, ref tempH,
							tempCounter,
							dataArray, currentOffset,
							false);
					}
				}

				_a = tempA;
				_b = tempB;
				_c = tempC;
				_d = tempD;
				_e = tempE;
				_f = tempF;
				_g = tempG;
				_h = tempH;

				_counter = tempCounter;
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				var remainder = FinalizeInputBuffer;
				var remainderCount = (remainder?.Length).GetValueOrDefault();

				var tempA = _a;
				var tempB = _b;
				var tempC = _c;
				var tempD = _d;
				var tempE = _e;
				var tempF = _f;
				var tempG = _g;
				var tempH = _h;

				var tempCounter = _counter;

				if (_delayedInputBuffer != null)
				{
					cancellationToken.ThrowIfCancellationRequested();

					tempCounter += new UInt128(0, 128);

					Compress(
						ref tempA, ref tempB, ref tempC, ref tempD, ref tempE, ref tempF, ref tempG, ref tempH,
						tempCounter,
						_delayedInputBuffer, 0,
						remainderCount == 0);
				}

				if (remainderCount > 0 || _delayedInputBuffer == null)
				{
					cancellationToken.ThrowIfCancellationRequested();

					var finalBuffer = new byte[128];

					if (remainderCount > 0)
					{
						Array.Copy(remainder, 0, finalBuffer, 0, remainderCount);
					}

					tempCounter += new UInt128(0, (ulong)remainderCount);

					Compress(
						ref tempA, ref tempB, ref tempC, ref tempD, ref tempE, ref tempF, ref tempG, ref tempH,
						tempCounter,
						finalBuffer, 0,
						true);
				}

				var hashValueBytes = Endianness.GetBytesLittleEndian(tempA)
					.Concat(Endianness.GetBytesLittleEndian(tempB))
					.Concat(Endianness.GetBytesLittleEndian(tempC))
					.Concat(Endianness.GetBytesLittleEndian(tempD))
					.Concat(Endianness.GetBytesLittleEndian(tempE))
					.Concat(Endianness.GetBytesLittleEndian(tempF))
					.Concat(Endianness.GetBytesLittleEndian(tempG))
					.Concat(Endianness.GetBytesLittleEndian(tempH))
					.Take(_hashSizeInBits / 8)
					.ToArray();

				return new HashValue(hashValueBytes, _hashSizeInBits);
			}

			private static void Compress(
				ref ulong a, ref ulong b, ref ulong c, ref ulong d, ref ulong e, ref ulong f, ref ulong g, ref ulong h,
				UInt128 counter,
				byte[] dataArray, int dataOffset,
				bool isFinal)
			{
				var reinterpretedData = new ulong[16];
				Buffer.BlockCopy(dataArray, dataOffset, reinterpretedData, 0, BlockSizeBytes);

				var v = new ulong[16] {
					a, b, c, d, e, f, g, h,
					IV1, IV2, IV3, IV4, IV5, IV6, IV7, IV8
				};

				v[12] ^= counter.GetLower();
				v[13] ^= counter.GetUpper();

				if (isFinal)
				{
					v[14] ^= ulong.MaxValue;
				}

				ComputeRounds(v, reinterpretedData);

				//Finalization
				a ^= v[0] ^ v[8];
				b ^= v[1] ^ v[9];
				c ^= v[2] ^ v[10];
				d ^= v[3] ^ v[11];
				e ^= v[4] ^ v[12];
				f ^= v[5] ^ v[13];
				g ^= v[6] ^ v[14];
				h ^= v[7] ^ v[15];
			}
		}
	}

}
