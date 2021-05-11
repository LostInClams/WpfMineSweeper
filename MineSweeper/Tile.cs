using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Tile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Point m_index;

        readonly bool m_isMine;
        public bool IsMine => m_isMine;

        bool m_isRevealed = false;
        public bool IsRevealed
        {
            get => m_isRevealed;
            set
            {
                if (value == m_isRevealed)
                {
                    return;
                }

                m_isRevealed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRevealed)));
            }
        }

        bool m_isFlagged = false;
        public bool IsFlagged
        {
            get => m_isFlagged;
            set
            {
                if (value == m_isFlagged)
                {
                    return;
                }

                m_isFlagged = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFlagged)));
            }
        }

        public int X => m_index.X;
        public int Y => m_index.Y;

        HashSet<Point> m_adjacentMineIndices = new HashSet<Point>();
        public int NumAdjacentMines => m_adjacentMineIndices.Count;
        public string TileText => m_isMine ? "X" : $"{NumAdjacentMines}";

        public Tile(Point index, bool isMine)
        {
            m_isMine = isMine;
            m_index = index;
        }

        public void AddAdjacentMine(Point index)
        {
            m_adjacentMineIndices.Add(index);
        }
    }
}
