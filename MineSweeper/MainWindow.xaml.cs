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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board m_board;

        public Board Board => m_board;
        
        public MainWindow()
        {
            InitializeComponent();

            m_board = new Board(10, 10, 10);
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
                    tile.IsRevealed = true;
                }
            }
        }

        private void MarkTile(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
