// <copyright file="DefaultOsxSecureArrayCall.cs" company="Isopoh">
// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.
// </copyright>

using Isopoh.Cryptography.SecureArray.OsxNative;
using System.Runtime.InteropServices;

namespace Isopoh.Cryptography.SecureArray
{
	/// <summary>
	/// A <see cref="SecureArrayCall"/> with defaults for the OSX operating system.
	/// </summary>
	internal class DefaultOsxSecureArrayCall : SecureArrayCall
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultOsxSecureArrayCall"/> class.
		/// </summary>
		public DefaultOsxSecureArrayCall()
			: base(
				  (m, l) => UnsafeNativeMethods.OsxMemset(m, 0, l),
				  (m, l) => UnsafeNativeMethods.OsxMlock(m, l) != 0 ? $"mlock error code: {Marshal.GetLastWin32Error()}" : null,
				  (m, l) =>
				  {
					  _ = UnsafeNativeMethods.OsxMunlock(m, l);
				  },
				  "OSX")
		{
		}
	}
}
