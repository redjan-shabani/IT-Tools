using Infokom.Tools.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class EncryptorViewModel : DependencyObject
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register("Password", typeof(string), typeof(EncryptorViewModel), new PropertyMetadata("1234", OnPropertyChanged));

		public static readonly DependencyProperty SelectedAlgorithmProperty =
			DependencyProperty.Register("SelectedAlgorithm", typeof(Cipher.Algorithm), typeof(EncryptorViewModel), new PropertyMetadata(Cipher.Algorithm.AES, OnPropertyChanged));


		public static readonly DependencyProperty DecryptedTextProperty = DependencyProperty.Register(
			nameof(DecryptedText),
			typeof(string),
			typeof(EncryptorViewModel),
			new PropertyMetadata("Hello, World!", OnPropertyChanged));

		public static readonly DependencyProperty EncryptedTextProperty = DependencyProperty.Register(
			nameof(EncryptedText),
			typeof(string), 
			typeof(EncryptorViewModel),
			new PropertyMetadata(Cipher.Algorithm.AES.Encrypt("1234", "Hello, World!")));


		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is EncryptorViewModel vm)
			{
				var encryptedText = string.Empty;

				try
				{
					encryptedText = vm.SelectedAlgorithm.Encrypt(vm.Password, vm.DecryptedText);
				}
				catch { }

				vm.EncryptedText = encryptedText;
			}
		}


		public IEnumerable<Cipher.Algorithm> AvailableAlgorithms => [ Cipher.Algorithm.AES, Cipher.Algorithm.TripleDES ];

		public Cipher.Algorithm SelectedAlgorithm { get => (Cipher.Algorithm)this.GetValue(SelectedAlgorithmProperty); set => this.SetValue(SelectedAlgorithmProperty, value); }

		public string Password { get => (string)this.GetValue(PasswordProperty); set => this.SetValue(PasswordProperty, value); }



		public string DecryptedText
		{
			get => (string)this.GetValue(DecryptedTextProperty);
			set => this.SetValue(DecryptedTextProperty, value);
		}

		public string EncryptedText
		{
			get => (string)this.GetValue(EncryptedTextProperty);
			set => this.SetValue(EncryptedTextProperty, value);
		}
	}

	public class DecryptorViewModel : DependencyObject
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register("Password", typeof(string), typeof(DecryptorViewModel), new PropertyMetadata("1234", OnPropertyChanged));

		public static readonly DependencyProperty SelectedAlgorithmProperty =
			DependencyProperty.Register("SelectedAlgorithm", typeof(Cipher.Algorithm), typeof(DecryptorViewModel), new PropertyMetadata(Cipher.Algorithm.AES, OnPropertyChanged));


		public static readonly DependencyProperty DecryptedTextProperty = DependencyProperty.Register(
			nameof(DecryptedText),
			typeof(string),
			typeof(DecryptorViewModel),
			new PropertyMetadata("Hello, World!"));

		public static readonly DependencyProperty EncryptedTextProperty = DependencyProperty.Register(
			nameof(EncryptedText),
			typeof(string),
			typeof(DecryptorViewModel),
			new PropertyMetadata(Cipher.Algorithm.AES.Encrypt("1234", "Hello, World!"), OnPropertyChanged));

		private static void OnPropertyChanged(DependencyObject d,  DependencyPropertyChangedEventArgs e)
		{
			if(d is DecryptorViewModel vm)
			{
				var decryptedText = string.Empty;

				try
				{
					decryptedText = vm.SelectedAlgorithm.Decrypt(vm.Password, vm.EncryptedText);
				}
				catch { }

				vm.DecryptedText = decryptedText;
			}
		}





		public IEnumerable<Cipher.Algorithm> AvailableAlgorithms => [Cipher.Algorithm.AES, Cipher.Algorithm.TripleDES];

		public Cipher.Algorithm SelectedAlgorithm { get => (Cipher.Algorithm)this.GetValue(SelectedAlgorithmProperty); set => this.SetValue(SelectedAlgorithmProperty, value); }

		public string Password { get => (string)this.GetValue(PasswordProperty); set => this.SetValue(PasswordProperty, value); }



		public string DecryptedText
		{
			get => (string)this.GetValue(DecryptedTextProperty);
			set => this.SetValue(DecryptedTextProperty, value);
		}

		public string EncryptedText
		{
			get => (string)this.GetValue(EncryptedTextProperty);
			set => this.SetValue(DecryptedTextProperty, value);
		}
	}
}
