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
using HashifyNet.Algorithms.HMACMD5;
using HashifyNet.Algorithms.HMACSHA1;
using HashifyNet.Algorithms.HMACSHA256;
using HashifyNet.Algorithms.HMACSHA384;
using HashifyNet.Algorithms.HMACSHA512;
using HashifyNet.Algorithms.HMACSHA3_256;
using HashifyNet.Algorithms.HMACSHA3_384;
using HashifyNet.Algorithms.HMACSHA3_512;
using HashifyNet.Algorithms.MD5;
using HashifyNet.Algorithms.SHA1;
using HashifyNet.Algorithms.SHA256;
using HashifyNet.Algorithms.SHA384;
using HashifyNet.Algorithms.SHA512;
using HashifyNet.Algorithms.SHA3_256;
using HashifyNet.Algorithms.SHA3_384;
using HashifyNet.Algorithms.SHA3_512;

namespace HashifyNet.UnitTests.Algorithms.HashAlgorithmWrapper
{
	public class Tests
	{
		#region HMACMD5_Tests

		#region HMACMD5_Constructor

		#region HMACMD5_Config

		[Fact]
		public void HMACMD5_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACMD5_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACMD5_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACMD5Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(128);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACMD5Config());
			}

			GC.KeepAlive(
				new HMACMD5_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACMD5_Config

		#endregion // HMACMD5_Constructor

		#region HMACMD5_ConfigValidation
		[Fact]
		public void HMACMD5Config_Defaults_HaventChanged()
		{
			var config = new HMACMD5Config();
			Assert.Equal(128, config.HashSizeInBits);
		}
		#endregion // HMACMD5_ConfigValidation

		#endregion // HMACMD5_Tests

		#region HMACSHA1_Tests

		#region HMACSHA1_Constructor

		#region HMACSHA1_Config

		[Fact]
		public void HMACSHA1_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA1_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA1_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA1Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(160);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA1Config());
			}

			GC.KeepAlive(
				new HMACSHA1_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA1_Config

		#endregion // HMACSHA1_Constructor

		#region HMACSHA1_ConfigValidation
		[Fact]
		public void HMACSHA1Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA1Config();
			Assert.Equal(160, config.HashSizeInBits);
		}
		#endregion // HMACSHA1_ConfigValidation

		#endregion // HMACSHA1_Tests

		#region HMACSHA256_Tests

		#region HMACSHA256_Constructor

		#region HMACSHA256_Config

		[Fact]
		public void HMACSHA256_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA256_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA256_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA256Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(256);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA256Config());
			}

			GC.KeepAlive(
				new HMACSHA256_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA256_Config

		#endregion // HMACSHA256_Constructor

		#region HMACSHA256_ConfigValidation
		[Fact]
		public void HMACSHA256Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA256Config();
			Assert.Equal(256, config.HashSizeInBits);
		}
		#endregion // HMACSHA256_ConfigValidation

		#endregion // HMACSHA256_Tests

		#region HMACSHA384_Tests

		#region HMACSHA384_Constructor

		#region HMACSHA384_Config

		[Fact]
		public void HMACSHA384_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA384_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA384_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA384Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(384);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA384Config());
			}

			GC.KeepAlive(
				new HMACSHA384_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA384_Config

		#endregion // HMACSHA384_Constructor

		#region HMACSHA384_ConfigValidation
		[Fact]
		public void HMACSHA384Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA384Config();
			Assert.Equal(384, config.HashSizeInBits);
		}
		#endregion // HMACSHA384_ConfigValidation

		#endregion // HMACSHA384_Tests

		#region HMACSHA512_Tests

		#region HMACSHA512_Constructor

		#region HMACSHA512_Config

		[Fact]
		public void HMACSHA512_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA512_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA512_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA512Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(512);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA512Config());
			}

			GC.KeepAlive(
				new HMACSHA512_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA512_Config

		#endregion // HMACSHA512_Constructor

		#region HMACSHA512_ConfigValidation
		[Fact]
		public void HMACSHA512Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA512Config();
			Assert.Equal(512, config.HashSizeInBits);
		}
		#endregion // HMACSHA512_ConfigValidation

		#endregion // HMACSHA512_Tests

		#region HMACSHA3_256_Tests

		#region HMACSHA3_256_Constructor

		#region HMACSHA3_256_Config

		[Fact]
		public void HMACSHA3_256_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA3_256_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA3_256_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA3_256Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(256);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA3_256Config());
			}

			GC.KeepAlive(
				new HMACSHA3_256_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA3_256_Config

		#endregion // HMACSHA3_256_Constructor

		#region HMACSHA3_256_ConfigValidation
		[Fact]
		public void HMACSHA3_256Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA3_256Config();
			Assert.Equal(256, config.HashSizeInBits);
		}
		#endregion // HMACSHA3_256_ConfigValidation

		#endregion // HMACSHA3_256_Tests

		#region HMACSHA3_384_Tests

		#region HMACSHA3_384_Constructor

		#region HMACSHA3_384_Config

		[Fact]
		public void HMACSHA3_384_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA3_384_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA3_384_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA3_384Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(384);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA3_384Config());
			}

			GC.KeepAlive(
				new HMACSHA3_384_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA3_384_Config

		#endregion // HMACSHA3_384_Constructor

		#region HMACSHA3_384_ConfigValidation
		[Fact]
		public void HMACSHA3_384Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA3_384Config();
			Assert.Equal(384, config.HashSizeInBits);
		}
		#endregion // HMACSHA3_384_ConfigValidation

		#endregion // HMACSHA3_384_Tests

		#region HMACSHA3_512_Tests

		#region HMACSHA3_512_Constructor

		#region HMACSHA3_512_Config

		[Fact]
		public void HMACSHA3_512_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new HMACSHA3_512_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void HMACSHA3_512_Implementation_Constructor_Config_IsCloned()
		{
			var configMock = new Mock<IHMACSHA3_512Config>();
			{
				configMock.Setup(bc => bc.HashSizeInBits)
					.Returns(512);

				configMock.Setup(bc => bc.Clone())
					.Returns(new HMACSHA3_512Config());
			}

			GC.KeepAlive(
				new HMACSHA3_512_Implementation(configMock.Object));

			configMock.Verify(bc => bc.Clone(), Times.Once);

			configMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#endregion // HMACSHA3_512_Config

		#endregion // HMACSHA3_512_Constructor

		#region HMACSHA3_512_ConfigValidation
		[Fact]
		public void HMACSHA3_512Config_Defaults_HaventChanged()
		{
			var config = new HMACSHA3_512Config();
			Assert.Equal(512, config.HashSizeInBits);
		}
		#endregion // HMACSHA3_512_ConfigValidation

		#endregion // HMACSHA3_512_Tests

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

		#region SHA3_256_Tests

		#region SHA3_256_Constructor

		#region SHA3_256_Config

		[Fact]
		public void SHA3_256_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_256_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_256_Implementation_Constructor_Config_IsCloned()
		{
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
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_384_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_384_Implementation_Constructor_Config_IsCloned()
		{
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
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new SHA3_512_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void SHA3_512_Implementation_Constructor_Config_IsCloned()
		{
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
			var config = new SHA3_512Config();
			Assert.Equal(512, config.HashSizeInBits);
		}
		#endregion // SHA3_512_ConfigValidation

		#endregion // SHA3_512_Tests
	}
}