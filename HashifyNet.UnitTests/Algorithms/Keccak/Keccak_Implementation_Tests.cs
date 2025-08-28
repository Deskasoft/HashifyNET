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

using HashifyNet.Algorithms.Keccak;
using HashifyNet.UnitTests.Utilities;
using Moq;

namespace HashifyNet.UnitTests.Algorithms.Keccak
{
	public class Keccak_Implementation_Tests
	{
		#region Constructor

		#region Config

		[Fact]
		public void Keccak_Implementation_Constructor_Config_IsNull_Throws()
		{
			Assert.Equal(
				"config",
				Assert.Throws<ArgumentNullException>(
						() => new Keccak_Implementation(null))
					.ParamName);
		}

		[Fact]
		public void Keccak_Implementation_Constructor_Config_IsCloned()
		{
			var KeccakConfigMock = new Mock<IKeccakConfig>();
			{
				KeccakConfigMock.SetupGet(bc => bc.HashSizeInBits)
					.Returns(512);

				KeccakConfigMock.Setup(bc => bc.Clone())
					.Returns(new KeccakConfig() { HashSizeInBits = 512 });
			}

			GC.KeepAlive(
				new Keccak_Implementation(KeccakConfigMock.Object));

			KeccakConfigMock.Verify(bc => bc.Clone(), Times.Once);

			KeccakConfigMock.VerifyGet(bc => bc.HashSizeInBits, Times.Never);
		}

		#region HashSize

		[Fact]
		public void Keccak_Implementation_Constructor_Config_HashSize_IsInvalid_Throws()
		{
			var invalidHashSizes = new[] { -1, 0, 800, 801, 900, 1024 };

			foreach (var invalidHashSize in invalidHashSizes)
			{
				var KeccakConfigMock = new Mock<IKeccakConfig>();
				{
					KeccakConfigMock.SetupGet(bc => bc.HashSizeInBits)
						.Returns(invalidHashSize);

					KeccakConfigMock.Setup(bc => bc.Clone())
						.Returns(() => KeccakConfigMock.Object);
				}

				Assert.Equal(
					"config.HashSizeInBits",
					Assert.Throws<ArgumentOutOfRangeException>(() =>
							new Keccak_Implementation(
								KeccakConfigMock.Object))
						.ParamName);
			}
		}

		#endregion

		#region HashSizeInBits

		[Fact]
		public void Keccak_Implementation_HashSizeInBits_IsFromConfig()
		{
			var keccakConfig2 = Mock.Of<IKeccakConfig>(bc => bc.HashSizeInBits == 512 && bc.Clone() == new KeccakConfig());
			var keccakConfig = Mock.Of<IKeccakConfig>(bc => bc.Clone() == keccakConfig2);

			var keccakHash = new Keccak_Implementation(keccakConfig);

			Assert.Equal(512, keccakHash.Config.HashSizeInBits);
		}

		#endregion

		#endregion

		#endregion

		public class IStreamableHashFunction_Tests_DefaultConstructor
			: IStreamableHashFunction_TestBase<IKeccak, IKeccakConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(512, TestConstants.Empty, "0eab42de4c3ceb9235fc91acffe746b29c29a8c366b7c60e4e67c466f36a4304c00fa9caf9d87976ba469bcbe06713b435f091ef2769fb160cdab33d3670680e"),
					new KnownValue(512, TestConstants.FooBar, "927618d193a11374f6072cdcb8c410e2f18e0c433eb35a9f11ce3035b0066811db6c03a723a2855c4a8ee2b1c842e28d4982a1ff312dd4ddaf807b96d4d2ee1b"),
					new KnownValue(512, TestConstants.LoremIpsum, "235620aad1eb5bdd61a518547b8fd33b327daa1cab303f33760aa1b3ddbb5761437929fc387053335c34fe94d537f5691fe7518d64388784ea5b1b5f7dda06da"),
					new KnownValue(512, new byte[4096], "fbbd12bb56f59360b5994f47df5264f923500506457b16c102a4a885e6109675747b018789116afb6d24311b975a84db85f39e88be960d213f4cad93c1c2aa3f"),
				};

			protected override IKeccak CreateHashFunction(int hashSize) =>
				new Keccak_Implementation(
					new KeccakConfig());
		}

		public class IStreamableHashFunction_Tests_WithHashSize
			: IStreamableHashFunction_TestBase<IKeccak, IKeccakConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(224, TestConstants.Empty, "f71837502ba8e10837bdd8d365adb85591895602fc552b48b7390abd"),
					new KnownValue(256, TestConstants.Empty, "c5d2460186f7233c927e7db2dcc703c0e500b653ca82273b7bfad8045d85a470"),
					new KnownValue(384, TestConstants.Empty, "2c23146a63a29acf99e73b88f8c24eaa7dc60aa771780ccc006afbfa8fe2479b2dd2b21362337441ac12b515911957ff"),
					new KnownValue(512, TestConstants.Empty, "0eab42de4c3ceb9235fc91acffe746b29c29a8c366b7c60e4e67c466f36a4304c00fa9caf9d87976ba469bcbe06713b435f091ef2769fb160cdab33d3670680e"),

					new KnownValue(224, TestConstants.FooBar, "f5dd6617f67e2b6a7b5ef75d1931ef36ee63ca35d06bcc714a74a386"),
					new KnownValue(256, TestConstants.FooBar, "38d18acb67d25c8bb9942764b62f18e17054f66a817bd4295423adf9ed98873e"),
					new KnownValue(384, TestConstants.FooBar, "e8c02310ada7fbf1c550713cdaa0a3eaf02ee13990f73851e7e5a183f99df541d833424e702e4e22eb4306b7bcbeb965"),
					new KnownValue(512, TestConstants.FooBar, "927618d193a11374f6072cdcb8c410e2f18e0c433eb35a9f11ce3035b0066811db6c03a723a2855c4a8ee2b1c842e28d4982a1ff312dd4ddaf807b96d4d2ee1b"),

					new KnownValue(224, TestConstants.LoremIpsum, "0383bddddb710bf224625014ae7712e2545f47bdd714c9b9ed1a2e96"),
					new KnownValue(256, TestConstants.LoremIpsum, "b910cbdf53f2953cd11500979573cd475e1ea30fdd10964fde7c1442f3335930"),
					new KnownValue(384, TestConstants.LoremIpsum, "b6e95407b75dc5591fa604d63c7849234a4a8fa30db1ead839f47ca2798f3cdc3090f194cfe0589aba9d33e6205da4ee"),
					new KnownValue(512, TestConstants.LoremIpsum, "235620aad1eb5bdd61a518547b8fd33b327daa1cab303f33760aa1b3ddbb5761437929fc387053335c34fe94d537f5691fe7518d64388784ea5b1b5f7dda06da"),

					new KnownValue(224, new byte[4096], "5ab054891ed5b12db51d662ddfcf694291ded57b02c572679a39a451"),
					new KnownValue(256, new byte[4096], "a8bae11751799de4dbe638406c5c9642c0e791f2a65e852a05ba4fdf0d88e3e6"),
					new KnownValue(384, new byte[4096], "ddd86d18feed5764bfcc14fc894c751fd933bde8ea51ecb4c0f6057c4253abfb9f2573f9477b267b903aba1de5e3c1f6"),
					new KnownValue(512, new byte[4096], "fbbd12bb56f59360b5994f47df5264f923500506457b16c102a4a885e6109675747b018789116afb6d24311b975a84db85f39e88be960d213f4cad93c1c2aa3f"),
				};

			protected override IKeccak CreateHashFunction(int hashSize) =>
				new Keccak_Implementation(
					new KeccakConfig()
					{
						HashSizeInBits = hashSize
					});
		}

		public class IStreamableHashFunction_Tests_WithHashSize_And_SHA3_Padding
	: IStreamableHashFunction_TestBase<IKeccak, IKeccakConfig>
		{
			protected override IEnumerable<KnownValue> KnownValues { get; } =
				new KnownValue[] {
					new KnownValue(224, TestConstants.Empty, "6b4e03423667dbb73b6e15454f0eb1abd4597f9a1b078e3f5b5a6bc7"),
					new KnownValue(256, TestConstants.Empty, "a7ffc6f8bf1ed76651c14756a061d662f580ff4de43b49fa82d80a4b80f8434a"),
					new KnownValue(384, TestConstants.Empty, "0c63a75b845e4f7d01107d852e4c2485c51a50aaaa94fc61995e71bbee983a2ac3713831264adb47fb6bd1e058d5f004"),
					new KnownValue(512, TestConstants.Empty, "a69f73cca23a9ac5c8b567dc185a756e97c982164fe25859e0d1dcc1475c80a615b2123af1f5f94c11e3e9402c3ac558f500199d95b6d3e301758586281dcd26"),

					new KnownValue(224, TestConstants.FooBar, "1ad852ba147a715fe5a3df39a741fad08186c303c7d21cefb7be763b"),
					new KnownValue(256, TestConstants.FooBar, "09234807e4af85f17c66b48ee3bca89dffd1f1233659f9f940a2b17b0b8c6bc5"),
					new KnownValue(384, TestConstants.FooBar, "0fa8abfbdaf924ad307b74dd2ed183b9a4a398891a2f6bac8fd2db7041b77f068580f9c6c66f699b496c2da1cbcc7ed8"),
					new KnownValue(512, TestConstants.FooBar, "ff32a30c3af5012ea395827a3e99a13073c3a8d8410a708568ff7e6eb85968fccfebaea039bc21411e9d43fdb9a851b529b9960ffea8679199781b8f45ca85e2"),

					new KnownValue(224, TestConstants.LoremIpsum, "1d5b37fd655287140291f720f9c7b5e77c20f57215cb1ec4b0d4a0fd"),
					new KnownValue(256, TestConstants.LoremIpsum, "9eb0310ee026103ec862e8a48618a50ef6ef5811d57c4ed55bb8983286c54aa2"),
					new KnownValue(384, TestConstants.LoremIpsum, "e0ed9372a56fe0120efb0849055310a64ce27ec68adc3966b2afbd405a97626c0cb60cbd4fb68eaec0c47de37bb7f252"),
					new KnownValue(512, TestConstants.LoremIpsum, "b9e641a23f9adcbd9b429810788bbb267f8ad3c104c339b0d66d20e370f9b0c8d576e344b2cb367a60af8fefadc884821044d2495c391fc65ec81cc842ded77e"),

					new KnownValue(224, new byte[4096], "3599d0b6e892aaa6eeecb4766b2c060ccf10bb06cd5b466369e8cd1f"),
					new KnownValue(256, new byte[4096], "a99f9ed58079237f7f0275887f0c03a0c9d7d8de4443842297fceea67e423563"),
					new KnownValue(384, new byte[4096], "101a0ad750000a40201c131839c306d8fb9b12eaf3467ca0c2dba40a1dd781834e2ca757f188da856c0d1b7616638105"),
					new KnownValue(512, new byte[4096], "a86605e7ef28ed75ea27bc86d402c324ee7c9773a42b689f058cf850c92a3424d29a5d9a58584d18b3a116dc9bf4086227ed45e0ed9b53453ab41a699473a811"),
				};

			protected override IKeccak CreateHashFunction(int hashSize) =>
				new Keccak_Implementation(
					new KeccakConfig()
					{
						HashSizeInBits = hashSize,
						UseSha3Padding = true,
					});
		}
	}
}