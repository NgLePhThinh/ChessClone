using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Bishop: Piece
    {
        public Bishop(bool white, int x, int y, Board board) : base(white, x, y, PieceDigits.Bishop, board)
        {
        }
        /// <summary>
        /// Tìm điểm bắt đầu và kết thúc của quân tượng đang xét
        /// </summary>
        /// <param name="x">Điểm mục tiêu</param>
        /// <param name="start_x">Tọa độ x ban đầu</param>
        /// <param name="end_x">Tọa độ y ban đầu </param>
        /// <param name="start_y">Tọa độ x mục tiêu</param>
        /// <param name="end_y">Tọa độ y mục tiêu</param>
        public void findStartEnd(Point x, ref int start_x, ref int end_x, ref int start_y, ref int end_y)
        {
            if (x.X < Point.X)//phần bên trái quân tượng
            {
                if (x.Y < Point.Y)//dưới bên trái
                {
                    start_x = x.X + 1;
                    end_x = Point.X - 1;
                    start_y = x.Y + 1;
                    end_y = Point.Y - 1;
                }
                if (x.Y > Point.Y)// trên bên trái
                {
                    start_x = x.X + 1;
                    end_x = Point.X - 1;
                    start_y = Point.Y + 1;
                    end_y = x.Y - 1;
                }
            }
            if (x.X > Point.X)
            {
                if (x.Y < Point.Y)// dưới bên phải
                {
                    start_x = Point.X + 1;
                    end_x = x.X - 1;
                    start_y = x.Y + 1;
                    end_y = Point.Y - 1;
                }
                if (x.Y > Point.Y)
                {
                    start_x = Point.X + 1;
                    end_x = x.X - 1;
                    start_y = Point.Y + 1;
                    end_y = x.Y - 1;
                }
            }
        }

        public override bool checkEmptySpot(Point X)
        {
            int start_x = 0, end_x = 0, start_y = 0, end_y = 0;
            int dx = 9999, dy = 9999, cnt = 0;
            findStartEnd(X, ref start_x, ref end_x, ref start_y, ref end_y);
            for (int y = start_y; y <= end_y; y++)
            {
                for (int x = start_x; x <= end_x; x++)
                {
                    base.calcdxdy(new Point(x, y), ref dx, ref dy);
                    if (dx == dy && Board.checkPiece(new Point(x, y), PieceDigits.Empty))
                    {
                        cnt++;
                    }
                }
            }
            return cnt == (end_x - start_x) + 1;
        }
        public override bool checkMoveto(Point x)
        {
            //this.Board.IsSafeMove(this.Point);
            int dx = -99999, dy = 99999;
            calcdxdy(x, ref dx, ref dy);
            if (dx == 0 && dy == 0) return false;
            return (dx == dy)// đúng quy luật
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
