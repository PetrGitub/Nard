using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class Board
    {
        //private int m = 7, n = 8;
        public int[,] hracideska = new int[7, 8];

        public Board()
        {
           
        }

        public void VykonejTah(int[] nejakyPohyb)
        {
            for (int index = 0; index < nejakyPohyb.Length; index += 4)     // smyčka, která při každém průchodu zpracuje 4 prvky, provede změnu na desce a pak se posune o 4 prvky dál. Takhle projede celé pole, ať má délku 8, nebo 12 prvků...
            {                                                               // |A|1|1|0| - na souřadnici A1 je figurka 1 a to se změní na 0 (figurka zmizí)     |A|2|0|1| - na souřadnici A2 není nic (0) a objejví se tam figurka 1
                int x = nejakyPohyb[ index + 0 ];
                int y = nejakyPohyb[ index + 1 ];
                int novaHodnotaFigurky = nejakyPohyb[ index + 3 ];
                this.hracideska[ x, y ] = novaHodnotaFigurky;
            }

            //this.hracideska[nejakyPohyb[3], nejakyPohyb[4]] = nejakyPohyb[5];   // na policko KAMtahnu vlozim info jakou figurkou {1  v  -1  v  0}
            //this.hracideska[nejakyPohyb[0], nejakyPohyb[1]] = 0;                // tam odkud jsem odesel se vlozi 0 => tzn., neni tam zadna figurka
        }

        public int GetValue(int coordX, int coordY)                 // metoda vrátí hodnotu (figurku: 1  v  -1  v  0), která stojí na daných souřadnicích
        {
               return hracideska[coordX, coordY];
        }
         
        public void SetValue(int coordX, int coordY, int value)     // metoda vloží hodnotu (figurku: 1  v  -1  v  0), na dané souřadnice
        {
            hracideska[coordX, coordY] = value;
        }
    }
}
