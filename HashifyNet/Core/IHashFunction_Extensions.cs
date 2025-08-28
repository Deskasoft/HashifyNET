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

using HashifyNet.Core.Utilities;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace HashifyNet
{
	/// <summary>
	/// Static class to provide extension functions for IHashFunction instances.
	/// </summary>
	public static class IHashFunction_Extensions
	{
		#region ComputeHash

		#region Sugar

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, bool data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, byte data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				new[] { data });
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, char data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, double data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, float data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, int data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, long data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, sbyte data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				new[] { (byte)data });
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, short data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		/// <remarks>
		/// UTF-8 encoding used to convert string to bytes.
		/// </remarks>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, string data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				Encoding.UTF8.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, uint data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, ulong data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, ushort data) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data));
		}

		#endregion

		#region Sugar with desiredSize

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, bool data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, byte data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				new[] { data },
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, char data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, double data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, float data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, int data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, long data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, sbyte data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				new[] { (byte)data },
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, short data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		/// <remarks>
		/// UTF-8 encoding used to convert string to bytes.
		/// </remarks>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, string data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				Encoding.UTF8.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, uint data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, ulong data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, ushort data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			return hashFunction.ComputeHash(
				BitConverter.GetBytes(data),
				desiredHashSize);
		}

		/// <summary>
		/// Computes hash value for given data.
		/// </summary>
		/// <param name="hashFunction">Hash function to use.</param>
		/// <param name="data">Data to be hashed.</param>
		/// <param name="desiredHashSize">Desired size of resulting hash, in bits.</param>
		/// <returns>
		/// Hash value of the data as byte array.
		/// </returns>
		public static IHashValue ComputeHash<CName>(this IHashFunction<CName> hashFunction, byte[] data, int desiredHashSize) where CName : IHashConfig<CName>
		{
			var hash = new BigInteger();
			var desiredHashBytes = (desiredHashSize + 7) / 8;

			var seededData = new byte[data.Length + 4];
			Array.Copy(data, 0, seededData, 4, data.Length);

			IHashConfig<CName> config = hashFunction.Config;
			if (config == null)
			{
				throw new ArgumentException($"{nameof(hashFunction)} does not have a valid {nameof(IHashConfig<CName>)} instance.", nameof(hashFunction));
			}

			var hashesNeeded = (desiredHashSize + (config.HashSizeInBits - 1)) / config.HashSizeInBits;

			// Compute as many hashes as needed
			for (int x = 0; x < Math.Max(hashesNeeded, 1); ++x)
			{
				byte[] currentData;

				if (x != 0)
				{
					Array.Copy(BitConverter.GetBytes(x), seededData, 4);
					currentData = seededData;

				}
				else
				{
					// Use original data for first 
					currentData = data;
				}


				var elementHash = new BigInteger(
					hashFunction.ComputeHash(currentData)
						.Hash
						.Concat(new[] { (byte)0 })
						.ToArray());

				hash |= elementHash << (x * config.HashSizeInBits);
			}


			// XOr-fold the extra bits
			if (hashesNeeded * config.HashSizeInBits != desiredHashSize)
			{
				var mask = (new BigInteger(1) << desiredHashSize) - 1;

				hash = (hash ^ (hash >> desiredHashSize)) & mask;
			}


			// Convert to array that contains desiredHashSize bits
			var hashBytes = hash.ToByteArray();

			// Account for missing or extra bytes.
			if (hashBytes.Length != desiredHashBytes)
			{
				var buffer = new byte[desiredHashBytes];
				Array.Copy(hashBytes, buffer, Math.Min(hashBytes.Length, desiredHashBytes));

				hashBytes = buffer;
			}

			return new HashValue(hashBytes, desiredHashSize);
		}

		#endregion

		#endregion
	}
}