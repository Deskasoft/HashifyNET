// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the ""Software""), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
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

namespace HashifyNet.Algorithms.SM3
{
	/// <summary>
	/// Provides an implementation of the SM3 cryptographic hash function, a secure hash algorithm widely used in Chinese
	/// cryptographic standards. This class is designed to compute a 256-bit hash value for a given input stream of data.
	/// </summary>
	/// <remarks>The SM3 hash function operates on 512-bit (64-byte) blocks and produces a fixed 256-bit hash value.
	/// It is suitable for use in cryptographic applications requiring data integrity, digital signatures, and message
	/// authentication codes (MACs). <para> This implementation enforces a fixed hash size of 256 bits, as required by the
	/// SM3 standard. </para> <para> To use this class, create an instance by providing an <see cref="ISM3Config"/>
	/// configuration object. The configuration must specify a hash size of 256 bits; otherwise, an exception will be
	/// thrown during initialization. </para></remarks>
	[HashAlgorithmImplementation(typeof(ISM3), typeof(SM3Config))]
	internal partial class SM3_Implementation
		: CryptographicStreamableHashFunctionBase<ISM3Config>,
		  ISM3
	{
		public override ISM3Config Config => _config.Clone();

		private readonly ISM3Config _config;
		public SM3_Implementation(ISM3Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits != 256)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, $"{nameof(config)}.{nameof(config.HashSizeInBits)} must be fixed at 256 bits.");
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer();
		}

		private class BlockTransformer : BlockTransformerBase<BlockTransformer>
		{
			private uint[] _v;
			private long _messageLength;

			private static readonly uint[] IV = {
				0x7380166F, 0x4914B2B9, 0x172442D7, 0xDA8A0600,
				0xA96F30BC, 0x163138AA, 0xE38DEE4D, 0xB0FB0E4E
			};

			private static readonly uint[] T = {
				0x79CC4519, 0x7A879D8A
			};

			public BlockTransformer() : base(inputBlockSize: 64) // SM3 uses 512-bit (64-byte) blocks
			{
				Initialize();
			}

			private void Initialize()
			{
				_v = (uint[])IV.Clone();
				_messageLength = 0;
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				other._v = (uint[])_v.Clone();
				other._messageLength = _messageLength;
			}

			protected override void TransformByteGroupsInternal(ReadOnlySpan<byte> data)
			{
				_messageLength += data.Length;
				ProcessBlock(data);
			}

			protected override IHashValue FinalizeHashValueInternal(ReadOnlySpan<byte> leftover, CancellationToken cancellationToken)
			{
				ReadOnlySpan<byte> finalBlock = CreatePaddedBlock(leftover);

				for (int i = 0; i < finalBlock.Length; i += 64)
				{
					ProcessBlock(finalBlock.Slice(i, 64));
				}

				byte[] hash = new byte[32];
				for (int i = 0; i < 8; i++)
				{
					byte[] wordBytes = Endianness.GetBytesBigEndian(_v[i]);
					Buffer.BlockCopy(wordBytes, 0, hash, i * 4, 4);
				}

				return new HashValue(ValueEndianness.BigEndian, hash, 256);
			}

			private ReadOnlySpan<byte> CreatePaddedBlock(ReadOnlySpan<byte> remainder)
			{
				long totalBits = (_messageLength + remainder.Length) * 8;
				int remainderLen = remainder.Length;

				int paddingLen = (remainderLen < 56) ? (56 - remainderLen) : (120 - remainderLen);
				Span<byte> padded = new Span<byte>(new byte[remainderLen + paddingLen + 8]);
				remainder.CopyTo(padded.Slice(0, remainderLen));

				padded[remainderLen] = 0x80;

				byte[] lengthBytes = Endianness.GetBytesBigEndian((ulong)totalBits);
				lengthBytes.AsSpan().CopyTo(padded.Slice(padded.Length - 8, 8));

				return padded;
			}

			private void ProcessBlock(ReadOnlySpan<byte> block)
			{
				uint[] W = new uint[68];
				uint[] W_prime = new uint[64];

				for (int i = 0; i < 16; i++)
				{
					W[i] = Endianness.ToUInt32BigEndian(block, i * 4);
				}

				for (int j = 16; j < 68; j++)
				{
					W[j] = P1(W[j - 16] ^ W[j - 9] ^ RotL(W[j - 3], 15)) ^ RotL(W[j - 13], 7) ^ W[j - 6];
				}

				for (int j = 0; j < 64; j++)
				{
					W_prime[j] = W[j] ^ W[j + 4];
				}

				uint A = _v[0];
				uint B = _v[1];
				uint C = _v[2];
				uint D = _v[3];
				uint E = _v[4];
				uint F = _v[5];
				uint G = _v[6];
				uint H = _v[7];

				for (int j = 0; j < 64; j++)
				{
					uint SS1 = RotL(RotL(A, 12) + E + RotL(GetT(j), j), 7);
					uint SS2 = SS1 ^ RotL(A, 12);
					uint TT1 = FF(A, B, C, j) + D + SS2 + W_prime[j];
					uint TT2 = GG(E, F, G, j) + H + SS1 + W[j];
					D = C;
					C = RotL(B, 9);
					B = A;
					A = TT1;
					H = G;
					G = RotL(F, 19);
					F = E;
					E = P0(TT2);
				}

				_v[0] ^= A;
				_v[1] ^= B;
				_v[2] ^= C;
				_v[3] ^= D;
				_v[4] ^= E;
				_v[5] ^= F;
				_v[6] ^= G;
				_v[7] ^= H;
			}

			private static uint GetT(int j) => (j >= 0 && j <= 15) ? T[0] : T[1];
			private static uint FF(uint x, uint y, uint z, int j) => (j >= 0 && j <= 15) ? (x ^ y ^ z) : ((x & y) | (x & z) | (y & z));
			private static uint GG(uint x, uint y, uint z, int j) => (j >= 0 && j <= 15) ? (x ^ y ^ z) : ((x & y) | (~x & z));
			private static uint P0(uint x) => x ^ RotL(x, 9) ^ RotL(x, 17);
			private static uint P1(uint x) => x ^ RotL(x, 15) ^ RotL(x, 23);
			private static uint RotL(uint x, int n) => (x << n) | (x >> (32 - n));
		}
	}
}
