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

namespace HashifyNet.Algorithms.MetroHash
{
	/// <summary>
	/// Implementation of MetroHash64 and MetroHash128 as specified at https://github.com/jandrewrogers/MetroHash.
	/// 
	/// "
	/// MetroHash is a set of state-of-the-art hash functions for non-cryptographic use cases. 
	/// They are notable for being algorithmically generated in addition to their exceptional performance. 
	/// The set of published hash functions may be expanded in the future, 
	/// having been selected from a very large set of hash functions that have been constructed this way.
	/// "
	/// </summary>
	public interface IMetroHash
		: IStreamableHashFunction<IMetroHashConfig>
	{
	}
}