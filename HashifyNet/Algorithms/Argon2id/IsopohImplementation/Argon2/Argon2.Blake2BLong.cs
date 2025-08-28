// <copyright file="Argon2.Blake2BLong.cs" company="Isopoh">
// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.
// </copyright>

using HashifyNet;
using HashifyNet.Algorithms.Blake2;
using Isopoh.Cryptography.SecureArray;
using System;

namespace Isopoh.Cryptography.Argon2
{
	/// <summary>
	/// Argon2 Hashing of passwords.
	/// </summary>
	internal sealed partial class Argon2
	{
		/// <summary>
		/// Does a Blake2 hash with the ability to truncate or extend the hash to any length.
		/// </summary>
		/// <param name="hash">
		/// The buffer to fill with the hash.
		/// </param>
		/// <param name="inputBuffer">
		/// What to hash.
		/// </param>
		/// <param name="secureArrayCall">
		/// The methods that get called to secure arrays. A null value defaults to <see cref="SecureArray"/>.<see cref="Isopoh.Cryptography.SecureArray.SecureArray.DefaultCall"/>.
		/// </param>
		private static void Blake2BLong(byte[] hash, byte[] inputBuffer, SecureArrayCall secureArrayCall)
		{
			var outputLengthBytes = new byte[4];
			using (var intermediateHash = SecureArray<byte>.Best(64, secureArrayCall))
			{
				var blake2BConfig = new Blake2BConfig
				{
					HashSizeInBits = hash.Length > 64 ? 512 : hash.Length * 8,
				};
				Store32(outputLengthBytes, hash.Length);
				var blakeHash = HashFactory<IBlake2B, IBlake2BConfig>.Instance.Create(blake2BConfig);
				var transformer = blakeHash.CreateBlockTransformer();
				{
					if (outputLengthBytes != null && outputLengthBytes.Length > 0)
					{
						transformer.TransformBytes(outputLengthBytes);
					}

					if (inputBuffer != null && outputLengthBytes.Length > 0)
					{
						transformer.TransformBytes(inputBuffer);
					}

					var hv = transformer.FinalizeHashValue();
					Array.Copy(hv.Hash, intermediateHash.Buffer, hv.Hash.Length);
				}

				if (hash.Length <= intermediateHash.Buffer.Length)
				{
					// less than or equal to 64 bytes, just copy the hash result
					Array.Copy(intermediateHash.Buffer, hash, hash.Length);
					return;
				}

				// greater than 64 bytes, copy a chain of half-hash results until the final up-to-full hash result
				const int b2B2 = 64 / 2;
				Array.Copy(intermediateHash.Buffer, hash, b2B2); // copy half hash result
				int pos = b2B2;
				int lastHashIndex = hash.Length - 64;
				using (var toHash = SecureArray<byte>.Best(64, secureArrayCall))
				{
					while (pos < lastHashIndex)
					{
						Array.Copy(intermediateHash.Buffer, toHash.Buffer, intermediateHash.Buffer.Length); // set toHash to be the previous hash
						var hv = HashFactory<IBlake2B, IBlake2BConfig>.Instance.Create(blake2BConfig).ComputeHash(toHash.Buffer);
						Array.Copy(hv.Hash, intermediateHash.Buffer, hv.Hash.Length);

						Array.Copy(intermediateHash.Buffer, 0, hash, pos, b2B2); // copy half hash result
						pos += b2B2;
					}

					// between 33 and 64 bytes left to load
					Array.Copy(intermediateHash.Buffer, toHash.Buffer, intermediateHash.Buffer.Length); // set toHash to be the previous hash
					var remaining = hash.Length - pos;
					if (remaining < 64)
					{
						blake2BConfig = new Blake2BConfig
						{
							HashSizeInBits = (hash.Length - pos) * 8,
						};
					}

					{
						var hv = HashFactory<IBlake2B, IBlake2BConfig>.Instance.Create(blake2BConfig).ComputeHash(toHash.Buffer);
						Array.Copy(hv.Hash, intermediateHash.Buffer, hv.Hash.Length);
					}
					Array.Copy(intermediateHash.Buffer, 0, hash, pos, hash.Length - pos); // copy the final bytes from the first part of the hash result
				}
			}
		}
	}
}