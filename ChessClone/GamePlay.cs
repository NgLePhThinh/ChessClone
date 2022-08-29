using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ChessClone
{
    public class GamePlay
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns>true: Điểm chọn nằm trên bàn cờ ; false: Ngược lại</returns>
        public bool ImproperPoint(Point x)
        {
            return 1 <= x.X && x.X <= 8 && 1 <= x.Y && x.Y <= 8;
        }
        public int GetCellType(Point x, Board board)
        {
            if(!board.checkPiece(x, PieceDigits.Empty))
            {
                return board.Pieces[x.Y, x.X].White == true ? 1 : -1;
            }
            return 0;
        }
        public void MoveChessPiece(Point start, Point end,Board map)
        {

            Piece startPiece = map.GetPiece(start);
            Piece endPiece = map.GetPiece(end);
            map.SetPiece(startPiece, end);
            UpdateChessImage(map.GetCellAt(start), map.GetCellAt(end));
            map.SetEmptyPiece(start, map);
        }
        private void UpdateChessImage(PictureBox ptbStart, PictureBox ptbEnd)
        {
            ptbEnd.Image = ptbStart.Image;
            ptbStart.Image = null;
        }
        
        public void CancelChoose(Point x, Board board)
        {
            Color color = Math.Abs(x.X - x.Y) % 2 == 0 ? board.Whitecolor : board.Blackcolor;
            board.GetCellAt(x).BackColor = color;
        }
        public void PossibleMove(Point point, Board board)
        {
            for(int y = 1;y <= Contant.Rows; y ++)
            {
                for(int x = 1; x <= Contant.Cols; x++)
                {
                    Point goalPoint = new Point(x, y);
                    if (board.Pieces[point.Y, point.X].checkMoveto(goalPoint))
                    {
                        if (GetCellType(new Point(x, y), board) == 0)
                        {
                            board.GetCellAt(goalPoint).Image = Image.FromFile(@"Resources\PossibleMove.png");
                        }
                        else board.GetCellAt(goalPoint).BackColor = Color.LightPink;
                    }
                }
            }
        }
        public void Checked(Board board,bool p1Turn)
        {
            Point kingLocate = p1Turn ? board.WhiteKingLocate : board.BlackKingLocate;
            Piece King = board.GetPiece(kingLocate);
            for(int y = 1; y <= Contant.Rows; y++)
            {
                for(int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if(board.checkPiece(new Point(x, y), King.White) && piece.checkMoveto(kingLocate))
                    {
                        board.PictureBoxes[kingLocate.Y,kingLocate.X].BackColor = Color.LightPink;
                    }

                }
            }
        }
        public void ResetPossibleMove(Board board, Point point)
        {
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Point goalPoint = new Point(x, y);
                    if (board.GetCellAt(goalPoint).Image != null && board.Pieces[goalPoint.Y, goalPoint.X].Digit == PieceDigits.Empty)
                        board.GetCellAt(goalPoint).Image = null;
                    if (board.GetCellAt(goalPoint).BackColor == Color.LightPink)
                        CancelChoose(goalPoint, board);
                }
            }
        }


    }
}
