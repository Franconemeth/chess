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

        
    }
}
