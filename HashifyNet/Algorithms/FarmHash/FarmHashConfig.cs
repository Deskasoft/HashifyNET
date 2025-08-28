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

namespace HashifyNet.Algorithms.FarmHash
{
	/// <summary>
	/// Represents the configuration settings for the FarmHash algorithm.
	/// </summary>
	/// <remarks>This class allows customization of the hash size used by the FarmHash algorithm. The default hash
	/// size is  64 bits, but it can be configured to 32, 64, or 128 bits. The configuration can be cloned to create a new
	/// instance with the same settings.</remarks>
	public class FarmHashConfig : IFarmHashConfig
	{
		/// <summary>
		/// Gets or sets the desired hash size, in bits.
		/// </summary>
		/// <value>
		/// The desired hash size, in bits.
		/// </value>
		/// <remarks>
		/// Defaults to <c>64</c>. Implementation expects the value to be <c>32</c>, <c>64</c>, or <c>128</c>.
		/// </remarks>
		public int HashSizeInBits { get; set; } = 64;

		/// <summary>
		/// Creates a new instance of <see cref="IFarmHashConfig"/> with the same configuration settings as the current
		/// instance.
		/// </summary>
		/// <returns>A new <see cref="IFarmHashConfig"/> instance that is a copy of the current configuration.</returns>
		public IFarmHashConfig Clone()
		{
			return new FarmHashConfig()
			{
				HashSizeInBits = this.HashSizeInBits,
			};
		}
	}
}
