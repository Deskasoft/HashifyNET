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

namespace HashifyNet.Algorithms.Argon2id
{
	/// <summary>
	/// Represents the configuration settings for the Argon2id hashing algorithm.
	/// </summary>
	/// <remarks>This interface defines the parameters required to configure the Argon2id hashing algorithm, 
	/// including version, secret key, associated data, and resource usage settings such as iterations,  memory size, and
	/// degree of parallelism. These settings influence the security and performance  characteristics of the hashing
	/// process.</remarks>
	public interface IArgon2idConfig : ICryptographicHashConfig<IArgon2idConfig>
	{
		/// <summary>
		/// Gets the version of the Argon2 algorithm to be used.
		/// For example, Argon2id version 1.3 is represented as 0x13.
		/// </summary>
		Argon2idVersion Version { get; }

		/// <summary>
		/// Gets the secret input for the hashing function.
		/// </summary>
		byte[] Secret { get; }

		/// <summary>
		/// Gets the cryptographic salt.
		/// </summary>
		byte[] Salt { get; }

		/// <summary>
		/// Gets the optional associated data (or "keyed hash"). This data is included in the
		/// hash computation but is not considered secret. It can be used to bind the
		/// hash to a specific context, like a user ID or domain name.
		/// </summary>
		byte[] AssociatedData { get; }

		/// <summary>
		/// Gets the number of iterations (time cost, t). This parameter controls the
		/// execution time by defining the number of passes over the memory.
		/// Increasing this value makes the algorithm slower and more resistant to brute-force attacks.
		/// </summary>
		int Iterations { get; }

		/// <summary>
		/// Gets the memory size in kibibytes (KiB) (memory cost, m). This parameter defines
		/// the amount of memory the algorithm will use. A larger memory size
		/// increases the cost of hashing and provides better resistance against hardware-based attacks.
		/// </summary>
		int MemorySize { get; }

		/// <summary>
		/// Gets the degree of parallelism (parallelism cost, p). This parameter defines
		///  the number of parallel threads (or lanes) to use, influencing how resistant the hash is
		///  to GPU cracking attacks.
		/// </summary>
		int DegreeOfParallelism { get; }
	}
}
