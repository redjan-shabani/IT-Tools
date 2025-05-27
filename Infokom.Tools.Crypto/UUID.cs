using System.Security.Cryptography;
using System.Text;

namespace Infokom.Tools.Crypto
{

	public static class UUID
	{
		public enum Version : byte
		{
			NIL = 0,
			V1 = 1,
			V3 = 3,
			V4 = 4,
			V5 = 5
		}





		public enum Namespace
		{
			DNS,
			URL,
			OID,
			X500
		}

		private const string DNS_NAMESPACE = "6ba7b810-9dad-11d1-80b4-00c04fd430c8";
		private const string URL_NAMESPACE = "6ba7b811-9dad-11d1-80b4-00c04fd430c8";
		private const string OID_NAMESPACE = "6ba7b812-9dad-11d1-80b4-00c04fd430c8";
		private const string X500_NAMESPACE = "6ba7b814-9dad-11d1-80b4-00c04fd430c8";

		public static Guid GetUUID(this Namespace source) => source switch
		{
			Namespace.DNS => Guid.Parse(DNS_NAMESPACE),
			Namespace.URL => Guid.Parse(URL_NAMESPACE),
			Namespace.OID => Guid.Parse(OID_NAMESPACE),
			Namespace.X500 => Guid.Parse(X500_NAMESPACE),
			_ => throw new InvalidOperationException(),
		};




		// RFC 4122 requires specific byte order
		private static void SwapGuidByteOrder(byte[] guidBytes)
		{
			void Swap(int a, int b) => (guidBytes[a], guidBytes[b]) = (guidBytes[b], guidBytes[a]);

			Swap(0, 3); Swap(1, 2);
			Swap(4, 5);
			Swap(6, 7);
		}

		private static Guid GenerateV1()
		{
			byte[] uuid = new byte[16];

			// Time since 1582-10-15 in 100-nanosecond intervals
			DateTimeOffset epoch = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);
			long timestamp = (DateTimeOffset.UtcNow - epoch).Ticks * 100;

			// Insert timestamp (low-mid-hi bits)
			byte[] timeBytes = BitConverter.GetBytes(timestamp);
			if (BitConverter.IsLittleEndian) Array.Reverse(timeBytes);
			Array.Copy(timeBytes, 2, uuid, 0, 4);  // time_low
			Array.Copy(timeBytes, 0, uuid, 4, 2);  // time_mid
			Array.Copy(timeBytes, 6, uuid, 6, 2);  // time_hi

			// Set version to 1
			uuid[6] = (byte)((uuid[6] & 0x0F) | 0x10);

			// Clock sequence
			RandomNumberGenerator.Fill(uuid.AsSpan(8, 2));
			uuid[8] = (byte)((uuid[8] & 0x3F) | 0x80); // RFC 4122 variant

			// Node (simulate MAC address)
			RandomNumberGenerator.Fill(uuid.AsSpan(10, 6));
			uuid[10] |= 0x01; // Set multicast bit to avoid real MAC

			return new Guid(uuid);
		}

		public static Guid[] V1(int quantity = 1)
		{
			if (quantity <= 0) return Array.Empty<Guid>();

			var guids = new Guid[quantity];
			for (int i = 0; i < quantity; i++)
			{
				guids[i] = GenerateV1();
			}
			return guids;
		}


		private static Guid GenerateUuidV3(Guid namespaceId, string name)
		{
			// Convert namespace UUID to bytes
			byte[] namespaceBytes = namespaceId.ToByteArray();
			SwapGuidByteOrder(namespaceBytes); // Fix byte order for RFC

			// Combine with name bytes
			byte[] nameBytes = Encoding.UTF8.GetBytes(name);
			byte[] input = new byte[namespaceBytes.Length + nameBytes.Length];
			Buffer.BlockCopy(namespaceBytes, 0, input, 0, namespaceBytes.Length);
			Buffer.BlockCopy(nameBytes, 0, input, namespaceBytes.Length, nameBytes.Length);

			// Compute MD5 hash
			using MD5 md5 = MD5.Create();
			byte[] hash = md5.ComputeHash(input);

			// Set version to 3 (MD5)
			hash[6] = (byte)((hash[6] & 0x0F) | (3 << 4));
			// Set variant to RFC 4122
			hash[8] = (byte)((hash[8] & 0x3F) | 0x80);

			// First 16 bytes = UUID
			byte[] newGuid = new byte[16];
			Array.Copy(hash, 0, newGuid, 0, 16);
			SwapGuidByteOrder(newGuid); // Restore .NET byte order

			return new Guid(newGuid);
		}

		public static Guid GenerateV4() => Guid.NewGuid();

		public static Guid[] V4(int quantity = 1)
		{
			if (quantity <= 0) return Array.Empty<Guid>();

			var guids = new Guid[quantity];
			for (int i = 0; i < quantity; i++)
			{
				guids[i] = GenerateV4();
			}
			return guids;
		}

		private static Guid GenerateV5(Guid namespaceId, string name)
		{
			// Convert namespace UUID to bytes
			byte[] namespaceBytes = namespaceId.ToByteArray();
			SwapGuidByteOrder(namespaceBytes); // RFC 4122 byte order fix

			// Combine with name bytes
			byte[] nameBytes = Encoding.UTF8.GetBytes(name);
			byte[] hashInput = new byte[namespaceBytes.Length + nameBytes.Length];
			Buffer.BlockCopy(namespaceBytes, 0, hashInput, 0, namespaceBytes.Length);
			Buffer.BlockCopy(nameBytes, 0, hashInput, namespaceBytes.Length, nameBytes.Length);

			// SHA1 hash
			using SHA1 sha1 = SHA1.Create();
			byte[] hash = sha1.ComputeHash(hashInput);

			// Convert hash to UUID format
			hash[6] = (byte)((hash[6] & 0x0F) | (5 << 4)); // Version 5
			hash[8] = (byte)((hash[8] & 0x3F) | 0x80);     // Variant RFC 4122

			// Copy first 16 bytes into GUID
			byte[] newGuid = new byte[16];
			Array.Copy(hash, 0, newGuid, 0, 16);
			SwapGuidByteOrder(newGuid); // Undo byte order fix for .NET

			return new Guid(newGuid);
		}

	}

}
