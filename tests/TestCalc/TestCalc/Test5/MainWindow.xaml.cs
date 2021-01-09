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

namespace Test5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int number1;
        private int number2;
        private int operation;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = comboBox1.SelectedIndex;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.number1 = Int32.Parse(textBox1.Text);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.number2 = Int32.Parse(textBox2.Text);
        }
    }
}
