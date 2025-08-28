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

namespace HashifyNet.Algorithms.Tiger
{
	/// <summary>
	/// Concrete implementation of the Tiger configuration.
	/// </summary>
	public class TigerConfig : ITigerConfig
	{
		/// <summary>
		/// Gets or sets the size of the hash, in bits. The default is 192 bits.
		/// </summary>
		public int HashSizeInBits { get; set; } = 192;
		/// <summary>
		/// Gets or sets the number of passes to be performed. The default is 4 passes.
		/// </summary>
		public int Passes { get; set; } = 4;

		/// <summary>
		/// Creates a new instance of the <see cref="TigerConfig"/> class with the same configuration settings as the current
		/// instance.
		/// </summary>
		/// <remarks>The cloned instance will have the same values for <see cref="HashSizeInBits"/> and <see
		/// cref="Passes"/> as the original instance.</remarks>
		/// <returns>A new <see cref="ITigerConfig"/> instance that is a copy of the current configuration.</returns>
		public ITigerConfig Clone() => new TigerConfig()
		{
			HashSizeInBits = this.HashSizeInBits,
			Passes = this.Passes,
		};

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			// No resources to dispose
		}
	}
}