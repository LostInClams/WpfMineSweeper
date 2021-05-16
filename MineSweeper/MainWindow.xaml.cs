using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Board _board;
        public Board Board {
            get => _board;
            private set
            {
                if (_board == value)
                {
                    return;
                }
                _board = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Board)));
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            //Board = new Board(10, 10, 1);
        }

        public void StartNewGame()
        {
            Board = new Board(10, 10, 10);
        }

        private void ThisAsDataContext(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void RevealTile(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is Tile tile)
                {
                    Board.RevealTile(tile);
                }
            }

            Board.CheckComplete();
        }

        private void MarkTile(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is Tile tile)
                {
                    tile.IsFlagged = !tile.IsFlagged;
                }
            }
        }

        private void NewGameClicked(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }
}
