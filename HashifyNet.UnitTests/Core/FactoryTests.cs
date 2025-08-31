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
using HashifyNet.Core.HashAlgorithm;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests.Core
{
	public class FactoryTests
	{
		[Fact]
		public void Factory_CreateInstance_ValidInputs_Works()
		{
			var instance = HashFactory<ICRC>.Instance.Create(CRCConfig.CRC32);
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
					HashFactory<CRC_Implementation>.Instance.Create();
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
			IHashFunctionBase[] functions = HashFactory.Instance.GetHashFunctions(HashFunctionType.Noncryptographic, new Dictionary<Type, IHashConfigBase>() 
			{
				{ typeof(ICRC), CRCConfig.CRC32 },
				{ typeof(IPearson), new WikipediaPearsonConfig() },
				{ typeof(IFNV1), FNVConfig.GetPredefinedConfig(32) },
				{ typeof(IFNV1a), FNVConfig.GetPredefinedConfig(32) },
				{ typeof(IBuzHash), new DefaultBuzHashConfig() },
			});

			Assert.NotNull(functions);
			Assert.NotEmpty(functions);
			Assert.All(functions, item => Assert.NotNull(item));

			foreach (IHashFunctionBase function in functions)
			{
				IHashValue hv = function.ComputeHash(TestConstants.FooBar);
				Assert.NotNull(hv);
				Assert.NotNull(hv.Hash);
				Assert.NotEmpty(hv.Hash);
			}
		}


		[Fact]
		public void Factory_ComputeCryptographicHashes_Works()
		{
			IHashFunctionBase[] functions = HashFactory.Instance.GetHashFunctions(HashFunctionType.Cryptographic, new Dictionary<Type, IHashConfigBase>()
			{
				{ typeof(IArgon2id), Argon2idConfig.OWASP_Standard }
			}, typeof(IHashAlgorithmWrapper));

			Assert.NotNull(functions);
			Assert.NotEmpty(functions);
			Assert.All(functions, item => Assert.NotNull(item));

			foreach (IHashFunctionBase function in functions)
			{
				IHashValue hv = function.ComputeHash(TestConstants.FooBar);
				Assert.NotNull(hv);
				Assert.NotNull(hv.Hash);
				Assert.NotEmpty(hv.Hash);

				(function as ICryptographicHashFunctionBase).Dispose();
			}
		}

		[Fact]
		public void Factory_GetWithType_Works()
		{
			Type type = typeof(ICRC);
			Assert.NotNull(type);

			var instance = HashFactory.Instance.Create(type, CRCConfig.CRC32);
			Assert.NotNull(instance);
			Assert.IsType<CRC_Implementation>(instance);
		}

		[Fact]
		public void Factory_CreateInstance_WithConfig_Works()
		{
			ICRC crc = HashFactory<ICRC>.Instance.Create(CRCConfig.CRC7);
			Assert.NotNull(crc);
			Assert.IsType<CRC_Implementation>(crc);
			Assert.Equal(CRCConfig.CRC7.HashSizeInBits, crc.Config.HashSizeInBits);
			Assert.Equal(CRCConfig.CRC7.InitialValue, crc.Config.InitialValue);
			Assert.Equal(CRCConfig.CRC7.Polynomial, crc.Config.Polynomial);
			Assert.Equal(CRCConfig.CRC7.ReflectIn, crc.Config.ReflectIn);
			Assert.Equal(CRCConfig.CRC7.ReflectOut, crc.Config.ReflectOut);
			Assert.Equal(CRCConfig.CRC7.XOrOut, crc.Config.XOrOut);
		}
	}
}
