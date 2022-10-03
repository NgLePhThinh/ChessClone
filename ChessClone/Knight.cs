using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Knight: Piece
    {
        public Knight(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.Knight, board) { }
        public override bool checkMoveto(Point x)
        {
            //this.Board.IsSafeMove(this.Point);
            return Math.Abs(x.X - Point.X) * Math.Abs(x.Y - Point.Y) == 2
                && this.Board.checkPiece(x, this.White, PieceDigits.Empty);
        }
        public override bool Moveto(Point x)
        {
            if (checkMoveto(x))
            {
                this.Point = new Point(x.X, x.Y);
                return true;
            }
            return false;
        }

        public override bool checkEmptySpot(Point x)
        {
            throw new NotImplementedException();
        }
        public override void deleteFirstMove()
        {  
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
