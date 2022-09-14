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
            if (!board.checkPiece(x, PieceDigits.Empty))
            {
                return board.Pieces[x.Y, x.X].White == true ? 1 : -1;
            }
            return 0;
        }
        public void MoveChessPiece(Point start, Point end, Board map)
        {

            Piece startPiece = map.GetPiece(start);
            Piece endPiece = map.GetPiece(end);
            PictureBox ptbStart = map.GetCellAt(startPiece.Point);
            PictureBox ptbEnd = map.GetCellAt(endPiece.Point);
            if(startPiece.Digit == PieceDigits.King )
            {
                King king = new King(startPiece.White, startPiece.Point.X, startPiece.Point.Y, map);
                if (king.checkCastling(end))
                {
                    Piece rook = endPiece.Point.X < startPiece.Point.X ? 
                        map.GetPiece(new Point(1, king.Point.Y)): map.GetPiece(new Point(8, king.Point.Y)); 
                    PictureBox ptbRook = map.GetCellAt(rook.Point);
                    Piece empty = endPiece.Point.X < startPiece.Point.X ? 
                        map.GetPiece(new Point(4, king.Point.Y)) : map.GetPiece(new Point(6, king.Point.Y));
                    PictureBox ptbempty = map.GetCellAt(empty.Point);
                    //ptbempty.Image = ptbRook.Image;
                    //ptbRook.Image = null;
                    MoveChessPiece(rook.Point, empty.Point, map);
                }


            }
            UpdateChessImage(ptbStart, ptbEnd);
            map.SetPiece(startPiece, end);
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
            if(!board.IsSafeMove(point)) return;
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
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
        public List<Piece> AttackTo(Board board, Point point)
        {
            List<Piece> AttackingPiece = new List<Piece>();
            //Point kingLocate = whiteTurn ? Board.WhiteKingLocate : Board.BlackKingLocate;

            Piece attcked = board.GetPiece(point);
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if (board.checkPiece(new Point(x, y), attcked.White) && piece.checkMoveto(point))
                    {
                        AttackingPiece.Add(piece);
                    }

                }
            }
            return AttackingPiece;
        }
        public bool CanProtectKing(Board board, Piece piece, List <Piece> AttackingPiece)
        {
            Point kingLocate = piece.White ? board.WhiteKingLocate : board.BlackKingLocate;
            Piece King = board.GetPiece(kingLocate);
            for(int y = 1; y <= Contant.Rows; y++)
            {
                for(int x = 1; x <= Contant.Cols; x++)
                {
                    if(piece.checkMoveto(new Point(x, y)))
                    {
                        Piece temp = board.GetPiece(new Point(x, y));
                        board.SetPiece(piece, new Point(x, y));
                        foreach(Piece Attacking in AttackingPiece)
                        {
                            if(!Attacking.checkMoveto(King.Point))
                                return true;
                        }
                        board.SetPiece(temp, new Point(x, y));
                    }
                }
            }
            return false;
        }
        public bool CheckMate(Board board, Piece king, List<Piece> AttackingPiece)
        {
            int[,] dangerous = king.White ? board.DangerousWhite : board.DangerousBlack;
            int[] delta = new[] { -1, 0, 1 };
            foreach (var item in dangerous) if (item == 0) return false;
            for (int y = 1; y <= Contant.Rows; y++)
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if(piece.White == king.White)
                    {
                        if(CanProtectKing(board, piece, AttackingPiece))
                            return false;
                    }
                    
                }
            return true;
        }
        public int Check(Board board, bool p1Turn,Piece enemy)
        {
            Point kingLocate = p1Turn ? board.WhiteKingLocate : board.BlackKingLocate;
            Piece King = board.GetPiece(kingLocate);
            List<Piece> AttackingPiece = AttackTo(board, kingLocate);
            if (AttackingPiece.Any())
            {
                board.PictureBoxes[kingLocate.Y, kingLocate.X].BackColor = Color.Red;
                if (CheckMate(board, King, AttackingPiece))
                {
                    ResetPossibleMove(board, enemy.Point);
                    MessageBox.Show("CheckMate","Notification!!!");
                    
                }
            }
            return 0;
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
        public void UpdateDangerousKingMove(Board board, bool white)
        {
            Point KingLocate = !white ? board.BlackKingLocate : board.WhiteKingLocate;
            Piece king = board.GetPiece(KingLocate);
            int[,] dangerous =  white ? board.DangerousWhite : board.DangerousBlack;
            int[] delta = new int[] { -1, 0, 1 };
            for(int i = 0; i < 3; i ++)
                for(int j = 0; j < 3; j++)
                {
                    Point point = new Point(KingLocate.X + delta[j], KingLocate.Y + delta[i]);
                    if (point.X >= 1 && point.X <= 8 && point .Y >= 1 && point.Y <= 8)
                    {
                        if(board.GetPiece(point).Digit == PieceDigits.Empty)
                        {
                            dangerous[i, j] = AttackTo(board, point).Count();
                        }
                        else if (board.GetPiece(point).White == king.White )
                            dangerous[i, j] = -1;
                       
                    }
                        
                }
                 
        }
     


    }
}
