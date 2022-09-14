using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Queen: Piece
    {
        public Queen(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.Queen, board)
        {
        }
        public override bool checkMoveto(Point x)
        {
            //this.Board.IsSafeMove(this.Point);

            Rook xe = new Rook(this.White, this.Point.X, this.Point.Y, this.Board);
            Bishop tuong = new Bishop(this.White, this.Point.X, this.Point.Y, this.Board);
            return xe.checkMoveto(x) || tuong.checkMoveto(x);
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
        public override void deleteFirstMove()
        {
           
        }
    }
}
