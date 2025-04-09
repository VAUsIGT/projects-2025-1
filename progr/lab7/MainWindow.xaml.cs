using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Lab7
{
    public partial class MainWindow : Window
    {
        private int _randomClicks = 0;
        private const int MaxClicks = 5;

        public MainWindow()
        {
            InitializeComponent();
            RedSlider.ValueChanged += (s, e) => UpdateColor();
            GreenSlider.Scroll += (s, e) => UpdateColor();
            BlueSlider.Scroll += (s, e) => UpdateColor();
            UpdateColor();
            OpacitySlider.ValueChanged += (s, e) => UpdateTransparency();
        }

        private void UpdateColor()
        {
            if (RedSlider == null || GreenSlider == null || BlueSlider == null || ColorRectangle == null) return;

            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;

            // Обновляем цвет прямоугольника
            if (ColorRectangle.Fill is SolidColorBrush brush)
            {
                brush.Color = Color.FromRgb(r, g, b);
            }
            else
            {
                ColorRectangle.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            }
            // Инвертируем цвет текста
            BackgroundLabel.Foreground = new SolidColorBrush(
                Color.FromRgb((byte)(255 - r), (byte)(255 - g), (byte)(255 - b))
            );
        }

        private void UpdateTransparency()
        {
            if (OpacitySlider == null || ColorRectangle == null || BackgroundLabel == null || RandomButton == null) return;

            ColorRectangle.Opacity = OpacitySlider.Value / 100.0;
            BackgroundLabel.Visibility = OpacitySlider.Value < 0 ? Visibility.Hidden : Visibility.Visible;
            RandomButton.IsEnabled = !(_randomClicks >= MaxClicks || OpacitySlider.Value < 25);
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            if (RedSlider == null || GreenSlider == null || BlueSlider == null) return;

            Random rand = new Random();
            RedSlider.Value = (int)rand.Next(0, 11) * 25; // Шаг 25 (0, 25, 50, ..., 250)
            GreenSlider.Value = (int)rand.Next(0, 256);
            BlueSlider.Value = (int)rand.Next(0, 256);
            _randomClicks++;
            UpdateColor();
            UpdateTransparency();
        }

        private void GreenSlider_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateColor();
        }
        private void BlueSlider_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateColor();
        }

        private void OpacitySlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (OpacitySlider == null) return;
            OpacitySlider.Value += e.Delta > 0 ? 25 : -25;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RedSlider == null || GreenSlider == null || BlueSlider == null) return;

            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int value))
            {
                value = value < 0 ? 0 : (value > 255 ? 255 : value);

                // Для RedSlider округляем до шага 25
                if (textBox == RedTextBox)
                {
                    value = (int)Math.Round(value / 25.0) * 25;
                    RedSlider.Value = value;
                }
                else if (textBox == GreenTextBox) GreenSlider.Value = value;
                else if (textBox == BlueTextBox) BlueSlider.Value = value;
            }
            UpdateColor();
        }
    }
}
