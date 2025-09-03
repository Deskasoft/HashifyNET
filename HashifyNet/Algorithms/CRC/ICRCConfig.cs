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

namespace HashifyNet.Algorithms.CRC
{
	/// <summary>
	/// Defines a configuration for a <see cref="ICRC"/> implementation.
	/// </summary>
	public interface ICRCConfig : IHashConfig<ICRCConfig>
	{
		/// <summary>
		/// Divisor to use when calculating the CRC.
		/// </summary>
		/// <value>
		/// The divisor that will be used when calculating the CRC value.
		/// </value>
		long Polynomial { get; }

		/// <summary>
		/// Value to initialize the CRC register to before calculating the CRC.
		/// </summary>
		/// <value>
		/// The value that will be used to initialize the CRC register before the calculation of the CRC value.
		/// </value>
		long InitialValue { get; }

		/// <summary>
		/// If true, the CRC calculation processes input as big endian bit order.
		/// </summary>
		/// <value>
		/// <c>true</c> if the input should be processed in big endian bit order; otherwise, <c>false</c>.
		/// </value>
		bool ReflectIn { get; }

		/// <summary>
		/// If true, the CRC calculation processes the output as big endian bit order.
		/// </summary>
		/// <value>
		/// <c>true</c> if the CRC calculation processes the output as big endian bit order; otherwise, <c>false</c>.
		/// </value>
		bool ReflectOut { get; }

		/// <summary>
		/// Value to xor with the final CRC value.
		/// </summary>
		/// <value>
		/// The value to xor with the final CRC value.
		/// </value>
		long XOrOut { get; }
	}
}
