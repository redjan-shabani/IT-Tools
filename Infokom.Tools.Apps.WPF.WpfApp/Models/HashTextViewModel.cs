using Infokom.Tools.Crypto;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class HashTextViewModel : DependencyObject
	{
		private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is HashTextViewModel vm)
			{
				vm.ComputeHashes();
			}
		}




		public static readonly DependencyProperty TextToHashProperty =
			DependencyProperty.Register("TextToHash", typeof(string), typeof(HashTextViewModel), new PropertyMetadata("Hello, World!", PropertyChangedCallback));	


		public static readonly DependencyProperty DigestEncodingProperty =
			DependencyProperty.Register("DigestEncoding", typeof(string), typeof(HashTextViewModel), new PropertyMetadata(Hashes.DigestEncoding.Hexadecimal.ToString(), PropertyChangedCallback));


		private static readonly DependencyPropertyKey MD5PropertyKey = DependencyProperty.RegisterReadOnly(
			"MD5",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty MD5Property = MD5PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA1PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA1",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA1Property = SHA1PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA256PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA256",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA256Property = SHA256PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA224PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA224",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA224Property = SHA224PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA512PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA512",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA512Property = SHA512PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA384PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA384",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA384Property = SHA384PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SHA3PropertyKey = DependencyProperty.RegisterReadOnly(
			"SHA3",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty SHA3Property = SHA3PropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey RIPEMD160PropertyKey = DependencyProperty.RegisterReadOnly(
			"RIPEMD160",
			typeof(string),
			typeof(HashTextViewModel),
			new PropertyMetadata(string.Empty));
		public static readonly DependencyProperty RIPEMD160Property = RIPEMD160PropertyKey.DependencyProperty;



		public HashTextViewModel()
		{
			this.TextToHash = "Hello, World!";
			this.DigestEncoding = Hashes.DigestEncoding.Hexadecimal.ToString();

			this.CopyMD5 = new RelayCommand(_ => !string.IsNullOrEmpty(this.MD5), _ => Clipboard.SetText(this.MD5));
			this.CopySHA1 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA1), _ => Clipboard.SetText(this.SHA1));
			this.CopySHA256 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA256), _ => Clipboard.SetText(this.SHA256));
			this.CopySHA224 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA224), _ => Clipboard.SetText(this.SHA224));
			this.CopySHA512 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA512), _ => Clipboard.SetText(this.SHA512));
			this.CopySHA384 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA384), _ => Clipboard.SetText(this.SHA384));
			this.CopySHA3 = new RelayCommand(_ => !string.IsNullOrEmpty(this.SHA3), _ => Clipboard.SetText(this.SHA3));
			this.CopyRIPEMD160 = new RelayCommand(_ => !string.IsNullOrEmpty(this.RIPEMD160), _ => Clipboard.SetText(this.RIPEMD160));

			this.ComputeHashes();
		}



		public string TextToHash
		{
			get => (string)GetValue(TextToHashProperty);
			set => SetValue(TextToHashProperty, value);
		}


		public IEnumerable<string> DigestEncodings => Enum.GetNames(typeof(Hashes.DigestEncoding));

		public string DigestEncoding
		{
			get => (string)GetValue(DigestEncodingProperty);
			set => SetValue(DigestEncodingProperty, value);
		}

		public string MD5
		{
			get => (string)GetValue(MD5Property);
			private set => SetValue(MD5PropertyKey, value);
		}

		public ICommand CopyMD5 { get; }

		public string SHA1
		{
			get => (string)GetValue(SHA1Property);
			private set => SetValue(SHA1PropertyKey, value);
		}
		public ICommand CopySHA1 { get; }

		public string SHA256
		{
			get => (string)GetValue(SHA256Property);
			private set => SetValue(SHA256PropertyKey, value);
		}
		public ICommand CopySHA256 { get; }

		public string SHA224
		{
			get => (string)GetValue(SHA224Property);
			private set => SetValue(SHA224PropertyKey, value);
		}
		public ICommand CopySHA224 { get; }

		public string SHA512
		{
			get => (string)GetValue(SHA512Property);
			private set => SetValue(SHA512PropertyKey, value);
		}
		public ICommand CopySHA512 { get; }

		public string SHA384
		{
			get => (string)GetValue(SHA384Property);
			private set => SetValue(SHA384PropertyKey, value);
		}
		public ICommand CopySHA384 { get; }

		public string SHA3
		{
			get => (string)GetValue(SHA3Property);
			private set => SetValue(SHA3PropertyKey, value);
		}
		public ICommand CopySHA3 { get; }

		public string RIPEMD160
		{
			get => (string)GetValue(RIPEMD160Property);
			private set => SetValue(RIPEMD160PropertyKey, value);
		}
		public ICommand CopyRIPEMD160 { get; }


		public void ComputeHashes()
		{
			var data = System.Text.Encoding.UTF8.GetBytes(this.TextToHash ?? string.Empty);			

			if(Enum.TryParse<Hashes.DigestEncoding>(this.DigestEncoding, out var encoding))
			{
				var md5 = Hashes.DigestAlgorithm.MD5.ComputeHash(data);
				var sha1 = Hashes.DigestAlgorithm.SHA1.ComputeHash(data);
				var sha256 = Hashes.DigestAlgorithm.SHA256.ComputeHash(data);
				var sha224 = Hashes.DigestAlgorithm.SHA224.ComputeHash(data);
				var sha512 = Hashes.DigestAlgorithm.SHA512.ComputeHash(data);
				var sha384 = Hashes.DigestAlgorithm.SHA384.ComputeHash(data);
				var sha3 = Hashes.DigestAlgorithm.SHA3.ComputeHash(data);
				var ripemd160 = Hashes.DigestAlgorithm.RIPEMD160.ComputeHash(data);

				MD5 = encoding.Encode(md5);
				SHA1 = encoding.Encode(sha1);
				SHA256 = encoding.Encode(sha256);
				SHA224 = encoding.Encode(sha224);
				SHA512 = encoding.Encode(sha512);
				SHA384 = encoding.Encode(sha384);
				SHA3 = encoding.Encode(sha3);
				RIPEMD160 = encoding.Encode(ripemd160);
			}
			else
			{
				MD5 = string.Empty;
				SHA1 = string.Empty;
				SHA256 = string.Empty;
				SHA224 = string.Empty;
				SHA512 = string.Empty;
				SHA384 = string.Empty;
				SHA3 = string.Empty;
				RIPEMD160 = string.Empty;
			}











		}

	}
}
