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
    public partial class Promote : Form
    {
       
        Data data;
        public PieceDigits pieceDigits { get; set; }
        public Promote(bool white)
        {
            InitializeComponent();
            data = new Data(white, panel1);
            LoadForm();
        }
        public void LoadForm()
        {
            this.Width = Contant.PromoteWidth;
            this.Height = Contant.PromoteHeight;
            pieceDigits = new PieceDigits();
            pieceDigits = PieceDigits.Queen;
            btnPromote.Location = new System.Drawing.Point(Contant.PromoteWidth / 2 - btnPromote.Width / 2, Contant.Height + 100);
            panel1.Location = new System.Drawing.Point(Contant.PromoteWidth / 2 - panel1.Width * 2, 50);
            data.Click += Data_Click;
            btnPromote.Click += BtnPromote_Click;
        }

        private void BtnPromote_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Data_Click(object sender, EventArgs e)
        {
            PictureBox ptb = sender as PictureBox;
            PieceDigits digit = data.GetNumPicture(ptb);
            pieceDigits = digit;
            for(int i = 0; i < 4; i++)
            {
                if (data.GetPictureBox(i).BackColor == Color.LightPink)
                    data.GetPictureBox(i).BackColor = i % 2 == 0 ? Color.Gray : Color.White;
            }
            ptb.BackColor = Color.LightPink;
        }

      

        private void Promote_Resize(object sender, EventArgs e)
        {
            //Form form = sender as Form;
            //form.Height = height;
            //form.Width = width;
        }

        
    }
    public class Data
    {
        private bool white;
        private PictureBox[] pictureBoxes;
        private PieceDigits[] pieceDigits;
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
        public Data(bool White, Panel panel)
        {
            this.White = White;
            PictureBoxes = new PictureBox[4];
            PieceDigits = new PieceDigits[4];
            panel.Height = (int) (Contant.Height * 1.5);
            panel.Width = Contant.Width * 4;
            int startx = (Contant.PromoteWidth - Contant.Height * 4) / 2;
            for (int i = 0; i < 4; i++)
            {
                PictureBoxes[i] = new PictureBox();
                PictureBoxes[i].Width = Contant.Width;
                PictureBoxes[i].Height = Contant.Height;
                String path = @"Resources\";
                path += this.White == true ? "W" : "B";
                path += i == 0 ? "Queen" : i == 1 ? "Rook" : i == 2 ? "Bishop" : "Knight";
                path += ".png";
                PictureBoxes[i].Image = Image.FromFile(path);
                PictureBoxes[i].BackColor = i % 2 == 0 ? Color.Gray : Color.White;
                PictureBoxes[i].Location = new System.Drawing.Point(startx +Contant.Width * i, Contant.Height);
                panel.Controls.Add(PictureBoxes[i]);
                PieceDigits[i] = i == 0 ? ChessClone.PieceDigits.Queen : i == 1 ?
                    ChessClone.PieceDigits.Rook : i == 2 ? ChessClone.PieceDigits.Bishop : ChessClone.PieceDigits.Knight;
                PictureBoxes[i].Click += Data_Click;
            }


        }

        private void Data_Click(object sender, EventArgs e)
        {
            Onlick(sender);
        }

        public bool White { get => white; set => white = value; }
        public PictureBox[] PictureBoxes { get => pictureBoxes; set => pictureBoxes = value; }
        public PieceDigits[] PieceDigits { get => pieceDigits; set => pieceDigits = value; }

        public PieceDigits GetNumPicture(PictureBox ptb)
        {
            for (int i = 0; i < 4; i++)
                if (PictureBoxes[i] == ptb) return PieceDigits[i];
            return ChessClone.PieceDigits.Empty;
        }
        public int GetNum(PictureBox ptb)
        {
            for (int i = 0; i < 4; i++)
                if (PictureBoxes[i] == ptb) return i;
            return -1;
        }
        public PictureBox GetPictureBox(int i)
        {
            return PictureBoxes[i];
        }
    }
}

