﻿using System;
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
           
        }

        public void VykonejTah(int[] nejakyPohyb)
        {
            this.hracideska[nejakyPohyb[3], nejakyPohyb[4]] = nejakyPohyb[5];   // na policko KAMtahnu vlozim info jakou figurkou {1  v   2  v  0}
            this.hracideska[nejakyPohyb[0], nejakyPohyb[1]] = 0;                // tam odkud jsem odesel se vlozi 0 > tzn., neni tam zadna figurka
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
