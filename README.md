> [!IMPORTANT]
> [HashifyNET Command Line Interface](https://github.com/Deskasoft/HashifyNETCLI) is now available!

HashifyNET<img width="32" height="32" src="https://github.com/Deskasoft/HashifyNET/blob/main/logo.png" alt="logo" />
==========

HashifyNET is a CLS-Compliant, platform-independent .NET library designed to offer a common interface for a wide range of [cryptographic](https://en.wikipedia.org/wiki/List_of_hash_functions#Keyed_cryptographic_hash_functions) and [non-cryptographic](https://en.wikipedia.org/wiki/List_of_hash_functions#Non-cryptographic_hash_functions) hashing algorithms.

All library functionality is tested using [xUnit](https://github.com/xunit/xunit). A primary requirement for each release is 100% code coverage by these tests.
All code within the library is commented using Visual Studio-compatible XML comments.

Should you require assistance or wish to join our community, please use the following link to access our Discord server: https://discord.gg/PrKery9

Differences
-----------
This library is the successor to https://github.com/Deskasoft/Data.HashFunction. The principal modification introduced by this library, while preserving substantial continuity in the API structure, is the rectification of the dependency burden previously incurred by `Data.HashFunction`.
All hash algorithms are now centralized within a singular assembly and corresponding package, HashifyNET. 
Furthermore, the associated namespaces have been concisely streamlined to HashifyNet.

The former factory-based implementation, which assigned a unique factory to each hash function, has been superseded by a more modular and centralized factory.
This new model provides superior accessibility and efficiency.

In addition, we introduced 33 more hash algorithms, sorted below:
- Adler32
- Blake3
- Gost
- HighwayHash
- SipHash
- Tiger
- Tiger2
- Whirlpool
- SM3
- Keccak
- Argon2id
- RapidHash
- T1HA0
- T1HA1
- T1HA2

Implementations introduced by wrapping existing .NET implementations:
- xxHash3
- xxHash3_128
- HMACMD5
- HMACSHA1
- HMACSHA256
- HMACSHA384
- HMACSHA512
- HMACSHA3_256
- HMACSHA3_384
- HMACSHA3_512
- MD5
- SHA1
- SHA256
- SHA384
- SHA512
- SHA3_256
- SHA3_384
- SHA3_512

> [!NOTE]
> Please check Implementations for the full list of available hash algorithms.

### Demo
You can see a working example of HasihfyNET using Blazor right [here](https://deskasoft.github.io/HashifyNETDemo/).
You can also click on the showcase below to go to the demo.
[<img width="1280" height="720" alt="hashifynet_demo_short" src="https://github.com/Deskasoft/HashifyNETDemo/blob/main/hashifynet_demo.webp" />](https://deskasoft.github.io/HashifyNETDemo/)

### NuGet
You can directly bind HashifyNET to your project through NuGet below:

[![Version Status](https://img.shields.io/nuget/v/HashifyNET.svg)](https://www.nuget.org/packages/HashifyNET/)

Implementations
---------------

The following hash functions have been implemented from the most reliable reference that could be found:

* [Adler32](https://en.wikipedia.org/wiki/Adler-32)
  * Original Adler32 implementation
* [Argon2id](https://en.wikipedia.org/wiki/Argon2)
  * Argon2id - Implements through a custom Isopoh Cryptography code that uses our Blake2B implementation (without any dependency) - https://github.com/mheyman/Isopoh.Cryptography.Argon2
  * Argon2id Standards - 3 implementations for standardized Argon2id configs, including but not limited to OWASP's Standard.
* [Bernstein Hash](http://www.eternallyconfuzzled.com/tuts/algorithms/jsw_tut_hashing.aspx#djb)
  * BernsteinHash - Original
  * ModifiedBernsteinHash - Minor update that is said to result in better distribution
* [Blake2](https://blake2.net/)
  * Blake2b
* [Blake3](https://en.wikipedia.org/wiki/BLAKE_(hash_function)#BLAKE3)
* [BuzHash](http://www.serve.net/buz/hash.adt/java.002.html)
  * BuzHashBase - Abstract implementation; there is no authoritative implementation
  * DefaultBuzHash - Concrete implementation, uses 256 random 64-bit integers
* [CityHash](https://code.google.com/p/cityhash/)
* [CRC](http://en.wikipedia.org/wiki/Cyclic_redundancy_check)
  * CRC - Generalized implementation to allow any CRC parameters between 1 and 64 bits.
  * CRCStandards - 71 implementations on top of CRC that use the parameters defined by their respective standard.  Standards and their parameters provided by [CRC RevEng's catalogue](http://reveng.sourceforge.net/crc-catalogue/).
* [ELF64](http://downloads.openwatcom.org/ftp/devel/docs/elf-64-gen.pdf)
* [FarmHash](https://github.com/google/farmhash)
* [FNV](http://www.isthe.com/chongo/tech/comp/fnv/index.html)
  * FNV1Base - Abstract base of the FNV-1 algorithms
  * FNV1 - Original
  * FNV1a - Minor variation of FNV-1
* [Gost](https://en.wikipedia.org/wiki/GOST_(hash_function))
  * Gost - Implementation of Streebog Family (GOST R 34.11-2012).
* [HighwayHash](https://github.com/google/highwayhash)
* [Jenkins](http://en.wikipedia.org/wiki/Jenkins_hash_function)
  * JenkinsOneAtATime - Original
  * JenkinsLookup2 - Improvement upon One-at-a-Time hash function
  * JenkinsLookup3 - Further improvement upon Jenkins' Lookup2 hash function
* [Keccak](https://en.wikipedia.org/wiki/SHA-3)
  * Keccak - Implementation of both with SHA-3 padding and with original Keccak padding.
* [MetroHash](https://github.com/jandrewrogers/MetroHash)
* [Murmur Hash](https://code.google.com/p/smhasher/wiki/MurmurHash)
  * MurmurHash1 - Original
  * MurmurHash2 - Improvement upon MurmurHash1
  * MurmurHash3 - Further improvement upon MurmurHash2, addresses minor flaws
* [Pearson hashing](http://en.wikipedia.org/wiki/Pearson_hashing)
  * PearsonBase - Abstract implementation; there is no authoritative implementation
  * WikipediaPearson - Concrete implementation, uses values from the Wikipedia article
* [RapidHash](https://github.com/Nicoshev/rapidhash)
  * Supports Original, Micro, and Nano modes.
* [SipHash](https://en.wikipedia.org/wiki/SipHash)
* [SM3](https://en.wikipedia.org/wiki/SM3_(hash_function))
* [SpookyHash](http://burtleburtle.net/bob/hash/spooky.html)
  * SpookyHashV1 - Original
  * SpookyHashV2 - Improvement upon SpookyHashV1, fixes bug in original specification
* [Tiger](https://en.wikipedia.org/wiki/Tiger_(hash_function))
  * Tiger - Original
  * Tiger2 - The padding at the beginning of the algorithm changes from 0x01 to 0x80. This is the only difference from the original Tiger algorithm.
* [T1HA](https://github.com/erthink/t1ha)
  * T1HA0 - 64-bit Little-Endian version.
  * T1HA1 - 64-bit Little-Endian version.
  * T1HA2 - 64-bit and 128-bit Little-Endian versions.
* [Whirlpool](https://en.wikipedia.org/wiki/Whirlpool_(hash_function))
* [xxHash](https://code.google.com/p/xxhash/)
  * xxHash - Original and 64-bit version.

Wrapped Implementations
-----------------------
The implementations below are backed by .NET directly and are added to be accessible through our universal factory:

* [xxHash3](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.xxhash3)
* [xxHash128](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.xxhash128)
* [HMACMD5](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACMD5)
* [HMACSHA1](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA1)
* [HMACSHA256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA256)
* [HMACSHA384](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA384)
* [HMACSHA512](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA512)
* [HMACSHA3_256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA3_256)
* [HMACSHA3_384](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA3_384)
* [HMACSHA3_512](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.HMACSHA3_512)
* [MD5](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.MD5)
* [SHA1](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA1)
* [SHA256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA256)
* [SHA384](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA384)
* [SHA512](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA512)
* [SHA3_256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA3_256)
* [SHA3_384](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA3_384)
* [SHA3_512](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.SHA3_512)

Usage
-----

Usage for all hash functions has been further standardized and is now accessible through the `HashifyNet.HashFactory` class.

The `HashifyNet.HashFactory` class supports both generics and with access to each hash function via `System.Type`.

To access a hash function via generics, you can do this:
``` C#
using System;
using HashifyNet;
using HashifyNet.Algorithms.Adler32;

public class Program
{
    static void Main()
    {
        IAdler32 adler32 = HashFactory<IAdler32>.Create();
        IHashValue computedHash = adler32.ComputeHash("foobar");

        Console.WriteLine(computedHash.AsHexString()); 
    }
}
```

To access a hash function via `System.Type`, you can do this:
``` C#
using System;
using HashifyNet;
using HashifyNet.Algorithms.Adler32;

public class Program
{
    static void Main()
    {
        IHashFunctionBase adler32 = HashFactory.Create(typeof(IAdler32));
        IHashValue computedHash = adler32.ComputeHash("foobar");

        Console.WriteLine(computedHash.AsHexString()); 
    }
}
```

## Batch Computation Examples
Thanks to our latest design, we can now calculate multiple hashes at once, in case, for some reason, they need all cryptographic or non-cryptographic hashes.

### Example using 'System.Type' to compute all non-cryptographic hashes:
``` CSharp
IHashFunctionBase[] functions = HashFactory.CreateHashAlgorithms(HashFunctionType.Noncryptographic, new Dictionary<Type, IHashConfigBase>()
{

	// Only adding configs that require us to pick or define one, for the rest of the hash algorithms, the default provided configs will be used instead.
	{ typeof(ICRC), new CRCConfigProfileCRC32() },
	{ typeof(IPearson), new PearsonConfigProfileWikipedia() },
	{ typeof(IFNV1), new FNVConfigProfile32Bits()},
	{ typeof(IFNV1a), new FNVConfigProfile32Bits() },
	{ typeof(IBuzHash), new BuzHashConfigProfileDefault() },

});

foreach (IHashFunctionBase function in functions)
{
	IHashValue hv = function.ComputeHash("foobar");
	// Use the computed hash here...
}
```

### Example using 'System.Type' to compute all cryptographic hashes except `IBlake3`:
``` CSharp
IHashFunctionBase[] functions = HashFactory.CreateHashAlgorithms(HashFunctionType.Cryptographic, new Dictionary<Type, IHashConfigBase>()
{

	// Only adding configs that require us to pick or define one, for the rest of the hash algorithms, the default provided configs will be used instead.
	{ typeof(IArgon2id), new Argon2idConfigProfileOWASP() }

}, typeof(IBlake3)); // (Example) We do not want Blake3, though you can add as many as you want to ignore, including base interfaces to ignore all derived interfaces (such as IFNV to also ignore IFNV1 and IFNV1a).

foreach (IHashFunctionBase function in functions)
{
	IHashValue hv = function.ComputeHash("foobar");

	// This ensures that we only try disposing of cryptographic hashes.
	if (function is ICryptographicHashFunctionBase cryptoHash)
	{
		cryptoHash.Dispose();
	}
}
```

A significant number of additional modifications have been implemented within the library. We encourage you to integrate these updates into your project and explore them fully.
We are eager to observe the innovative applications you develop, or have already developed, using our library. Please feel free to share your work with us by joining our community at: https://discord.gg/PrKery9.

Please visit [wiki/Release-Notes](https://github.com/Deskasoft/HashifyNET/wiki/Release-Notes) for more information about usage examples and the latest features available.

## Versioning Guarantees

This library generally abides by [Semantic Versioning](https://semver.org). Packages are published in `MAJOR.MINOR.PATCH` version format.

### Patch component

An increment of the **PATCH** component always indicates that an internal-only change was made, generally a bug fix. These changes will not affect the public-facing API in any way, and are always guaranteed to be forward/backwards-compatible with your codebase, any pre-compiled dependencies of your codebase.

### Minor component

An increment of the **MINOR** component indicates that some addition was made to the library,
and this addition **may not be** backwards-compatible with prior versions.

### Major component

An increment of the **MAJOR** component indicates that breaking changes have been made to the library;
**Consumers should check the release notes to determine what changes need to be made.**

Contributing
------------

Feel free to propose changes, notify of issues, or contribute code using GitHub!  Submit issues and/or pull requests as necessary. 

There are no special requirements for change proposals or issue notifications. 

Code contributions should follow the existing code's methodologies and style, along with XML comments for all public and protected namespaces, classes, and functions added.

License
-------

HashifyNET is released under the terms of the MIT license. See [LICENSE](https://github.com/deskasoft/HashifyNET/blob/master/LICENSE) for more information or see http://opensource.org/licenses/MIT.


