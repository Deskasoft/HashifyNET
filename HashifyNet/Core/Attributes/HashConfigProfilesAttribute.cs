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

using HashifyNet.Core.Utilities;
using System;

namespace HashifyNet.Core
{
	/// <summary>
	/// Specifies the configuration profile types associated with a class.
	/// </summary>
	/// <remarks>This attribute is used to associate one or more configuration profile types with a class. Each
	/// specified type must meet the following criteria: <list type="bullet"> <item><description>It must be a non-abstract
	/// class.</description></item> <item><description>It must be marked with the <see
	/// cref="HashConfigProfileAttribute"/>.</description></item> <item><description>It must implement the <see
	/// cref="IHashConfigBase"/> interface.</description></item> </list></remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal sealed class ConfigProfilesAttribute : Attribute
	{
		public Type[] ProfileTypes { get; }
		public ConfigProfilesAttribute(params Type[] profileTypes)
		{
			ProfileTypes = profileTypes ?? throw new ArgumentNullException(nameof(profileTypes));

			foreach (Type t in ProfileTypes)
			{
				if (t == null)
				{
					throw new ArgumentException($"{nameof(profileTypes)} must not contain null values.", nameof(profileTypes));
				}
				if (!t.IsClass)
				{
					throw new ArgumentException($"{nameof(profileTypes)} must only contain class types.", nameof(profileTypes));
				}
				if (t.IsAbstract)
				{
					throw new ArgumentException($"{nameof(profileTypes)} must not contain abstract class types.", nameof(profileTypes));
				}

				if (!Attribute.IsDefined(t, typeof(HashConfigProfileAttribute), false))
				{
					throw new ArgumentException($"{nameof(profileTypes)} must only contain types marked with {nameof(HashConfigProfileAttribute)}.", nameof(profileTypes));
				}

				if (!t.HasInterface(typeof(IHashConfigBase)))
				{
					throw new ArgumentException($"{nameof(profileTypes)} must only contain types implementing {nameof(IHashConfigBase)}.", nameof(profileTypes));
				}

				if (!t.HasConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, 0))
				{
					throw new ArgumentException($"{nameof(profileTypes)} must only contain types with a public parameterless constructor.", nameof(profileTypes));
				}
			}
		}
	}
}
