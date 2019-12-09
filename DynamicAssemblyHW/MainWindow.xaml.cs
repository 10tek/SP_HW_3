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
        private string path = @"D:\repos\ProjectForSP_HW3\Calculator\bin\Debug\netcoreapp3.0\Calculator.dll";

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
            }
            if (numberList.Count != 10)
            {
                MessageBox.Show("Вы ввели не корректные данные или недостаточное кол-во чисел");
                return;
            }
            
            //Для проверки.
            var currentDomain = AppDomain.CurrentDomain.GetAssemblies();
            WeakReference weakReference;
            numberList = CountFactorials(out weakReference, numberList);
            for (int i = 0; weakReference.IsAlive && (i < 10); i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            //Для проверки. 
            currentDomain = AppDomain.CurrentDomain.GetAssemblies();

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
                context.Unload();

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
