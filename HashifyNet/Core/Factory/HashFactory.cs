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

using HashifyNet.Core;
using HashifyNet.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
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

		private static readonly Hashtable _implementations;
		private static readonly Hashtable _concreteConfigTypes;
		private static readonly Hashtable _configProfiles;
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
			_configProfiles = new Hashtable();

			Type[] types = ReflectionHelper.GetClasses(typeof(HashAlgorithmImplementationAttribute), false);
			if (types == null || types.Length < 1)
			{
				throw new InvalidOperationException("No hash algorithm implementations found.");
			}

			List<Tuple<Type, HashAlgorithmImplementationAttribute>> validTypes = new List<Tuple<Type, HashAlgorithmImplementationAttribute>>();
			foreach (Type type in types)
			{
				if (type.GetCustomAttribute<ObsoleteAttribute>() != null)
				{
					continue;
				}

				HashAlgorithmImplementationAttribute implementationAttribute  = type.GetCustomAttribute<HashAlgorithmImplementationAttribute>(false);
				if (implementationAttribute == null)
				{
					continue;
				}

				if (!type.HasConstructor(BindingFlags.Public | BindingFlags.Instance, 1, new Type[] { typeof(IHashConfigBase) }))
				{
					continue;
				}

				validTypes.Add(new Tuple<Type, HashAlgorithmImplementationAttribute>(type, implementationAttribute));
			}

			if (validTypes.Count < 1)
			{
				throw new InvalidOperationException("No valid hash algorithm implementations found.");
			}

			foreach (Tuple<Type, HashAlgorithmImplementationAttribute> t in validTypes)
			{
				ConstructorInfo factoryCtor = t.Item1.GetConstructor(BindingFlags.Public | BindingFlags.Instance, 1, new Type[] { typeof(IHashConfigBase) });
				Func<IHashConfigBase, IHashFunctionBase> factory = ReflectionHelper.CreateInstanceWithSingleParameter<IHashFunctionBase, IHashConfigBase>(factoryCtor);

				Type configType = t.Item2.ConcreteConfig;

				_concreteConfigTypes.Add(t.Item2.ImplementedInterface, configType);

				IEnumerable<DeclareHashConfigProfileAttribute> configProfileDeclarationAttributes = configType.GetCustomAttributes<DeclareHashConfigProfileAttribute>(false);
				List<Type> configProfileTypes = null;
				foreach (DeclareHashConfigProfileAttribute attr in configProfileDeclarationAttributes)
				{
					if (configProfileTypes == null)
					{
						configProfileTypes = new List<Type>();
					}

					if (attr != null)
					{
						// Assuming ProfileType is ensured to be non-null and valid by the attribute's constructor.
						configProfileTypes.Add(attr.ProfileType);
					}
				}

				if (configProfileTypes != null && configProfileTypes.Count > 0)
				{
					// Create instances of each config profile type and push them into an array then to the lookup table.
					// This implementation assumes that HashAlgorithmImplementationAttribute's constructor already ensures a public parameterless constructor for every config profile type.
					{
						IHashConfigProfile[] profiles = new IHashConfigProfile[configProfileTypes.Count];
						for (int i = 0; i < configProfileTypes.Count; ++i)
						{
							Type configProfileType = configProfileTypes[i];
							ConstructorInfo configProfileCtor = configProfileType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, 0);
							if (configProfileCtor == null)
							{
								// This should not have happened, as ConfigProfilesAttribute already checks for this in its constructor.
								// Just in case, we throw here.
								throw new InvalidOperationException($"The config profile type '{configProfileType.FullName}' does not have a public parameterless constructor. This should not have happened in normal cases, so it probably points to a bug.");
							}

							DefineHashConfigProfileAttribute configProfileAttribute = configProfileType.GetCustomAttribute<DefineHashConfigProfileAttribute>(false);
							if (configProfileAttribute == null)
							{
								// This should not have happened, as ConfigProfilesAttribute already checks for this in its constructor.
								// Just in case, we throw here.
								throw new InvalidOperationException($"The config profile type '{configProfileType.FullName}' is not marked with {nameof(DefineHashConfigProfileAttribute)}. This should not have happened in normal cases, so it probably points to a bug.");
							}

							Func<IHashConfigBase> configProfileFactory = ReflectionHelper.CreateInstance<IHashConfigBase>(configProfileCtor);
							profiles[i] = new HashConfigProfile(configProfileAttribute.Name, configProfileAttribute.Description, configProfileFactory);
						}

						_configProfiles.Add(t.Item2.ImplementedInterface, profiles);
					}
				}

				// If the concrete config has no parameterless constructor, we cannot create a default instance. In this case, the parameterless Create function will throw.
				Func<IHashConfigBase> configFactory = null;
				ConstructorInfo configCtor = configType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, 0);
				if (configCtor != null)
				{
					configFactory = ReflectionHelper.CreateInstance<IHashConfigBase>(configCtor);
				}

				_implementations.Add(t.Item2.ImplementedInterface, Tuple.Create<Func<IHashConfigBase, IHashFunctionBase>, Func<IHashConfigBase>>(factory, configFactory));
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

			Tuple<Func<IHashConfigBase, IHashFunctionBase>, Func<IHashConfigBase>> factory = (Tuple<Func<IHashConfigBase, IHashFunctionBase>, Func<IHashConfigBase>>)_implementations[type];
			if (factory.Item2 == null)
			{
				throw new NotImplementedException($"No default configuration available for type {type.FullName}.");
			}

			return factory.Item1(factory.Item2());
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

			Tuple<Func<IHashConfigBase, IHashFunctionBase>, Func<IHashConfigBase>> factory = (Tuple<Func<IHashConfigBase, IHashFunctionBase>, Func<IHashConfigBase>>)_implementations[type];
			return factory.Item1(config);
		}
	}
}

