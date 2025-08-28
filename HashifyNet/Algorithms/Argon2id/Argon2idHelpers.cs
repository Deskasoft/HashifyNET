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

using Isopoh.Cryptography.Argon2;
using System;

namespace HashifyNet.Algorithms.Argon2id
{
	internal static class Argon2idHelpers
	{
		internal static byte[] GenerateSalt()
		{
			byte[] salt = new byte[16];
			using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		internal static Argon2Config GetArgon2Config(IArgon2idConfig config, byte[] password, byte[] salt)
		{
			return new Argon2Config()
			{
				AssociatedData = config.AssociatedData,
				HashLength = config.HashSizeInBits / 8,
				Lanes = config.DegreeOfParallelism,
				Salt = salt,
				Password = password,
				Secret = config.Secret,
				Threads = Environment.ProcessorCount / 2,
				TimeCost = config.Iterations,
				Type = Argon2Type.HybridAddressing,
				Version = (Argon2Version)config.Version,
				MemoryCost = config.MemorySize,

				ClearPassword = false,
				ClearSecret = false,
			};
		}
	}
}
