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

	public class RandomTokenProvider : INotifyPropertyChanged, IObservable<string>
	{
		private const string UPPERCASE = "ABCDEFGHIJKLMOPQRSTUVWXYZ";
		private const string LOWERCASE = "abcdefghijklmopqrstuvwxyz";
		private const string NUMBERS = "0123456789";
		private const string SYMBOLS = ".,;:!?./-\"'#{([-|\\@)]=}*+";



		private bool _withUppercase;
		private bool _withLowercase;
		private bool _withNumbers;
		private bool _withSymbols;
		private int _length;

		private string _currentToken;

		public RandomTokenProvider(bool withUppercase = true, bool withLowercase = true, bool withNumbers = true, bool withSymbols = false, int length = 64)
		{

			WithUppercase = withUppercase;
			WithLowercase = withLowercase;
			WithNumbers = withNumbers;
			WithSymbols = withSymbols;
			Length = length;
		}


		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			if(propertyName == nameof(WithUppercase) ||
				propertyName == nameof(WithLowercase) ||
				propertyName == nameof(WithNumbers) ||
				propertyName == nameof(WithSymbols) ||
				propertyName == nameof(Length))
			{
				_currentToken = GenerateToken();
				OnPropertyChanged(nameof(Token));
			}
		}
		public bool WithUppercase
		{
			get => _withUppercase;
			set
			{
				if (_withUppercase != value)
				{
					_withUppercase = value;
					OnPropertyChanged(nameof(WithUppercase));
				}
			}
		}
		public bool WithLowercase
		{
			get => _withLowercase;
			set
			{
				if (_withLowercase != value)
				{
					_withLowercase = value;
					OnPropertyChanged(nameof(WithLowercase));
				}
			}
		}
		public bool WithNumbers
		{
			get => _withNumbers;
			set
			{
				if (_withNumbers != value)
				{
					_withNumbers = value;
					OnPropertyChanged(nameof(WithNumbers));
				}
			}
		}
		public bool WithSymbols
		{
			get => _withSymbols;
			set
			{
				if (_withSymbols != value)
				{
					_withSymbols = value;
					OnPropertyChanged(nameof(WithSymbols));
				}
			}
		}

		public int Length
		{
			get => _length;
			set
			{
				if (_length != value)
				{
					_length = value;
					OnPropertyChanged(nameof(Length));
				}
			}
		}



		public bool IsValid => Length > 0 && (WithUppercase || WithLowercase || WithNumbers || WithSymbols);


		public string GetAlphabet()
		{
			if (!IsValid)
				throw new InvalidOperationException("Token options are not valid. Ensure at least one character type is selected and length is greater than zero.");

			var alphabet = string.Empty;
			if (WithUppercase)
				alphabet += UPPERCASE;
			if (WithLowercase)
				alphabet += LOWERCASE;
			if (WithNumbers)
				alphabet += NUMBERS;
			if (WithSymbols)
				alphabet += SYMBOLS;
			return alphabet;
		}








		private string GenerateToken()
		{
			if (!IsValid)
				return string.Empty;
			var alphabet = GetAlphabet();
			var length = Length;
			var characters = new char[length];
			for (int i = 0; i < length; i++)
			{
				var j = Random.Shared.Next(0, alphabet.Length - 1);
				characters[i] = alphabet[j];
			}
			return new string(characters);
		}

		public void Refresh()
		{
			if (!IsValid)
				this.Token = string.Empty;

			var alphabet = GetAlphabet();
			var length = Length;
			var characters = new char[length];
			for (int i = 0; i < length; i++)
			{
				var j = Random.Shared.Next(0, alphabet.Length - 1);
				characters[i] = alphabet[j];
			}

			this.Token = new string(characters);
		}

		public string Token
		{
			get => _currentToken;
			private set
			{
				if (_currentToken != value)
				{
					_currentToken = value;

					this.OnPropertyChanged(nameof(Token));
				}
			}
		}

		public IDisposable Subscribe(IObserver<string> observer)
		{
			this.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == nameof(Token))
				{
					observer.OnNext(Token);
				}
			};

			return Disposable.Create(() =>
			{
				this.PropertyChanged -= (sender, e) =>
				{
					if (e.PropertyName == nameof(Token))
					{
						observer.OnNext(Token);
					}
				};
			});
		}

	}
}
