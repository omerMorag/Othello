using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Othello
{
    public class Board
    {
        private readonly int r_Size;
        private Cell[,] m_Board = null;

        public int Size
        {
            get { return r_Size; }
        }

        public Board(int i_BoardSize)
        {
            r_Size = i_BoardSize;
            m_Board = new Cell[i_BoardSize, i_BoardSize];
            initBoard();
        }

        public Cell this[int i_XIndex, int i_Yindex]
        {
            get { return m_Board[i_XIndex, i_Yindex]; }
            set { m_Board[i_XIndex, i_Yindex] = value; }
        }

        private void initBoard()
        {
            for (int i = 0; i < r_Size; i++)
            {
                for (int j = 0; j < r_Size; j++)
                {
                    m_Board[j, i] = new Cell(Color.Empty, i, j);
                }
            }
        }

        public void Restart()
        {
            for (int i = 0; i < r_Size; i++)
            {
                for (int j = 0; j < r_Size; j++)
                {
                    m_Board[j, i].Color = Color.Empty;
                }
            }
        }

        public void InitNewOthelloGame()
        {
            int center = r_Size / 2;
            UpdateCell(center - 1, center - 1, Color.White);
            UpdateCell(center, center, Color.White);
            UpdateCell(center - 1, center, Color.Black);
            UpdateCell(center, center - 1, Color.Black);
        }

        // updates the cell in the board with the requested color
        public void UpdateCell(int i_X, int i_Y, Color i_Color)
        {
            m_Board[i_X, i_Y].Color = i_Color;
        }

        public bool IsCoordinatesInBounds(int i_X, int i_Y)
        {
            bool isInBounds = true;
            if (i_X >= r_Size || i_X < 0 || i_Y >= r_Size || i_Y < 0)
            {
                isInBounds = false;
            }

            return isInBounds;
        }
    }
}