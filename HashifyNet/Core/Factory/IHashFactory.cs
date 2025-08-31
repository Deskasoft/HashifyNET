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
	/// Defines the base contract for creating hash algorithm instances.
	/// </summary>
	/// <remarks>This interface serves as a foundation for factories that produce hash algorithm objects.
	/// Implementations of this interface are expected to provide specific hash algorithm instances tailored to their use
	/// cases.</remarks>
	public interface IHashFactory
	{
		/// <summary>
		/// Creates an instance of the specified hash function type.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the hash function to create. Must implement <see cref="IHashFunctionBase"/>.</param>
		/// <returns>An instance of the specified hash function type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="type"/> does not implement <see cref="IHashFunctionBase"/>.</exception>
		/// <exception cref="NotImplementedException">Thrown if no implementation is registered for the specified <paramref name="type"/> or if no default
		/// configuration is available for the specified type.</exception>
		IHashFunctionBase CreateInstance(Type type);

		/// <summary>
		/// Creates an instance of a hash function implementation based on the specified type and configuration.
		/// </summary>
		/// <param name="type">The type of the hash function to create. Must implement <see cref="IHashFunctionBase"/>.</param>
		/// <param name="config">The configuration object used to initialize the hash function.</param>
		/// <returns>An instance of <see cref="IHashFunctionBase"/> corresponding to the specified type and configuration.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is <see langword="null"/> or if <paramref name="config"/> is <see
		/// langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="type"/> does not implement <see cref="IHashFunctionBase"/>.</exception>
		/// <exception cref="NotImplementedException">Thrown if no implementation is registered for the specified <paramref name="type"/>.</exception>
		IHashFunctionBase CreateInstance(Type type, IHashConfigBase config);
	}
}
