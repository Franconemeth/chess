using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace chess_2._0
{
    internal class Figure
    {
        private bool isWhite;
        private bool kingInWay = false;
        private int position;
        public bool P_isWhite { get { return isWhite; } set { isWhite = value; } }
        public bool P_kingInWay { get { return kingInWay; } set { kingInWay = value; } }
        public int P_position { get { return position; } set { position = value; } }

        public virtual bool CheckMove(int targetPos)
        { return false; }
        public void ProhibitMove()
        {/* to si nechavam na konec protoze fuj*/ }
        public void MovePiece(int targetPos)
        {
            if (CheckMove(targetPos))
            {
                position = targetPos;
                Program.whitesTurn = !Program.whitesTurn;
            }
            else
            {
                ProhibitMove();
            }
        }

    }
    internal class Bishop : Figure
    {
        public override bool CheckMove(int targetPos)
        {
            //we create a really long condition - we have to make sure if the move is in this case diagonal,
            //then we have to check if there isnt any other piece in between our trail,
            //then we have to check if we dont land on a piece of our colour,
            //and at last we have to make sure that after making this move our king does not fall in check,
            bool movement, freeWay, ownPiece, kingSafe;
            int sumSteps = Math.Abs(targetPos - P_position);
            movement = sumSteps == 9 || sumSteps == 7;
            for (int i = 0; i < sumSteps / 7 - 1; i++)
            {
                if (P_position == 1 || P_position == 2 || P_position ==3)
                {
                    if (true || false || false || false )
                    {
                        Console.WriteLine("Hádám, že to nefunguje když nemám wifi, škoda");
                        for (bool j = true;)
                    }
                }
            }
        }
    }
}
