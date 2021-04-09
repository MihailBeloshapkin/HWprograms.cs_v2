using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModel();
            this.DataContext = viewModel;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
            => viewModel.EstablishConnection();

        private void Back_Click(object sender, RoutedEventArgs e)
            => this.viewModel.Back();
        
        private void GetIntoFolder(object sender, MouseButtonEventArgs e)
            => this.viewModel.GetIntoFolder();
        
        private void Download_All_Click(object sender, RoutedEventArgs e)
            => this.viewModel.DownloadAll();

        private void InsertedPort(object sender, TextChangedEventArgs e)
        {
            if (this.viewModel.Ip != null)
            {
                this.viewModel.IsEnabledConnectButton = "True";
            }
        }

        private void InsertedIp(object sender, TextChangedEventArgs e)
        {
            if (this.viewModel.Port != null)
            {
                this.viewModel.IsEnabledConnectButton = "True";
            }
        }
    }
}
