using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Board
    {
        readonly Tile[] m_boardTiles;
        public Tile[] Tiles => m_boardTiles;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Board(int width, int height, int numMines)
        {
            Width = width;
            Height = height;
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

        public int Clamp(int val, int min, int max)
        {
            return Math.Min(Math.Max(val, min), max);
        }
    }
}
