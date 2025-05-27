using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace Infokom.Tools.Crypto
{

	public static class Tokens
	{
		private const string UPPERCASE = "ABCDEFGHIJKLMOPQRSTUVWXYZ";
		private const string LOWERCASE = "abcdefghijklmopqrstuvwxyz";
		private const string NUMBERS = "0123456789";
		private const string SYMBOLS = ".,;:!?./-\"'#{([-|\\@)]=}*+";



		public static string GenerateToken(bool withUppercase = true, bool withLowercase = true, bool withNumbers = true, bool withSymbols = false, int length = 64)
		{
			if (length <= 0 || !(withUppercase || withLowercase || withNumbers || withSymbols))
				return string.Empty;

			var alphabet = string.Empty;
			if (withUppercase)
				alphabet += UPPERCASE;
			if (withLowercase)
				alphabet += LOWERCASE;
			if (withNumbers)
				alphabet += NUMBERS;
			if (withSymbols)
				alphabet += SYMBOLS;
			var characters = new char[length];
			for (int i = 0; i < length; i++)
			{
				var j = Random.Shared.Next(0, alphabet.Length - 1);
				characters[i] = alphabet[j];
			}

			return new string(characters);
		}
	}

}
