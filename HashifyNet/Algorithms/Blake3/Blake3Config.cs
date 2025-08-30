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
using System.Collections.Generic;
using System.Linq;

namespace HashifyNet.Algorithms.Blake3
{
	/// <summary>
	/// Concrete implementation of the Blake3 configuration.
	/// </summary>
	public class Blake3Config : IBlake3Config
	{
		/// <summary>
		/// Represents the default hash size, in bits, used by the hashing algorithm.
		/// </summary>
		/// <remarks>This constant is typically used as the default value for hash size configurations in
		/// cryptographic operations or hashing utilities.</remarks>
		public const int DefaultHashSizeInBits = 512;

		/// <summary>
		/// Gets the desired hash size, in bits.
		/// </summary>
		/// <value>
		/// The desired hash size, in bits.
		/// </value>
		/// <remarks>
		/// Defaults to <c>512</c>.
		/// </remarks>
		public int HashSizeInBits { get; set; } = DefaultHashSizeInBits;

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Key { get; set; } = null;

		/// <summary>
		/// Gets the salt.
		/// </summary>
		/// <value>
		/// The salt.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Salt { get; set; } = null;

		/// <summary>
		/// Gets the personalization sequence.
		/// </summary>
		/// <value>
		/// The personalization sequence.
		/// </value>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public IReadOnlyList<byte> Personalization { get; set; } = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="Blake3Config"/> class with the specified hash size, key, salt, and
		/// personalization values.
		/// </summary>
		/// <remarks>This constructor allows configuring the BLAKE3 hashing algorithm with optional parameters for
		/// keyed hashing, salting, and personalization. If no key, salt, or personalization is provided, the hash will be
		/// computed using the default configuration.</remarks>
		/// <param name="hashSizeInBits">The size of the hash output in bits. Must be a positive integer. The default value is <see
		/// cref="DefaultHashSizeInBits"/>.</param>
		/// <param name="key">An optional key used for keyed hashing. If provided, it must be a non-null list of bytes. If null, no key is used.</param>
		/// <param name="salt">An optional salt value to customize the hash output. If provided, it must be a non-null list of bytes. If null, no
		/// salt is used.</param>
		/// <param name="personalization">An optional personalization value to further customize the hash output. If provided, it must be a non-null list of
		/// bytes. If null, no personalization is applied.</param>
		public Blake3Config(int hashSizeInBits = DefaultHashSizeInBits, IReadOnlyList<byte> key = null, IReadOnlyList<byte> salt = null, IReadOnlyList<byte> personalization = null)
		{
			HashSizeInBits = hashSizeInBits;
			Key = key;
			Salt = salt;
			Personalization = personalization;
		}

  		/// <summary>
		/// Initializes a new instance of the <see cref="Blake3Config"/> class.
		/// </summary>
		/// <remarks>This constructor creates a default configuration for the Blake3 hashing algorithm.</remarks>
		public Blake3Config()
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Blake3Config"/> class with the same configuration settings as the current
		/// instance.
		/// </summary>
		/// <remarks>The cloned configuration includes the same hash size, key, salt, and personalization values as
		/// the original instance.  Modifications to the cloned instance do not affect the original instance, and vice
		/// versa.</remarks>
		/// <returns>A new <see cref="IBlake3Config"/> instance that is a copy of the current configuration.</returns>
		public IBlake3Config Clone() => new Blake3Config()
		{
			HashSizeInBits = HashSizeInBits,
			Key = Key?.ToArray(),
			Salt = Salt?.ToArray(),
			Personalization = Personalization?.ToArray(),
		};

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			if (Key is byte[] keyArray)
			{
				ArrayHelpers.ZeroFill(keyArray);
			}

			if (Salt is byte[] saltArray)
			{
				ArrayHelpers.ZeroFill(saltArray);
			}

			if (Personalization is byte[] personalizationArray)
			{
				ArrayHelpers.ZeroFill(personalizationArray);
			}

			Key = null;
			Salt = null;
			Personalization = null;
		}
	}

}
