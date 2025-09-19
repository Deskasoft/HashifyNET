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

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Represents the configuration settings for the Argon2id key derivation function.
	/// </summary>
	/// <remarks>This class provides properties to configure the behavior of the Argon2id algorithm, including
	/// memory usage, iteration count, and parallelism. It also includes predefined configurations recommended by OWASP and
	/// IETF RFC 9106 for various use cases, such as memory-constrained or high-memory systems.</remarks>
	[DeclareHashConfigProfile(typeof(Argon2idConfigProfileOWASP))]
	[DeclareHashConfigProfile(typeof(Argon2idConfigProfileIETFLowMemory))]
	[DeclareHashConfigProfile(typeof(Argon2idConfigProfileIETFHighMemory))]
	public class Argon2idConfig : IArgon2idConfig
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public Argon2idVersion Version { get; set; } = Argon2idVersion.Version13;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int HashSizeInBits { get; set; } = 512;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public byte[] Secret { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public byte[] Salt { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public byte[] AssociatedData { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int Iterations { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int MemorySize { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int DegreeOfParallelism { get; set; }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IArgon2idConfig Clone() => new Argon2idConfig()
		{
			HashSizeInBits = this.HashSizeInBits,
			Secret = this.Secret == null ? null : (byte[])this.Secret.Clone(),
			AssociatedData = this.AssociatedData == null ? null : (byte[])this.AssociatedData.Clone(),
			Iterations = this.Iterations,
			MemorySize = this.MemorySize,
			DegreeOfParallelism = this.DegreeOfParallelism,
			Salt = this.Salt == null ? null : (byte[])this.Salt.Clone(),
		};

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			if (Secret != null)
			{
				ArrayHelpers.ZeroFill(Secret);
				Secret = null;
			}

			if (AssociatedData != null) // Not necessary for this one but anyway.
			{
				ArrayHelpers.ZeroFill(AssociatedData);
				AssociatedData = null;
			}

			if (Salt != null) // Not necessary for this one but anyway.
			{
				ArrayHelpers.ZeroFill(Salt);
				Salt = null;
			}
		}
	}
}
