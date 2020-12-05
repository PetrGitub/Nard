using System;
using System.Collections.Generic;
using System.Text;

namespace Nard
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
