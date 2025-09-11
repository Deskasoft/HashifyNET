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

using HashifyNet.Core;

namespace HashifyNet.Algorithms.CRC
{
	/// <summary>
	/// Defines a configuration for a <see cref="ICRC"/> implementation.
	/// </summary>
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC3_ROHC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC4_ITU))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC5_EPC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC5_ITU))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC5_USB))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC6_CDMA2000A))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC6_CDMA2000B))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC6_DARC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC6_ITU))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC7))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC7_ROHC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_CDMA2000))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_DARC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_DVBS2))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_EBU))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_ICODE))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_ITU))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_MAXIM))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_ROHC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC8_WCDMA))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC10))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC10_CDMA2000))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC11))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC12_3GPP))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC12_CDMA2000))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC12_DECT))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC13_BBC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC14_DARC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC15))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC15_MPT1327))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileARC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_AUGCCITT))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_BUYPASS))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_CCITTFALSE))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_CDMA2000))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_DDS110))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_DECTR))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_DECTX))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_DNP))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_EN13757))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_GENIBUS))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_MAXIM))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_MCRF4XX))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_RIELLO))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_T10DIF))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_TELEDISK))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_TMS37157))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC16_USB))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRCA))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileKERMIT))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileMODBUS))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileX25))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileXMODEM))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC24))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC24_FLEXRAYA))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC24_FLEXRAYB))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC31_PHILIPS))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32_BZIP2))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32C))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32D))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32_MPEG2))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32_POSIX))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC32Q))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileJAMCRC))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileXFER))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC40_GSM))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC64))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC64_WE))]
	[DeclareHashConfigProfile(typeof(CRCConfigProfileCRC64_XZ))]
	public class CRCConfig
		: ICRCConfig
	{
		/// <summary>
		/// Length of the produced CRC value, in bits.
		/// </summary>
		/// <value>
		/// The length of the produced CRC value, in bits
		/// </value>
		public int HashSizeInBits { get; set; }

		/// <summary>
		/// Divisor to use when calculating the CRC.
		/// </summary>
		/// <value>
		/// The divisor that will be used when calculating the CRC value.
		/// </value>
		public long Polynomial { get; set; }

		/// <summary>
		/// Value to initialize the CRC register to before calculating the CRC.
		/// </summary>
		/// <value>
		/// The value that will be used to initialize the CRC register before the calculation of the CRC value.
		/// </value>
		public long InitialValue { get; set; }

		/// <summary>
		/// If true, the CRC calculation processes input as big endian bit order.
		/// </summary>
		/// <value>
		/// <c>true</c> if the input should be processed in big endian bit order; otherwise, <c>false</c>.
		/// </value>
		public bool ReflectIn { get; set; }

		/// <summary>
		/// If true, the CRC calculation processes the output as big endian bit order.
		/// </summary>
		/// <value>
		/// <c>true</c> if the CRC calculation processes the output as big endian bit order; otherwise, <c>false</c>.
		/// </value>
		public bool ReflectOut { get; set; }

		/// <summary>
		/// Value to xor with the final CRC value.
		/// </summary>
		/// <value>
		/// The value to xor with the final CRC value.
		/// </value>
		public long XOrOut { get; set; }

		/// <summary>
		/// Makes a deep clone of current instance.
		/// </summary>
		/// <returns>A deep clone of the current instance.</returns>
		public ICRCConfig Clone() =>
			new CRCConfig()
			{
				HashSizeInBits = HashSizeInBits,
				Polynomial = Polynomial,
				InitialValue = InitialValue,
				ReflectIn = ReflectIn,
				ReflectOut = ReflectOut,
				XOrOut = XOrOut
			};
	}
}
