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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if NET8_0_OR_GREATER
using HashifyNet.Algorithms.HMACSHA3_256;
using HashifyNet.Algorithms.HMACSHA3_384;
using HashifyNet.Algorithms.HMACSHA3_512;
using HashifyNet.Algorithms.SHA3_256;
using HashifyNet.Algorithms.SHA3_384;
using HashifyNet.Algorithms.SHA3_512;
#endif

namespace HashifyNet
{
	public sealed partial class HashFactory
	{
		/// <summary>
		/// <inheritdoc cref="IHashFactory.CreateInstance(Type)"/>
		/// </summary>
		/// <param name="type"><inheritdoc cref="IHashFactory.CreateInstance(Type)"/></param>
		/// <returns><inheritdoc cref="IHashFactory.CreateInstance(Type)"/></returns>
		/// <exception cref="ArgumentNullException"><inheritdoc cref="IHashFactory.CreateInstance(Type)"/></exception>
		/// <exception cref="ArgumentException"><inheritdoc cref="IHashFactory.CreateInstance(Type)"/></exception>
		/// <exception cref="NotImplementedException"><inheritdoc cref="IHashFactory.CreateInstance(Type)"/></exception>
		public static IHashFunctionBase Create(Type type)
		{
			return Singleton.CreateInstance(type);
		}

		/// <summary>
		/// <inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/>
		/// </summary>
		/// <param name="type"><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></param>
		/// <param name="config"><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></param>
		/// <returns><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></returns>
		/// <exception cref="ArgumentNullException"><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></exception>
		/// <exception cref="ArgumentException"><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></exception>
		/// <exception cref="NotImplementedException"><inheritdoc cref="IHashFactory.CreateInstance(Type, IHashConfigBase)"/></exception>
		public static IHashFunctionBase Create(Type type, IHashConfigBase config)
		{
			return Singleton.CreateInstance(type, config);
		}

		/// <summary>
		/// Creates an array of hash function instances based on the specified hash function type.
		/// </summary>
		/// <param name="type">The type of hash functions to retrieve. This determines the set of hash algorithms to be instantiated.</param>
		/// <param name="defaultConfigMap">An optional dictionary mapping hash function types to their corresponding configuration objects.  If provided, the
		/// configurations in this dictionary will be used to initialize the hash function instances. If a type is not present
		/// in the dictionary, a default configuration will be used if available.</param>
		/// <param name="ignoredFunctions">An optional array of hash function types to be ignored during instantiation. Any types present in this array (and any types derive from the types in this array) will be skipped.</param>
		/// <returns>An array of <see cref="IHashFunctionBase"/> instances representing the hash functions for the specified type.</returns>
		/// <exception cref="InvalidOperationException">Thrown if no hash algorithms are found for the specified <paramref name="type"/>.</exception>
		public static IHashFunctionBase[] CreateHashAlgorithms(HashFunctionType type, Dictionary<Type, IHashConfigBase> defaultConfigMap = null, params Type[] ignoredFunctions)
		{
			Type[] functions = GetHashAlgorithms(type);

			if (functions == null || functions.Length < 1)
			{
				throw new InvalidOperationException("No hash algorithms found.");
			}

			List<IHashFunctionBase> instances = new List<IHashFunctionBase>();
			for (int i = 0; i < functions.Length; ++i)
			{
				Type funcType = functions[i];

				if (ignoredFunctions != null && ignoredFunctions.Length > 0)
				{
					foreach (Type ignoredType in ignoredFunctions)
					{
						if ((funcType.IsGenericType && (funcType.GetGenericTypeDefinition() == ignoredType || ignoredType.IsAssignableFrom(funcType.GetGenericTypeDefinition()))) || funcType == ignoredType || ignoredType.IsAssignableFrom(funcType))
						{
							funcType = null;
							break;
						}
					}
				}

				if (funcType == null)
				{
					continue;
				}

				if (defaultConfigMap != null && defaultConfigMap.ContainsKey(funcType))
				{
					instances.Add(Singleton.CreateInstance(funcType, defaultConfigMap[funcType]));
				}
				else
				{
					instances.Add(Singleton.CreateInstance(funcType));
				}
			}

			return instances.ToArray();
		}

#if NET8_0_OR_GREATER
		/// <summary>
		/// Retrieves a list of hash algorithm types that are not supported on the current platform.
		/// </summary>
		/// <remarks>This method checks the availability of specific hash algorithms, such as SHA-3 and HMAC-SHA-3
		/// variants, on the current platform. The returned array can be used to determine which algorithms are unsupported
		/// and may require alternative handling in your application.</remarks>
		/// <returns>An array of <see cref="Type"/> objects representing the hash algorithms that are unavailable. If all algorithms
		/// are supported, the array will be empty.</returns>
		public static Type[] GetUnavailableHashAlgorithms()
		{
			List<Tuple<Type, bool>> possibleUnavailableAlgorithms = new List<Tuple<Type, bool>>()
			{
				Tuple.Create(typeof(IHMACSHA3_256), IHMACSHA3_256.IsSupported),
				Tuple.Create(typeof(IHMACSHA3_384), IHMACSHA3_384.IsSupported),
				Tuple.Create(typeof(IHMACSHA3_512), IHMACSHA3_512.IsSupported),

				Tuple.Create(typeof(ISHA3_256), ISHA3_256.IsSupported),
				Tuple.Create(typeof(ISHA3_384), ISHA3_384.IsSupported),
				Tuple.Create(typeof(ISHA3_512), ISHA3_512.IsSupported),
			};

			Type[] unavailableAlgorithms = possibleUnavailableAlgorithms
																		.Where(t => !t.Item2)
																		.Select(t => t.Item1)
																		.ToArray();

			return unavailableAlgorithms;
		}
#endif

		/// <summary>
		/// Retrieves an array of concrete configuration types associated with the specified hash function type.
		/// </summary>
		/// <remarks>This method inspects the hash algorithm types associated with the specified <paramref
		/// name="type"/> and retrieves the concrete configuration types defined by the <see
		/// cref="Core.HashAlgorithmImplementationAttribute"/> applied to each type.</remarks>
		/// <param name="type">The hash function type for which to retrieve the associated concrete configuration types.</param>
		/// <returns>An array of <see cref="Type"/> objects representing the concrete configuration types for the specified hash
		/// function type. If no configurations are found, an empty array is returned.</returns>
		public static Type[] GetConcreteConfigs(HashFunctionType type)
		{
			Type[] types = GetHashAlgorithms(type);
			List<Type> retval = new List<Type>();
			foreach (Type t in types)
			{
				if (!_concreteConfigTypes.ContainsKey(t))
				{
					// This should not happen but handle it gracefully.
					continue;
				}

				retval.Add((Type)_concreteConfigTypes[t]);
			}

			return retval.ToArray();
		}

		/// <summary>
		/// Creates a default concrete configuration instance for the specified type.
		/// </summary>
		/// <param name="type">The type for which to create the default configuration. This must be a type that has a registered implementation.</param>
		/// <returns>An instance of <see cref="IHashConfigBase"/> representing the default configuration for the specified type.</returns>
		/// <exception cref="KeyNotFoundException">Thrown if no implementation is registered for the specified <paramref name="type"/>.</exception>
		/// <exception cref="NotSupportedException">Thrown if no default configuration is available for the specified <paramref name="type"/>.</exception>
		public static IHashConfigBase CreateDefaultConcreteConfig(Type type)
		{
			_ = type ?? throw new ArgumentNullException(nameof(type));

			if (!_implementations.ContainsKey(type))
			{
				throw new KeyNotFoundException($"No implementation registered for type '{type.FullName}'.");
			}

			Func<object[], object> configFactory = ((Tuple<Func<object[], object>, Func<object[], object>>)_implementations[type]).Item2;
			if (configFactory == null)
			{
				throw new NotSupportedException($"No default configuration available for type '{type.FullName}'.");
			}

			return (IHashConfigBase)configFactory(null);
		}

		/// <summary>
		/// Attempts to create a default concrete configuration for the specified type.
		/// </summary>
		/// <remarks>This method catches and handles <see cref="NotSupportedException"/> and <see
		/// cref="KeyNotFoundException"/> internally, returning <see langword="false"/> in such cases. Other exceptions may
		/// propagate to the caller.</remarks>
		/// <param name="type">The type for which to create the default concrete configuration.</param>
		/// <param name="config">When this method returns, contains the created configuration if the operation succeeds; otherwise, <see
		/// langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the default concrete configuration was successfully created; otherwise, <see
		/// langword="false"/>.</returns>
		public static bool TryCreateDefaultConcreteConfig(Type type, out IHashConfigBase config)
		{
			try
			{
				config = CreateDefaultConcreteConfig(type);
				return true;
			}
			catch (NotSupportedException)
			{
				config = null;
				return false;
			}
			catch (KeyNotFoundException)
			{
				config = null;
				return false;
			}
		}

		/// <summary>
		/// Retrieves an array of hash algorithm types based on the specified hash function category.
		/// </summary>
		/// <remarks>The returned array may include both cryptographic and non-cryptographic hash algorithms if the
		/// specified <paramref name="type"/> includes both categories.</remarks>
		/// <param name="type">A bitwise combination of <see cref="HashFunctionType"/> values that specifies the category of hash functions to
		/// retrieve. Use <see cref="HashFunctionType.Cryptographic"/> to include cryptographic hash algorithms, <see
		/// cref="HashFunctionType.Noncryptographic"/> to include non-cryptographic hash algorithms, or both.</param>
		/// <returns>An array of <see cref="Type"/> objects representing the hash algorithms that match the specified category. If no
		/// algorithms match the specified category, an empty array is returned. If the given <paramref name="type"/> neither contains <seealso cref="HashFunctionType.Cryptographic"/> nor <seealso cref="HashFunctionType.Noncryptographic"/>, the return value will be <see langword="null"/>.</returns>
		public static Type[] GetHashAlgorithms(HashFunctionType type)
		{
			Type[] functions = null;
			if ((type & HashFunctionType.Cryptographic) == HashFunctionType.Cryptographic)
			{
				functions = GetAllCryptographicHashAlgorithms();
			}

			if ((type & HashFunctionType.Noncryptographic) == HashFunctionType.Noncryptographic)
			{
				Type[] nonCrypto = GetAllNonCryptographicHashAlgorithms();
				if (functions == null)
				{
					functions = nonCrypto;
				}
				else
				{
					functions = functions.Union(nonCrypto).ToArray();
				}
			}

			return functions;
		}

		private static Type[] GetAllHashAlgorithms()
		{
			if (_implementations.Count < 1)
			{
				return null;
			}

			Type[] types = new Type[_implementations.Keys.Count];
			_implementations.Keys.CopyTo(types, 0);
			return types;
		}

		private static Type[] GetAllCryptographicHashAlgorithms()
		{
			Type[] all = GetAllHashAlgorithms();
			if (all == null || all.Length < 1)
			{
				return null;
			}

			return all.Where(t =>
	t.GetInterfaces().Any(i => i.IsGenericType &&
		i.GetGenericTypeDefinition() == typeof(ICryptographicHashFunction<>))
	).ToArray();
		}

		private static Type[] GetAllNonCryptographicHashAlgorithms()
		{
			Type[] all = GetAllHashAlgorithms();
			if (all == null || all.Length < 1)
			{
				return null;
			}

			return all.Where(t =>
	t.GetInterfaces().Any(i => i.IsGenericType &&
		i.GetGenericTypeDefinition() == typeof(IHashFunction<>))
	&&
	!t.GetInterfaces().Any(i => i.IsGenericType &&
		i.GetGenericTypeDefinition() == typeof(ICryptographicHashFunction<>))
				).ToArray();
		}
	}
}
