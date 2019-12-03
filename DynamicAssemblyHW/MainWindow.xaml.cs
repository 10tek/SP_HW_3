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

namespace DynamicAssemblyHW
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

        private void CalcBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(numbersTB.Text))
            {
                MessageBox.Show("Поле пустое!");
                return;
            }
            numbersTB.Text.Remove(' ');
            var numberStrings = numbersTB.Text.Split(',');
            var numberList = new List<int>();
            foreach (var number in numberStrings)
            {
                if (int.TryParse(number, out var num))
                {
                    numberList.Add(num);
                }
                else
                {
                    MessageBox.Show("Некорректные данные!");
                    return;
                }
            }
            if (numberList.Count != 10)
            {
                MessageBox.Show("Вы ввели не 10 чисел");
                return;
            }


        }
    }
}
