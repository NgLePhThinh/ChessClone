using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Rook: Piece
    {
        private bool firstMove;

        public bool FirstMove { get => firstMove; set => firstMove = value; }

        public Rook(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.Rook, board)
        {
            FirstMove = true;
        }
        /// <summary>
        /// Tìm điểm bắt đầu và kết thúc của tọa độ hoặc x hoặc y quân xe
        /// </summary>
        /// <param name="x">Tọa độ đi đến</param>
        /// <param name="start_check">Điểm bắt đầu</param>
        /// <param name="end_check">Điểm kết thúc</param>
        public void findStartEnd(Point x, ref int start_check, ref int end_check)
        {
            int dx = Math.Abs(Point.X - x.X);
            int dy = Math.Abs(Point.Y - x.Y);
            if (dx == 0)//đi theo cột dọc
            {
                if (x.Y < Point.Y)// đi xuống
                {
                    start_check = x.Y + 1;
                    end_check = Point.Y - 1;
                }
                if (x.Y > Point.Y)//đi lên
                {
                    start_check = Point.Y + 1;
                    end_check = x.Y - 1;
                }
            }
            if (dy == 0)//đi theo hàng ngang
            {
                if (x.X < Point.X)//qua trái
                {
                    start_check = x.X + 1;
                    end_check = Point.X - 1;
                }
                if (x.X > Point.X)//qua phải
                {
                    start_check = Point.X + 1;
                    end_check = x.X - 1;
                }
            }
        }
        public override bool checkEmptySpot(Point x)
        {
            int start_check = 0, end_check = 0;
            findStartEnd(x, ref start_check, ref end_check);
            int dx = 9999, dy = 9999;
            calcdxdy(x, ref dx, ref dy);
            for (int i = start_check; i <= end_check; i++)
            {
                if (dx == 0)
                {
                    if (!this.Board.checkPiece(new Point(x.X, i), PieceDigits.Empty))
                        return false;
                }
                if (dy == 0)
                {
                    if (!this.Board.checkPiece(new Point(i, x.Y), PieceDigits.Empty))
                        return false;
                }

            }
            return true;
        }
        public override bool checkMoveto(Point x)
        {
            //this.Board.IsSafeMove(this.Point);
            int dx = 9999, dy = 9999;
            calcdxdy(x, ref dx, ref dy);
            return ((dx == 0 || dy == 0) && dx != dy)//đúng quy luật
                && checkEmptySpot(x) // trên đường đi không bị cản
                && this.Board.checkPiece(x, this.White, PieceDigits.Empty);// ô mục tiêu là địch hoặc là rỗng
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
        public override void deleteFirstMove()
        {
            firstMove = false;
        }
    }
}
