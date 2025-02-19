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
using System.Xml.Linq;

namespace lab0
{
    public partial class MainWindow : Window
    {
        private readonly Thickness _initialMargin; // Начальное положение элемента
        public MainWindow()
        {
            InitializeComponent();

            Top.Background = Brushes.GreenYellow;
            Top.FontSize = 10;
            Left.Background = Brushes.GreenYellow;
            Left.FontSize = 10;
            Right.Background = Brushes.GreenYellow;
            Right.FontSize = 10;
            Bottom.Background = Brushes.GreenYellow;
            Bottom.FontSize = 10;

            Cursor.FontSize = 60;

            Don_win.Width = 800;
            Don_win.Height = 600;
            // минимальные границы и максимальные
            Don_win.MinWidth = 800;
            Don_win.MinHeight = 600;
            Don_win.MaxWidth = 800;
            Don_win.MaxHeight = 600;
            // установка цвета
            Don_win.Background = Brushes.Gray;

            _initialMargin = Cursor.Margin; // Сохраняем начальное положение
            UpdateButtonStates(); // Обновляем состояние кнопок при запуске

            Don_win.SizeChanged += (s, e) => UpdateButtonStates();
        }

        private void MoveCursor(int deltaX, int deltaY)
        {
            var margin = Cursor.Margin;
            margin.Left += deltaX;
            margin.Top += deltaY;
            Cursor.Margin = margin;
            UpdateButtonStates(); // Обновляем состояние кнопок после перемещения
        }

        private void UpdateButtonStates()
        {
            var element = Cursor;
            var container = Gun_grid;

            // Рассчитываем границы элемента
            double left = element.Margin.Left - element.ActualWidth/2;
            double top = element.Margin.Top - element.ActualHeight/2;
            double right = container.ActualWidth - Cursor.FontSize/2 - left - element.ActualWidth;
            double bottom = container.ActualHeight - Cursor.FontSize/2 - top - element.ActualHeight;

            // Блокируем кнопки при достижении границ
            Left.IsEnabled = left > -(Don_win.Width) + step;
            Right.IsEnabled = right > 0 + step;
            Top.IsEnabled = top > -(Don_win.Height) + step;
            Bottom.IsEnabled = bottom >0 + step;
        }

        int step = 40;
        private void LeftB(object sender, RoutedEventArgs e)
        {
            MoveCursor(-step, 0);
        }

        private void TopB(object sender, RoutedEventArgs e)
        {
            MoveCursor(0, -step);
        }

        private void RightB(object sender, RoutedEventArgs e)
        {
            MoveCursor(step, 0);
        }

        private void DownB(object sender, RoutedEventArgs e)
        {
            MoveCursor(0, step);
        }

        private void InfoB(object sender, RoutedEventArgs e)
        {
            var currentMargin = Cursor.Margin;
            // Вычисляем отклонение от начального положения
            var deltaX = currentMargin.Left - _initialMargin.Left;
            var deltaY = currentMargin.Top - _initialMargin.Top;
            // Формируем сообщение
            string message = $"Положение: {currentMargin.Left}({deltaX:+0;-0;0}); {currentMargin.Top}({deltaY:+0;-0;0})";
            // Выводим MessageBox
            MessageBox.Show(message, "Информация о положении", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
