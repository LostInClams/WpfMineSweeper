using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class AIPlayer
    {
        Board m_board;
        float[] tileMineProbability;

        public AIPlayer(Board board)
        {
            m_board = board;
            tileMineProbability = new float[board.Width * board.Height];
        }

        public IEnumerator SolveBoard()
        {
            while (!m_board.CheckComplete())
            {
                yield return DoOneMove();
            }
        }

        private bool DoOneMove()
        {
            var rand = new Random();
            var numberedTiles = m_board.Tiles.Where((tile) =>
            {
                return tile.IsRevealed && tile.NumAdjacentMines > 0;
            });
            if (numberedTiles.Count() == 0)
            {
                // Reveal random tile
                m_board.RevealTile(m_board.Tiles[rand.Next(0, m_board.Width * m_board.Height)]);
                return true;
            }

            for (int i = 0; i < tileMineProbability.Length; i++)
            {
                tileMineProbability[i] = -1f;
            }

            foreach (var numberedTile in numberedTiles)
            {
                var adjFlagged = m_board.GetAdjacentTiles(numberedTile).Where((adjTile) =>
                {
                    return adjTile.IsFlagged;
                });
                var adjNoneRevealed = m_board.GetAdjacentTiles(numberedTile).Where((adjTile) =>
                {
                    return !adjTile.IsRevealed;
                });
                var adjNoneRevealedNoneFlagged = m_board.GetAdjacentTiles(numberedTile).Where((adjTile) =>
                {
                    return !adjTile.IsFlagged && !adjTile.IsRevealed;
                });
                if (adjFlagged.Count() == numberedTile.NumAdjacentMines)
                {
                    bool anyRevealed = false;
                    m_board.DoForAdjacentTiles(numberedTile, (adjTile) =>
                    {
                        if (!adjTile.IsFlagged && !adjTile.IsRevealed)
                        {
                            m_board.RevealTile(adjTile);
                            anyRevealed = true;
                        }
                    });
                    if (anyRevealed)
                    {
                        return true;
                    }
                }
                else
                {
                    var numUnflaggedAdjacentMines = numberedTile.NumAdjacentMines - adjFlagged.Count();
                    var possibleAdjMines = adjNoneRevealed.Count() - adjFlagged.Count();
                    if (possibleAdjMines == numUnflaggedAdjacentMines)
                    {
                        foreach (var mineToFlagg in adjNoneRevealed)
                        {
                            mineToFlagg.IsFlagged = true;
                        }
                        return true;
                    }

                    if (numUnflaggedAdjacentMines > 0)
                    {
                        
                        m_board.DoForAdjacentTiles(numberedTile, (adjTile) =>
                        {
                            if (adjTile.IsRevealed)
                            {
                                return;
                            }
                            var seqIndex = m_board.SequentialIndex(adjTile);
                            tileMineProbability[seqIndex] = Math.Max((float)numUnflaggedAdjacentMines / adjNoneRevealedNoneFlagged.Count(), tileMineProbability[seqIndex]);
                        });
                    }
                }
            }

            for (int i = 0; i < tileMineProbability.Length; i++)
            {
                bool anyFlagged = false;
                if (tileMineProbability[i] > .99f)
                {
                    m_board.Tiles[i].IsFlagged = true;
                    anyFlagged = true;
                }
                if (anyFlagged)
                {
                    return true;
                }
            }

            var indexOfLowestProbability = -1;
            var lowestProbability = 1f;
            for (int i = 0; i < tileMineProbability.Length; i++)
            {
                var mineProbability = tileMineProbability[i];
                if (mineProbability > 0 && mineProbability < lowestProbability)
                {
                    lowestProbability = mineProbability;
                    indexOfLowestProbability = i;
                }
            }

            if (indexOfLowestProbability != -1)
            {
                m_board.RevealTile(m_board.Tiles[indexOfLowestProbability]);
            }
            return false;
        }
    }
}
