using Infokom.Tools.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class RSAKeyPairGeneratorViewModel : DependencyObject
	{
		public static readonly DependencyProperty KeySizeProperty = DependencyProperty.Register(
			"KeySize", 
			typeof(int),
			typeof(RSAKeyPairGeneratorViewModel),
			new PropertyMetadata(2048, OnKeySizeChanged));

		private static void OnKeySizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is RSAKeyPairGeneratorViewModel vm)
			{
				vm.GenerateKeys();
			}
		}

		public static readonly DependencyProperty PublicKeyProperty = DependencyProperty.Register(
			"PublicKey",
			typeof(string),
			typeof(RSAKeyPairGeneratorViewModel),
			new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty PrivateKeyProperty = DependencyProperty.Register(
			"PrivateKey",
			typeof(string), 
			typeof(RSAKeyPairGeneratorViewModel),
			new PropertyMetadata(string.Empty));

		public RSAKeyPairGeneratorViewModel()
		{
			this.RefreshCommand = new RelayCommand(
				_ => true,
				_ => this.GenerateKeys()
			);

			this.GenerateKeys();
		}

		public int KeySize
		{
			get => (int)this.GetValue(KeySizeProperty);
			set => this.SetValue(KeySizeProperty, value);
		}

		public string PublicKey
		{
			get => (string)this.GetValue(PublicKeyProperty);
			set => this.SetValue(PublicKeyProperty, value);
		}

		public string PrivateKey
		{
			get => (string)this.GetValue(PrivateKeyProperty);
			set => this.SetValue(PrivateKeyProperty, value);
		}


		public ICommand RefreshCommand { get; }



		private void GenerateKeys()
		{
			(this.PublicKey, this.PrivateKey) = RSA.GenerateKeys(this.KeySize);
		}
	}
}
