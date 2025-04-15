using System;
using System.Windows;
using System.Windows.Controls;

namespace PuzzleGame
{
    public partial class MainWindow : Window
    {
        private readonly string[] levelImages = new string[]
        {
            "C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Images\\puzzle_image1.jpg",
            "C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Images\\puzzle_image2.jpg",
            "C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Images\\puzzle_image3.jpg",
            "C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Images\\puzzle_image4.jpg"
        };
        public MainWindow()
        {
            InitializeComponent();
            LoadLevelTimes();
        }

        private void LoadLevelTimes()
        {
            Level1Time.Text = GameSettings.Level1Time;
            Level2Time.Text = GameSettings.Level2Time;
            Level3Time.Text = GameSettings.Level3Time;
            Level4Time.Text = GameSettings.Level4Time;

            // Разблокируем уровни в зависимости от пройденных
            if (!string.IsNullOrEmpty(GameSettings.Level1Time))
                Level2Button.IsEnabled = true;
            if (!string.IsNullOrEmpty(GameSettings.Level2Time))
                Level3Button.IsEnabled = true;
            if (!string.IsNullOrEmpty(GameSettings.Level3Time))
                Level4Button.IsEnabled = true;
        }

        private void LevelButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int level = int.Parse(button.Tag.ToString());

            // Получаем путь к изображению для уровня (пример)
            string imagePath = $"C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Images\\puzzle_image{level}.jpg";

            PuzzleWindow puzzleWindow = new PuzzleWindow(level, imagePath);
            puzzleWindow.LevelCompleted += PuzzleWindow_LevelCompleted;
            puzzleWindow.ShowDialog();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GameSettings.Level1Time = "";
            GameSettings.Level2Time = "";
            GameSettings.Level3Time = "";
            GameSettings.Level4Time = "";

            Level1Time.Text = "";
            Level2Time.Text = "";
            Level3Time.Text = "";
            Level4Time.Text = "";

            Level2Button.IsEnabled = false;
            Level3Button.IsEnabled = false;
            Level4Button.IsEnabled = false;
        }

        private void PuzzleWindow_LevelCompleted(object sender, LevelCompletedEventArgs e)
        {
            // Сохраняем время прохождения уровня
            switch (e.Level)
            {
                case 1:
                    GameSettings.Level1Time = $"Время: {e.CompletionTime}";
                    Level1Time.Text = GameSettings.Level1Time;
                    Level2Button.IsEnabled = true;
                    break;
                case 2:
                    GameSettings.Level2Time = $"Время: {e.CompletionTime}";
                    Level2Time.Text = GameSettings.Level2Time;
                    Level3Button.IsEnabled = true;
                    break;
                case 3:
                    GameSettings.Level3Time = $"Время: {e.CompletionTime}";
                    Level3Time.Text = GameSettings.Level3Time;
                    Level4Button.IsEnabled = true;
                    break;
                case 4:
                    GameSettings.Level4Time = $"Время: {e.CompletionTime}";
                    Level4Time.Text = GameSettings.Level4Time;
                    break;
            }
        }
    }
}