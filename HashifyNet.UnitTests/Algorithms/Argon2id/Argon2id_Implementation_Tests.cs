// *
// *****************************************************************************
// *
// * Copyright (c) 2025 Deskasoft International
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
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
using HashifyNet.Core.Utilities;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Argon2id
{
	public class Argon2id_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Argon2id_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Argon2id_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Argon2id_Implementation_Constructor_Config_Unset_Throws()
		{
			Assert.Throws<ArgumentOutOfRangeException>(
					() => new Argon2id_Implementation(new Argon2idConfig()));
		}

		[Fact]
		public void Argon2id_Implementation_Constructor_Config_IsCloned()
		{
			var argon2idConfigMock = new Mock<IArgon2idConfig>();
			{
				argon2idConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(512);

				argon2idConfigMock.SetupGet(bc => bc.Iterations)
					.Returns(3);

				argon2idConfigMock.SetupGet(bc => bc.MemorySize)
					.Returns(16384);

				argon2idConfigMock.SetupGet(bc => bc.DegreeOfParallelism)
					.Returns(2);

				argon2idConfigMock.SetupGet(bc => bc.Version)
					.Returns(Argon2idVersion.Version13);

				argon2idConfigMock.Setup(bc => bc.Clone())
					.Returns(new Argon2idConfig() { HashSizeInBits = 512, Iterations = 3, DegreeOfParallelism = 2, MemorySize = 16384, Version = Argon2idVersion.Version13 });
			}

			GC.KeepAlive(
				new Argon2id_Implementation(argon2idConfigMock.Object));

			argon2idConfigMock.Verify(bc => bc.Clone(), Times.Once);

			argon2idConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void Argon2id_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var argon2idConfigMock = new Mock<IArgon2idConfig>();
				{
					argon2idConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					argon2idConfigMock.Setup(bc => bc.Clone())
						.Returns(() => argon2idConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Argon2id_Implementation(
								argon2idConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Argon2id_Implementation_HashSizeInBits_IsFromConfig()
		{
			var argon2idConfig2 = Mock.Of<IArgon2idConfig>(bc => bc.HashSizeInBits == 512 && bc.Iterations == 2 && bc.DegreeOfParallelism == 2 && bc.MemorySize == 16384 && bc.Version == Argon2idVersion.Version13 && bc.Clone() == new Argon2idConfig());
			var argon2idConfig = Mock.Of<IArgon2idConfig>(bc => bc.Clone() == argon2idConfig2);

			var argon2idHash = new Argon2id_Implementation(argon2idConfig);

			Assert.Equal(512, argon2idHash.Config.HashSizeInBits);
		}

		#endregion

		#endregion

		#endregion

		[Fact]
		public void Argon2id_Implementation_Verify_Works()
		{
			Argon2id_Implementation impl = new Argon2id_Implementation(Argon2idConfig.OWASP_Standard);
			IHashValue hash1 = impl.ComputeHash(TestConstants.FooBar);

			Assert.NotNull(hash1);
			Assert.NotNull(hash1.Hash);

			Assert.True(
				hash1.VerifyArgon2id(TestConstants.FooBar)
				);
		}

		[Fact]
		public void Argon2id_Implementation_Verify_Irrelevant_Fails()
		{
			Argon2id_Implementation impl = new Argon2id_Implementation(Argon2idConfig.OWASP_Standard);
			IHashValue hash1 = impl.ComputeHash(TestConstants.FooBar);

			Assert.NotNull(hash1);
			Assert.NotNull(hash1.Hash);

			Assert.False(
				hash1.VerifyArgon2id(TestConstants.LoremIpsum)
				);
		}

		public class IHashFunction_Tests_DefaultConstructor
			: IHashFunction_TestBase<IArgon2id, IArgon2idConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(512, TestConstants.Empty, "86e6ef8e480613500685379b99ed76ab8597af5dc238275538cef6703b5ccaa5e12f7dc9159a556afe911de23e3aa2431ca8de6be2db1d7ceb58eb61d50becd8"),

					new KnownValue(512, TestConstants.FooBar, "0b66ffaa12cb06aa2c4066d2a2c907ffc446d3ad58f00a97a8b2a05498069944bd89943e429f54ef4ebfb35c05bfc13ed10cf23e35c5adf034f895d99b6b875b"),

					new KnownValue(512, TestConstants.LoremIpsum, "50be118c6fbd0120f6f28ab2f284ddc05e9a75be6ce3fe98e040641bdd63ddad31c7b87bfd0f7aafd0fd99c2fb3a765e4e690d9b5fb1d64c48022eadc18b798a"),

					new KnownValue(512, new byte[4096], "4b3029b231d7707de6c009a379e11174134ecd586649adb0725cf4ea411ec51f359563470af8f5b90c8132ed5a78d666cad55b09ab23737fdccb7a309bb23792"),
				};

			protected override IHashValue ComputeHash(IArgon2id hf, byte[] data)
			{
				Argon2id_Implementation impl = hf as Argon2id_Implementation;
				IHashValue value = impl.ComputeHashWithSaltInternal(data, new byte[8]);
				byte[] actualHash = value.DecodeArgon2id();
				return new HashValue(actualHash, impl.Config.HashSizeInBits);
			}

			protected override IArgon2id CreateHashFunction(int hashSize) =>
				new Argon2id_Implementation(
					Argon2idConfig.OWASP_Standard);
		}
	}
}