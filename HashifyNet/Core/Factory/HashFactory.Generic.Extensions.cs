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
using System.Collections.Generic;

namespace HashifyNet
{
	public sealed partial class HashFactory<TAlgorithmInterface, TConfig>
	{
		/// <summary>
		/// <inheritdoc cref="HashFactory{TAlgorithmInterface, TConfig}.CreateInstance()"/>
		/// </summary>
		/// <returns><inheritdoc cref="HashFactory{TAlgorithmInterface, TConfig}.CreateInstance()"/></returns>
		public static TAlgorithmInterface Create()
		{
			return Singleton.CreateInstance();
		}

		/// <summary>
		/// <inheritdoc cref="HashFactory{TAlgorithmInterface, TConfig}.CreateInstance(TConfig)"/>
		/// </summary>
		/// <param name="config"><inheritdoc cref="HashFactory{TAlgorithmInterface, TConfig}.CreateInstance(TConfig)"/></param>
		/// <returns><inheritdoc cref="HashFactory{TAlgorithmInterface, TConfig}.CreateInstance(TConfig)"/></returns>
		public static TAlgorithmInterface Create(TConfig config)
		{
			return Singleton.CreateInstance(config);
		}

		/// <summary>
		/// Retrieves the configuration profiles associated with <typeparamref name="TAlgorithmInterface"/> algorithm.
		/// </summary>
		/// <returns>An array of <see cref="IHashConfigProfile"/> objects associated with <typeparamref name="TAlgorithmInterface"/> algorithm, or an empty array
		/// if no configuration profiles are found for the specified type.</returns>
		public static IHashConfigProfile[] GetConfigProfiles()
		{
			return HashFactory.GetConfigProfiles(typeof(TAlgorithmInterface));
		}

		/// <summary>
		/// Creates a default concrete configuration instance for <typeparamref name="TAlgorithmInterface"/> algorithm.
		/// </summary>
		/// <returns>An instance of <typeparamref name="TAlgorithmInterface"/> representing the default configuration for <typeparamref name="TAlgorithmInterface"/> algorithm.</returns>
		/// <exception cref="KeyNotFoundException">Thrown if no implementation is registered for <typeparamref name="TAlgorithmInterface"/> algorithm.</exception>
		/// <exception cref="NotSupportedException">Thrown if no default configuration is available for <typeparamref name="TAlgorithmInterface"/> algorithm.</exception>
		public static TConfig CreateDefaultConcreteConfig()
		{
			return (TConfig)HashFactory.CreateDefaultConcreteConfig(typeof(TAlgorithmInterface));
		}

		/// <summary>
		/// Attempts to create a default concrete configuration for <typeparamref name="TAlgorithmInterface"/> algorithm.
		/// </summary>
		/// <remarks>This method catches and handles <see cref="NotSupportedException"/> and <see
		/// cref="KeyNotFoundException"/> internally, returning <see langword="false"/> in such cases. Other exceptions may
		/// propagate to the caller.</remarks>
		/// <param name="config">When this method returns, contains the created configuration if the operation succeeds; otherwise, <see
		/// langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the default concrete configuration was successfully created; otherwise, <see
		/// langword="false"/>.</returns>
		public static bool TryCreateDefaultConcreteConfig(out TConfig config)
		{
			bool result = HashFactory.TryCreateDefaultConcreteConfig(typeof(TAlgorithmInterface), out IHashConfigBase c);
			if (c != null)
			{
				config = (TConfig)c;
			}
			else
			{
				config = default;
			}

			return result;
		}
	}
}
