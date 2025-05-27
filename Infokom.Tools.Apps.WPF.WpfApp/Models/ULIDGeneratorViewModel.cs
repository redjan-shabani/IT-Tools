using Infokom.Tools.Crypto;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class ULIDGeneratorViewModel : DependencyObject
	{

		public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register(
			"Quantity",
			typeof(int),
			typeof(ULIDGeneratorViewModel),
			new PropertyMetadata(10, (d,e) => 
			{
				if(d is ULIDGeneratorViewModel vm)
				{
					vm.Refresh();
				}
			}));

		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
			"Format",
			typeof(string),
			typeof(ULIDGeneratorViewModel),
			new PropertyMetadata("Raw"));


		public static readonly DependencyProperty ULIDsProperty = DependencyProperty.Register(
			"ULIDs",
			typeof(string),
			typeof(ULIDGeneratorViewModel),
			new PropertyMetadata(string.Empty));


		public ULIDGeneratorViewModel()
		{
			this.RefreshCommand = new RelayCommand(_ => true, _ => this.Refresh());
			
			this.CopyCommand = new RelayCommand(_ => !string.IsNullOrEmpty(this.ULIDs), _ => Clipboard.SetText(this.ULIDs));

			this.Refresh();
		}


		public int Quantity
		{
			get => (int)GetValue(QuantityProperty);
			set => SetValue(QuantityProperty, value);
		}

		public string[] Formats { get; } = ["Raw", "JSON"];

		public string Format
		{
			get => (string)GetValue(FormatProperty);		
			set => SetValue(FormatProperty, value);
		}

		public string ULIDs
		{
			get => (string)GetValue(ULIDsProperty);
			set => SetValue(ULIDsProperty, value);
		}


		public ICommand RefreshCommand { get; set; }

		public ICommand CopyCommand { get; set; }


		private void Refresh()
		{
			var sb = new StringBuilder();


			for (int i = 0; i < this.Quantity; i++)
			{
				var ulid = ULID.GenerateULID();
				sb.AppendLine(ulid);
			}

			this.ULIDs = sb.ToString().TrimEnd();
		}
	}
}
