using Infokom.Tools.Crypto;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Infokom.Tools.Apps.WPF.WpfApp
{

	public class TokenGeneratorViewModel : DependencyObject
	{
		public static readonly DependencyProperty WithUppercaseProperty =
			DependencyProperty.Register("WithUppercase", typeof(bool), typeof(TokenGeneratorViewModel), new PropertyMetadata(true, PropertyChangedCallback));

		public static readonly DependencyProperty WithLowercaseProperty =
			DependencyProperty.Register("WithLowercase", typeof(bool), typeof(TokenGeneratorViewModel), new PropertyMetadata(true, PropertyChangedCallback));
		public static readonly DependencyProperty WithNumbersProperty =
			DependencyProperty.Register("WithNumbers", typeof(bool), typeof(TokenGeneratorViewModel), new PropertyMetadata(true, PropertyChangedCallback));
		public static readonly DependencyProperty WithSymbolsProperty =
			DependencyProperty.Register("WithSymbols", typeof(bool), typeof(TokenGeneratorViewModel), new PropertyMetadata(false, PropertyChangedCallback));
		public static readonly DependencyProperty LengthProperty =
			DependencyProperty.Register("Length", typeof(int), typeof(TokenGeneratorViewModel), new PropertyMetadata(64, PropertyChangedCallback));
		public static readonly DependencyProperty TokenProperty =
			DependencyProperty.Register("Token", typeof(string), typeof(TokenGeneratorViewModel), new PropertyMetadata(string.Empty));

		private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TokenGeneratorViewModel vm)
			{
				vm.RefreshCommand.Execute(null);
			}
		}

		public TokenGeneratorViewModel()
		{
			WithUppercase = true;
			WithLowercase = true;
			WithNumbers = true;
			WithSymbols = false;
			Length = 64;
			Token = Tokens.GenerateToken(WithUppercase, WithLowercase, WithNumbers, WithSymbols, Length);


			this.CopyCommand = new RelayCommand(_ => !string.IsNullOrEmpty(Token), _ => Clipboard.SetText(Token));


			this.RefreshCommand = new RelayCommand(_ => WithUppercase || WithLowercase || WithSymbols || WithNumbers, _ =>
			{
				Token = Tokens.GenerateToken(WithUppercase, WithLowercase, WithNumbers, WithSymbols, Length);
			});

		}


		public bool WithUppercase
		{
			get => (bool)GetValue(WithUppercaseProperty);
			set => SetValue(WithUppercaseProperty, value);
		}

		public bool WithLowercase
		{
			get => (bool)GetValue(WithLowercaseProperty);
			set => SetValue(WithLowercaseProperty, value);
		}

		public bool WithNumbers
		{
			get => (bool)GetValue(WithNumbersProperty);
			set => SetValue(WithNumbersProperty, value);
		}

		public bool WithSymbols
		{
			get => (bool)GetValue(WithSymbolsProperty);
			set => SetValue(WithSymbolsProperty, value);
		}

		public int Length
		{
			get => (int)GetValue(LengthProperty);
			set => SetValue(LengthProperty, value);
		}

		public string Token
		{
			get => (string)GetValue(TokenProperty);
			set => SetValue(TokenProperty, value);
		}

		public ICommand RefreshCommand { get; set; }

		public ICommand CopyCommand { get; set; }



		private class CopyCommandImpl : ICommand
		{
			private readonly TokenGeneratorViewModel _owner;

			public CopyCommandImpl(TokenGeneratorViewModel owner)
			{
				_owner = owner;
			}

			public event EventHandler CanExecuteChanged;

			public bool CanExecute(object parameter)
			{
				return !string.IsNullOrEmpty(_owner.Token);
			}

			public void Execute(object parameter)
			{
				Clipboard.SetText(_owner.Token);
			}
		}

		private class RefreshCommandImpl : ICommand
		{
			private readonly TokenGeneratorViewModel _owner;

			public RefreshCommandImpl(TokenGeneratorViewModel owner)
			{
				_owner = owner;
			}

			public event EventHandler CanExecuteChanged
			{
				add => CommandManager.RequerySuggested += value;
				remove => CommandManager.RequerySuggested -= value;
			}

			public bool CanExecute(object parameter)
			{
				return _owner.WithUppercase || _owner.WithLowercase || _owner.WithSymbols || _owner.WithNumbers;
			}

			public void Execute(object parameter)
			{
				_owner.Token = Tokens.GenerateToken(_owner.WithUppercase, _owner.WithLowercase, _owner.WithNumbers, _owner.WithSymbols, _owner.Length);
			}
		}
	}
}
