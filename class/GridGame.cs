﻿using NAudio.Wave;

namespace TetrisGame
{
    public class GridGame
    {
        private readonly int[,] grid;
        public int Rows { get; }

        public int Columns { get; }

        public int this[int row, int column]
        {
            get => grid[row, column];
            set => grid[row, column] = value;
        }

        public GridGame(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInside(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        public bool IsEmpty(int row, int column)
        {
            return IsInside(row, column) && grid[row, column] == 0;
        }

        public bool IsRowFull(int row)
        {
            for (int column =0; column < Columns; column++)
            {
                if (grid[row, column] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRow(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row, column] = 0;
            }
            SoundManager.PlaySound(@"Assets\sound\me_game_gameover.wav", 0.1f);
        }

        private void MoveRowDown(int row, int rowsNum)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row + rowsNum, column] = grid[row, column];
                grid[row, column] = 0;
            }
        }

        public int ClearFullRows()
        {
            int rowsCleared = 0;
            for (int row = Rows-1; row >= 0; row--)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    SoundManager.PlaySound(@"Assets\sound\me_game_plvup.wav", 0.1f);
                    rowsCleared++;
                }
                else if (rowsCleared > 0)
                {
                    MoveRowDown(row, rowsCleared);
                }
            }
            return rowsCleared;
        }
    }
}
