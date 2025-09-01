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
	/// Specifies that a class implements a hash algorithm.
	/// </summary>
	/// <remarks>This attribute is used to annotate classes that provide an implementation of a hash algorithm. It
	/// is intended for internal use and is not inherited by derived classes.</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class HashAlgorithmImplementationAttribute : Attribute
	{
		/// <summary>
		/// Gets the interface type that the current class implements.
		/// </summary>
		/// <remarks>This property is useful for scenarios where reflection or type analysis is required to determine
		/// the specific interface implemented by the class.</remarks>
		public Type ImplementedInterface { get; }

		/// <summary>
		/// Gets the concrete configuration type associated with the current instance.
		/// </summary>
		public Type ConcreteConfig { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HashAlgorithmImplementationAttribute"/> class, specifying the
		/// interface implemented by the hash algorithm and the concrete configuration type.
		/// </summary>
		/// <param name="implementedInterface">The interface type that the hash algorithm implementation adheres to.  This parameter must represent an interface
		/// type.</param>
		/// <param name="concreteConfig">The concrete class type that provides the configuration for the hash algorithm implementation.  This parameter
		/// must represent a class type.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="implementedInterface"/> or <paramref name="concreteConfig"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="implementedInterface"/> is not an interface type,  or if <paramref
		/// name="concreteConfig"/> is not a class type.</exception>
		public HashAlgorithmImplementationAttribute(Type implementedInterface, Type concreteConfig)
		{
			if (implementedInterface == null)
			{
				throw new ArgumentNullException(nameof(implementedInterface), $"{nameof(implementedInterface)} must not be null.");
			}

			string memberName = implementedInterface.FullName;

			if (!implementedInterface.IsInterface)
			{
				throw new ArgumentException($"{memberName}: The provided type must be an interface.", nameof(implementedInterface));
			}

			if (concreteConfig == null)
			{
				throw new ArgumentNullException(nameof(concreteConfig), $"{memberName}: {nameof(concreteConfig)} must not be null.");
			}

			if (!concreteConfig.IsClass)
			{
				throw new ArgumentException($"{memberName}: The provided type for concrete config must be a class.", nameof(concreteConfig));
			}

			if (!implementedInterface.HasInterface(typeof(IHashFunction<>)))
			{
				throw new ArgumentException($"{memberName}: The provided interface type must implement IHashFunction<>.", nameof(implementedInterface));
			}

			if (!concreteConfig.HasInterface(typeof(IHashConfig<>)))
			{
				throw new ArgumentException($"{memberName}: The provided concrete config type must implement IHashConfig<>.", nameof(concreteConfig));
			}

			ImplementedInterface = implementedInterface;
			ConcreteConfig = concreteConfig;
		}
	}
}
