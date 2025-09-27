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

using HashifyNet.Algorithms.Argon2id;
using HashifyNet.Algorithms.BuzHash;
using HashifyNet.Algorithms.CRC;
using HashifyNet.Algorithms.FNV;
using HashifyNet.Algorithms.Pearson;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests.Core
{
	public class FactoryTests
	{
		[Fact]
		public void Factory_CreateInstance_ValidInputs_Works()
		{
			var instance = HashFactory<ICRC>.Create(new CRCConfigProfileCRC32());
			Assert.NotNull(instance);
			Assert.IsType<CRC_Implementation>(instance);
		}

		[Fact]
		public void Factory_CreateInstance_Null_Throws()
		{
			Assert.Equal(
				"The provided type 'HashifyNet.Algorithms.CRC.CRC_Implementation' is not an interface. (Parameter 'type')",
				Assert.Throws<ArgumentException>(() =>
				{
					HashFactory<CRC_Implementation>.Create();
				}).Message);
		}

		[Fact]
		public void Factory_GetAllHashAlgorithms_Works()
		{
			var all = HashFactory.GetHashAlgorithms(HashFunctionType.Cryptographic | HashFunctionType.Noncryptographic);
			Assert.NotNull(all);
			Assert.NotEmpty(all);
			Assert.All(all, item => Assert.NotNull(item));
			Assert.All(all, item => typeof(IHashFunctionBase).IsAssignableFrom(item));
		}

		[Fact]
		public void Factory_GetAllCryptographicHashAlgorithms_Works()
		{
			var all = HashFactory.GetHashAlgorithms(HashFunctionType.Cryptographic);
			Assert.NotNull(all);
			Assert.NotEmpty(all);
			Assert.All(all, item => Assert.NotNull(item));
			Assert.All(all, item => Assert.Contains(typeof(ICryptographicHashFunction<>), item.GetInterfaces().Where(t => t.IsGenericType).ToList().ConvertAll(t => t.GetGenericTypeDefinition())));
		}

		[Fact]
		public void Factory_GetAllNonCryptographicHashAlgorithms_Works()
		{
			var all = HashFactory.GetHashAlgorithms(HashFunctionType.Noncryptographic);

			Assert.NotNull(all);
			Assert.NotEmpty(all);
			Assert.All(all, item => Assert.NotNull(item));
			Assert.All(all, item => Assert.Contains(typeof(IHashFunctionBase), item.GetInterfaces()));
			Assert.All(all, item => Assert.DoesNotContain(typeof(ICryptographicHashFunction<>), item.GetInterfaces().Where(t => t.IsGenericType).ToList().ConvertAll(t => t.GetGenericTypeDefinition())));
		}

		[Fact]
		public void Factory_ComputeNonCryptographicHashes_Works()
		{
			IHashFunctionBase[] functions = HashFactory.CreateHashAlgorithms(HashFunctionType.Noncryptographic, new Dictionary<Type, IHashConfigBase>() 
			{
				{ typeof(ICRC), new CRCConfigProfileCRC32() },
				{ typeof(IPearson), new PearsonConfigProfileWikipedia() },
				{ typeof(IFNV1), new FNVConfigProfile32Bits() },
				{ typeof(IFNV1a), new FNVConfigProfile32Bits() },
				{ typeof(IBuzHash), new BuzHashConfigProfileDefault() },
			});

			Assert.NotNull(functions);
			Assert.NotEmpty(functions);
			Assert.All(functions, item => Assert.NotNull(item));

			foreach (IHashFunctionBase function in functions)
			{
				IHashValue hv = function.ComputeHash(TestConstants.FooBar);
				Assert.NotNull(hv);
				Assert.NotEmpty(hv.Hash);
			}
		}

		[Fact]
		public void Factory_ComputeCryptographicHashes_Works()
		{
			IHashFunctionBase[] functions = HashFactory.CreateHashAlgorithms(HashFunctionType.Cryptographic, new Dictionary<Type, IHashConfigBase>()
			{
				{ typeof(IArgon2id), new Argon2idConfigProfileOWASP() }
			}
#if !LEGACY
			, HashFactory.GetUnavailableHashAlgorithms()
#endif		
			);

			Assert.NotNull(functions);
			Assert.NotEmpty(functions);
			Assert.All(functions, item => Assert.NotNull(item));

			foreach (IHashFunctionBase function in functions)
			{
				IHashValue hv = function.ComputeHash(TestConstants.FooBar);
				Assert.NotNull(hv);
				Assert.NotEmpty(hv.Hash);

				(function as ICryptographicHashFunctionBase).Dispose();
			}
		}

		[Fact]
		public void Factory_GetWithType_Works()
		{
			Type type = typeof(ICRC);
			Assert.NotNull(type);

			var instance = HashFactory.Create(type, new CRCConfigProfileCRC32());
			Assert.NotNull(instance);
			Assert.IsType<CRC_Implementation>(instance);
		}

		[Fact]
		public void Factory_CreateInstance_WithConfig_Works()
		{
			ICRC crc = HashFactory<ICRC>.Create(new CRCConfigProfileCRC7());
			Assert.NotNull(crc);
			Assert.IsType<CRC_Implementation>(crc);

			ICRCConfig crc7 = new CRCConfigProfileCRC7();
			Assert.Equal(crc7.HashSizeInBits, crc.Config.HashSizeInBits);
			Assert.Equal(crc7.InitialValue, crc.Config.InitialValue);
			Assert.Equal(crc7.Polynomial, crc.Config.Polynomial);
			Assert.Equal(crc7.ReflectIn, crc.Config.ReflectIn);
			Assert.Equal(crc7.ReflectOut, crc.Config.ReflectOut);
			Assert.Equal(crc7.XOrOut, crc.Config.XOrOut);
		}

		[Fact]
		public void Factory_GetConcreteConfigs_Works()
		{
			var configs = HashFactory.GetConcreteConfigs(HashFunctionType.Cryptographic | HashFunctionType.Noncryptographic);
			Assert.NotNull(configs);
			Assert.NotEmpty(configs);
			Assert.All(configs, item => Assert.NotNull(item));
			Assert.All(configs, item => Assert.Contains(typeof(IHashConfigBase), item.GetInterfaces()));
		}

#if !LEGACY
		[Fact]
		public void Factory_GetUnavailableHashAlgorithms_Works()
		{
			var unavailable = HashFactory.GetUnavailableHashAlgorithms();
			Assert.NotNull(unavailable);
		}
#endif

		[Fact]
		public void Factory_CreateDefaultConcreteConfig_Null_Throws()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				HashFactory.CreateDefaultConcreteConfig(null);
			});
		}

		[Fact]
		public void Factory_TryCreateDefaultConcreteConfig_NeverThrows()
		{
			HashFactory.TryCreateDefaultConcreteConfig(typeof(ICRC), out _);
		}

		[Fact]
		public void Factory_GetConfigProfiles_Works()
		{
			var algorithms = HashFactory.GetHashAlgorithms(HashFunctionType.Cryptographic | HashFunctionType.Noncryptographic);

			List<IHashConfigProfile> allProfiles = new();
			foreach (var algorithm in algorithms)
			{
				allProfiles.AddRange(HashFactory.GetConfigProfiles(algorithm));
			}

			Assert.NotEmpty(allProfiles);
			Assert.All(allProfiles, item => Assert.NotNull(item));
			Assert.All(allProfiles, item => Assert.False(string.IsNullOrWhiteSpace(item.Name)));
		}

		[Fact]
		public void Factory_GetConfigProfiles_Null_Throws()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				HashFactory.GetConfigProfiles(null);
			});
		}

		[Fact]
		public void Factory_GetConfigProfiles_NoProfiles_Works()
		{
			var profiles = HashFactory.GetConfigProfiles(typeof(string));
			Assert.NotNull(profiles);
			Assert.Empty(profiles);
		}

		[Fact]
		public void Factory_GetConfigProfiles_HasUniqueNames()
		{
			var algorithms = HashFactory.GetHashAlgorithms(HashFunctionType.Cryptographic | HashFunctionType.Noncryptographic);
			foreach (var algorithm in algorithms)
			{
				var profiles = HashFactory.GetConfigProfiles(algorithm);
				var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (var profile in profiles)
				{
					Assert.True(names.Add(profile.Name), $"Duplicate profile name '{profile.Name}' found for algorithm '{algorithm.FullName}'.");
				}
			}
		}

		[Fact]
		public void Factory_GetConfigProfiles_Create_Works()
		{
			var algorithms = HashFactory.GetHashAlgorithms(HashFunctionType.Cryptographic | HashFunctionType.Noncryptographic);
			foreach (var algorithm in algorithms)
			{
				var profiles = HashFactory.GetConfigProfiles(algorithm);
				foreach (var profile in profiles)
				{
					IHashConfigBase config = profile.Create();
					Assert.NotNull(config);
					Assert.IsType(profile.ProfileType, config);
				}
			}
		}
	}
}
