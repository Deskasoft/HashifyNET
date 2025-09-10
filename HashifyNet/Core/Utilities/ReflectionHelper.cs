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
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HashifyNet.Core.Utilities
{
	/// <summary>
	/// Provides utility methods for working with reflection, such as accessing types, properties, and methods dynamically.
	/// </summary>
	/// <remarks>This class contains static helper methods designed to simplify common reflection tasks. It is
	/// intended for internal use and may include functionality for dynamically inspecting or invoking members of types at
	/// runtime.</remarks>
	internal static class ReflectionHelper
	{
		public static Type[] GetClassTypes()
		{
			return GetClassTypes(out _);
		}

		public static Type[] GetClassTypes(out Assembly[] asms)
		{
			return GetAllLoadableTypes(t => t.IsClass, out asms);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Type[] GetLoadableTypes(Assembly assembly)
		{
			if (assembly == null)
			{
				return null;
			}

			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types;
			}
		}

		public static Type[] GetAllLoadableTypes(Predicate<Type> predicate, AppDomain domain = null)
		{
			return GetAllLoadableTypes(predicate, out _, domain);
		}

		public static Type[] GetAllLoadableTypes(Predicate<Type> predicate, out Assembly[] asms, AppDomain domain = null)
		{
			asms = new Assembly[1] { Assembly.GetExecutingAssembly() };

			List<Type> retval = new List<Type>();
			foreach (Assembly asm in asms)
			{
				if (asm == null)
				{
					continue;
				}

				Type[] types = GetLoadableTypes(asm);
				if (types == null || types.Length < 1)
				{
					continue;
				}

				retval.AddRange(types.ToList().FindAll(t => t != null && predicate(t)));
			}

			return retval.ToArray();
		}

		public static Type[] GetClasses(Type baseAttr, bool inherit)
		{
			return GetClasses(baseAttr, inherit, out _);
		}

		public static Type[] GetClasses(Type baseAttr, bool inherit, out Assembly[] asms)
		{
			_ = baseAttr ?? throw new ArgumentNullException(nameof(baseAttr));

			List<Type> mods = new List<Type>();
			Type[] types = GetClassTypes(out asms);

			if (types == null || types.Length < 1)
			{
				return null;
			}

			mods.AddRange(types.ToList().FindAll(t => t != null && t.IsDefined(baseAttr, inherit)));

			return mods.ToArray();
		}

		public static bool HasConstructor(this Type type, BindingFlags flags, int parameterCount, Type[] parameterTypes = null)
		{
			return GetConstructor(type, flags, parameterCount, parameterTypes) != null;
		}

		public static ConstructorInfo GetConstructor(this Type type, BindingFlags flags, int parameterCount, Type[] parameterTypes = null)
		{
			if (parameterTypes != null)
			{
				if (parameterCount != parameterTypes.Length)
				{
					throw new ArgumentException("parameterCount must be equal to parameterTypes", nameof(parameterCount));
				}
			}

			ConstructorInfo[] ctors = type.GetConstructors(flags);
			if (ctors == null || ctors.Length < 1)
			{
				return null;
			}

			ConstructorInfo retval = null;
			for (int o = 0; o < ctors.Length; ++o)
			{
				ConstructorInfo ctor = ctors[o];
				ParameterInfo[] ctorPis = ctor.GetParameters();
				if (ctorPis == null || ctorPis.Length == parameterCount)
				{
					if (parameterTypes == null)
					{
						retval = ctor;
						break;
					}
					else
					{
						bool equal = true;
						for (int i = 0; i < ctorPis.Length; ++i)
						{
							ParameterInfo pi = ctorPis[i];
							Type askedParameterType = parameterTypes[i].IsGenericType ? parameterTypes[i].GetGenericTypeDefinition() : parameterTypes[i];
							Type piParameterType = pi.ParameterType.IsGenericType ? pi.ParameterType.GetGenericTypeDefinition() : pi.ParameterType;
							if (piParameterType != askedParameterType && !askedParameterType.IsAssignableFrom(piParameterType))
							{
								equal = false;
								break;
							}
						}

						if (equal)
						{
							retval = ctor;
							break;
						}
					}
				}
			}

			return retval;
		}

		public static Func<object[], TType> CreateInstanceWithParameters<TType>(ConstructorInfo ctor, params Type[] parameterTypes)
		{
			_ = ctor ?? throw new ArgumentNullException(nameof(ctor));
			var argumentsParameter = Expression.Parameter(typeof(object[]), "args");

			var parameterExpressions = parameterTypes.Select((paramType, i) =>
			{
				var arrayAccess = Expression.ArrayIndex(argumentsParameter, Expression.Constant(i));
				return Expression.Convert(arrayAccess, paramType);
			}).ToArray();

			var newExpression = Expression.New(ctor, parameterExpressions);
			var convertExpression = Expression.Convert(newExpression, typeof(TType));

			var lambda = Expression.Lambda<Func<object[], TType>>(convertExpression, argumentsParameter);
			return lambda.Compile();
		}

		public static Func<TParameter, TType> CreateInstanceWithSingleParameter<TType, TParameter>(ConstructorInfo ctor)
		{
			_ = ctor ?? throw new ArgumentNullException(nameof(ctor));

			ParameterInfo[] constructorParams = ctor.GetParameters();
			if (constructorParams.Length != 1)
			{
				throw new ArgumentException("The provided constructor must have exactly one parameter.", nameof(ctor));
			}

			ParameterExpression parameterExpression = Expression.Parameter(typeof(TParameter), "param");
			Expression argumentExpression = parameterExpression;

			Type constructorParamType = constructorParams[0].ParameterType;
			if (parameterExpression.Type != constructorParamType)
			{
				argumentExpression = Expression.Convert(parameterExpression, constructorParamType);
			}

			NewExpression newExpression = Expression.New(ctor, argumentExpression);
			Expression<Func<TParameter, TType>> lambda;
			if (newExpression.Type != typeof(TType))
			{
				UnaryExpression convertExpression = Expression.Convert(newExpression, typeof(TType));
				lambda = Expression.Lambda<Func<TParameter, TType>>(convertExpression, parameterExpression);
			}
			else
			{
				lambda = Expression.Lambda<Func<TParameter, TType>>(newExpression, parameterExpression);
			}

			return lambda.Compile();
		}

		public static Func<TType> CreateInstance<TType>(ConstructorInfo ctor)
		{
			_ = ctor ?? throw new ArgumentNullException(nameof(ctor));

			var newExpression = Expression.New(ctor);
			var convertExpression = Expression.Convert(newExpression, typeof(TType));

			var lambda = Expression.Lambda<Func<TType>>(convertExpression);
			return lambda.Compile();
		}

		public static Type[] GetInterfaces(Type currentType)
		{
			_ = currentType ?? throw new ArgumentNullException(nameof(currentType));

			Type[] interfaces = currentType.GetInterfaces();
			if (interfaces == null || interfaces.Length < 1)
			{
				return null;
			}

			return interfaces;
		}

		public static bool HasInterface(this Type type, Type interfaceType)
		{
			return type.GetInterfaces().Any(t => (t == interfaceType) || (t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType));
		}
	}
}
