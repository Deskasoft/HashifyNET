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

using Moq;
using HashifyNet.Algorithms.MD5;
using HashifyNet.Algorithms.SHA1;
using HashifyNet.Algorithms.SHA256;
using HashifyNet.Algorithms.SHA384;
using HashifyNet.Algorithms.SHA512;

#if !LEGACY
using HashifyNet.Algorithms.SHA3_256;
using HashifyNet.Algorithms.SHA3_384;
using HashifyNet.Algorithms.SHA3_512;
#endif

namespace HashifyNet.UnitTests.Algorithms.HashAlgorithmWrapper
{
	public class Tests
	{
#region MD5_Tests

#region MD5_Constructor

#region MD5_Config

		[Fact]
		public void MD5_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new MD5_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void MD5_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IMD5Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(128);

				configMock.Setup(bc => bc.Clone())
					.Returns(new MD5Config());
			}

			GC.KeepAlive(
				new MD5_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // MD5_Config

#endregion // MD5_Constructor

#region MD5_ConfigValidation
		[Fact]
		public void MD5Config_Defaults_HaventChanged()
		{
			var config = new MD5Config();
			Assert.Equal(128, config.HashSizeInBits);
		}
#endregion // MD5_ConfigValidation

#endregion // MD5_Tests

#region SHA1_Tests

#region SHA1_Constructor

#region SHA1_Config

		[Fact]
		public void SHA1_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA1_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA1_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<ISHA1Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(160);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA1Config());
			}

			GC.KeepAlive(
				new SHA1_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA1_Config

#endregion // SHA1_Constructor

#region SHA1_ConfigValidation
		[Fact]
		public void SHA1Config_Defaults_HaventChanged()
		{
			var config = new SHA1Config();
			Assert.Equal(160, config.HashSizeInBits);
		}
#endregion // SHA1_ConfigValidation

#endregion // SHA1_Tests

#region SHA256_Tests

#region SHA256_Constructor

#region SHA256_Config

		[Fact]
		public void SHA256_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA256_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA256_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<ISHA256Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(256);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA256Config());
			}

			GC.KeepAlive(
				new SHA256_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA256_Config

#endregion // SHA256_Constructor

#region SHA256_ConfigValidation
		[Fact]
		public void SHA256Config_Defaults_HaventChanged()
		{
			var config = new SHA256Config();
			Assert.Equal(256, config.HashSizeInBits);
		}
#endregion // SHA256_ConfigValidation

#endregion // SHA256_Tests

#region SHA384_Tests

#region SHA384_Constructor

#region SHA384_Config

		[Fact]
		public void SHA384_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA384_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA384_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<ISHA384Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(384);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA384Config());
			}

			GC.KeepAlive(
				new SHA384_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA384_Config

#endregion // SHA384_Constructor

#region SHA384_ConfigValidation
		[Fact]
		public void SHA384Config_Defaults_HaventChanged()
		{
			var config = new SHA384Config();
			Assert.Equal(384, config.HashSizeInBits);
		}
#endregion // SHA384_ConfigValidation

#endregion // SHA384_Tests

#region SHA512_Tests

#region SHA512_Constructor

#region SHA512_Config

		[Fact]
		public void SHA512_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA512_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA512_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<ISHA512Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(512);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA512Config());
			}

			GC.KeepAlive(
				new SHA512_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA512_Config

#endregion // SHA512_Constructor

#region SHA512_ConfigValidation
		[Fact]
		public void SHA512Config_Defaults_HaventChanged()
		{
			var config = new SHA512Config();
			Assert.Equal(512, config.HashSizeInBits);
		}
#endregion // SHA512_ConfigValidation

#endregion // SHA512_Tests

#if !LEGACY

#region SHA3_256_Tests

#region SHA3_256_Constructor

#region SHA3_256_Config

		[Fact]
		public void SHA3_256_Implementation_Constructor_Config_IsNull_Throws()
		{
			if (!ISHA3_256.IsSupported)	return;

			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_256_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_256_Implementation_Constructor_Config_IsCloned()
		{
			if (!ISHA3_256.IsSupported)	return;

			var configMock = new Mock<ISHA3_256Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(256);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA3_256Config());
			}

			GC.KeepAlive(
				new SHA3_256_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA3_256_Config

#endregion // SHA3_256_Constructor

#region SHA3_256_ConfigValidation
		[Fact]
		public void SHA3_256Config_Defaults_HaventChanged()
		{
			if (!ISHA3_256.IsSupported)	return;

			var config = new SHA3_256Config();
			Assert.Equal(256, config.HashSizeInBits);
		}
#endregion // SHA3_256_ConfigValidation

#endregion // SHA3_256_Tests

#region SHA3_384_Tests

#region SHA3_384_Constructor

#region SHA3_384_Config

		[Fact]
		public void SHA3_384_Implementation_Constructor_Config_IsNull_Throws()
		{
			if (!ISHA3_384.IsSupported)	return;

			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_384_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_384_Implementation_Constructor_Config_IsCloned()
		{
			if (!ISHA3_384.IsSupported)	return;

			var configMock = new Mock<ISHA3_384Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(384);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA3_384Config());
			}

			GC.KeepAlive(
				new SHA3_384_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA3_384_Config

#endregion // SHA3_384_Constructor

#region SHA3_384_ConfigValidation
		[Fact]
		public void SHA3_384Config_Defaults_HaventChanged()
		{
			if (!ISHA3_384.IsSupported)	return;

			var config = new SHA3_384Config();
			Assert.Equal(384, config.HashSizeInBits);
		}
#endregion // SHA3_384_ConfigValidation

#endregion // SHA3_384_Tests

#region SHA3_512_Tests

#region SHA3_512_Constructor

#region SHA3_512_Config

		[Fact]
		public void SHA3_512_Implementation_Constructor_Config_IsNull_Throws()
		{
			if (!ISHA3_512.IsSupported)	return;

			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_512_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_512_Implementation_Constructor_Config_IsCloned()
		{
			if (!ISHA3_512.IsSupported)	return;

			var configMock = new Mock<ISHA3_512Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(512);

				configMock.Setup(bc => bc.Clone())
					.Returns(new SHA3_512Config());
			}

			GC.KeepAlive(
				new SHA3_512_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

#endregion // SHA3_512_Config

#endregion // SHA3_512_Constructor

#region SHA3_512_ConfigValidation
		[Fact]
		public void SHA3_512Config_Defaults_HaventChanged()
		{
			if (!ISHA3_512.IsSupported)	return;

			var config = new SHA3_512Config();
			Assert.Equal(512, config.HashSizeInBits);
		}
#endregion // SHA3_512_ConfigValidation

#endregion // SHA3_512_Tests

#endif
	}
}

