using System.Windows;
using System.Windows.Input;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
	public class DateTimeConverterViewModel : DependencyObject
	{
		private static readonly DependencyProperty ISO8601Property = DependencyProperty.Register(
			"ISO8601",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty ISO9075Property = DependencyProperty.Register(
			"ISO9075",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty RFC3339Property = DependencyProperty.Register(
			"RFC3339",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty RFC7231Property = DependencyProperty.Register(
			"RFC7231",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty UTCProperty = DependencyProperty.Register(
			"UTC",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty TimestampProperty = DependencyProperty.Register(
			"Timestamp",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty UnixTimestampProperty = DependencyProperty.Register(
			"UnixTimestamp",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty ExcelDateTimeProperty = DependencyProperty.Register(
			"ExcelDateTime",
			typeof(string),
			typeof(DateTimeConverterViewModel),
			new PropertyMetadata(string.Empty));


		private bool _busy;

		public DateTimeConverterViewModel()
		{
			var timer = new System.Timers.Timer(TimeSpan.FromMilliseconds(100));
			timer.Elapsed += (sender, e) =>
			{
				_busy = true;
				this.Dispatcher.Invoke(() =>
				{
					try
					{
						UpdateDateTime(DateTime.Now);
					}
					finally
					{
						_busy = false;
					}
				});
			};

			this.PlayCommand = new RelayCommand(_ => !timer.Enabled, _ => timer.Start());
			this.PauseCommand = new RelayCommand(_ => timer.Enabled, _ => timer.Stop());

			timer.Start();


		}

		

		public string ISO8601
		{
			get => (string)this.GetValue(ISO8601Property);
			set => this.SetValue(ISO8601Property, value);
		}

		public string ISO9075
		{
			get => (string)this.GetValue(ISO9075Property);
			set => this.SetValue(ISO9075Property, value);
		}

		public string RFC3339
		{
			get => (string)this.GetValue(RFC3339Property);
			set => this.SetValue(RFC3339Property, value);
		}

		public string RFC7231
		{
			get => (string)this.GetValue(RFC7231Property);
			set => this.SetValue(RFC7231Property, value);
		}


		public string UTC
		{
			get => (string)this.GetValue(UTCProperty);
			set => this.SetValue(UTCProperty, value);
		}


		public string Timestamp
		{
			get => (string)this.GetValue(TimestampProperty);
			set => this.SetValue(TimestampProperty, value);
		}

		public string UnixTimestamp
		{
			get => (string)this.GetValue(UnixTimestampProperty);
			set => this.SetValue(UnixTimestampProperty, value);
		}

		public string ExcelDateTime
		{
			get => (string)this.GetValue(ExcelDateTimeProperty);
			set => this.SetValue(ExcelDateTimeProperty, value);
		}



		private void UpdateDateTime(DateTime dateTime)
		{
			this.ISO8601 = dateTime.ToString("o");
			this.ISO9075 = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
			this.RFC3339 = dateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
			this.RFC7231 = dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
			this.UTC = dateTime.ToUniversalTime().ToString("o");
			this.Timestamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds().ToString();
			this.UnixTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds().ToString();
			this.ExcelDateTime = (dateTime - new DateTime(1899, 12, 30)).TotalDays.ToString();
		}



		public ICommand PlayCommand { get; }

		public ICommand PauseCommand { get; }


	}
}
