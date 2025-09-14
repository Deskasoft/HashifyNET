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
// * LIABILITY, WHETHER IN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
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
using Isopoh.Cryptography.Argon2;
using System;
using System.Threading;

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Provides an implementation of the Argon2id password hashing algorithm, which is a memory-hard and CPU-intensive
	/// key derivation function designed for secure password storage.
	/// </summary>
	/// <remarks>This implementation supports configurable parameters such as hash size, iterations, degree of
	/// parallelism, and memory size, allowing users to adjust the algorithm's performance and security characteristics. 
	/// The configuration is validated during initialization to ensure that all parameters meet the required
	/// constraints.</remarks>
	[HashAlgorithmImplementation(typeof(IArgon2id), typeof(Argon2idConfig))]
	internal class Argon2id_Implementation : CryptographicHashFunctionBase<IArgon2idConfig>,
		  IArgon2id
	{
		public override IArgon2idConfig Config => _config.Clone();

		private readonly IArgon2idConfig _config;
		public Argon2id_Implementation(IArgon2idConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			_config = config.Clone();

			if (_config.HashSizeInBits < 32)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.HashSizeInBits)}", config.HashSizeInBits, "Hash size must be greater than or equal to 32 bits for Argon2id.");
			}

			if (_config.Iterations < 1)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.Iterations)}", config.Iterations, "Iterations must be at least 1.");
			}

			int maxProcessors;
#if NET8_0_OR_GREATER
			if (OperatingSystem.IsBrowser())
#else
			if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Create("Browser")))
#endif
			{
				// TODO: Review this.
				maxProcessors = int.MaxValue; // Just assume we can use as many as we want in WASM.
			}
			else
			{
				maxProcessors = Environment.ProcessorCount;
			}

			if (_config.DegreeOfParallelism < 1 || _config.DegreeOfParallelism > maxProcessors)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.DegreeOfParallelism)}", config.DegreeOfParallelism, $"Degree of parallelism must be at least 1 and smaller or equal to processor count '{Environment.ProcessorCount}'.");
			}

			if (_config.MemorySize < 8 * _config.DegreeOfParallelism)
			{
				throw new ArgumentOutOfRangeException($"{nameof(config)}.{nameof(config.MemorySize)}", config.MemorySize, $"Memory size must be at least {8 * _config.DegreeOfParallelism} KiB.");
			}
		}

		protected override IHashValue ComputeHashInternal(ArraySegment<byte> data, CancellationToken cancellationToken)
		{
			byte[] salt;
			if (_config.Salt != null)
				salt = _config.Salt;
			else
				salt = Argon2idHelpers.GenerateSalt();

			return ComputeHashWithSaltInternal(data, salt);
		}

		internal IHashValue ComputeHashWithSaltInternal(ArraySegment<byte> data, byte[] salt)
		{
			Argon2Config config = Argon2idHelpers.GetArgon2Config(_config, data.Array, salt);
			string hash = Argon2.Hash(config);

			byte[] encodedHash = Argon2idSerializer.Serialize(hash);
			Argon2Config overwrite = new Argon2Config();
			if (!DecodeExtension.DecodeString(overwrite, hash, out var buffer))
			{
				throw new InvalidOperationException("Could not decode Argon2id hash.");
			}

			try
			{
				return new EncodedHashValue(encodedHash, (i) => Argon2idSerializer.Deserialize((byte[])i), buffer.Buffer, _config.HashSizeInBits);
			}
			finally
			{
				buffer.Dispose();
			}
		}
	}
}
