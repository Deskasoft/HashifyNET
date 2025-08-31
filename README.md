HashifyNET
=================

HashifyNET is a C# library designed to offer a common interface for a wide range of [cryptographic](https://en.wikipedia.org/wiki/List_of_hash_functions#Keyed_cryptographic_hash_functions) and [non-cryptographic](https://en.wikipedia.org/wiki/List_of_hash_functions#Non-cryptographic_hash_functions) hashing algorithms, while also providing built-in implementations of numerous well-known hash functions.

All functionality of the library is tested using [xUnit](https://github.com/xunit/xunit). A primary requirement for each release is 100% code coverage by these tests.
All code within the library is commented using Visual Studio-compatible XML comments.

You can join our Discord at https://discord.gg/PrKery9 any time you'd need support or just to join our Family.

Differences
-----------
This library is the successor to https://github.com/Deskasoft/Data.HashFunction. While keeping the API structure *mostly* the same, the biggest change this library brings is the clearance of the dependency overhaul previously caused by Data.HashFunction.
Every hash algorithm is now collected under one single assembly and package named HashifyNET, and all of the namespaces are shortened to HashifyNet.

The former factory-based implementation, which assigned a unique factory to each hash function, has been superseded by a more modular and centralized factory.
This new model provides superior accessibility and efficiency.

As an addition, we introduced 11 more hash algorithms, sorted below:
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

> [!NOTE]
> Please check Implementations for the full list of available hash algorithms.

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
* [Hash Algorithm Wrapper](http://msdn.microsoft.com/en-us/library/system.security.cryptography.hashalgorithm%28v=vs.110%29.aspx)
  * HashAlgorithmWrapper - Wraps an existing instance of a .NET HashAlgorithm
  * HashAlgorithmWrapper<HashAlgorithmT> - Wraps a managed instance of a .NET HashAlgorithm
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
* [SipHash](https://en.wikipedia.org/wiki/SipHash)
* [SM3](https://en.wikipedia.org/wiki/SM3_(hash_function))
* [SpookyHash](http://burtleburtle.net/bob/hash/spooky.html)
  * SpookyHashV1 - Original
  * SpookyHashV2 - Improvement upon SpookyHashV1, fixes bug in original specification
* [Tiger](https://en.wikipedia.org/wiki/Tiger_(hash_function))
  * Tiger - Original
  * Tiger2 - The padding at the beginning of the algorithm changes from 0x01 to 0x80. This is the only difference from the original Tiger algorithm.
* [Whirlpool](https://en.wikipedia.org/wiki/Whirlpool_(hash_function))
* [xxHash](https://code.google.com/p/xxhash/)
  * xxHash - Original and 64-bit version.

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
        IAdler32 adler32 = HashFactory<IAdler32>.Instance.Create();
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
        IHashFunctionBase adler32 = HashFactory.Instance.Create(typeof(IAdler32));
        IHashValue computedHash = adler32.ComputeHash("foobar");

        Console.WriteLine(computedHash.AsHexString()); 
    }
}
```

There are a lot more changes made to the library, and feel free to explore them by adding to your project.
We'd love to see what you are going to do or have done with our library, so feel free to share them with us at https://discord.gg/PrKery9.

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







