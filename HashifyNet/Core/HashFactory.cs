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
	/// Provides a factory for creating instances of hash algorithms that implement the specified algorithm interface and
	/// configuration type.
	/// </summary>
	/// <remarks>This factory is designed to create instances of hash algorithms that conform to the specified
	/// interface and configuration type. It supports creating algorithms with default configurations or with a
	/// user-specified configuration.</remarks>
	/// <typeparam name="TAlgorithmInterface">The interface type that the hash algorithm implements. This must be a type that implements <see
	/// cref="IHashFunction{TConfig}"/>.</typeparam>
	/// <typeparam name="TConfig">The type of the configuration object used to configure the hash algorithm. This must be a type that implements <see
	/// cref="IHashConfig{TConfig}"/>.</typeparam>
	public sealed class HashFactory<TAlgorithmInterface, TConfig> : IHashAlgorithmFactory<TAlgorithmInterface, TConfig> where TAlgorithmInterface : IHashFunction<TConfig>
		where TConfig : IHashConfig<TConfig>
	{
		/// <summary>
		/// Represents a singleton instance of the <see cref="HashFactory{TAlgorithmInterface, TConfig}"/> class.
		/// </summary>
		/// <remarks>This static field holds the single, shared instance of the <see
		/// cref="HashFactory{TAlgorithmInterface, TConfig}"/>  for the application. It is initialized and managed internally
		/// to ensure only one instance exists.</remarks>
		private static HashFactory<TAlgorithmInterface, TConfig> _singleton = null;

		/// <summary>
		/// Gets the singleton instance of the <see cref="HashFactory{TAlgorithmInterface, TConfig}"/> class.
		/// </summary>
		/// <remarks>This property implements the singleton pattern, ensuring that only one instance of the factory is
		/// created and shared across the application. Accessing this property is thread-safe after the first
		/// initialization.</remarks>
		public static HashFactory<TAlgorithmInterface, TConfig> Instance
		{
			get
			{
				if (_singleton == null)
				{
					_singleton = new HashFactory<TAlgorithmInterface, TConfig>();
				}

				return _singleton;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="NotImplementedException"><inheritdoc/></exception>
		public TAlgorithmInterface Create()
		{
			return (TAlgorithmInterface)HashFactory.Instance.Create(typeof(TAlgorithmInterface));
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="config"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="NotImplementedException"><inheritdoc/></exception>
		public TAlgorithmInterface Create(TConfig config)
		{
			return (TAlgorithmInterface)HashFactory.Instance.Create(typeof(TAlgorithmInterface), config);
		}
	}

	/// <summary>
	/// Provides a factory for creating instances of hash algorithms that implement the specified interface or class.
	/// </summary>
	/// <remarks>The <see cref="HashFactory{TAlgorithmInterface}"/> class is a generic factory designed to create
	/// instances of hash algorithms  that conform to the specified interface or class. It ensures that only one instance
	/// of the factory is created and shared  across the application through the <see cref="Instance"/> property. This
	/// class supports thread-safe access to the singleton  instance after initialization.</remarks>
	/// <typeparam name="TAlgorithmInterface">The type of the hash algorithm interface or class that this factory creates instances of.  This type must be
	/// supported by the underlying factory.</typeparam>
	public sealed class HashFactory<TAlgorithmInterface> : IHashAlgorithmFactoryBase where TAlgorithmInterface : IHashFunctionBase
	{
		/// <summary>
		/// Represents a singleton instance of the <see cref="HashFactory{TAlgorithmInterface}"/> class.
		/// </summary>
		/// <remarks>This static field holds the single, shared instance of the <see
		/// cref="HashFactory{TAlgorithmInterface}"/> class. It is initialized to <see langword="null"/> and should be
		/// accessed or initialized in a thread-safe manner.</remarks>
		private static HashFactory<TAlgorithmInterface> _singleton = null;

		/// <summary>
		/// Gets the singleton instance of the <see cref="HashFactory{TAlgorithmInterface}"/> class.
		/// </summary>
		/// <remarks>This property ensures that only one instance of the <see
		/// cref="HashFactory{TAlgorithmInterface}"/> class is created and shared across the application. Accessing this
		/// property is thread-safe after the instance has been initialized.</remarks>
		public static HashFactory<TAlgorithmInterface> Instance
		{
			get
			{
				if (_singleton == null)
				{
					_singleton = new HashFactory<TAlgorithmInterface>();
				}

				return _singleton;
			}
		}

		/// <summary>
		/// Creates a new <typeparamref name="TAlgorithmInterface"/> instance with the default configuration.
		/// </summary>
		/// <returns>A new <typeparamref name="TAlgorithmInterface"/> instance.</returns>
		public TAlgorithmInterface Create()
		{
			return (TAlgorithmInterface)HashFactory.Instance.Create(typeof(TAlgorithmInterface));
		}

		/// <summary>
		/// Creates a new <typeparamref name="TAlgorithmInterface"/> instance with given configuration.
		/// </summary>
		/// <param name="config">The configuration to use. This cannot be <see langword="null"/>.</param>
		/// <returns>A new <typeparamref name="TAlgorithmInterface"/> instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> is <see langword="null"/>.</exception>
		public TAlgorithmInterface Create(IHashConfigBase config)
		{
			return (TAlgorithmInterface)HashFactory.Instance.Create(typeof(TAlgorithmInterface), config);
		}
	}

	/// <summary>
	/// Provides a factory for creating instances of hash function implementations.
	/// </summary>
	/// <remarks>The <see cref="HashFactory"/> class is responsible for discovering, validating, and registering
	/// hash algorithm implementations at runtime. It ensures that only valid implementations are available for use and
	/// provides methods to create instances of these implementations with default or custom configurations. <para> This
	/// class follows the singleton pattern, and its instance can be accessed via the <see cref="Instance"/> property.
	/// </para> <para> Hash algorithm implementations must meet specific criteria to be registered, including being
	/// decorated with the <see cref="Core.HashAlgorithmImplementationAttribute"/> and providing a public constructor that
	/// accepts a single parameter of type <c>IHashConfig&lt;&gt;</c>. </para></remarks>
	public sealed class HashFactory : IHashAlgorithmFactoryBase
	{
		private static HashFactory _singleton = null;
		/// <summary>
		/// Gets the singleton instance of the <see cref="HashFactory"/> class.
		/// </summary>
		public static HashFactory Instance
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

			Type[] types = ReflectionHelper.GetClasses(typeof(Core.HashAlgorithmImplementationAttribute), false);
			if (types == null || types.Length < 1)
			{
				throw new InvalidOperationException("No hash algorithm implementations found.");
			}

			List<Tuple<Type, Core.HashAlgorithmImplementationAttribute>> validTypes = new List<Tuple<Type, Core.HashAlgorithmImplementationAttribute>>();
			foreach (Type type in types)
			{
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

		/// <summary>
		/// Initializes a new instance of the <see cref="HashFactory"/> class.
		/// </summary>
		/// <remarks>This constructor is private to prevent direct instantiation of the <see cref="HashFactory"/>
		/// class.  Use the provided factory methods to create instances as needed.</remarks>
		private HashFactory()
		{
		}

		/// <summary>
		/// Retrieves all available hash algorithm types registered in the system.
		/// </summary>
		/// <remarks>The returned array contains the types of all registered hash algorithm implementations. If no
		/// hash algorithms are registered, the method returns <see langword="null"/>.</remarks>
		/// <returns>An array of <see cref="Type"/> objects representing the registered hash algorithm types, or <see
		/// langword="null"/> if no hash algorithms are available.</returns>
		public Type[] GetAllAvailableHashAlgorithms()
		{
			if (_implementations.Count < 1)
			{
				return null;
			}

			Type[] types = new Type[_implementations.Keys.Count];
			_implementations.Keys.CopyTo(types, 0);
			return types;
		}

		/// <summary>
		/// Creates an instance of the specified hash function type.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the hash function to create. Must implement <see cref="IHashFunctionBase"/>.</param>
		/// <returns>An instance of the specified hash function type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="type"/> does not implement <see cref="IHashFunctionBase"/>.</exception>
		/// <exception cref="NotImplementedException">Thrown if no implementation is registered for the specified <paramref name="type"/> or if no default
		/// configuration is available for the specified type.</exception>
		public IHashFunctionBase Create(Type type)
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
		/// Creates an instance of a hash function implementation based on the specified type and configuration.
		/// </summary>
		/// <param name="type">The type of the hash function to create. Must implement <see cref="IHashFunctionBase"/>.</param>
		/// <param name="config">The configuration object used to initialize the hash function.</param>
		/// <returns>An instance of <see cref="IHashFunctionBase"/> corresponding to the specified type and configuration.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is <see langword="null"/> or if <paramref name="config"/> is <see
		/// langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="type"/> does not implement <see cref="IHashFunctionBase"/>.</exception>
		/// <exception cref="NotImplementedException">Thrown if no implementation is registered for the specified <paramref name="type"/>.</exception>
		public IHashFunctionBase Create(Type type, IHashConfigBase config)
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
