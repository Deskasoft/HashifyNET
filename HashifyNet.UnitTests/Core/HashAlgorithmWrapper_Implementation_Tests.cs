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

using HashifyNet.Algorithms.SHA1;
using HashifyNet.Algorithms.SHA256;
using HashifyNet.Algorithms.SHA384;
using HashifyNet.Algorithms.SHA512;
using HashifyNet.Algorithms.MD5;
using HashifyNet.Core.HashAlgorithm;
using HashifyNet.UnitTests.Utilities;

namespace HashifyNet.UnitTests.Core.HashAlgorithm
{
	public class HashAlgorithmWrapper_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void HashAlgorithmWrapper_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
					() => new HashAlgorithmWrapper_Implementation(null))
				.ParamName);
		}

		#endregion

		#endregion

		public class IHashFunction_Tests_SHA1
			: IHashFunction_TestBase<ISHA1, ISHA1Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(160, TestConstants.Empty, "da39a3ee5e6b4b0d3255bfef95601890afd80709"),
					new KnownValue(160, TestConstants.FooBar, "8843d7f92416211de9ebb963ff4ce28125932878"),
					new KnownValue(160, TestConstants.LoremIpsum, "2dd4010f15f21c9e26e31a693ba31c6ab78a5a4c"),
					new KnownValue(160, TestConstants.RandomShort, "d64df40c72068b01e7dfb5ceb2b519ad3b483eb0"),
					new KnownValue(160, TestConstants.RandomLong, "e5901cb4679133729c5555210c3cfe3e5851a2aa"),
				};

			protected override ISHA1 CreateHashFunction(int hashSize) => new SHA1_Implementation(new SHA1Config());
		}

		public class IHashFunction_Tests_SHA256
			: IHashFunction_TestBase<ISHA256, ISHA256Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(256, TestConstants.FooBar, "c3ab8ff13720e8ad9047dd39466b3c8974e592c2fa383d4a3960714caef0c4f2"),
				};

			protected override ISHA256 CreateHashFunction(int hashSize) => new SHA256_Implementation(new SHA256Config());
		}

		public class IHashFunction_Tests_SHA384
			: IHashFunction_TestBase<ISHA384, ISHA384Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(384, TestConstants.FooBar, "3c9c30d9f665e74d515c842960d4a451c83a0125fd3de7392d7b37231af10c72ea58aedfcdf89a5765bf902af93ecf06"),
				};

			protected override ISHA384 CreateHashFunction(int hashSize) => new SHA384_Implementation(new SHA384Config());
		}

		public class IHashFunction_Tests_SHA512
			: IHashFunction_TestBase<ISHA512, ISHA512Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(512, TestConstants.FooBar, "0a50261ebd1a390fed2bf326f2673c145582a6342d523204973d0219337f81616a8069b012587cf5635f6925f1b56c360230c19b273500ee013e030601bf2425"),
				};

			protected override ISHA512 CreateHashFunction(int hashSize) => new SHA512_Implementation(new SHA512Config());
		}

		public class IHashFunction_Tests_MD5
			: IHashFunction_TestBase<IMD5, IMD5Config>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(128, TestConstants.FooBar, "3858f62230ac3c915f300c664312c63f"),
				};

			protected override IMD5 CreateHashFunction(int hashSize) => new MD5_Implementation(new MD5Config());
		}
	}
}
