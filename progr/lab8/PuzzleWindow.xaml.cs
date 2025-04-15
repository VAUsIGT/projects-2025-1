using System;
using System.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PuzzleGame
{
    public partial class PuzzleWindow : Window
    {
        private readonly string _imagePath;
        private int level;
        private int rows, cols;
        private double pieceWidth, pieceHeight;
        private int correctPieces = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private bool isCompleted = false;
        private Image draggedPiece = null;
        private Point offset;
        private HashSet<Image> placedPieces = new HashSet<Image>();
        private ScaleTransform scaleTransform = new ScaleTransform();
        private BitmapImage originalBitmap;
        private double scaleRatio = 1.0;

        public event EventHandler<LevelCompletedEventArgs> LevelCompleted;

        public PuzzleWindow(int level, string imagePath)
        {
            InitializeComponent();
            this.level = level;
            this._imagePath = imagePath;

            rows = 2 + level;
            cols = 3 + level;

            Title = $"Пазл - Уровень {level}";
            PuzzleCanvas.RenderTransform = scaleTransform;
            Loaded += (s, e) => InitializePuzzle();
        }

        private void InitializePuzzle()
        {
            try
            {
                originalBitmap = new BitmapImage(new Uri(_imagePath));

                // Рассчитываем масштаб для заполнения области
                CalculateScale();

                // Создаем кусочки с учетом масштаба
                CreatePuzzlePieces();

                stopwatch.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
                Close();
            }
        }

        private void CalculateScale()
        {
            double areaWidth = PuzzleArea.ActualWidth - 4;
            double areaHeight = PuzzleArea.ActualHeight - 4;
            double imageRatio = originalBitmap.PixelWidth / (double)originalBitmap.PixelHeight;
            double areaRatio = areaWidth / areaHeight;

            scaleRatio = imageRatio > areaRatio
                ? areaWidth / originalBitmap.PixelWidth
                : areaHeight / originalBitmap.PixelHeight;

            PuzzleCanvas.Width = originalBitmap.PixelWidth * scaleRatio;
            PuzzleCanvas.Height = originalBitmap.PixelHeight * scaleRatio;
        }

        private void CreatePuzzlePieces()
        {
            List<Image> puzzlePieces = new List<Image>();
            double originalPieceWidth = originalBitmap.PixelWidth / cols;
            double originalPieceHeight = originalBitmap.PixelHeight / rows;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    CroppedBitmap cropped = new CroppedBitmap(originalBitmap,
                        new Int32Rect(
                            (int)(col * originalPieceWidth),
                            (int)(row * originalPieceHeight),
                            (int)originalPieceWidth,
                            (int)originalPieceHeight));

                    Image piece = new Image
                    {
                        Source = cropped,
                        Width = originalPieceWidth * scaleRatio,
                        Height = originalPieceHeight * scaleRatio,
                        Tag = new Point(
                            col * originalPieceWidth * scaleRatio,
                            row * originalPieceHeight * scaleRatio),
                        Cursor = Cursors.Hand
                    };

                    piece.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
                    piece.MouseMove += Piece_MouseMove;
                    piece.MouseLeftButtonUp += Piece_MouseLeftButtonUp;
                    puzzlePieces.Add(piece);
                }
            }

            ShufflePieces(puzzlePieces);
            AddPiecesToPanel(puzzlePieces);
        }

        private void ShufflePieces(List<Image> pieces)
        {
            Random rnd = new Random();
            for (int i = pieces.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (pieces[i], pieces[j]) = (pieces[j], pieces[i]);
            }
        }

        private void AddPiecesToPanel(List<Image> pieces)
        {
            foreach (var piece in pieces)
            {
                PiecesPanel.Children.Add(piece);
            }
        }

        private void Piece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCompleted) return;

            draggedPiece = (Image)sender;
            offset = e.GetPosition(draggedPiece);

            if (PiecesPanel.Children.Contains(draggedPiece))
            {
                PiecesPanel.Children.Remove(draggedPiece);
                PuzzleCanvas.Children.Add(draggedPiece);
            }

            var pos = e.GetPosition(PuzzleCanvas);
            Canvas.SetLeft(draggedPiece, pos.X - offset.X);
            Canvas.SetTop(draggedPiece, pos.Y - offset.Y);

            draggedPiece.CaptureMouse();
            draggedPiece.Opacity = 0.7;
            new SoundPlayer("C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Sounds\\drag.wav").Play();
        }

        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedPiece == null || isCompleted) return;

            var currentPosition = e.GetPosition(PuzzleCanvas);
            Canvas.SetLeft(draggedPiece, currentPosition.X - offset.X);
            Canvas.SetTop(draggedPiece, currentPosition.Y - offset.Y);
        }

        private void Piece_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedPiece == null || isCompleted) return;

            draggedPiece.ReleaseMouseCapture();
            draggedPiece.Opacity = 1.0;

            Point correctPos = (Point)draggedPiece.Tag;
            double currentLeft = Canvas.GetLeft(draggedPiece);
            double currentTop = Canvas.GetTop(draggedPiece);

            bool isCorrectPosition =
                Math.Abs(currentLeft - correctPos.X) <= 50 * scaleTransform.ScaleX &&
                Math.Abs(currentTop - correctPos.Y) <= 50 * scaleTransform.ScaleY;

            if (isCorrectPosition && !placedPieces.Contains(draggedPiece))
            {
                Canvas.SetLeft(draggedPiece, correctPos.X);
                Canvas.SetTop(draggedPiece, correctPos.Y);
                placedPieces.Add(draggedPiece);
                correctPieces++;

                // Отключаем взаимодействие с поставленным кусочком
                draggedPiece.IsHitTestVisible = false;

                if (correctPieces == rows * cols)
                    CompletePuzzle();

                new SoundPlayer("C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Sounds\\correct.wav").Play();
            }
            else
            {
                // Если кусочек уже был поставлен, не возвращаем его в панель
                if (!placedPieces.Contains(draggedPiece))
                {
                    PuzzleCanvas.Children.Remove(draggedPiece);
                    PiecesPanel.Children.Add(draggedPiece);
                    new SoundPlayer("C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Sounds\\uncorrect.wav").Play();
                }
            }

            draggedPiece = null;
        }

        private void CompletePuzzle()
        {
            isCompleted = true;
            stopwatch.Stop();
            new SoundPlayer("C:\\Users\\XOMA\\Documents\\GitHub\\projects-2024-2\\progr-sem-2\\lab8\\Sounds\\win.wav").Play();
            TimeSpan elapsed = stopwatch.Elapsed;
            string time = $"{elapsed:mm\\:ss}";

            new CompletionWindow(level, time).ShowDialog();
            LevelCompleted?.Invoke(this, new LevelCompletedEventArgs(level, time));
            Close();
        }

        private void PuzzleArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (originalBitmap == null) return;

            CalculateScale();
            PuzzleCanvas.Children.Clear();
            PiecesPanel.Children.Clear();
            CreatePuzzlePieces();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stopwatch.Stop();
        }
    }
}