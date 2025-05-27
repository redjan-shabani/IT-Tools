using Infokom.Tools.Crypto;
using System;
using System.Text;
using System.Windows;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class UUIDGeneratorViewModel : DependencyObject
	{

		public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
			"Version",
			typeof(string),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(UUID.Version.V3.ToString(), OnPropertyChanged));


		private static readonly DependencyPropertyKey HasNamespacePropertyKey = DependencyProperty.RegisterReadOnly(
			"HasNamespace",
			typeof(bool),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(true));
		public static readonly DependencyProperty HasNamespaceProperty = HasNamespacePropertyKey.DependencyProperty; 

		public static readonly DependencyProperty NamespaceProperty = DependencyProperty.Register(
			"Namespace",
			typeof(string),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(UUID.Namespace.URL.ToString(), OnPropertyChanged));

		private static readonly DependencyPropertyKey NamespaceUUIDPropertyKey = DependencyProperty.RegisterReadOnly(
			"NamespaceUUID",
			typeof(Guid),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(UUID.Namespace.URL.GetUUID()));
		public static readonly DependencyProperty NamespaceUUIDProperty = NamespaceUUIDPropertyKey.DependencyProperty;

		public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register(
			"Quantity",
			typeof(int),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(5, OnPropertyChanged));

		public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
			"Name",
			typeof(string),
			typeof(UUIDGeneratorViewModel),
			new PropertyMetadata(string.Empty, OnPropertyChanged));



		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is UUIDGeneratorViewModel vm)
			{
				vm.HasNamespace = vm.Version == UUID.Version.V3.ToString() || vm.Version == UUID.Version.V5.ToString();

				vm.NamespaceUUID = Enum.TryParse<UUID.Namespace>(vm.Namespace, out var uuid) ? uuid.GetUUID() : Guid.Empty;

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
			this.GenerateUUIDs();
		}

		public string[] Versions { get; } = Enum.GetNames<UUID.Version>();

		public string Version
		{
			get => (string)this.GetValue(VersionProperty);
			set => this.SetValue(VersionProperty, value);
		}


		public bool HasNamespace 
		{
			get => (bool)this.GetValue(HasNamespaceProperty);
			private set => this.SetValue(HasNamespacePropertyKey, value);
		}

		public string[] Namespaces { get; } = Enum.GetNames<UUID.Namespace>();

		public string Namespace
		{
			get => (string)this.GetValue(NamespaceProperty);
			set => this.SetValue(NamespaceProperty, value);
		}

		public Guid NamespaceUUID
		{
			get => (Guid)this.GetValue(NamespaceUUIDProperty);
			private set => this.SetValue(NamespaceUUIDPropertyKey, value);
		}

		public string Name 
		{
			get => (string)this.GetValue(NameProperty);
			set => this.SetValue(NameProperty, value);
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
