using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ChessClone
{
    public class Board
    {
        private Piece[,] pieces;
        private PictureBox[,] pictureBoxes;
        private Color whitecolor, blackcolor;
        private Point whiteKingLocate, blackKingLocate;
        private int[,] dangerousWhite, dangerousBlack;



        #region property
        public Piece[,] Pieces { get => pieces; set => pieces = value; }
        public Color Whitecolor { get => whitecolor; set => whitecolor = value; }
        public Color Blackcolor { get => blackcolor; set => blackcolor = value; }
        public Point WhiteKingLocate { get => whiteKingLocate; set => whiteKingLocate = value; }
        public Point BlackKingLocate { get => blackKingLocate; set => blackKingLocate = value; }
        public PictureBox[,] PictureBoxes { get => pictureBoxes; set => pictureBoxes = value; }
        public int[,] DangerousWhite { get => dangerousWhite; set => dangerousWhite = value; }
        public int[,] DangerousBlack { get => dangerousBlack; set => dangerousBlack = value; }


        #endregion
        public Board()
        {
            Pieces = new Piece[9, 9];
            PictureBoxes = new PictureBox[9, 9];
            Whitecolor = Color.White;
            Blackcolor = Color.Gray;
            WhiteKingLocate = new Point(5, 1);
            BlackKingLocate = new Point(5, 8);
            DangerousWhite = new int[3, 3] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
            DangerousBlack = new int[3, 3] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };

        }
        #region click event
        private event EventHandler click;
        public event EventHandler Click
        {
            add
            {
                click += value;
            }
            remove
            {
                click -= value;
            }
        }
        public void Onlick(object sender)
        {
            if (click != null)
                click(sender, new EventArgs());
        }
        #endregion

        public void resetBoard(Panel pnl)
        {
            for (int y = 3; y <= 6; y++)
            {
                for (int x = 1; x <= 8; x++)
                {
                    this.SetPiece(new Empty(true, x, y, PieceDigits.Empty, this), new Point(x, y));
                }
            }
            for (int x = 1; x <= 8; x++)
            {
                this.SetPiece(new Pawn(true, x, 2, this), new Point(x, 2));
                this.SetPiece(new Pawn(false, x, 7, this), new Point(x, 7));
            }
            //WhitePieces
            this.SetPiece(new King(true, 5, 1, this), new Point(5, 1));
            this.SetPiece(new Queen(true, 4, 1, this), new Point(4, 1));
            this.SetPiece(new Bishop(true, 3, 1, this), new Point(3, 1));
            this.SetPiece(new Bishop(true, 6, 1, this), new Point(6, 1));
            this.SetPiece(new Knight(true, 2, 1, this), new Point(2, 1));
            this.SetPiece(new Knight(true, 7, 1, this), new Point(7, 1));
            this.SetPiece(new Rook(true, 1, 1, this), new Point(1, 1));
            this.SetPiece(new Rook(true, 8, 1, this), new Point(8, 1));


            //BlackPieces
            this.SetPiece(new King(false, 5, 8, this), new Point(5, 8));
            this.SetPiece(new Queen(false, 4, 8, this), new Point(4, 8));
            this.SetPiece(new Bishop(false, 3, 8, this), new Point(3, 8));
            this.SetPiece(new Bishop(false, 6, 8, this), new Point(6, 8));
            this.SetPiece(new Knight(false, 2, 8, this), new Point(2, 8));
            this.SetPiece(new Knight(false, 7, 8, this), new Point(7, 8));
            this.SetPiece(new Rook(false, 1, 8, this), new Point(1, 8));
            this.SetPiece(new Rook(false, 8, 8, this), new Point(8, 8));

            pnl.Width = Contant.Width * Contant.Cols;
            pnl.Height = Contant.Height * Contant.Rows;
            Boolean iswhite = true;
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    PictureBox ptb = new PictureBox();
                    ptb.Width = Contant.Width;
                    ptb.Height = Contant.Height;
                    Color color = iswhite ? this.Whitecolor : this.Blackcolor;
                    ptb.BackColor = color;
                    ptb.Location = new System.Drawing.Point(Contant.Width * (x - 1), Contant.Height * (Contant.Rows - y));
                    ptb.Click += Ptb_Click;
                    pnl.Controls.Add(ptb);
                    PictureBoxes[y, x] = ptb;
                    //PictureBoxes[y, x].Enabled = false;
                    iswhite = !iswhite;

                }
                iswhite = !iswhite;
            }
            insertChessImage();
        }
        /// <summary>
        /// Chèn ảnh quân cờ 
        /// </summary>
        public void insertChessImage()
        {
            for (int y = Contant.Rows; y >= 1; y--)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    if (Pieces[y, x].Digit == PieceDigits.Empty)
                        PictureBoxes[y, x].Image = null;
                    else
                    {
                        String path = @"Resources\";
                        path += Pieces[y, x].White == true ? "W" : "B";
                        path += Pieces[y, x].Digit;
                        path += ".png";
                        PictureBoxes[y, x].Image = Image.FromFile(path);


                    }
                }
            }

        }
        private void Ptb_Click(object sender, EventArgs e)
        {
            Onlick(sender);
        }
        public bool checkPoint(Point x)
        {
            return false;
        }
        public Piece GetPiece(Point x)
        {
            return Pieces[x.Y, x.X];
        }
        public void SetPiece(Piece piece, Point x)
        {
            Pieces[x.Y, x.X] = piece;
            Pieces[x.Y, x.X].Point = x;
        }
        public void SetEmptyPiece(Point x, Board board)
        {
            board.Pieces[x.Y, x.X] = new Empty(true, x.X, x.Y, PieceDigits.Empty, board);
            board.Pieces[x.Y, x.X].Point = x;
        }
        /// <summary>
        /// Kiểm tra trạng thái ô cờ
        /// </summary>
        /// <param name="x">Tọa độ ô cờ</param>
        /// <param name="digits">Trạng thái ô cờ</param>
        /// <returns>true: cùng trạng thái, false: Khác trạng thái</returns>
        public bool checkPiece(Point x, PieceDigits digits)
        {
            Piece piece = GetPiece(x);
            return piece.Digit == digits;
        }
        /// <summary>
        /// Kiểm tra màu của quân cờ có phải quân địch không
        /// </summary>
        /// <param name="x">Tọa độ ô cờ</param>
        /// <param name="white">Màu cờ</param>
        /// <returns>true: là quân địch; false: không là quân địch</returns>
        public bool checkPiece(Point x, bool white)
        {
            Piece piece = GetPiece(x);
            if (PieceDigits.Empty != piece.Digit)
                return piece.White != white;
            return false;
        }
        public bool checkPiece(Point x, bool white, PieceDigits digits)
        {
            return checkPiece(x, white) || checkPiece(x, digits);
        }
        /// <summary>
        /// Lấy tọa độ của Picturebox
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <returns>Tọa độ PictureBox ,(-1,-1) khi tọa độ không hợp lệ</returns>
        public Point GetPicturePoint(PictureBox pictureBox)
        {
            for (int y = 1; y <= Contant.Rows; y++)
            {
                for (int x = 1; x <= Contant.Cols; x++)
                {
                    if (this.PictureBoxes[y, x] == pictureBox)
                        return new Point(x, y);
                }
            }
            return new Point(-1, -1);
        }
        public PictureBox GetCellAt(Point x)
        {
            return this.PictureBoxes[x.Y, x.X];
        }
        public bool IsSafeMove(Point x)
        {
            GamePlay gamePlay = new GamePlay();
            Piece piece = this.GetPiece(x);
            Point kingLocate = this.GetPiece(x).White ? this.WhiteKingLocate : this.BlackKingLocate;
            this.SetEmptyPiece(x, this);
            bool isSafe = gamePlay.AttackTo(this, kingLocate).Any() ? false : true;
            this.SetPiece(piece, x);
            return piece.Digit == PieceDigits.King ? true : isSafe;
        }

    }
}
