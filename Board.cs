using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class Board
    {
        private int m = 7, n = 8;
        public int[,] hracideska = new int[7, 8];
        public Board()
        {
            NaplnBoard();
        }

        // Naplneni matice 
        public void NaplnBoard()
        {
            for (int i = 0; i < this.hracideska.GetLength(0); i++)
            {
                for (int j = 0; j < this.hracideska.GetLength(1); j++)
                {
                    if (i <= 1)
                    {
                        this.hracideska[i, j] = 1;
                    }
                    else if (i >= 5)
                    {
                        this.hracideska[i, j] = 2;
                    }
                    else
                    {
                        this.hracideska[i, j] = 0;
                    }
                }
            }
        }
    }
}
