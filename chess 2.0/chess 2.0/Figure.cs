using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace chess_2._0
{
    enum figType
    {
        pawn = 0,
        rook = 1,
        knight = 2,
        bishop = 3,
        queen = 4,
        king = 5
    }
    internal class Figure
    {
        private figType type;
        private bool isWhite;
        private int position;
        public figType P_type { get { return type; } set { type = value; } }
        public bool P_isWhite { get { return isWhite; } set { isWhite = value; } }
        public int P_position { get { return position; } set { position = value; } }
        public Figure()
        {

        }
        public Figure(figType type, bool isWhite, int position)
        {
            this.type = type;
            this.isWhite = isWhite;
            this.position = position;
        }

        public bool CheckMove(int targetPos, int startPos, Board board)
        {
            //we create a really long condition - we have to make sure if the move is in this case diagonal,
            //then we have to check if there isnt any other piece in between our trail,
            //then we have to check if we dont land on a piece of our colour,
            //and at last we have to make sure that after making this move our king does not fall in check,
            bool movement, freeWay, ownPiece, kingSafe;
            movement =
            if (movement && freeWay && ownPiece && kingSafe)
            {
                return true;
            }
            return false;
        }
        public bool CheckMovement(int targetPos, int startPos, Board board)
        {
            int diff = Math.Abs(startPos - targetPos);
            int rowS = CoordConv(startPos).Item1;
            int colS = CoordConv(startPos).Item2;
            int rowT = CoordConv(targetPos).Item1;
            int colT = CoordConv(targetPos).Item2;
            switch (board[startPos].P_figure.P_type)
            {
                case figType.bishop:
                    if (diff == 7 || diff == 9)
                        return true;
                    else
                        return false;
                case figType.rook:
                    if (rowS == rowT || colS == colT)
                        return true;
                    else
                        return false;
                case figType.queen:
                    bool rook = rowS == rowT || colS == colT;
                    bool bishop = diff == 7 || diff == 9;
                    if (rook || bishop)
                        return true;
                    else
                        return false;
                case figType.pawn:
                    bool isWhite = board[startPos].P_figure.P_isWhite;
                    bool jump = (isWhite && colT == colS - 1) || (!isWhite && colT == colS + 1);
                    bool doubleJump = (isWhite && colS == 6 && colT == 4) || (!isWhite && colS == 1 && colT == 3);
                    bool take =;
                default: return false;
            }
        }
        public void ProhibitMove()
        {/* to si nechavam na konec protoze fuj*/ /*bude tu pravidla krale, sachy, rosada atd, a taky en passant :(*/ }
        public void MovePiece(int targetPos, Figure[,] board)
        {
            if (CheckMove(targetPos, board))
            {
                position = targetPos;
                Program.whitesTurn = !Program.whitesTurn;
            }
            else
            {
                ProhibitMove();
            }
        }
        public (int, int) CoordConv(int position)
        {
            return ((position - 1) / 8, (position - 1) % 8);
        }
        public int PosConv(int row, int col)
        {
            return row * 8 + col + 1;
        }
    }
}
