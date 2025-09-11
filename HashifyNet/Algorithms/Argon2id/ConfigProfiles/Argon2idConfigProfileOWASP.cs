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
	/// Provides a preconfigured Argon2id hashing configuration that adheres to the OWASP recommendations for memory,
	/// iterations, and parallelism.
	/// </summary>
	/// <remarks>This configuration is designed to balance security and performance based on OWASP guidelines. It
	/// sets the memory size to 19 MiB, the number of iterations to 2, and the degree of parallelism to 1. These values are
	/// suitable for most general-purpose applications but may need adjustment for specific use cases or
	/// environments.</remarks>
	[DefineHashConfigProfile("OWASP", "Provides a preconfigured Argon2id hashing configuration that adheres to the OWASP recommendations for memory, iterations, and parallelism.")]
	public sealed class Argon2idConfigProfileOWASP : Argon2idConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Argon2idConfigProfileOWASP"/> class with default configuration values
		/// recommended by the OWASP Password Storage Cheat Sheet.
		/// </summary>
		/// <remarks>The default configuration values are: <list type="bullet"> <item><description><see
		/// cref="Argon2idConfig.MemorySize"/>: 19 MiB (19456 KiB).</description></item> <item><description><see cref="Argon2idConfig.Iterations"/>:
		/// 2.</description></item> <item><description><see cref="Argon2idConfig.DegreeOfParallelism"/>: 1.</description></item> </list>
		/// These values are suitable for environments with limited resources while maintaining reasonable security.</remarks>
		public Argon2idConfigProfileOWASP()
		{
			MemorySize = 19456;          // 19 MiB
			Iterations = 2;
			DegreeOfParallelism = 1;
		}
	}
}
