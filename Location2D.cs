using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeProgram
{
    public class Location2D
    {
        private int row;
        private int col;
        public Location2D(int r = 0, int c = 0 )
        {
            row = r;
            col = c;
        }
        public int Row { get; set; }
        public int Col { get; set; }
        

    }
}
