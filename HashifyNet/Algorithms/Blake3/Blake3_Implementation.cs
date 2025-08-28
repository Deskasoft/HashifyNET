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

namespace HashifyNet.Algorithms.Blake3
{
	/// <summary>
	/// Provides an implementation of the BLAKE3 cryptographic hash function, supporting streaming input and configurable
	/// options such as keying, salting, and personalization.
	/// </summary>
	/// <remarks>This class implements the BLAKE3 hash function, which is a cryptographic hash function designed for
	/// high performance, security, and flexibility. It supports keyed hashing, salting, and personalization, making it
	/// suitable for a variety of use cases, including message authentication and unique hash derivation.  The
	/// implementation is designed to handle streaming input efficiently, allowing large data to be processed
	/// incrementally. It also supports variable-length output, as specified in the configuration.  This class is internal
	/// and intended for use within the library. For external usage, refer to the public interfaces or wrappers that expose
	/// this functionality.</remarks>
	[HashAlgorithmImplementation(typeof(IBlake3), typeof(Blake3Config))]
	internal partial class Blake3_Implementation
		: CryptographicStreamableHashFunctionBase<IBlake3Config>,
		  IBlake3
	{
		public override IBlake3Config Config => _config.Clone();

		private readonly IBlake3Config _config;

		private readonly byte[] _key;
		private readonly byte[] _salt;
		private readonly byte[] _personalization;

		private const int MaxKeySizeBytes = 32;
		private const int SaltSizeBytes = 32;
		private const int PersonalizationSizeBytes = 32;

		// Sizes
		private const int BLOCK_LEN = 64;
		private const int CHUNK_LEN = 1024;
		private const int OUT_BYTES_PER_BLOCK = 64;
		private const int ROUNDS = 7;

		// Flags (full set preserved)
		private const uint CHUNK_START = 1u << 0;
		private const uint CHUNK_END = 1u << 1;
		private const uint PARENT = 1u << 2;
		private const uint ROOT = 1u << 3;

		// IV (little-endian)
		private static readonly uint[] IV = {
			0x6A09E667U, 0xBB67AE85U, 0x3C6EF372U, 0xA54FF53AU,
			0x510E527FU, 0x9B05688CU, 0x1F83D9ABU, 0x5BE0CD19U
		};

		// Permutation and derived schedule
		private static readonly byte[] PERM = {
			2, 6, 3, 10, 7, 0, 4, 13, 1, 11, 12, 5, 9, 14, 15, 8
		};

		private static readonly byte[,] MSG_SCHEDULE = BuildMsgSchedule();

		public Blake3_Implementation(IBlake3Config config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits <= 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, "Expected a minimum of 8 bits.");
			}

			if (_config.HashSizeInBits % 8 != 0)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", _config.HashSizeInBits, "Hash size must be a multiple of 8.");
			}

			_key = config.Key?.ToArray() ?? null;
			if (_key != null)
			{
				if (_key.Length != MaxKeySizeBytes)
				{
					throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Key)}", _key.Length, $"Key size must be {MaxKeySizeBytes} bytes.");
				}
			}

			_salt = config.Salt?.ToArray() ?? null;
			if (_salt != null)
			{
				if (_salt.Length > SaltSizeBytes)
				{
					throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Salt)}", _salt.Length, $"Salt size must be below {SaltSizeBytes} bytes.");
				}
			}

			_personalization = config.Personalization?.ToArray() ?? null;
			if (_personalization != null)
			{
				if (_personalization.Length > PersonalizationSizeBytes)
				{
					throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Personalization)}", _personalization.Length, $"Personalization size must be below {PersonalizationSizeBytes} bytes.");
				}
			}
		}

		public override IBlockTransformer CreateBlockTransformer()
		{
			return new BlockTransformer(this, _config.HashSizeInBits);
		}

		private void MixIntoIV(uint[] state, byte[] data)
		{
			// XOR in little‑endian words, cycling if shorter than state
			for (int i = 0; i < state.Length; i++)
			{
				int baseIdx = i * 4 % data.Length;
				uint w = BitConverter.ToUInt32(data, baseIdx);
				state[i] ^= w;
			}
		}

		private uint[] InitVectorWithOptions()
		{
			var iv = (uint[])IV.Clone();

			if (_salt != null)
			{
				MixIntoIV(iv, _salt);
			}

			if (_key != null)
			{
				MixIntoIV(iv, _key);
			}

			if (_personalization != null)
			{
				MixIntoIV(iv, _personalization);
			}

			return iv;
		}

		private sealed class BlockTransformer
			: BlockTransformerBase<BlockTransformer>
		{
			private int _hashSizeInBits;

			// Streaming state: store completed chunk CVs; final partial chunk is handled via FinalizeInputBuffer
			private readonly List<uint[]> _chunkCvs = new List<uint[]>(64);
			private ulong _chunkCounter;

			private uint[] _singleChunkLastBlockWords;  // last 64-byte block words of the first (and possibly only) chunk
			private uint _singleChunkLastBlockLen;
			private uint _singleChunkLastBlockFlags;
			private ulong _singleChunkCounter;
			private uint[] _singleChunkCv;

			private readonly uint[] _currentIV;

			private readonly Blake3_Implementation _owner;

			public BlockTransformer()
				: base(inputBlockSize: CHUNK_LEN)
			{
				_currentIV = GetPopulatedIV();
			}

			public BlockTransformer(Blake3_Implementation owner, int hashSizeInBits)
				: this()
			{
				_owner = owner;
				_hashSizeInBits = hashSizeInBits;
				ResetState();
			}

			private uint[] GetPopulatedIV()
			{
				if (_owner == null)
				{
					return (uint[])IV.Clone();
				}

				return _owner.InitVectorWithOptions();
			}

			protected override void CopyStateTo(BlockTransformer other)
			{
				base.CopyStateTo(other);

				other._hashSizeInBits = _hashSizeInBits;

				other._chunkCvs.Clear();
				foreach (var cv in _chunkCvs)
				{
					other._chunkCvs.Add((uint[])cv.Clone());
				}

				other._chunkCounter = _chunkCounter;

				other._singleChunkLastBlockWords = _singleChunkLastBlockWords is null ? null : (uint[])_singleChunkLastBlockWords.Clone();
				other._singleChunkLastBlockLen = _singleChunkLastBlockLen;
				other._singleChunkLastBlockFlags = _singleChunkLastBlockFlags;
				other._singleChunkCounter = _singleChunkCounter;
				other._singleChunkCv = _singleChunkCv is null ? null : (uint[])_singleChunkCv.Clone();
			}

			private void ResetState()
			{
				_chunkCvs.Clear();
				_chunkCounter = 0UL;

				_singleChunkLastBlockWords = null;
				_singleChunkLastBlockLen = 0U;
				_singleChunkLastBlockFlags = 0U;
				_singleChunkCounter = 0UL;
				_singleChunkCv = null;
			}

			protected override void TransformByteGroupsInternal(ArraySegment<byte> data)
			{
				var dataArray = data.Array;
				var dataOffset = data.Offset;
				var dataCount = data.Count;

				if (dataArray == null || dataCount == 0)
				{
					return;
				}

				// Base ensures dataCount is a multiple of inputBlockSize (CHUNK_LEN)
				int endOffset = dataOffset + dataCount;
				for (int currentOffset = dataOffset; currentOffset < endOffset; currentOffset += CHUNK_LEN)
				{
					EmitFullChunk(dataArray, currentOffset);
				}
			}

			protected override IHashValue FinalizeHashValueInternal(CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var remainder = FinalizeInputBuffer;
				int remainderCount = (remainder?.Length).GetValueOrDefault();

				byte[] result;
				int outLen = _hashSizeInBits / 8;

				// Case A: no full chunks were emitted
				if (_chunkCvs.Count == 0)
				{
					int totalLen = remainderCount;
					int blocks = (totalLen + BLOCK_LEN - 1) / BLOCK_LEN;
					if (totalLen == 0)
					{
						blocks = 1;
					}

					uint[] cv = (uint[])_currentIV.Clone();
					ulong chunkCounter = 0UL;
					uint[] lastBlockWords = null;
					uint lastFlags = 0U;
					uint lastBlockLen = 0U;

					for (int b = 0; b < blocks; b++)
					{
						int blockOffset = b * BLOCK_LEN;
						int blockLen = Math.Min(BLOCK_LEN, Math.Max(0, totalLen - (b * BLOCK_LEN)));
						uint flags = 0;
						if (b == 0)
						{
							flags |= CHUNK_START;
						}

						if (b == blocks - 1)
						{
							flags |= CHUNK_END;
						}

						var m = LoadBlockWords(remainder ?? Array.Empty<byte>(), blockOffset, blockLen);

						if (b == blocks - 1)
						{
							lastBlockWords = m;
							lastFlags = flags;
							lastBlockLen = (uint)blockLen;
						}
						else
						{
							var st = Compress(cv, m, chunkCounter, (uint)blockLen, flags);
							Array.Copy(st, 0, cv, 0, 8);
						}
					}

					result = new byte[outLen];
					FillXof(cv, lastBlockWords ?? new uint[16], chunkCounter, lastBlockLen, lastFlags, result);

					ResetState();
					return new HashValue(result, _hashSizeInBits);
				}

				// Case B: exactly one full chunk emitted and no remainder
				if (_chunkCvs.Count == 1 && remainderCount == 0)
				{
					if (_singleChunkCv == null || _singleChunkLastBlockWords == null)
					{
						throw new InvalidOperationException("Internal error: missing single-chunk metadata.");
					}

					result = new byte[outLen];
					FillXof(_singleChunkCv, _singleChunkLastBlockWords, _singleChunkCounter, _singleChunkLastBlockLen, _singleChunkLastBlockFlags, result);

					ResetState();
					return new HashValue(result, _hashSizeInBits);
				}

				// Case C: true multi-chunk (>=2), or 1 chunk + tail (will become >=2 after tail)
				var cvList = new List<uint[]>(_chunkCvs);

				if (remainderCount > 0)
				{
					var lastCv = CompressChunkToCv(_currentIV, remainder, 0, remainderCount, _chunkCounter);
					cvList.Add(lastCv);
					_chunkCounter++;
				}

				// Reduce to two CVs and build the root message block, then fill XOF with PARENT|ROOT
				if (cvList.Count < 2)
				{
					throw new InvalidOperationException("Expected at least two chunk CVs for multi-chunk root.");
				}

				var rootMessageBlock = GetRootMessageBlockFromCvs(_currentIV, cvList);

				result = new byte[outLen];
				FillXof(_currentIV, rootMessageBlock, 0UL, BLOCK_LEN, PARENT, result);

				ResetState();
				return new HashValue(result, _hashSizeInBits);
			}

			private void EmitFullChunk(byte[] src, int offset)
			{
				var cv = CompressChunkToCv(_currentIV, src, offset, CHUNK_LEN, _chunkCounter);
				_chunkCvs.Add(cv);

				if (_chunkCvs.Count == 1)
				{
					_singleChunkLastBlockWords = LoadBlockWords(src, offset + CHUNK_LEN - BLOCK_LEN, BLOCK_LEN);
					_singleChunkLastBlockLen = BLOCK_LEN;
					_singleChunkLastBlockFlags = CHUNK_END;
					_singleChunkCounter = _chunkCounter;
					_singleChunkCv = (uint[])cv.Clone();
				}

				_chunkCounter++;
			}
		}

		private static byte[,] BuildMsgSchedule()
		{
			var sched = new byte[ROUNDS, 16];
			byte[] idx = new byte[16];
			for (byte i = 0; i < 16; i++)
			{
				idx[i] = i;
			}

			for (int r = 0; r < ROUNDS; r++)
			{
				for (int i = 0; i < 16; i++)
				{
					sched[r, i] = idx[i];
				}

				byte[] next = new byte[16];
				for (int i = 0; i < 16; i++)
				{
					next[i] = idx[PERM[i]];
				}

				idx = next;
			}
			return sched;
		}

		private static uint RotR(uint x, int n) => (x >> n) | (x << (32 - n));

		private static void G(ref uint a, ref uint b, ref uint c, ref uint d, uint mx, uint my)
		{
			a = a + b + mx;
			d ^= a; d = RotR(d, 16);
			c = c + d;
			b ^= c; b = RotR(b, 12);
			a = a + b + my;
			d ^= a; d = RotR(d, 8);
			c = c + d;
			b ^= c; b = RotR(b, 7);
		}

		private static uint[] Compress(uint[] cv, uint[] m, ulong counter, uint blockLen, uint flags)
		{
			uint[] v = new uint[16];
			for (int i = 0; i < 8; i++)
			{
				v[i] = cv[i];
			}

			for (int i = 0; i < 8; i++)
			{
				v[8 + i] = IV[i];
			}

			// Assignment injection (not XOR)
			v[12] = (uint)(counter & 0xFFFFFFFFU);
			v[13] = (uint)(counter >> 32);
			v[14] = blockLen;
			v[15] = flags;

			for (int r = 0; r < ROUNDS; r++)
			{
				byte s0 = MSG_SCHEDULE[r, 0], s1 = MSG_SCHEDULE[r, 1], s2 = MSG_SCHEDULE[r, 2], s3 = MSG_SCHEDULE[r, 3];
				byte s4 = MSG_SCHEDULE[r, 4], s5 = MSG_SCHEDULE[r, 5], s6 = MSG_SCHEDULE[r, 6], s7 = MSG_SCHEDULE[r, 7];
				byte s8 = MSG_SCHEDULE[r, 8], s9 = MSG_SCHEDULE[r, 9], s10 = MSG_SCHEDULE[r, 10], s11 = MSG_SCHEDULE[r, 11];
				byte s12 = MSG_SCHEDULE[r, 12], s13 = MSG_SCHEDULE[r, 13], s14 = MSG_SCHEDULE[r, 14], s15 = MSG_SCHEDULE[r, 15];

				// Columns
				G(ref v[0], ref v[4], ref v[8], ref v[12], m[s0], m[s1]);
				G(ref v[1], ref v[5], ref v[9], ref v[13], m[s2], m[s3]);
				G(ref v[2], ref v[6], ref v[10], ref v[14], m[s4], m[s5]);
				G(ref v[3], ref v[7], ref v[11], ref v[15], m[s6], m[s7]);

				// Diagonals
				G(ref v[0], ref v[5], ref v[10], ref v[15], m[s8], m[s9]);
				G(ref v[1], ref v[6], ref v[11], ref v[12], m[s10], m[s11]);
				G(ref v[2], ref v[7], ref v[8], ref v[13], m[s12], m[s13]);
				G(ref v[3], ref v[4], ref v[9], ref v[14], m[s14], m[s15]);
			}

			uint[] outWords = new uint[16];
			for (int i = 0; i < 8; i++)
			{
				outWords[i] = v[i] ^ v[i + 8];
				outWords[i + 8] = cv[i] ^ v[i + 8];
			}
			return outWords;
		}

		private static uint[] LoadBlockWords(byte[] input, int offset, int blockLen)
		{
			uint[] m = new uint[16];
			int i = 0, pos = offset;
			for (; i < blockLen / 4; i++, pos += 4)
			{
				m[i] = (uint)(input[pos]
					| (input[pos + 1] << 8)
					| (input[pos + 2] << 16)
					| (input[pos + 3] << 24));
			}
			int rem = blockLen & 3;
			if (rem != 0)
			{
				uint w = 0;
				for (int b = 0; b < rem; b++)
				{
					w |= (uint)input[pos + b] << (8 * b);
				}

				m[i] = w;
			}
			return m;
		}

		private static void WordsToBytes(uint[] words, Span<byte> outBytes)
		{
			for (int i = 0; i < words.Length; i++)
			{
				uint w = words[i];
				int p = i * 4;
				outBytes[p + 0] = (byte)(w & 0xFF);
				outBytes[p + 1] = (byte)((w >> 8) & 0xFF);
				outBytes[p + 2] = (byte)((w >> 16) & 0xFF);
				outBytes[p + 3] = (byte)((w >> 24) & 0xFF);
			}
		}

		// Derive chunk CV: iterate blocks, updating CV for every block; return final CV.
		private static uint[] CompressChunkToCv(uint[] key, byte[] input, int offset, int length, ulong chunkCounter)
		{
			uint[] cv = (uint[])key.Clone();
			int blocks = (length + BLOCK_LEN - 1) / BLOCK_LEN;
			if (length == 0)
			{
				blocks = 1;
			}

			for (int b = 0; b < blocks; b++)
			{
				int blockOffset = offset + (b * BLOCK_LEN);
				int blockLen = Math.Min(BLOCK_LEN, Math.Max(0, length - (b * BLOCK_LEN)));
				uint flags = 0;
				if (b == 0)
				{
					flags |= CHUNK_START;
				}

				if (b == blocks - 1)
				{
					flags |= CHUNK_END;
				}

				uint[] m = LoadBlockWords(input, blockOffset, blockLen);
				uint[] st = Compress(cv, m, chunkCounter, (uint)blockLen, flags);
				Array.Copy(st, 0, cv, 0, 8);
			}
			return cv;
		}

		// Parent CV from two child CVs.
		private static uint[] CompressParentsToCv(uint[] iv, uint[] left, uint[] right)
		{
			uint[] m = new uint[16];
			Array.Copy(left, 0, m, 0, 8);
			Array.Copy(right, 0, m, 8, 8);
			uint[] st = Compress(iv, m, 0UL, BLOCK_LEN, PARENT);
			uint[] parentCv = new uint[8];
			Array.Copy(st, 0, parentCv, 0, 8);
			return parentCv;
		}

		// Build the root message block (two child CVs) by reducing all chunk CVs down to exactly two.
		private static uint[] GetRootMessageBlockFromCvs(uint[] iv, List<uint[]> cvs)
		{
			if (cvs == null)
			{
				throw new ArgumentNullException(nameof(cvs));
			}

			if (cvs.Count < 2)
			{
				throw new InvalidOperationException("Root message block requires at least two chaining values.");
			}

			var cvList = new List<uint[]>(cvs);
			while (cvList.Count > 2)
			{
				var next = new List<uint[]>((cvList.Count + 1) / 2);
				for (int i = 0; i < cvList.Count; i += 2)
				{
					if (i + 1 < cvList.Count)
					{
						next.Add(CompressParentsToCv(iv, cvList[i], cvList[i + 1]));
					}
					else
					{
						next.Add(cvList[i]); // odd carry
					}
				}
				cvList = next;
			}

			var messageBlock = new uint[16];
			Array.Copy(cvList[0], 0, messageBlock, 0, 8);
			Array.Copy(cvList[1], 0, messageBlock, 8, 8);
			return messageBlock;
		}

		// Generate outLen bytes by repeatedly compressing the same root node with an increasing counter.
		// Flags always include ROOT; baseFlags must be the node type (CHUNK_* for single-chunk root, or PARENT for multi-chunk root).
		private static void FillXof(uint[] inputCv, uint[] messageBlock, ulong counter, uint blockLen, uint baseFlags, byte[] output)
		{
			uint flags = baseFlags | ROOT;
			int filled = 0;
			ulong blockCounter = counter;

			while (filled < output.Length)
			{
				uint[] outputWords = Compress(inputCv, messageBlock, blockCounter, blockLen, flags);

				Span<byte> blockBytes = new byte[OUT_BYTES_PER_BLOCK];
				WordsToBytes(outputWords, blockBytes);

				int take = Math.Min(OUT_BYTES_PER_BLOCK, output.Length - filled);
				blockBytes.Slice(0, take).CopyTo(new Span<byte>(output, filled, take));

				filled += take;
				blockCounter++;
			}
		}
	}
}