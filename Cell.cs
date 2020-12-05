using System;
using System.Collections.Generic;
using System.Text;

namespace _07_B___poleCELL_piece_STRING
{
    class Cell
    {
        public Coordinates Coord { get; set; }
        public Piece pcs { get; set; }

        public Cell(Coordinates coord)
        {
            this.Coord = coord;
            this.pcs = null;
        }
    }
}
