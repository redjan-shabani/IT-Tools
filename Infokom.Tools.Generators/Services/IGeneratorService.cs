using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infokom.Tools.Generators.Services
{
	public interface IGeneratorService
	{
	}



	public record GenerateRandomTokenRequest
	{
		public bool IncludeUppercase { get; set; }
		public bool IncludeLowercase { get; set; }
		public bool IncludeNumbers { get; set; }
		public bool IncludeSymbols { get; set; }
		public int Length { get; set; } = 64;
	}

	public class GenerateRandomTokenResponse
	{
		public string Token { get; set; }
		public int Length { get; set; }
	}
}
