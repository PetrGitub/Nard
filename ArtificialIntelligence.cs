using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class ArtificialIntelligence
    {
        private Board NardAI;
        private int HloubkaAI;
        private int HracNaTahuAI;
        private Random nahodnyVyber = new Random();

        public ArtificialIntelligence(Board deska, int hloubka, int kdoJeNaTahu)
        {
            NardAI = deska;
            HloubkaAI = hloubka;
            HracNaTahuAI = kdoJeNaTahu;
        }

        public int[] VyberNahodnyTah( List<int[]> seznamTahu )
        {
            //int nahodnyTah = nahodnyVyber.Next(seznamTahu.Length);
            //int vybranyTah = seznamTahu[nahodnyTah];
            //return vybranyTah;
            return seznamTahu[nahodnyVyber.Next(seznamTahu.Count)];
        }
    }
}
