using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

namespace lab0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _count = 0; // Начальное значение счётчика
        private bool _wasPositive = true; // Флаг для отслеживания знака
        //string Colors = []
        public MainWindow()
        {
            InitializeComponent();
            LabelSupport1.Background = Brushes.BlueViolet;
            LabelSupport1.FontSize = 16;
            LabelSupport1.FontFamily = new FontFamily("Times New Roman");
            LabelSupport2.FontStyle = FontStyles.Italic;
            LabelSupport2.FontSize = 20;
            LabelSupport2.FontFamily = new FontFamily("Arial");

            Counter.FontWeight = FontWeights.Bold;
            Counter.FontSize = 18;

            Don_win.Width = 800;
            Don_win.Height = 600;
            // минимальные границы и максимальные
            Don_win.MinWidth = 600;
            Don_win.MinHeight = 400;
            Don_win.MaxWidth = 1000;
            Don_win.MaxHeight = 800;
            // установка цвета
            Don_win.Background = Brushes.Orchid;
        }
        private void LeftMouse(object sender, MouseButtonEventArgs e)
        {
            _count += 1;
            CheckConditions(); // Проверка условий
            UpdateCounterLabel(); // Обновление отображения
        }

        private void RightMouse(object sender, MouseButtonEventArgs e)
        {
            _count -= 2;
            CheckConditions(); // Проверка условий
            UpdateCounterLabel(); // Обновление отображения
        }
        private void UpdateCounterLabel()
        {
            Counter.Content = $"Счёт: {_count}";
        }
        private void CheckConditions()
        {
            bool isPositive = _count >= 0;

            // Проверка на смену знака
            if (isPositive != _wasPositive)
            {
                MessageBox.Show("Значение поменяло знак!", "Внимание", MessageBoxButton.OK);
                _wasPositive = isPositive;
            }

            // Проверка на совпадение с номером варианта (№2)
            if (_count == 2)
            {
                MessageBox.Show("Значение совпало с номером варианта!", "Внимание", MessageBoxButton.OK);
            }
        }
    }
}
