using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class King: Piece
    {

        private bool firstMove;
        public bool FirstMove { get => firstMove; set => firstMove = value; }

        public King(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.King, board)
        {
            FirstMove = true;
        }
        GamePlay gamePlay = new GamePlay();
        public bool checkCastling(Point x)
        {
            Rook rook = new Rook(true, 0, 0, this.Board);
            int xStart = 0, xEnd = 0;
            if(x.X == 7)
            {
                xStart = 6;
                xEnd = 7;
                if (Board.GetPiece(new Point(8, this.Point.Y)).Digit == PieceDigits.Rook)
                    rook = (Rook)Board.GetPiece(new Point(8, this.Point.Y));
            }
            if (x.X == 3)
            {
                xStart = 3;
                xEnd = 4;
                if(Board.GetPiece(new Point(1,this.Point.Y)).Digit == PieceDigits.Rook)
                    rook = (Rook)Board.GetPiece(new Point(1, this.Point.Y));
            }
            for(int x1 = xStart; x1 <= xEnd; x1++)
            {
                if (Board.GetPiece(new Point(x1, this.Point.Y)).Digit != PieceDigits.Empty 
                    || gamePlay.AttackToEmpty(this.Board,new Point(x1,this.Point.Y),this.White).Any()) 
                    return false;
            }
            return rook.FirstMove;
        }
        public bool CheckDangerousMove(Point x)
        {
            int[] delta = new int[] { -1, 0, 1 };
            int[,] dangerous = this.White ? Board.DangerousWhite : Board.DangerousBlack;
            for(int i = 0; i < 3; i ++)
                for(int j = 0; j < 3; j++)
                {
                    if (dangerous[i, j] > 0 && Point.X + delta[j] == x.X && Point.Y + delta[i] == x.Y)
                        return true;
                }
            return false;
        } 
        public override bool checkMoveto(Point x)
        {

            if (CheckDangerousMove(x)) return false;
            int dx = 9999, dy = 9999;
            calcdxdy(x, ref dx, ref dy);
            if (firstMove && dy == 0 && dx == 2 && (x.X == 3 || x.X == 7))
                return checkCastling(x);
            return  (dx * dx + dy * dy <= 2)
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
        public override void deleteFirstMove()
        {
            firstMove = false;
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
