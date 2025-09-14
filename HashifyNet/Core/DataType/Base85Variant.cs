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
	/// Specifies the supported Base85 encoding variants.
	/// </summary>
	/// <remarks>Base85 encoding is a binary-to-text encoding scheme that represents binary data in an ASCII-safe
	/// format. This enumeration defines the available variants of Base85 encoding, each with its own specific use case and
	/// encoding rules: <list type="bullet"> <item> <term><see cref="Ascii85"/></term> <description>The Adobe standard used
	/// in PostScript and PDF, with '&lt;~' and '~&gt;' delimiters.</description> </item> <item> <term><see
	/// cref="Z85"/></term> <description>The ZeroMQ standard (RFC 32/Z85), designed to be safe for inclusion in source
	/// code.</description> </item> <item> <term><see cref="Rfc1924"/></term> <description>The variant defined in RFC 1924,
	/// originally created for encoding IPv6 addresses.</description> </item> </list></remarks>
	public enum Base85Variant
	{
		/// <summary>
		/// The Adobe standard used in PostScript and PDF, with '&lt;~' and '~&gt;' delimiters.
		/// </summary>
		Ascii85,

		/// <summary>
		/// The ZeroMQ standard (RFC 32/Z85), designed to be safe for source code.
		/// </summary>
		Z85,

		/// <summary>
		/// The standard defined in RFC 1924, originally for IPv6 addresses.
		/// </summary>
		Rfc1924
	}
}
