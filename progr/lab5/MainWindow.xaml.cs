using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Reflection;
using System.IO;
using System.Text.Json;

namespace lab0
{
    public partial class MainWindow : Window
    {
        private const string SETTINGS_PATH = "settings.json";
        private readonly Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings(); // Загрузка настроек перед инициализацией
            // Фиксируем размер окна
            MinWidth = MaxWidth = 800;
            MinHeight = MaxHeight = 600;

            // Инициализация случайных значений
            //InitializeRandomValues();
            UpdateText(null, null); // Первоначальное обновление

            // Подписка на события с передачей параметров
            cbBold.Checked += UpdateText;
            cbBold.Unchecked += UpdateText;
            cbItalic.Checked += UpdateText;
            cbItalic.Unchecked += UpdateText;
            cbUnderline.Checked += UpdateText;
            cbUnderline.Unchecked += UpdateText;
            rbLower.Checked += UpdateText;
            rbUpper.Checked += UpdateText;
            btnUpdate.Click += UpdateText;
            cbPinkBg.Checked += UpdateText;
            cbPinkBg.Unchecked += UpdateText;
            cbYellowText.Checked += UpdateText;
            cbYellowText.Unchecked += UpdateText;
            cbIncreaseFont.Checked += UpdateText;
            cbIncreaseFont.Unchecked += UpdateText;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        // Класс для хранения настроек
        private class AppSettings
        {
            public bool? Bold { get; set; }
            public bool? Italic { get; set; }
            public bool? Underline { get; set; }
            public bool? UpperCase { get; set; }
            public bool? LowerCase { get; set; }
            public bool? InstantUpdate { get; set; }
            public bool? PinkBg { get; set; }
            public bool? YellowText { get; set; }
            public bool? IncreaseFont { get; set; }
        }

        // Сохранение настроек
        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                Bold = cbBold.IsChecked,
                Italic = cbItalic.IsChecked,
                Underline = cbUnderline.IsChecked,
                LowerCase = rbLower.IsChecked,
                InstantUpdate = rbInstant.IsChecked,
                PinkBg = cbPinkBg.IsChecked,
                YellowText = cbYellowText.IsChecked,
                IncreaseFont = cbIncreaseFont.IsChecked
            };

            try
            {
                File.WriteAllText(SETTINGS_PATH, JsonSerializer.Serialize(settings));
            }
            catch { /* Игнорируем ошибки */ }
        }

        // Загрузка настроек
        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SETTINGS_PATH))
                {
                    var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SETTINGS_PATH));

                    // Применяем настройки
                    cbBold.IsChecked = settings.Bold;
                    cbItalic.IsChecked = settings.Italic;
                    cbUnderline.IsChecked = settings.Underline;
                    rbLower.IsChecked = settings.LowerCase;
                    rbUpper.IsChecked = !settings.LowerCase; // Фикс для регистра
                    rbInstant.IsChecked = settings.InstantUpdate;
                    rbOnClick.IsChecked = !settings.InstantUpdate;
                    cbPinkBg.IsChecked = settings.PinkBg;
                    cbYellowText.IsChecked = settings.YellowText;
                    cbIncreaseFont.IsChecked = settings.IncreaseFont;

                    // Форсируем обновление текста сразу после загрузки
                    UpdateText(null, null);
                }
            }
            catch { /* Игнорируем ошибки */ }
        }

        // Обновление обработчика закрытия окна
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            base.OnClosing(e);
        }

        private void InitializeRandomValues()
        {
            // CheckBox
            cbBold.IsChecked = GetRandomBool();
            cbItalic.IsChecked = GetRandomBool();
            cbUnderline.IsChecked = GetRandomBool();

            // RadioButton (регистр)
            if (_random.Next(2) == 0) rbLower.IsChecked = true;
            else rbUpper.IsChecked = true;

            // RadioButton (обновление)
            if (_random.Next(2) == 0) rbInstant.IsChecked = true;
            else rbOnClick.IsChecked = true;
        }

        private bool GetRandomBool() => _random.Next(2) == 1;

        private void UpdateText(object sender, RoutedEventArgs e)
        {
            if (rbInstant.IsChecked == true || sender == btnUpdate)
            {
                // Шрифт
                tbDemo.FontWeight = cbBold.IsChecked == true ? FontWeights.Bold : FontWeights.Normal;
                tbDemo.FontStyle = cbItalic.IsChecked == true ? FontStyles.Italic : FontStyles.Normal;
                tbDemo.TextDecorations = cbUnderline.IsChecked == true ? TextDecorations.Underline : null;

                // Размер шрифта
                tbDemo.FontSize = cbIncreaseFont.IsChecked == true ? 24 : 18; // 18 — начальный размер

                // Цвета
                tbDemo.Background = cbPinkBg.IsChecked == true
                    ? new SolidColorBrush(Color.FromRgb(255, 105, 180))
                    : Brushes.Transparent;

                tbDemo.Foreground = cbYellowText.IsChecked == true
                    ? Brushes.Yellow
                    : Brushes.White;

                // Регистр
                tbDemo.Text = rbUpper.IsChecked == true
                    ? tbDemo.Text.ToUpper()
                    : tbDemo.Text.ToLower();
            }
        }
    }
}
