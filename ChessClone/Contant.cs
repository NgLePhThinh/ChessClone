using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClone
{
    public static class Contant
    {
        private static readonly int width = 60;
        private static readonly int height = 60;
        private static readonly int rows = 8;
        private static readonly int cols = 8;
        private static readonly int promoteWidth = 400;
        private static readonly int promoteHeight = 300;

        public static int Rows => rows;
        public static int Cols => cols;
        public static int Width => width;
        public static int Height => height;
        public static int PromoteWidth => promoteWidth;
        public static int PromoteHeight => promoteHeight;
    }
}
