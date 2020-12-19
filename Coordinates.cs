using System;
using System.Collections.Generic;
using System.Text;

namespace Nard
{
    class Coordinates
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Coordinates(int x, int y)
        {
            RowNumber = x;
            ColumnNumber = y;
        } // zmena coords
    }
}
