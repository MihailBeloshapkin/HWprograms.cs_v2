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
        private int operation = -1;
        private int countOfAlreadyInput = 0;

        /// <summary>
        /// Get final result.
        /// </summary>
        private void GetResult()
        {
            if (this.countOfAlreadyInput > 2)
            {
                switch (operation)
                {
                    case 0:
                        MessageBox.Show($"{number1 + number2}");
                        break;
                    case 1:
                        MessageBox.Show($"{number1 - number2}");
                        break;
                    case 2:
                        MessageBox.Show($"{number1 * number2}");
                        break;
                    case 3:
                        MessageBox.Show($"{number1 / number2}");
                        break;
                }
            }
        }

        /// <summary>
        /// Get selected operation.
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.operation = comboBox1.SelectedIndex;
            this.countOfAlreadyInput++;
            this.GetResult();
        }

        /// <summary>
        /// Get the result using data from testBox1
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(textBox1.Text, out this.number1))
            {
                this.countOfAlreadyInput++;
                this.GetResult();
            }
        }

        /// <summary>
        /// Get the result using data from testBox2
        /// </summary>
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(textBox2.Text, out this.number2))
            {
                this.countOfAlreadyInput++;
                this.GetResult();
            }
        }
    }
}
