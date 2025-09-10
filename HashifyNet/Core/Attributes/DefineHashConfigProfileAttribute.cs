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

namespace HashifyNet.Core
{
	/// <summary>
	/// Marks a class as a standard, discoverable hash configuration.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal sealed class DefineHashConfigProfileAttribute : Attribute
	{
		private const int MaxNameLength = 64;
		private const int MaxDescriptionLength = 2048;

		public string Name { get; }
		public string Description { get; }

		public DefineHashConfigProfileAttribute(string name, string description = null
#if NET8_0_OR_GREATER
			!
#endif
			)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The config profile name must be a non-empty and non-whitespace string.", nameof(name));
			}

			if (name.Length > MaxNameLength)
			{
				throw new ArgumentException($"The config profile name must not exceed {MaxNameLength} characters in length.", nameof(name));
			}

			if (description != null && description.Length > MaxDescriptionLength)
			{
				throw new ArgumentException($"The config profile description must not exceed {MaxDescriptionLength} characters in length.", nameof(description));
			}

			Name = name;
			Description = description;
		}
	}
}

