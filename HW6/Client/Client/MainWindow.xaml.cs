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
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModel();
        
        //    dataBox.ItemsSource = viewModel.AllData;
            Downloading.ItemsSource = viewModel.Downloads;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
            => await viewModel.EstablishConnection();
        

        private void Download_Click(object sender, RoutedEventArgs e)
            => _ = viewModel.DownloadFile();
        

        private async void Show_Click(object sender, RoutedEventArgs e)
            => await this.viewModel.UpdateList();
        

        private void Back_Click(object sender, RoutedEventArgs e)
            => this.viewModel.Back();
        

        private void GetIntoFolder(object sender, MouseButtonEventArgs e)
        {
            var selected = dataBox.SelectedItem.ToString();
            this.viewModel.GetIntoFolder(selected);
        }

        private async void Download_All_Click(object sender, RoutedEventArgs e)
            => await this.viewModel.DownloadAll();
        
    }
}