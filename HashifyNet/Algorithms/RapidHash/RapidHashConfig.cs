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

namespace HashifyNet.Algorithms.RapidHash
{
	/// <summary>
	/// Represents the configuration settings for the RapidHash algorithm.
	/// </summary>
	/// <remarks>This class provides options to configure the behavior of the RapidHash algorithm, including the
	/// hash size and an optional seed value. It also supports cloning to create independent configuration
	/// instances.</remarks>
	public class RapidHashConfig : IRapidHashConfig
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int HashSizeInBits => 64;

		/// <summary>
		/// <inheritdoc/>
		/// <para>Defaults to 0.</para>
		/// </summary>
		public ulong Seed { get; set; } = 0;

		/// <summary>
		/// <inheritdoc/>
		/// <para>Defaults to <see cref="RapidHashMode.Original"/>.</para>
		/// </summary>
		public RapidHashMode Mode { get; set; } = RapidHashMode.Original;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public IRapidHashConfig Clone() => new RapidHashConfig() { Seed = this.Seed, Mode = this.Mode };
	}
}
