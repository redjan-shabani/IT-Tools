using System.Security.Cryptography;
using System.Text;

namespace Infokom.Tools.Crypto
{
	public static class RSA
	{
		public static (string PublicKey, string PrivateKey) GenerateKeys(int keySize = 2048)
		{
			using (var rsa = new RSACryptoServiceProvider(keySize))
			{
				var publicKey = rsa.ExportRSAPublicKeyPem();
				var privateKey = rsa.ExportRSAPrivateKeyPem();

				//var publicKey = rsa.ToXmlString(false); // false for public key
				//var privateKey = rsa.ToXmlString(true); // true for private key

				return (publicKey, privateKey);
			}
		}

		public static string Encrypt(string publicKey, string text)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.FromXmlString(publicKey);
				var data = Encoding.UTF8.GetBytes(text);
				var encryptedData = rsa.Encrypt(data, false);
				return Convert.ToBase64String(encryptedData);
			}
		}

		public static string Decrypt(string privateKey, string encryptedText)
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.FromXmlString(privateKey);
				var data = Convert.FromBase64String(encryptedText);
				var decryptedData = rsa.Decrypt(data, false);
				return Encoding.UTF8.GetString(decryptedData);
			}
		}
	}
}
