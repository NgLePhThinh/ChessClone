using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class King: Piece
    {
        public King(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.King, board)
        {
        }

        public override bool checkMoveto(Point x)
        {
            int dx = 9999, dy = 9999;
            calcdxdy(x, ref dx, ref dy);
            return (dx * dx + dy * dy <= 2)
                && this.Board.checkPiece(x, this.White, PieceDigits.Empty);
        }

        public override bool Moveto(Point x)
        {
            if (checkMoveto(x))
            {
                Point = new Point(x.X, x.Y);
                return true;
            }
            return false;
        }
        public override bool checkEmptySpot(Point x)
        {
            throw new NotImplementedException();
        }
    }
}
