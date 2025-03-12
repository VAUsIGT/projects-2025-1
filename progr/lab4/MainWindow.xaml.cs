using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lab0
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Window size settings (fixed)
            MinWidth = MaxWidth = 800;
            MinHeight = MaxHeight = 600;
        }

        private void AnswerB(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            int vladimir1 = int.Parse(WinCom11.Text);
            int vladimir2 = int.Parse(WinCom12.Text);
            int dmitriy1 = int.Parse(WinCom21.Text);
            int dmitriy2 = int.Parse(WinCom22.Text);

            int aplesi = vladimir1 + dmitriy1;
            int grusha = vladimir2 + dmitriy2;

            // Показываем результат
            ResultText.Text = $"Всего яблок: {aplesi}, а груш: {grusha}";
            ResultText.Visibility = Visibility.Visible;

            // Блокируем поля ввода
            WinCom11.IsEnabled = false;
            WinCom12.IsEnabled = false;
            WinCom21.IsEnabled = false;
            WinCom22.IsEnabled = false;
        }

        private void ReloadB(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей
            WinCom11.Clear();
            WinCom12.Clear();
            WinCom21.Clear();
            WinCom22.Clear();

            // Разблокировка полей
            WinCom11.IsEnabled = true;
            WinCom12.IsEnabled = true;
            WinCom21.IsEnabled = true;
            WinCom22.IsEnabled = true;

            // Очистка текста результата и скрытие
            ResultText.Text = "";
            ResultText.Visibility = Visibility.Hidden;

            // Убираем красный фон у полей
            ResetFieldColors();
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            string errorMessage = "Пожалуйста, введите только положительные целые числа:\n";

            isValid &= ValidateField(WinCom11, ref errorMessage, "Владимир 1");
            isValid &= ValidateField(WinCom12, ref errorMessage, "Владимир 2");
            isValid &= ValidateField(WinCom21, ref errorMessage, "Дмитрий 1");
            isValid &= ValidateField(WinCom22, ref errorMessage, "Дмитрий 2");

            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isValid;
        }

        private bool ValidateField(TextBox textBox, ref string errorMessage, string fieldName)
        {
            string input = textBox.Text.Trim(); // Убираем пробелы в начале и конце
            textBox.Text = input; // Убираем пробелы из поля
            if (input.StartsWith("0") && input != "0")
            {
                errorMessage += $"- {fieldName} (не должно начинаться с нуля)\n";
                textBox.Background = Brushes.LightCoral;
                return false;
            }
            if (!int.TryParse(textBox.Text, out int parsedValue) || parsedValue < 0)
            {
                errorMessage += $"- {fieldName} (только положительные целые числа)\n";
                textBox.Background = Brushes.LightCoral; // Подсветка красным
                return false;
            }
            else
            {
                textBox.Background = Brushes.White; // Убираем красный фон, если всё нормально
                return true;
            }
        }

        private void ResetFieldColors()
        {
            WinCom11.Background = Brushes.White;
            WinCom12.Background = Brushes.White;
            WinCom21.Background = Brushes.White;
            WinCom22.Background = Brushes.White;
        }
    }
}
