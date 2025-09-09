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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HashifyNet
{
	/// <summary>
	/// Provides a factory for creating instances of hash function implementations.
	/// </summary>
	/// <remarks>The <see cref="HashFactory"/> class is responsible for discovering, validating, and registering
	/// hash algorithm implementations at runtime. It ensures that only valid implementations are available for use and
	/// provides methods to create instances of these implementations with default or custom configurations.
	/// <para> Hash algorithm implementations must meet specific criteria to be registered, including being
	/// decorated with the <see cref="Core.HashAlgorithmImplementationAttribute"/> and providing a public constructor that
	/// accepts a single parameter of type <c>IHashConfig&lt;&gt;</c>.</para></remarks>
	public sealed partial class HashFactory : IHashFactory
	{
		private static HashFactory _singleton = null;
		private static HashFactory Singleton
		{
			get
			{
				if (_singleton == null)
				{
					_singleton = new HashFactory();
				}
				return _singleton;
			}
		}

		private static readonly Hashtable _concreteConfigTypes;
		private static readonly Hashtable _implementations;
		/// <summary>
		/// Initializes the static state of the <see cref="HashFactory"/> class by discovering and registering available hash
		/// algorithm implementations.
		/// </summary>
		/// <remarks>This static constructor scans for types marked with the <see
		/// cref="Core.HashAlgorithmImplementationAttribute"/>  and validates their compatibility as hash algorithm
		/// implementations. It ensures that at least one valid  implementation is found and registers them for use. If no
		/// valid implementations are found, an  <see cref="InvalidOperationException"/> is thrown. <para> Each implementation
		/// must meet the following criteria: <list type="bullet"> <item>Be decorated with the <see
		/// cref="Core.HashAlgorithmImplementationAttribute"/>.</item> <item>Provide a public constructor that accepts a
		/// single parameter of type <c>IHashConfig&lt;&gt;</c>.</item> </list> </para> The registered implementations are
		/// stored in an internal lookup table for later retrieval.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if no hash algorithm implementations are found or if no valid implementations meet the required criteria.</exception>
		static HashFactory()
		{
			_implementations = new Hashtable();
			_concreteConfigTypes = new Hashtable();

			Type[] types = ReflectionHelper.GetClasses(typeof(Core.HashAlgorithmImplementationAttribute), false);
			if (types == null || types.Length < 1)
			{
				throw new InvalidOperationException("No hash algorithm implementations found.");
			}

			List<Tuple<Type, Core.HashAlgorithmImplementationAttribute>> validTypes = new List<Tuple<Type, Core.HashAlgorithmImplementationAttribute>>();
			foreach (Type type in types)
			{
				if (type.GetCustomAttribute<ObsoleteAttribute>() != null)
				{
					continue;
				}

				IEnumerable<Core.HashAlgorithmImplementationAttribute> attrs = type.GetCustomAttributes<Core.HashAlgorithmImplementationAttribute>(false);
				if (attrs == null)
				{
					continue;
				}

				bool exists = false;
				foreach (Core.HashAlgorithmImplementationAttribute attr in attrs)
				{
					exists = true;
					break;
				}

				if (!exists)
				{
					continue;
				}

				if (!type.HasConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, 1, new Type[] { typeof(IHashConfigBase) }))
				{
					continue;
				}

				Core.HashAlgorithmImplementationAttribute implattr = attrs.ElementAt(0);
				validTypes.Add(new Tuple<Type, Core.HashAlgorithmImplementationAttribute>(type, implattr));
			}

			if (validTypes.Count < 1)
			{
				throw new InvalidOperationException("No valid hash algorithm implementations found.");
			}

			foreach (Tuple<Type, Core.HashAlgorithmImplementationAttribute> t in validTypes)
			{
				ConstructorInfo factoryCtor = t.Item1.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, 1, new Type[] { typeof(IHashConfigBase) });
				Func<object[], object> factory = ReflectionHelper.CreateInstanceWithParameters(factoryCtor, factoryCtor.GetParameters()[0].ParameterType);

				Type configType = t.Item2.ConcreteConfig;

				_concreteConfigTypes.Add(t.Item2.ImplementedInterface, configType);

				// If the concrete config has no parameterless constructor, we cannot create a default instance. In this case, the parameterless Create function will throw.
				Func<object[], object> configFactory = null;
				ConstructorInfo configCtor = configType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, 0);
				if (configCtor != null)
				{
					configFactory = ReflectionHelper.CreateInstanceWithParameters(configCtor);
				}

				_implementations.Add(t.Item2.ImplementedInterface, Tuple.Create<Func<object[], object>, Func<object[], object>>(factory, configFactory));
			}
		}

		private HashFactory()
		{
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="type"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentNullException"><inheritdoc/></exception>
		/// <exception cref="ArgumentException"><inheritdoc/></exception>
		/// <exception cref="NotImplementedException"><inheritdoc/></exception>
		public IHashFunctionBase CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (!type.IsInterface)
			{
				throw new ArgumentException($"The provided type '{type.FullName}' is not an interface.", nameof(type));
			}

			if (!typeof(IHashFunctionBase).IsAssignableFrom(type))
			{
				throw new ArgumentException($"The provided type '{type.FullName}' does not implement IHashFunctionBase.", nameof(type));
			}

			if (!_implementations.ContainsKey(type))
			{
				throw new NotImplementedException($"No implementation found for type {type.FullName}.");
			}

			Tuple<Func<object[], object>, Func<object[], object>> factory = (Tuple<Func<object[], object>, Func<object[], object>>)_implementations[type];
			if (factory.Item2 == null)
			{
				throw new NotImplementedException($"No default configuration available for type {type.FullName}.");
			}

			return (IHashFunctionBase)factory.Item1(new object[] { factory.Item2(null) });
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="type"><inheritdoc/></param>
		/// <param name="config"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentNullException"><inheritdoc/></exception>
		/// <exception cref="ArgumentException"><inheritdoc/></exception>
		/// <exception cref="NotImplementedException"><inheritdoc/></exception>
		public IHashFunctionBase CreateInstance(Type type, IHashConfigBase config)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (!type.IsInterface)
			{
				throw new ArgumentException($"The provided type '{type.FullName}' is not an interface.", nameof(type));
			}

			if (!typeof(IHashFunctionBase).IsAssignableFrom(type))
			{
				throw new ArgumentException($"The provided type '{type.FullName}' does not implement IHashFunctionBase.", nameof(type));
			}

			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			if (!_implementations.ContainsKey(type))
			{
				throw new NotImplementedException($"No implementation found for type {type.FullName}.");
			}

			Tuple<Func<object[], object>, Func<object[], object>> factory = (Tuple<Func<object[], object>, Func<object[], object>>)_implementations[type];
			return (IHashFunctionBase)factory.Item1(new object[] { config });
		}
	}
}
