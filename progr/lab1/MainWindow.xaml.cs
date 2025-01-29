using System;
using System.Collections.Generic;
using System.Linq;
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
        //string Colors = []
        public MainWindow()
        {
            InitializeComponent();
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

        private void Don_win_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Don_win.Background = Brushes.PaleGreen;
        }
        private void Don_win_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Don_win.Background = Brushes.PaleVioletRed;
        }
        //для внутренних операций, внешне не происходит
        private void GridDown(object sender, KeyEventArgs e)
        {
            Don_win.Width = 1000;
            Don_win.Height = 700;
        }

        private void GridUp(object sender, KeyEventArgs e)
        {
            Don_win.Width = 700;
            Don_win.Height = 400;
        }

        private void Don_win_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Don_win.WindowState = WindowState.Maximized;
        }

        private void Don_win_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Don_win.WindowState = WindowState.Minimized;
        }

        private void Don_win_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) {
                int WidthCenter = ((int)Don_win.MaxWidth + (int)Don_win.MinWidth) / 2;
                int HeightCenter = ((int)Don_win.MaxHeight + (int)Don_win.MinHeight) / 2;
                if (Don_win.Width > WidthCenter) { 
                    Don_win.Width = 2 * WidthCenter - Don_win.Width; 
                }
                else {
                    Don_win.Width = 2 * WidthCenter - Don_win.Width;
                }
                if (Don_win.Height > HeightCenter) {
                    Don_win.Height = 2 * HeightCenter - Don_win.Height;
                }
                else {
                    Don_win.Height = 2 * HeightCenter - Don_win.Height;
                }
            }
            if (e.Key == Key.Escape){
                Don_win.Close();
            }
        }
    }
}
