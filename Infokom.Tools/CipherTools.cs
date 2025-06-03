using System.Security.Cryptography;

namespace Infokom.Tools
{
	public static class CipherTools
	{		

		public enum Algorithm
		{
			AES,
			TripleDES,
			Rabbit,
			RC4,
		}

		private static SymmetricAlgorithm CreateSymmetricAlgorithm(this Algorithm source) => source switch
		{
			Algorithm.AES => Aes.Create(),
			Algorithm.TripleDES => TripleDES.Create(),
			_ => throw new NotSupportedException()
		};

		private static int KeySize(this Algorithm source) => source switch
		{
			Algorithm.AES => 32,
			Algorithm.TripleDES => 24,
			_ => throw new NotSupportedException()
		};

		private static int IVSize(this Algorithm source) => source switch
		{
			Algorithm.AES => 16,
			Algorithm.TripleDES => 8,
			_ => throw new NotSupportedException()
		};

		private static int SaltSize(this Algorithm source) => source switch
		{
			Algorithm.AES => 16,
			Algorithm.TripleDES => 8,
			_ => throw new NotSupportedException()
		};

		public static string Encrypt(this Algorithm source, string password, string text)
		{
			byte[] salt = RandomNumberGenerator.GetBytes(source.SaltSize());

			using (var algorithm = source.CreateSymmetricAlgorithm())
			{
				using (var keyDeriver = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256))
				{
					algorithm.Key = keyDeriver.GetBytes(source.KeySize());
					algorithm.IV = keyDeriver.GetBytes(source.IVSize());

					using var ms = new MemoryStream();
					ms.Write(salt, 0, salt.Length); // prepend salt

					using var cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
					using var sw = new StreamWriter(cs);
					sw.Write(text);

					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public static string Decrypt(this Algorithm source, string password, string text)
		{
			byte[] encryptedBytes = Convert.FromBase64String(text);

			byte[] salt = new byte[source.SaltSize()];
			Array.Copy(encryptedBytes, 0, salt, 0, 16);

			using (var algorithm = source.CreateSymmetricAlgorithm())
			{
				using (var keyDeriver = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256))
				{
					algorithm.Key = keyDeriver.GetBytes(source.KeySize());
					algorithm.IV = keyDeriver.GetBytes(source.IVSize());

					using var ms = new MemoryStream(encryptedBytes, 16, encryptedBytes.Length - 16);
					using var cs = new CryptoStream(ms, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
					using var sr = new StreamReader(cs);
					return sr.ReadToEnd();
				}
			}
		}
	}
}
