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

using System;

namespace HashifyNet
{
	/// <summary>
	/// Defines a factory for creating instances of a hash algorithm of type <typeparamref name="TAlgorithmInterface"/> with a
	/// specified configuration of type <typeparamref name="TConfig"/>.
	/// </summary>
	/// <remarks>This interface provides methods to create instances of <typeparamref name="TAlgorithmInterface"/> either
	/// with a default configuration  or with a user-specified configuration of type <typeparamref name="TConfig"/>. It is
	/// intended to abstract the creation process  of hash algorithms, allowing for flexibility and
	/// configurability.</remarks>
	/// <typeparam name="TAlgorithmInterface">The type of the hash algorithm to create. Must implement <see cref="IHashFunctionBase"/>.</typeparam>
	/// <typeparam name="TConfig">The type of the configuration used by the hash algorithm. Must implement <see cref="IHashConfigBase"/>.</typeparam>
	public interface IGenericHashFactory<out TAlgorithmInterface, in TConfig> where TAlgorithmInterface : IHashFunctionBase
		where TConfig : IHashConfigBase
	{
		/// <summary>
		/// Creates a new <typeparamref name="TAlgorithmInterface"/> instance with the default configuration.
		/// </summary>
		/// <returns>A new <typeparamref name="TAlgorithmInterface"/> instance.</returns>
		TAlgorithmInterface CreateInstance();

		/// <summary>
		/// Creates a new <typeparamref name="TAlgorithmInterface"/> instance with given configuration.
		/// </summary>
		/// <param name="config">The configuration to use. This cannot be <see langword="null"/>.</param>
		/// <returns>A new <typeparamref name="TAlgorithmInterface"/> instance.</returns>
		TAlgorithmInterface CreateInstance(TConfig config);
	}
}
