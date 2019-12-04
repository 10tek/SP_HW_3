using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private string path = @"C:\Users\ОралбаевГ\source\repos\Calculator\Calculator\bin\Debug\netcoreapp3.0\Calculator.dll";

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
            var numbers = numbersTB.Text.Replace(" ", "");
            var numberStrings = numbers.Split(',');
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

            try
            {
                var asm = Assembly.LoadFrom(path);

                Type t = asm.GetType("Calculator.Calc", true, true);

                // создаем экземпляр класса Program
                var obj = Activator.CreateInstance(t);

                // получаем метод GetResult
                var method = t.GetMethod("GetFactorials");

                // вызываем метод, передаем ему значения для параметров и получаем результат
                var result = method.Invoke(obj, new object[] { numberList });
                numberList = result as List<int>;
                numberList.ForEach(x => resultL.Content += x + ", ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
