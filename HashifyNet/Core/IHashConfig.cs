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

namespace HashifyNet
{
	/// <summary>
	/// Defines the configuration for a hash algorithm, including the hash size and the ability to create a copy of the
	/// configuration.
	/// </summary>
	/// <typeparam name="CName">The type of the hash configuration, which must implement <see cref="IHashConfig{CName}"/>.</typeparam>
	public interface IHashConfig<CName> : IHashConfigBase where CName : IHashConfig<CName>
	{
		/// <summary>
		/// Creates a new instance of the <typeparamref name="CName"/> object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new <typeparamref name="CName"/> object that is identical to the current instance.</returns>
		CName Clone();
	}
}
