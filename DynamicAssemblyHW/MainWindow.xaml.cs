using System;
using System.Collections.Generic;
using System.IO;
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
        private string path = @"C:\Users\ОралбаевГ\source\repos\ProjectForSP_HW3\Calculator\bin\Debug\netcoreapp3.0\Calculator.dll";

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
            var numberList = new List<long>();
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
            WeakReference weakReference;
            numberList = CountFactorials(out weakReference, numberList);
            numberList.ForEach(x => resultL.Content += x + ", ");
        }

        private List<long> CountFactorials(out WeakReference weakReference, List<long> numberList)
        {
            try
            {
                var context = new CustomAssemblyLoadContext();
                var assemblyPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);
                var assembly = context.LoadFromAssemblyPath(assemblyPath);

                weakReference = new WeakReference(context, true);

                var calculatorType = assembly.GetType("Calculator.Calc", true, true);
                var obj = Activator.CreateInstance(calculatorType);

                var greetMethod = calculatorType.GetMethod("GetFactorials");
                var result = greetMethod.Invoke(obj, new object[] { numberList });
                var currentDomain = AppDomain.CurrentDomain.GetAssemblies();
                context.Unload();
                currentDomain = AppDomain.CurrentDomain.GetAssemblies();

                return result as List<long>;
            }
            catch (Exception ex)
            {
                weakReference = null;
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
