
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
using System.Text;

namespace Infokom.Tools.Crypto
{

	
	public static class Hashes
	{
		private static readonly RipeMD160Digest RipeMD160Digest = new();
		private static readonly Sha1Digest SHA1Digest = new();
		private static readonly Sha224Digest SHA224Digest = new();
		private static readonly Sha256Digest SHA256Digest = new();
		private static readonly Sha384Digest SHA384Digest = new();
		private static readonly Sha512Digest SHA512Digest = new();
		private static readonly Sha3Digest SHA3Digest = new();
		private static readonly MD5Digest MD5Digest = new();


		private static byte[] ComputeHash(this IDigest digest, byte[] data)
		{
			// Convert the input string to a byte array and compute the hash.
			digest.BlockUpdate(data, 0, data.Length);

			byte[] hash = new byte[digest.GetDigestSize()];
			digest.DoFinal(hash, 0);
			
			return hash;
		}

		


		public enum DigestAlgorithm
		{
			MD5 = 1,
			RIPEMD160,
			SHA1,
			SHA224,
			SHA256,
			SHA384,
			SHA512,
			SHA3
		}

		public static byte[] ComputeHash(this DigestAlgorithm algorithm, byte[] data)
		{

			IDigest digest = algorithm switch
			{
				DigestAlgorithm.MD5 => MD5Digest,
				DigestAlgorithm.RIPEMD160 => RipeMD160Digest,
				DigestAlgorithm.SHA1 => SHA1Digest,
				DigestAlgorithm.SHA224 => SHA224Digest,
				DigestAlgorithm.SHA256 => SHA256Digest,
				DigestAlgorithm.SHA384 => SHA384Digest,
				DigestAlgorithm.SHA512 => SHA512Digest,
				DigestAlgorithm.SHA3 => SHA3Digest,
				_ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, "Digest not supported!"),
			};
			
			return digest.ComputeHash(data);
		}


		public static byte[] ComputeHash(this DigestAlgorithm algorithm, string text)
		{
			var data = Encoding.UTF8.GetBytes(text);

			var hash = algorithm.ComputeHash(data);

			return hash;
		}



		public enum DigestEncoding
		{
			Binary = 1,
			Hexadecimal,
			Base64,
			Base64Url
		}

		public static string Encode(this DigestEncoding encoding, byte[] data)
		{
			switch (encoding)
			{
				case DigestEncoding.Binary:
					{
						StringBuilder sb = new StringBuilder(data.Length * 8);
						foreach (byte b in data)
						{
							sb.Append(Convert.ToString(b, 2).PadLeft(8, '0')); // Convert each byte to binary and pad to 8 bits
						}
						return sb.ToString();
					}

				case DigestEncoding.Hexadecimal:
					{
						StringBuilder sb = new StringBuilder(data.Length * 2);
						foreach (byte b in data)
						{
							sb.Append(b.ToString("x2")); // "x2" formats to lowercase hex
						}
						return sb.ToString();
					}

				case DigestEncoding.Base64:
					return Convert.ToBase64String(data);
				case DigestEncoding.Base64Url:
					return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').TrimEnd('=');
				default:
					throw new ArgumentOutOfRangeException(nameof(encoding), encoding,"Digest encoding not supported!");
			}
		}
	}
}