using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Board : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        readonly Tile[] m_boardTiles;
        public ReadOnlyCollection<Tile> Tiles => Array.AsReadOnly(m_boardTiles);
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MineCount { get; private set; }

        bool _gameEnded = false;
        public bool GameEnded 
        {
            get => _gameEnded;
            private set
            {
                if (_gameEnded == value)
                {
                    return;
                }
                _gameEnded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameEnded)));
            }
        }

        bool _gameWon = false;
        public bool GameWon 
        {
            get => _gameWon;
            private set
            {
                if (_gameWon == value)
                {
                    return;
                }
                _gameWon = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameWon)));
            }
        }

        public Board(int width, int height, int numMines)
        {
            Width = width;
            Height = height;
            MineCount = numMines;
            m_boardTiles = new Tile[width * height];
            // TODO: Make less naive solution for picking which tiles should be mines
            var randGen = new Random();
            var mineIndices = new HashSet<int>();
            while (mineIndices.Count < numMines)
            {
                mineIndices.Add(randGen.Next(width * height));
            }

            // Create board
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = new Tile(new Point(x, y), mineIndices.Contains(x + y * width));
                    m_boardTiles[x + y * width] = tile;
                    for (int y1 = Math.Max(y - 1, 0); y1 <= Math.Min(y + 1, height-1); y1++)
                    {
                        for (int x1 = Math.Max(x - 1, 0); x1 <= Math.Min(x + 1, width-1); x1++)
                        {
                            if (x1 == x && y1 == y)
                            {
                                continue;
                            }
                            if (mineIndices.Contains(x1 + y1 * width))
                            {
                                tile.AddAdjacentMine(new Point(x1, y1));
                            }
                        }
                    }
                }
            }
        }

        public void RevealTile(Tile tile)
        {
            if (tile.IsMine)
            {
                EndGame(false);
            }

            if (tile.NumAdjacentMines > 0)
            {
                tile.IsRevealed = true;
            }
            else
            {
                tile.IsRevealed = true;
                DoForAdjacentTiles(tile, RevealTileRecursive);
            }
        }

        private void RevealTileRecursive(Tile tile)
        {
            if (tile.IsRevealed)
            {
                return;
            }
            tile.IsRevealed = true;
            if (tile.NumAdjacentMines == 0)
            {
                DoForAdjacentTiles(tile, RevealTileRecursive);
            }
        }

        public int SequentialIndex(Tile tile)
        {
            return tile.X + tile.Y * Width;
        }

        public void DoForAdjacentTiles(Tile tile, Action<Tile> action)
        {
            for (int y = Math.Max(tile.Y - 1, 0); y <= Math.Min(tile.Y + 1, Height - 1); y++)
            {
                for (int x = Math.Max(tile.X - 1, 0); x <= Math.Min(tile.X + 1, Width - 1); x++)
                {
                    if (x == tile.X && y == tile.Y)
                    {
                        continue;
                    }
                    action?.Invoke(m_boardTiles[x + y * Width]);
                }
            }
        }

        public IEnumerable<Tile> GetAdjacentTiles(Tile tile)
        {
            for (int y = Math.Max(tile.Y - 1, 0); y <= Math.Min(tile.Y + 1, Height - 1); y++)
            {
                for (int x = Math.Max(tile.X - 1, 0); x <= Math.Min(tile.X + 1, Width - 1); x++)
                {
                    if (x == tile.X && y == tile.Y)
                    {
                        continue;
                    }
                    yield return m_boardTiles[x + y * Width];
                }
            }
        }

        public bool CheckComplete()
        {
            var revealedTiles = m_boardTiles.Where((tile) => {
                return tile.IsRevealed;
            });

            if (revealedTiles.Count() == m_boardTiles.Length - MineCount)
            {
                EndGame(true);
                return true;
            }
            return false;
        }

        public void EndGame(bool won)
        {
            GameWon = won;
            GameEnded = true;
        }


        public int Clamp(int val, int min, int max)
        {
            return Math.Min(Math.Max(val, min), max);
        }
    }
}
