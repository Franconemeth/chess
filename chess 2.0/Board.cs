using System;
using System.Collections.Generic;
using System.Text;

namespace chess_2._0
{
    internal struct Square
    {
        private int position;
        private Figure figure;
        private bool whAtt;
        private bool blAtt;
        public bool P_whAtt { get { return whAtt; } set { whAtt = value; } }
        public bool P_blAtt { get { return blAtt; } set { blAtt = value; } }
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
        public void MovePiece(int startPos, int targetPos, Board board)
        {
            if (CheckMove(startPos, targetPos, board))
            {
                board[targetPos] = board[startPos];
                Square square = board[startPos];
                square.P_figure = null;
                board[startPos] = square;
                Program.whitesTurn = !Program.whitesTurn;
            }
            else
            {
                ProhibitMove();
            }
        }
        public bool CheckMove(int startPos, int targetPos, Board board)
        {
            //we create a really long condition - we have to make sure if the move is in this case diagonal,
            //then we have to check if there isnt any other piece in between our trail,
            //then we have to check if we dont land on a piece of our colour,
            //and at last we have to make sure that after making this move our king does not fall in check,
            bool movement, freeWay, ownPiece, kingSafe;
            movement = CheckMovement(startPos, targetPos, board);
            freeWay = CheckFreeWay(startPos, targetPos, board);
            ownPiece = board[targetPos].P_figure == null || PieceColor(targetPos, board) != PieceColor(startPos, board);
            kingSafe = CheckKingSafe(startPos, targetPos, board);
            if (movement && freeWay && ownPiece && kingSafe /*&& !ProhibitMove()*/)
            {
                return true;
            }
            return false;
        }
        public bool CheckMovement(int startPos, int targetPos, Board board)
        {
            int diff = Math.Abs(startPos - targetPos);
            int rowS = CoordConv(startPos).Item1;
            int colS = CoordConv(startPos).Item2;
            int rowT = CoordConv(targetPos).Item1;
            int colT = CoordConv(targetPos).Item2;
            switch (board[startPos].P_figure.P_type)
            {
                case figType.bishop:
                    if (diff % 7 == 0 || diff % 9 == 0)
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
                    bool bishop = diff % 7 == 0 || diff % 9 == 0;
                    if (rook || bishop)
                        return true;
                    else
                        return false;
                case figType.pawn:
                    bool isWhite = board[startPos].P_figure.P_isWhite;
                    bool jump = (colS == colT && board[targetPos].P_figure == null) && ((isWhite && rowT == rowS - 1) || (!isWhite && rowT == rowS + 1));
                    bool doubleJump = (board[targetPos].P_figure == null) && ((isWhite && rowS == 6 && rowT == 4 && board[targetPos + 8].P_figure == null) || (!isWhite && rowS == 1 && rowT == 3 && board[targetPos - 8].P_figure == null));
                    bool take = false;
                    if ((colS == 0 && colT == 1) || colS == 7 && colT == 6) take = (isWhite && rowT == rowS - 1) || (!isWhite && rowT == rowS + 1);
                    else if (colS > 0 && colS < 7) take = (isWhite && rowT == rowS - 1 && (colT == colS - 1 || colT == colS + 1)) || (!isWhite && rowT == rowS + 1 && (colT == colS - 1 || colT == colS + 1));
                    if (jump || doubleJump || take) return true;
                    else return false;
                case figType.king:
                    bool vert = diff == 8;
                    bool hori = diff == 1;
                    bool diag = diff == 7 || diff == 9;
                    if (vert || hori || diag) return true;
                    else return false;
                case figType.knight:
                    bool upDown = diff == 15 || diff == 17;
                    bool rightLeft = diff == 6 || diff == 10;
                    if (upDown || rightLeft) return true;
                    else return false;
                default: return false;
            }
        }
        public bool PieceColor(int pos, Board board)
        {
            return board[pos].P_figure.P_isWhite;
        }
        public bool CheckFreeWay(int startPos, int targetPos, Board board)
        {
            List<int> positions = new();
            int diff = Math.Abs(startPos - targetPos);
            int rowS = CoordConv(startPos).Item1;
            int colS = CoordConv(startPos).Item2;
            int rowT = CoordConv(targetPos).Item1;
            int colT = CoordConv(targetPos).Item2;
            int big, small;
            int a = diff % 9 == 0 ? 9 : 7;
            if (startPos > targetPos)
            {
                big = startPos;
                small = targetPos;
            }
            else
            {
                big = targetPos;
                small = startPos;
            }
            switch (board[startPos].P_figure.P_type)
            {

                case figType.rook:
                    if (rowS == rowT)
                    {
                        for (int i = small; i < big - 1; i++)
                        {
                            positions.Add(i + 1);
                        }
                    }
                    else
                    {
                        for (int i = 1; i < diff / 8; i++)
                        {
                            positions.Add(small + i * 8);
                        }
                    }
                    break;
                case figType.bishop:
                    for (int i = 1; i < diff / a; i++)
                    {
                        positions.Add(small + i * a);
                    }
                    break;
                case figType.queen:
                    if (rowS == rowT || colS == colT)
                        goto case figType.rook;
                    else if (diff % 7 == 0 || diff % 9 == 0)
                        goto case figType.bishop;
                    break;
                case figType.pawn:
                case figType.king:
                case figType.knight:
                    return true;
                default: return false;
            }
            for (int i = 0; i < positions.Count; i++)
            {
                if (board[positions[i]].P_figure == null)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public void ActualizeAttPaths(Board board)
        {
            List<int> whAttPos = new();
            List<int> blAttPos = new();
            List<int> pos = new();
            int rowS;
            int colS;
            for (int i = 1; i < 65; i++)
            {
                (rowS, colS) = CoordConv(i);
                if (board[i].P_figure != null)
                {
                    switch (board[i].P_figure.P_type)
                    {
                        case figType.bishop:
                            int a;
                            for (int j = 1; j < 65; j++)
                            {
                                if ((CoordConv(j).Item1 < CoordConv(i).Item1 && CoordConv(j).Item2 > CoordConv(i).Item2) || (CoordConv(j).Item1 > CoordConv(i).Item1 && CoordConv(j).Item2 < CoordConv(i).Item2))
                                    a = 7;
                                else a = 9;
                                if ((i - (j % a)) % a == 0 && j != i)
                                    pos.Add(j);
                            }
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            pos = new();
                            break;
                        case figType.rook:
                            for (int j = 1; j < 65; j++)
                                if (CoordConv(j).Item1 == rowS || CoordConv(j).Item2 == colS)
                                    pos.Add(j);
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            pos = new();
                            break;
                        case figType.queen:
                            for (int j = 1; j < 65; j++)
                            {
                                if ((CoordConv(j).Item1 < CoordConv(i).Item1 && CoordConv(j).Item2 > CoordConv(i).Item2) || (CoordConv(j).Item1 > CoordConv(i).Item1 && CoordConv(j).Item2 < CoordConv(i).Item2))
                                    a = 7;
                                else a = 9;
                                if ((i - (j % a)) % a == 0 && j != i)
                                    pos.Add(j);
                            }
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            pos = new();
                            for (int j = 1; j < 65; j++)
                                if (CoordConv(j).Item1 == rowS || CoordConv(j).Item2 == colS)
                                    pos.Add(j);
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            pos = new();
                            break;
                        case figType.pawn:
                            int b = board[i].P_figure.P_isWhite ? -1 : 1;
                            int c = colS == 0 ? -1 : 1;
                            if ((colS == 0) || colS == 7) pos.Add(PosConv(rowS + b, colS + c));
                            else if (colS > 0 && colS < 7) { pos.Add(PosConv(rowS + b, colS + 1)); pos.Add(PosConv(rowS + b, colS - 1)); }
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            pos = new();
                            break;
                        case figType.king:
                            int diff;
                            bool vert;
                            bool hori;
                            bool diag;
                            for (int j = 1; j < 65; j++)
                            {
                                diff = Math.Abs(i - j);
                                vert = diff == 8;
                                hori = diff == 1;
                                diag = diff == 7 || diff == 9;
                                if (vert || hori || diag)
                                    pos.Add(j);
                            }
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            break;
                        case figType.knight:
                            bool upDown;
                            bool rightLeft;
                            for (int j = 1; j < 65; j++)
                            {
                                diff = Math.Abs(i - j);
                                upDown = diff == 15 || diff == 17;
                                rightLeft = diff == 6 || diff == 10;
                                if (upDown || rightLeft)
                                    pos.Add(j);
                            }
                            if (board[i].P_figure.P_isWhite)
                                whAttPos.AddRange(pos);
                            else blAttPos.AddRange(pos);
                            break;
                    }
                }
            }

        }
        public bool WouldKingBeSafe(Board board)
        {
            int wKingPos = -1;
            int bKingPos = -1;
            for (int i = 1; i < 65; i++)
            {
                if (board[i].P_figure.P_type == figType.king && board[i].P_figure.P_isWhite)
                    wKingPos = i;
                else if (board[i].P_figure.P_type == figType.king && !board[i].P_figure.P_isWhite)
                    bKingPos = i;
                if (wKingPos != -1 && bKingPos != -1)
                    break;
            }
            if (Program.whitesTurn && !board[wKingPos].P_blAtt)
                return true;
            else if (!Program.whitesTurn && !board[bKingPos].P_whAtt)
                return true;
            else return false;
        }
        public bool CheckKingSafe(int startPos, int targetPos, Board board)
        {
            Board board2 = board;
            board2[targetPos] = board2[startPos];
            Square square = board2[startPos];
            square.P_figure = null;
            board2[startPos] = square;
            return WouldKingBeSafe(board2);
        }
        public void ProhibitMove()
        {/* to si nechavam na konec protoze fuj*/ /*bude tu rosada atd, a taky en passant :(*/ }

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
