using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public abstract class Piece
    {
        private Point point;
        private bool white;
        private PieceDigits digit;
        private Board board;
        public Piece(bool white, int x, int y, PieceDigits digits, Board board)
        {

            this.white =   white;
            this.point = new Point(x, y);
            this.digit = digits;
            this.board = board;
        }
        public bool White { get => white; set => white = value; }
        public Point Point { get => point; set => point = value; }
        public PieceDigits Digit { get => digit; set => digit = value; }
        public Board Board { get => board; set => board = value; }

        /// <summary>
        /// Kiểm tra nước đi hợp lệ
        /// </summary>
        /// <param name="x">Tọa độ muốn hướng đến</param>
        /// <returns>true: Hợp lệ; false: ! Hợp lệ</returns>
        public abstract bool checkMoveto(Point x);
        /// <summary>
        /// Kiểm tra nước đi, nếu hợp lệ thì cập nhật nước đi
        /// </summary>
        /// <param name="x">para</param>
        /// <returns>true: Hợp lệ, cập nhật; 
        ///          false: ! Hợp lệ</returns>
        public abstract bool Moveto(Point x);
        /// <summary>
        /// Kiểm tra trên đường đi của quân cờ có bị chặn hay không
        /// </summary>
        /// <param name="x">Ô mục tiêu (x,y)</param>
        /// <returns>true: nếu bị chặn; false: không bị chặn</returns>
        public abstract bool checkEmptySpot(Point x);
        /// <summary>
        /// Tính độ dời của tọa độ quân cờ 
        /// </summary>
        /// <param name="x"> Tọa độ mục tiêu</param>
        /// <param name="dx">Độ dời dx</param>
        /// <param name="dy">Độ dời dy</param>
        public void calcdxdy(Point x, ref int dx, ref int dy)
        {
            dx = Math.Abs(Point.X - x.X);
            dy = Math.Abs(Point.Y - x.Y);
        }
    }
}
