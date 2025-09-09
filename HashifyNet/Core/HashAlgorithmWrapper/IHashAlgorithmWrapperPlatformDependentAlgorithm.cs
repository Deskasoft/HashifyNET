#if NET8_0_OR_GREATER
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
	/// Any hash algorithm deriving from this points to an existing underlying platform-dependent .NET implementation that gets wrapped by HashifyNET.
	/// </summary>
	public interface IHashAlgorithmWrapperPlatformDependentAlgorithm<TAlgorithm> : IHashAlgorithmWrapperAlgorithm<TAlgorithm> where TAlgorithm : System.Security.Cryptography.HashAlgorithm
	{
		/// <summary>
		/// Gets a value that indicates whether the algorithm is supported on the current platform.
		/// </summary>
		static bool IsSupported
		{
			get
			{
				Type currentType = typeof(TAlgorithm);
				if (currentType == typeof(System.Security.Cryptography.HMACSHA3_256))
				{
					return System.Security.Cryptography.HMACSHA3_256.IsSupported;
				}
				else if (currentType == typeof(System.Security.Cryptography.HMACSHA3_384))
				{
					return System.Security.Cryptography.HMACSHA3_384.IsSupported;
				}
				else if (currentType == typeof(System.Security.Cryptography.HMACSHA3_512))
				{
					return System.Security.Cryptography.HMACSHA3_512.IsSupported;
				}
				else if (currentType == typeof(System.Security.Cryptography.SHA3_256))
				{
					return System.Security.Cryptography.SHA3_256.IsSupported;
				}
				else if (currentType == typeof(System.Security.Cryptography.SHA3_384))
				{
					return System.Security.Cryptography.SHA3_384.IsSupported;
				}
				else if (currentType == typeof(System.Security.Cryptography.SHA3_512))
				{
					return System.Security.Cryptography.SHA3_512.IsSupported;
				}

				return true;
			}
		}
	}
}
#endif
