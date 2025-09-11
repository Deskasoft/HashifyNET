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

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Represents a predefined configuration profile for the Argon2id key derivation function, optimized for high memory
	/// usage in compliance with IETF recommendations.
	/// </summary>
	/// <remarks>This configuration is tailored for scenarios where high memory availability is prioritized to
	/// enhance resistance against memory-hard attacks. The default settings are: <list type="bullet">
	/// <item><description><see cref="Argon2idConfig.MemorySize"/>: 2 GiB (2097152 KiB)</description></item> <item><description><see
	/// cref="Argon2idConfig.Iterations"/>: 1</description></item> <item><description><see cref="Argon2idConfig.DegreeOfParallelism"/>:
	/// 4</description></item> </list> These values are suitable for environments requiring strong security guarantees and
	/// where memory resources are not constrained.</remarks>
	[DefineHashConfigProfile("IETF/HighMemory", "Represents a predefined configuration profile for the Argon2id key derivation function, optimized for high memory usage in compliance with IETF recommendations.")]
	public sealed class Argon2idConfigProfileIETFHighMemory : Argon2idConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Argon2idConfigProfileIETFHighMemory"/> class with default configuration
		/// values optimized for high memory usage.
		/// </summary>
		/// <remarks>This configuration is designed to comply with the IETF Argon2id recommendations for high memory
		/// usage scenarios. The default values are: <list type="bullet"> <item><description><see cref="Argon2idConfig.MemorySize"/>: 2 GiB
		/// (2097152 KiB)</description></item> <item><description><see cref="Argon2idConfig.Iterations"/>: 1</description></item>
		/// <item><description><see cref="Argon2idConfig.DegreeOfParallelism"/>: 4</description></item> </list> These defaults are suitable
		/// for environments where memory availability is high and security requirements prioritize resistance to memory-hard
		/// attacks.</remarks>
		public Argon2idConfigProfileIETFHighMemory()
		{
			MemorySize = 2097152;        // 2 GiB
			Iterations = 1;
			DegreeOfParallelism = 4;
		}
	}
}
