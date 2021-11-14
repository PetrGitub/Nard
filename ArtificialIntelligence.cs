using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class ArtificialIntelligence
    {
        private Random nahodnyVyber = new Random();

        public ArtificialIntelligence()
        {

        }

        public int[] vyberNahodnyTah( List<int[]> seznamTahu )
        {
            //int nahodnyTah = nahodnyVyber.Next(seznamTahu.Length);
            //int vybranyTah = seznamTahu[nahodnyTah];
            //return vybranyTah;
            return seznamTahu[nahodnyVyber.Next(seznamTahu.Count)];
        }
    }
}
