using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public class Player
    {
        private bool isWhite;
        private bool isTurn;
        public Player(bool isWhite, bool isTurn)
         {
            this.IsWhite = isWhite;
            this.IsTurn = isTurn;
        }
       public bool IsWhite { get => isWhite; set => isWhite = value; }
        public bool IsTurn { get => isTurn; set => isTurn = value; }
    }
}
