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
        private bool p1Turn;

        public Form1()
        {
            InitializeComponent();
            LoadForm();

        }
        public void LoadForm()
        {
            gamePlay = new GamePlay();
            p1Turn = true;
            startPoint = new Point(-1, -1);
            map = new Board();
            map.resetBoard(panel1);
            map.Click += Map_Click;
            p1 = new Player(true,true);
            p2 = new Player(false,false);
        }

        private void Map_Click(object sender, EventArgs e)
        {
            var point = map.GetPicturePoint(sender as PictureBox);
            if (gamePlay.ImproperPoint(point))
            {
                var typeNumber = gamePlay.GetCellType(point, map);
                if (startPoint.X == -1 && typeNumber != 0)
                {
                    //Nước chọn quân hợp lệ
                    (sender as PictureBox).BackColor = Color.LightPink;
                    startPoint = point;
                    gamePlay.PossibleMove(point, map); 

                }
                else if (startPoint.X != -1 && startPoint.Equals(point) == false)
                {

                    if (typeNumber * gamePlay.GetCellType(startPoint, map) <= 0)
                    {
                        //Nước đi hợp lệ
                        Piece pieceStart = map.Pieces[startPoint.Y, startPoint.X];
                        if (pieceStart.checkMoveto(point))
                        {
                            if(pieceStart.Digit == PieceDigits.King)
                            {
                                if (pieceStart.White)
                                    map.WhiteKingLocate = point;
                                else
                                    map.BlackKingLocate = point;
                            }
                            gamePlay.MoveChessPiece(startPoint, point, map);
                            gamePlay.CancelChoose(startPoint, map);
                            p1Turn = !p1Turn;
                            gamePlay.Checked(map, p1Turn);
                            startPoint = new Point(-1, -1);

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
}
