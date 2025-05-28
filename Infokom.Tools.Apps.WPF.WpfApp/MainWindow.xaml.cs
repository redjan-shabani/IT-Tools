using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Infokom.Tools.Apps.WPF.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void BtnPopup_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void ButtonShowTokenGenerator_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemTokenGenerator;
		}

		private void ButtonShowHashText_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemHashText;
		}

		private void ButtonShowTextCipher_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemTextCypher;
		}

		private void ButtonShowUUIDGenerator_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemUUIDGenerator;
		}

		private void ButtonShowULIDGenerator_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemULIDGenerator;
		}

		private void ButtonShowRSAKeyPairGenerator_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemRSAKeyPairGenerator;
		}



		private void ButtonShowDateTimeConverter_Click(object sender, RoutedEventArgs e)
		{
			TabControlMain.SelectedItem = TabItemRSAKeyPairGenerator;
		}
	}
}