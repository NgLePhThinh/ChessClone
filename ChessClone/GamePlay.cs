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
            int dx = Math.Abs(startPiece.Point.X - endPiece.Point.X);
            int dy = Math.Abs(startPiece.Point.Y - endPiece.Point.Y);
            PictureBox ptbStart = map.GetCellAt(startPiece.Point);
            PictureBox ptbEnd = map.GetCellAt(endPiece.Point);

            #region Castling
            if (startPiece.Digit == PieceDigits.King)
            {
                if (start.Y - end.Y == 0 && Math.Abs(start.X - end.X) == 2)
                {
                    King king = new King(startPiece.White, startPiece.Point.X, startPiece.Point.Y, map);
                    if (king.checkCastling(end))
                    {
                        Piece rook = endPiece.Point.X < startPiece.Point.X ?
                            map.GetPiece(new Point(1, king.Point.Y)) : map.GetPiece(new Point(8, king.Point.Y));
                        PictureBox ptbRook = map.GetCellAt(rook.Point);
                        Piece empty = endPiece.Point.X < startPiece.Point.X ?
                            map.GetPiece(new Point(4, king.Point.Y)) : map.GetPiece(new Point(6, king.Point.Y));
                        PictureBox ptbempty = map.GetCellAt(empty.Point);
                        MoveChessPiece(rook.Point, empty.Point, map);
                    }
                }
            }
            #endregion

            #region En passant
            if (startPiece.Digit == PieceDigits.Pawn && endPiece.Digit == PieceDigits.Empty && dx * dy == 1)
            {
                Piece temp = startPiece.White ?
                    map.GetPiece(new Point(endPiece.Point.X, endPiece.Point.Y - 1)) : map.GetPiece(new Point(endPiece.Point.X, endPiece.Point.Y + 1));
                PictureBox ptbTemp = map.GetCellAt(temp.Point);
                ptbTemp.Image = null;
                map.SetEmptyPiece(temp.Point);
            }
            #endregion

            UpdateChessImage(ptbStart, ptbEnd);
            map.SetPiece(startPiece, end);
            map.SetEmptyPiece(start);
        }
        private void UpdateChessImage(PictureBox ptbStart, PictureBox ptbEnd)
        {
            ptbEnd.Image = ptbStart.Image;
            ptbStart.Image = null;
        }

        public void CancelChoose(Point x, Board board)
        {
            Color color = Math.Abs(x.X - x.Y) % 2 != 0 ? board.Whitecolor : board.Blackcolor;
            board.GetCellAt(x).BackColor = color;
        }
        public void PossibleMove(Point point, Board board)
        {
            Piece piece = board.GetPiece(point);
            Point kingLocacte = piece.White ? board.WhiteKingLocate : board.BlackKingLocate;
            var Attacking = AttackTo(board, kingLocacte);
            if (!board.IsSafeMove(point)) return;
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Point goalPoint = new Point(x, y);
                    Piece temp = board.GetPiece(goalPoint);
                    if (Attacking.Any() && piece.Digit != PieceDigits.King)
                    {
                        if (piece.checkMoveto(goalPoint))
                        {
                            int cnt = 0;
                            Point piecePoint = piece.Point;
                            board.SetPiece(piece,goalPoint);
                            board.SetEmptyPiece(piecePoint);
                            foreach(Piece attacking in Attacking)
                            {
                                if ((attacking.Point.X == x && attacking.Point.Y == y) ||!attacking.checkMoveto(kingLocacte))
                                {
                                    cnt++;   
                                }                                
                            }
                            board.SetPiece(piece, piecePoint);
                            board.SetPiece(temp, goalPoint);
                            if (cnt == Attacking.Count)
                            {
                                if (GetCellType(new Point(x, y), board) == 0)
                                {
                                    board.GetCellAt(goalPoint).Image = Image.FromFile(@"Resources\PossibleMove.png");
                                }
                                else board.GetCellAt(goalPoint).BackColor = Color.LightPink;
                            }
                        }
                    }
                    else
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

        public List<Piece> AttackToEmpty(Board board, Point point, bool White)
        {
            List<Piece> AttackingPiece = new List<Piece>();
            Piece attcked = board.GetPiece(point);
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if (piece.White != White && piece.Digit != PieceDigits.Empty && piece.checkMoveto(point))
                    {
                        AttackingPiece.Add(piece);
                    }
                }
            }
            return AttackingPiece;
        }

        /// <summary>
        /// Danh sách các quân cờ tấn công vào ô cờ cho trước
        /// </summary>
        /// <param name="board"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<Piece> AttackTo(Board board, Point point)
        {
            List<Piece> AttackingPiece = new List<Piece>();
            //Point kingLocate = whiteTurn ? Board.WhiteKingLocate : Board.BlackKingLocate;

            Piece attacked = board.GetPiece(point);
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if (board.checkPiece(new Point(x, y), attacked.White,attacked.Digit) && piece.checkMoveto(point))
                    {
                        AttackingPiece.Add(piece);
                    }

                }
            }
            return AttackingPiece;
        }

        /// <summary>
        /// Kiểm tra các quân cờ có thể bảo vệ Vua của mình hay không
        /// </summary>
        /// <param name="board"></param>
        /// <param name="piece"></param>
        /// <param name="AttackingPiece"></param>
        /// <returns></returns>
        public bool CanProtectKing(Board board, Piece piece, List<Piece> AttackingPiece)
        {
            
            Point kingLocate = piece.White ? board.WhiteKingLocate : board.BlackKingLocate;
            Piece King = board.GetPiece(kingLocate);
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    if (piece.checkMoveto(new Point(x, y)))
                    {
                        Point piecePoint = piece.Point;
                        Piece temp = board.GetPiece(new Point(x, y));
                        board.SetPiece(piece, new Point(x, y));
                        board.SetEmptyPiece(piecePoint);
                        foreach (Piece Attacking in AttackingPiece)
                        {
                            if (!Attacking.checkMoveto(King.Point))
                            {
                                board.SetPiece(piece, piecePoint);
                                board.SetPiece(temp, new Point(x, y));
                                return true;
                            }     
                        }

                        board.SetPiece(piece, piecePoint);
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
            Point KingLocate = king.Point;            
            for (int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 3; x++)
                {
                    if (dangerous[y,x] == 0)
                    {
                        Point point = new Point(KingLocate.X + delta[x], KingLocate.Y + delta[y]);
                        Piece piece = board.GetPiece(point);
                        Point kingPoint = king.Point;
                        board.SetPiece(king, point);
                        board.SetEmptyPiece(kingPoint);
                        foreach(Piece piece2 in AttackingPiece)
                        {
                            if (piece2.checkMoveto(kingPoint))
                            {
                                board.SetPiece(king, kingPoint);
                                board.SetPiece(piece, point);
                                return true;
                            }
                        }
                        board.SetPiece(king, kingPoint);
                        board.SetPiece(piece, point);

                    }
                }
            }
           

            for (int y = 1; y <= Contant.Rows; y++)
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    Piece piece = board.GetPiece(new Point(x, y));
                    if (piece.White == king.White && piece.Digit != PieceDigits.Empty)
                    {
                        if (CanProtectKing(board, piece, AttackingPiece))
                            return false;
                    }

                }
            
            return true;
        }

        /// <summary>
        /// Kiem tra chieu va chieu het
        /// </summary>
        /// <param name="board"></param>
        /// <param name="p1Turn"></param>
        /// <param name="enemy"></param>
        /// <returns>true: Chieu het</returns>
        ///
        public bool Check(Board board, bool p1Turn, Piece enemy)
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
                    MessageBox.Show("CheckMate", "Notification!!!");
                    return true;

                }
            }
            return false;
        }
        public void ResetCheck(Board board, bool p1Turn)
        {
            Point kingLocate = p1Turn ? board.WhiteKingLocate : board.BlackKingLocate;
            Piece King = board.GetPiece(kingLocate);
            List<Piece> AttackingPiece = AttackTo(board, kingLocate);
            if (!AttackingPiece.Any())
            {
                CancelChoose(kingLocate,board);
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
        public void UpdateDangerousKingMove(Board board, bool white)
        {
            Point KingLocate = !white ? board.BlackKingLocate : board.WhiteKingLocate;
            Piece king = board.GetPiece(KingLocate);
            int[,] dangerous1 = white ? board.DangerousWhite : board.DangerousBlack;
            int[] delta = new int[] { -1, 0, 1 };
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    Point point = new Point(KingLocate.X + delta[j], KingLocate.Y + delta[i]);
                    if (point.X >= 1 && point.X <= 8 && point.Y >= 1 && point.Y <= 8)
                    {
                        if (board.GetPiece(point).Digit == PieceDigits.Empty)
                            dangerous1[i, j] = AttackToEmpty(board, point, king.White).Count();
                        else if (board.GetPiece(point).White == king.White)
                            dangerous1[i, j] = -1;
                        Piece  piece = board.GetPiece(point);
                        if(piece.Digit != PieceDigits.Empty && piece.White != king.White)
                        {
                            if (piece.checkMoveto(king.Point))
                            {
                                Point kingPoint = king.Point;
                                board.SetPiece(king, point);
                                board.SetEmptyPiece(kingPoint);
                                int cnt = AttackTo(board, point).Count;
                                dangerous1[i, j] = cnt != 0 ? cnt : 0;
                                board.SetPiece(king, kingPoint);
                                board.SetPiece(piece, point);
                            }
                            else dangerous1[i, j] = 0;

                        }

                    }

                }
        }
        public void DeleteFirstMove(Board board, Piece piece)
        {
            if (piece.Digit == PieceDigits.Rook || piece.Digit == PieceDigits.King)
                piece.deleteFirstMove();
            else
            {
                for (int y = 1; y <= Contant.Rows; y++)
                {
                    for (int x = 1; x <= Contant.Cols; x++)
                    {
                        Piece temp = board.GetPiece(new Point(x, y));
                        if (temp.White != piece.White && temp.Digit == PieceDigits.Pawn)
                            temp.deleteFirstMove();
                    }
                }
            }

        }
        /// <summary>
        /// Cập nhật nước đi đầu tiên của quân tốt
        /// </summary>
        /// <param name="piece"></param>
        public void UpdateFirstMove(Board board, Piece startPiece, Point end)
        {
            Piece endPiece = board.GetPiece(end);
            if (startPiece.Digit == PieceDigits.Pawn && Math.Abs(startPiece.Point.Y - endPiece.Point.Y) == 2)
                startPiece.updateFirstMove();
        }
        public Piece Promote(Board board, Piece piece, PieceDigits digits)
        {
            String path = @"Resources\";
            path += piece.White == true ? "W" : "B";
            path += digits;
            path += ".png";
            board.PictureBoxes[piece.Point.Y, piece.Point.X].Image = Image.FromFile(path);
            switch (digits)
            { 
                case PieceDigits.Rook:
                    piece = new Rook(piece.White, piece.Point.X, piece.Point.Y, board);
                    break;
                case PieceDigits.Knight:
                    piece = new Knight(piece.White, piece.Point.X, piece.Point.Y, board);
                    break;
                case PieceDigits.Bishop:
                    piece = new Bishop(piece.White, piece.Point.X, piece.Point.Y, board);
                    break;
                case PieceDigits.Queen:
                    piece = new Queen(piece.White, piece.Point.X, piece.Point.Y, board);
                    break;
                default:
                    break;
            }
            board.SetPiece(piece, piece.Point);
            return piece;
        }


    }
}
