﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class UserCommunication
    {
        public UserCommunication()
        {

        }

        public void ConsoleClear()
        {
            Console.Clear();
        }

        // Vypsani matice
        public void VypisBoard(Board favouriteNard)
        {
            Console.Write("\n    A B C D E F G H\n");

            for (int x = favouriteNard.hracideska.GetLength(0) - 1; x >= 0; x--)
            {
                Console.Write("{0,3} ", (x + 1).ToString());
                for (int y = 0; y < favouriteNard.hracideska.GetLength(1); y++)
                {
                    Console.Write("{0} ", IntToCharacter(favouriteNard.hracideska[x, y]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Převod na string
        public string IntToCharacter(int value)
        {
            switch (value)
            {
                case 1:
                    return "x";
                case 2:
                    return "o";
                default:
                    return " ";
            }
        }

        // Kdo je na tahu a zadavani souradnic   .......   texty (uvnitr kódu řeším konkrétní tahy - TadejTah)
        public int[][] Volba(bool tahneBily)        // argument
        {
            if (tahneBily)  // podmínka
            {
                Console.WriteLine("Bily na tahu");
            }
            else
            {
                Console.WriteLine("Cerny na tahu");
            }

            int[][] Vlozeno = new int[2][];

            Console.Write("   Zadej souradnice pole ODKUD chces tahout: ");
            Vlozeno[0] = this.ZadejTah();                                           // souradnice tahu ODKUD jsou ve Vlozeno[0]

            Console.Write("   Zadej souradnice pole KAM chces tahout: ");
            Vlozeno[1] = this.ZadejTah();                                           // souradnice tahu KAM jsou ve Vlozeno[1]

            return Vlozeno;
        }

        // Zadej tah
        public int[] ZadejTah()
        {
            int chyba = 0;
            int b;
            int[] tah = new int[2];

            while (chyba > -1)
            {
                chyba = -1;
                
                string zadani = Console.ReadLine();

                if (zadani.Length == 2)
                {
                    zadani = zadani.ToLower();          // z VELKYCH - male

                    for (int i = 1; i >= 0; i--)
                    {
                        b = (int)zadani[i];
                        if (!(b < 49 || (55 < b && b < 97) || b > 104 || (zadani[0] > 48 && zadani[0] < 56)
                            || (!((zadani[1] > 48 && zadani[1] < 56)))))
                        {
                            if (b >= 49 && b <= 55)
                            {
                                tah[1 - i] = b - 49;
                            }
                            //else if (b >= 65 && b <= 72)
                            //{
                            //tah[1 - i] = b - 65;
                            //}
                            else if (b >= 97 && b <= 104)
                            {
                                tah[1 - i] = b - 97;
                            }
                        }
                        else
                        {
                            chyba += 1;
                            Console.WriteLine("V zadani tahu se objevila nespravna kombinace. Opakujte zadani.\n");
                        }
                    }
                }
                else
                {
                    chyba += 1;
                    Console.WriteLine("V zadani tahu se objevila nespravna kombinace. Opakujte zadani.\n");
                }
            }
            Console.WriteLine();
            return tah;
        }

        public void VypisZpravu(string anyMessage)
        {
            Console.WriteLine(anyMessage);
        }
    }
}
