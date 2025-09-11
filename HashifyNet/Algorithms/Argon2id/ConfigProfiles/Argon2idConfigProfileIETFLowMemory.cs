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
	/// Represents a preconfigured Argon2id hashing configuration optimized for low memory usage, adhering to the IETF
	/// recommended settings.
	/// </summary>
	/// <remarks>This configuration is designed for scenarios where memory usage is constrained, while still
	/// maintaining a balance between security and performance. It sets the memory size to 64 MiB, the number of iterations
	/// to 3, and the degree of parallelism to 4.</remarks>
	[DefineHashConfigProfile("IETF/LowMemory", "Represents a preconfigured Argon2id hashing configuration optimized for low memory usage, adhering to the IETF recommended settings.")]
	public sealed class Argon2idConfigProfileIETFLowMemory : Argon2idConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Argon2idConfigProfileIETFLowMemory"/> class with default configuration
		/// values optimized for low memory usage.
		/// </summary>
		/// <remarks>This constructor sets the following default values: <list type="bullet"> <item><description><see
		/// cref="Argon2idConfig.MemorySize"/>: 64 MiB (65536 KiB)</description></item> <item><description><see cref="Argon2idConfig.Iterations"/>:
		/// 3</description></item> <item><description><see cref="Argon2idConfig.DegreeOfParallelism"/>: 4</description></item> </list> These
		/// defaults are suitable for environments with constrained memory resources while maintaining reasonable security and
		/// performance.</remarks>
		public Argon2idConfigProfileIETFLowMemory()
		{
			MemorySize = 65536;           // 64 MiB
			Iterations = 3;
			DegreeOfParallelism = 4;
		}
	}
}
