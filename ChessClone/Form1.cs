using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessClone
{
    public partial class Form1 : Form
    {
        private Player p1, p2;
        private Board map;
        private GamePlay gamePlay;
        private Point startPoint;
        private bool whiteTurn;

        public Form1()
        {
            InitializeComponent();
            LoadForm();

        }
        public void LoadForm()
        {
            gamePlay = new GamePlay();
            whiteTurn = true;
            startPoint = new Point(-1, -1);
            map = new Board();
            map.resetBoard(panel1);
            map.Click += Map_Click;
            p1 = new Player(true, true);
            p2 = new Player(false, false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_Click(object sender, EventArgs e)
        {

            var point = map.GetPicturePoint(sender as PictureBox);

            var typeNumber = gamePlay.GetCellType(point, map);
            if (startPoint.X == -1 && typeNumber != 0 && map.GetPiece(point).White == whiteTurn) 
            {
                //Nước chọn quân hợp lệ
                (sender as PictureBox).BackColor = Color.LightPink;
                startPoint = point;
                gamePlay.PossibleMove(point, map);

            }
            else if (startPoint.X != -1 && startPoint.Equals(point) == false )
            {
                if (typeNumber * gamePlay.GetCellType(startPoint, map) <= 0)
                {
                    //Nước đi hợp lệ
                    Piece pieceStart = map.Pieces[startPoint.Y, startPoint.X];
                    if (pieceStart.checkMoveto(point) && map.IsSafeMove(startPoint))
                    {
                        if (pieceStart.Digit == PieceDigits.King)
                        {
                            if (pieceStart.White)
                                map.WhiteKingLocate = point;
                            else
                                map.BlackKingLocate = point;
                        }
                        pieceStart.deleteFirstMove();
                        gamePlay.MoveChessPiece(startPoint, point, map);
                        gamePlay.CancelChoose(startPoint, map);
                        whiteTurn = !whiteTurn;
                        startPoint = new Point(-1, -1);
                        gamePlay.UpdateDangerousKingMove(map, whiteTurn);
                        gamePlay.Check(map, whiteTurn,pieceStart);
                    }
                }
                else if (typeNumber != 0)
                {
                    //Nước đi không hợp lệ
                    gamePlay.CancelChoose(startPoint, map);
                    startPoint = new Point(-1, -1);

                }
                gamePlay.ResetPossibleMove(map, point);

            }
        }
    }
}
