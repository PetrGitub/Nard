using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class Rules
    {
        public Rules()
        {
           
        }

        // Naplneni matice 
        public void NaplnBoard(Board deska)
        {
            for (int i = 0; i < deska.hracideska.GetLength(0); i++)       // this ....... ukazuje na konkretni hraciDesku, protoze jich muze existovat vic
            {
                for (int j = 0; j < deska.hracideska.GetLength(1); j++)
                {
                    if (i <= 1)
                    {
                        deska.hracideska[i, j] = 1;
                    }
                    else if (i >= 5)
                    {
                        deska.hracideska[i, j] = 2;
                    }
                    else
                    {
                        deska.hracideska[i, j] = 0;
                    }
                }
            }
        }
    }
}
