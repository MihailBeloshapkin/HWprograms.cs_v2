﻿using System;
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
        ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModel();
            dataBox.ItemsSource = viewModel.DirectoriesAndFiles;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.serverPath = this.DownloadPath.Text;
            viewModel.Connection(this.textBoxIP.Text, this.textBoxPort.Text);
        }
    }
}