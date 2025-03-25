using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Lab6
{
    public partial class MainWindow : Window
    {
        private bool _isPhoneTextUpdating = false;
        private string _currentPhoneFormat = "+X (XXX) XXX-XX-XX";
        private string _currentCountryCode = "+7";

        public MainWindow()
        {
            InitializeComponent();
            InitializeDateControls();
            UpdatePhoneFormat();
            FormatPhoneNumber();
            DonWin.MaxHeight = DonWin.MinHeight = 400;
            DonWin.MaxWidth = DonWin.MinWidth = 600;
            DonWin.BorderBrush = Brushes.Brown;
            DonWin.BorderThickness = new Thickness(5);
        }

        private void InitializeDateControls()
        {
            // Заполняем месяцы и годы
            cbMonth.SelectedIndex = 0;
            txtYear.Text = DateTime.Now.Year.ToString();
            UpdateDays();
        }

        private void UpdateDays()
        {
            if (cbMonth == null || txtYear == null) return;

            int year;
            if (!int.TryParse(txtYear.Text, out year)) year = DateTime.Now.Year;

            int month = cbMonth.SelectedIndex + 1;
            int days = DateTime.DaysInMonth(year, month);

            cbDay.Items.Clear();
            for (int i = 1; i <= days; i++)
            {
                cbDay.Items.Add(i.ToString("00"));
            }

            if (cbDay.Items.Count > 0 && cbDay.SelectedIndex == -1)
                cbDay.SelectedIndex = 0;
        }



        private void UpdatePhoneFormat()
        {
            if (lbPhoneType == null) return;

            bool isHome = (lbPhoneType.SelectedItem as ListBoxItem)?.Content.ToString() == "Домашний";

            if (_currentCountryCode == "+77")
            {
                _currentPhoneFormat = isHome ? "XX XXX-XX-XX" : "XX XXX-XX-XX";
            }
            else if (_currentCountryCode == "+7")
            {
                _currentPhoneFormat = isHome ? " (XXXX) XX-XX-XX" : " (XXX) XXX-XX-XX";
            }
            else if (_currentCountryCode == "+91")
            {
                _currentPhoneFormat = " XXXXXXXXXX";
            }
            else if (_currentCountryCode == "+81")
            {
                _currentPhoneFormat = " XXX-XX-XXXX";
            }
            else if (_currentCountryCode == "+49")
            {
                _currentPhoneFormat = " XXX XXXXXX";
            }
            else if (_currentCountryCode == "+375")
            {
                _currentPhoneFormat = " (XX) XXX-XX-XX";
            }
            else if (_currentCountryCode == "+86")
            {
                _currentPhoneFormat = " XX-XXXXXXXX";
            }
            else if (_currentCountryCode == "+1")
            {
                _currentPhoneFormat = " XXX-XXX-XXXX";
            }
            else if (_currentCountryCode == "+")
            {
                _currentPhoneFormat = "XXX-XXX-XXX";
            }
        }

        private void FormatPhoneNumber()
        {
            if (_isPhoneTextUpdating || txtPhone == null) return; //защита от рекурсии
            _isPhoneTextUpdating = true;

            if (txtPhone == null) return;
            string rawNumber = Regex.Replace(txtPhone.Text, @"[^\d]", "");
            string countryCodeDigits = _currentCountryCode.TrimStart('+');

            // Извлекаем только национальную часть номера
            string nationalNumber = rawNumber.StartsWith(countryCodeDigits)
                ? rawNumber.Substring(countryCodeDigits.Length)
                : "";

            string formatted = _currentCountryCode;
            int index = 0;

            foreach (char c in _currentPhoneFormat)
            {
                if (c == 'X')
                {
                    if (index < nationalNumber.Length)
                        formatted += nationalNumber[index++];
                    else
                        break;
                }
                else
                {
                    formatted += c;
                }
            }

            // Удаляем лишние символы в конце
            formatted = Regex.Replace(formatted, @"\D+$", "");

            txtPhone.TextChanged -= txtPhone_TextChanged;
            txtPhone.Text = formatted;
            txtPhone.CaretIndex = formatted.Length;
            txtPhone.TextChanged += txtPhone_TextChanged;

            _isPhoneTextUpdating = false;
        }

        

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-ЯёЁa-zA-Z-]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnFlag_Click(object sender, RoutedEventArgs e)
        {
            var selector = new CountrySelectorWindow();
            if (selector.ShowDialog() == true)
            {
                UpdateCountry(selector.SelectedCountry);
            }
        }

        private void PhoneType_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePhoneFormat();
            FormatPhoneNumber(); // Принудительное применение новой маски
        }

        private void UpdateCountry(Country country)
        {
            imgFlag.Source = new BitmapImage(new Uri(country.FlagPath, UriKind.Relative));
            _currentCountryCode = country.Code;
            txtPhone.Text = country.Code;
            UpdatePhoneFormat();
            FormatPhoneNumber();
        }

        private bool IsPhoneNumberValid()
        {
            try
            {
                string rawNumber = Regex.Replace(txtPhone.Text, @"[^\d]", "");
                string countryCodeDigits = _currentCountryCode.TrimStart('+');

                // Извлекаем национальную часть номера
                string nationalNumber = rawNumber.StartsWith(countryCodeDigits)
                    ? rawNumber.Substring(countryCodeDigits.Length)
                    : rawNumber;

                // Определяем требуемое количество цифр
                int requiredDigits = _currentPhoneFormat.Count(c => c == 'X');

                return nationalNumber.Length == requiredDigits;
            }
            catch
            {
                return false;
            }
        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Проверяем, что номер начинается с текущего кода страны
                //if (!txtPhone.Text.StartsWith(_currentCountryCode))
                //{
                //    txtPhone.TextChanged -= txtPhone_TextChanged;
                //    txtPhone.Text = _currentCountryCode;
                //    txtPhone.CaretIndex = _currentCountryCode.Length;
                //    txtPhone.TextChanged += txtPhone_TextChanged;
                //    return;
                //}
                if (txtPhone.Text.StartsWith("+77"))
                {
                    _currentCountryCode = "+77";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/kazakhstan.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+375"))
                {
                    _currentCountryCode = "+375";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/belarus.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+7") || txtPhone.Text.StartsWith("7"))
                {
                    _currentCountryCode = "+7";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/russia.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+91") || txtPhone.Text.StartsWith("91"))
                {
                    _currentCountryCode = "+91";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/india.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+81") || txtPhone.Text.StartsWith("81"))
                {
                    _currentCountryCode = "+81";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/japan.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+49") || txtPhone.Text.StartsWith("49"))
                {
                    _currentCountryCode = "+49";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/germany.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+86") || txtPhone.Text.StartsWith("86"))
                {
                    _currentCountryCode = "+86";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/china.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else if (txtPhone.Text.StartsWith("+1") || txtPhone.Text.StartsWith("1"))
                {
                    _currentCountryCode = "+1";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/america.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                else
                {
                    _currentCountryCode = "+";
                    imgFlag.Source = new BitmapImage(new Uri("Resourses/unknown3.png", UriKind.Relative));
                    UpdatePhoneFormat();
                    FormatPhoneNumber();
                }
                // Обновляем форматирование
                //UpdatePhoneFormat();
                //FormatPhoneNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Date_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateDays();
        }

        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            // Проверка номера телефона
            if (!IsPhoneNumberValid())
            {
                int requiredDigits = _currentPhoneFormat.Count(c => c == 'X');
                MessageBox.Show($"Номер телефона должен содержать {requiredDigits} цифр после кода страны!");
                return;
            }
            // Проверка даты рождения
            if (cbDay.SelectedItem == null || cbMonth.SelectedItem == null || string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("Заполните все поля даты рождения!");
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Некорректный год рождения!");
                return;
            }

            int month = cbMonth.SelectedIndex + 1;
            int day = int.Parse(cbDay.SelectedItem.ToString());

            DateTime birthDate;
            try
            {
                birthDate = new DateTime(year, month, day);
            }
            catch
            {
                MessageBox.Show("Некорректная дата рождения!");
                return;
            }

            // Проверка возраста
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            if (age < 18 || age > 90)
            {
                MessageBox.Show("Возраст должен быть от 18 до 90 лет!");
                return;
            }

            // Сохранение в файл
            try
            {
                string userData = $"{DateTime.Now:yyyy-MM-dd HH:mm}| " +
                    $"Тип: {((ListBoxItem)lbPhoneType.SelectedItem)?.Content}, " +
                    $"Номер: {txtPhone.Text.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "")}, " +
                    $"ФИО: {txtLastName.Text} {txtFirstName.Text}, " +
                    $"Дата рождения: {birthDate:dd.MM.yyyy}";

                File.AppendAllText("users.txt", userData + Environment.NewLine);
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}