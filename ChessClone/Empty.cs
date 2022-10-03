using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Empty: Piece
    {
        public Empty(bool white, int x, int y, PieceDigits digits, Board board) : base(white, x, y, PieceDigits.Empty, board)
        {
        }
        public override bool checkMoveto(Point x)
        {
            return false;
        }

        public override bool Moveto(Point x)
        {
            return false;
        }
        public override bool checkEmptySpot(Point x)
        {
            return true;
        }
        public override void deleteFirstMove()
        {
            throw new NotImplementedException();
        }
        public override void updateFirstMove()
        {
        }
        public override bool getFirstMove()
        {
            return false;
        }
    }
}
