using System.Text;
using System.Windows;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class UUIDGeneratorViewModel : DependencyObject
	{

		public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register(
			"Quantity",
			typeof(int),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(5, OnQuantityChanged));

		private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is UUIDGeneratorViewModel vm)
			{
				vm.GenerateUUIDs();
			}
		}

		

		public static readonly DependencyProperty UUIDsProperty = DependencyProperty.Register(
			"UUIDs",
			typeof(string),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(string.Empty));


		public UUIDGeneratorViewModel()
		{
			this.Quantity = 5;
			this.GenerateUUIDs();
		}

		public int Quantity
		{
			get => (int)GetValue(QuantityProperty);
			set => SetValue(QuantityProperty, value);
		}

		public string UUIDs
		{
			get => (string)GetValue(UUIDsProperty);
			set => SetValue(UUIDsProperty, value);
		}


		private void GenerateUUIDs()
		{
			var sb = new StringBuilder();
			for (int i = 0; i < this.Quantity; i++)
			{
				var uuid = System.Guid.NewGuid().ToString();
				sb.AppendLine(uuid);
			}
			this.UUIDs = sb.ToString().TrimEnd();
		}
	}
}
