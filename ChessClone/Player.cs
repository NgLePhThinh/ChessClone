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
      
        public Player(bool isWhite, bool isTurn)
         {
            this.IsWhite = isWhite;
        }
       public bool IsWhite { get => isWhite; set => isWhite = value; }
    }
}
