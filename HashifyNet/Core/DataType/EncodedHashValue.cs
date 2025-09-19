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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace HashifyNet.Core.Utilities
{
	/// <summary>
	/// Implementation of <see cref="IHashValue"/> with an encoded value.
	/// </summary>
	public sealed class EncodedHashValue
		: HashValue, IEncodedHashValue
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public ImmutableArray<byte> EncodedHash { get; }

		private readonly Func<IEnumerable<byte>, string> _decodeOp;
		/// <summary>
		/// Initializes a new instance of the <see cref="EncodedHashValue"/> class with an encoded hash value and its bit length.
		/// </summary>
		/// <param name="endianness">The endianness of the hash value.</param>
		/// <param name="encodedHash">The encoded hash value as a sequence of bytes.</param>
		/// <param name="decodeOp">The function to be called for decoding the encoded hash into a string.</param>
		/// <param name="actualHash">The hash value as a sequence of bytes.</param>
		/// <param name="bitLength">The length of the hash value in bits.</param>
		public EncodedHashValue(ValueEndianness endianness, IEnumerable<byte> encodedHash, Func<IEnumerable<byte>, string> decodeOp, IEnumerable<byte> actualHash, int bitLength) : base(endianness, actualHash, bitLength)
		{
			_ = encodedHash ?? throw new ArgumentNullException(nameof(encodedHash));
			_decodeOp = decodeOp ?? throw new ArgumentNullException(nameof(decodeOp));

			EncodedHash = encodedHash.ToImmutableArray();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public string Decode()
		{
			return _decodeOp(EncodedHash.ToArray());
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="bitLength"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public override IHashValue Coerce(int bitLength)
		{
			IHashValue val = base.Coerce(bitLength);
			return new EncodedHashValue(Endianness, EncodedHash, _decodeOp, val.AsByteArray(), bitLength);
		}
	}
}