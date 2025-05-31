using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infokom.Tools.Crypto
{
	public interface IToken<TSelf, TOptions> where TSelf : IToken<TSelf, TOptions>
	{

		public static abstract TSelf NewToken(TOptions option);
	}



	public record RandomTokenOptions : IOptions<RandomTokenOptions>
	{
		public bool IncludeUppercase { get; set; }
		public bool IncludeLowercase { get; set; }
		public bool IncludeNumbers { get; set; }
		public bool IncludeSymbols { get; set; }
		public int Length { get; set; }



		public string GetAlphabet()
		{
			var alphabet = string.Empty;
			if (IncludeUppercase)
				alphabet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			if (IncludeLowercase)
				alphabet += "abcdefghijklmnopqrstuvwxyz";
			if (IncludeNumbers)
				alphabet += "0123456789";
			if (IncludeSymbols)
				alphabet += ".,;:!?./-\"'#{([-|\\@)]=}*+";
			return alphabet;
		}

		public string Generate()
		{
			var alphabet = this.GetAlphabet();
			var length = this.Length;

			var chars = new char[length];

			for (int i = 0; i < length; i++)
			{
				var j = Random.Shared.Next(0, alphabet.Length - 1);
				chars[i] = alphabet[j];
			}

			return new string(chars);
		}


		RandomTokenOptions IOptions<RandomTokenOptions>.Value => this;

		public static RandomTokenOptions Default => new ()
		{

			IncludeUppercase = true,
			IncludeLowercase = true,
			IncludeNumbers = true,
			IncludeSymbols = false,
			Length = 64
		};
	}


	public struct Token
	{
		private readonly BigInteger _value;

		public Token()
		{
			_value = new();
		}

		private Token(BigInteger value)
		{
			_value = value;
		}






		public static Token Parse(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));
			
			var bytes = Encoding.ASCII.GetBytes(text);

			var value = new BigInteger(bytes);

			return new Token(value);
		}
	}
}
