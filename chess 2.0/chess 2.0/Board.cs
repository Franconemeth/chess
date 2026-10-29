using System;
using System.Collections.Generic;
using System.Text;

namespace chess_2._0
{
    internal struct Square
    {
        private int position;
        private Figure figure;
        public int P_position { get { return position; } }
        public Figure P_figure { get { return figure; } set { figure = value; } }
        public Square(int position, Figure figure)
        {
            this.position = position;
            this.figure = figure;
        }

        public Square()
        {
            position = 0;
            figure = null!;
        }
        public static Square[,] DeclareBoard(Square[,] board) //this method works only for a standard 8x8 board!
        {
            List<figType> list = new();
            for (int i = 1; i < 9; i++)
            {
                if (i < 6)
                {
                    list.Add((figType)i);
                }
                else
                {
                    list.Add((figType)9 - i);
                }
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    board[i, j].position = i * board.GetLength(0) + j + 1;
                    if (board[i, j].position > 16 && board[i, j].position < 49)//empty squares
                    {
                        board[i, j].figure = null!;
                    }
                    else if (board[i, j].position > 8 && board[i, j].position < 17)//black pawns
                    {
                        board[i, j].figure = new Figure(figType.pawn, false, board[i, j].position);
                    }
                    else if (board[i, j].position < 57 && board[i, j].position > 48)//white pawns
                    {
                        board[i, j].figure = new Figure(figType.pawn, true, board[i, j].position);
                    }
                    else
                    {
                        if (board[i, j].position < 9)//black pieces
                        {
                            board[i, j].figure = new Figure(list[j], false, board[i, j].position);
                        }
                        else//white pieces
                        {
                            board[i, j].figure = new Figure(list[j], true, board[i, j].position);
                        }
                    }
                }
            }
            return board;
        }
    }
    internal struct Board
    {
        private Square[,] square = Square.DeclareBoard(new Square[8, 8]);
        public Square this[int position]
        {
            get { return square[(position - 1) / 8, (position - 1) % 8]; }
            set { square[(position - 1) / 8, (position - 1) % 8] = value; }
        }
        public Square this[int row, int column]
        {
            get { return square[row, column]; }
            set { square[row, column] = value; }
        }
        public Board()
        {

        }
    }
}
