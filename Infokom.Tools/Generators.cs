using System.Security.Cryptography;
using System.Text;

namespace Infokom.Tools
{

	public static class Generators
	{
		private static readonly char[] Base32Chars = "0123456789ABCDEFGHJKMNPQRSTVWXYZ".ToCharArray();

		private static string EncodeBase32(byte[] data)
		{
			StringBuilder sb = new StringBuilder(26);
			int bitBuffer = 0, bitBufferLen = 0;

			foreach (byte b in data)
			{
				bitBuffer = bitBuffer << 8 | b;
				bitBufferLen += 8;

				while (bitBufferLen >= 5)
				{
					int index = bitBuffer >> bitBufferLen - 5 & 0x1F;
					sb.Append(Base32Chars[index]);
					bitBufferLen -= 5;
				}
			}

			if (bitBufferLen > 0)
			{
				int index = bitBuffer << 5 - bitBufferLen & 0x1F;
				sb.Append(Base32Chars[index]);
			}

			return sb.ToString();
		}


		public static string GenerateULID()
		{
			var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			byte[] timeBytes = BitConverter.GetBytes(timestamp);
			if (BitConverter.IsLittleEndian) Array.Reverse(timeBytes);

			byte[] entropy = new byte[10];
			RandomNumberGenerator.Fill(entropy);

			byte[] ulidBytes = new byte[16];
			Array.Copy(timeBytes, 2, ulidBytes, 0, 6); // 48-bit time
			Array.Copy(entropy, 0, ulidBytes, 6, 10);  // 80-bit entropy

			return EncodeBase32(ulidBytes);
		}


		public static Guid GenerateGUID()
		{
			return Guid.NewGuid();
		}
	}

}
