using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Pawn : Piece
    {
        private bool fisrtMove;

        public bool FisrtMove { get => fisrtMove; set => fisrtMove = value; }

        public Pawn(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.Pawn, board)
        {
            FisrtMove = false;
        }
        public override bool checkMoveto(Point x)
        {
            int dx = Math.Abs(x.X - Point.X);
            if (this.White == true)
            {
                if(this.Point.Y == 5 && x.Y == 6 && Math.Abs(Point.X - x.X) == 1)
                {
                    Piece piece = this.Board.GetPiece(new Point(x.X,x.Y-1));
                    if(piece.Digit == PieceDigits.Pawn && piece.getFirstMove())
                    {
                        return true;
                    }
                }
                if(dx == 0)
                {

                    //nước đi đầu tiên 2 o
                    if (Point.Y == 2 && x.Y - Point.Y == 2
                        && (this.Board.checkPiece(new Point(Point.X, Point.Y + 1), PieceDigits.Empty)
                        && (this.Board.checkPiece(x, PieceDigits.Empty))))
                    {
                        return true;
                    }
                    //nước đi len bt
                    if (Point.Y >= 2 && (x.Y - Point.Y == 1) 
                        && this.Board.checkPiece(new Point(x.X, x.Y), PieceDigits.Empty))
                    {
                        return true;
                    }
                }
                //an cheo
                if (Point.Y + 1 == x.Y && (Point.X - 1 == x.X || Point.X + 1 == x.X)
                        && (!this.Board.checkPiece(new Point(x.X, x.Y), PieceDigits.Empty))
                        && (this.Board.checkPiece(new Point(x.X, x.Y), White)))
                    {
                        return true;
                    }
                else return false;
            }
            else
            {
                if (this.Point.Y == 4 && x.Y == 3 && Math.Abs(Point.X - x.X) == 1)
                {
                    Piece piece = this.Board.GetPiece(new Point(x.X, x.Y + 1));
                    if (piece.Digit == PieceDigits.Pawn && piece.getFirstMove())
                    {
                        return true;
                    }
                }
                if (dx == 0)
                {
                    //nước đi đầu tiên 2 o
                    if (Point.Y == 7 && -x.Y + Point.Y == 2
                        && (this.Board.checkPiece(new Point(Point.X, Point.Y - 1), PieceDigits.Empty)
                        && (this.Board.checkPiece(x, PieceDigits.Empty))))
                    {
                        return true;

                    }
                    //nước đi len bt
                    if (Point.Y <= 7 && (-x.Y + Point.Y == 1) 
                        && this.Board.checkPiece(new Point(Point.X, Point.Y - 1), PieceDigits.Empty))
                    {
                        return true;
                    }
                }
                //an cheo
                if (Point.Y - 1 == x.Y && (Point.X - 1 == x.X || Point.X + 1 == x.X)
                    && (!this.Board.checkPiece(new Point(x.X, x.Y), PieceDigits.Empty))
                    && (this.Board.checkPiece(new Point(x.X, x.Y), White)))
                {
                    return true;
                }
                else return false;
            }
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
        public override void deleteFirstMove(){
            FisrtMove = false;
        }
        public override void updateFirstMove()
        {
            FisrtMove=true;
        }
        public override bool getFirstMove()
        {
            return FisrtMove;
        }
    }
}

