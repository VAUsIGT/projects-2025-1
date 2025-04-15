using System.Windows;

namespace PuzzleGame
{
    public partial class CompletionWindow : Window
    {
        public CompletionWindow(int level, string time)
        {
            InitializeComponent();
            TimeText.Text = $"Время прохождения: {time}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}