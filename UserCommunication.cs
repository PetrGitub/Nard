using System;
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
        public void VypisBoard(Board currentNard)
        {
            Console.Write("\n    A B C D E F G H\n");

            for (int x = currentNard.hracideska.GetLength(0) - 1; x >= 0; x--)
            {
                Console.Write("{0,3} ", (x + 1).ToString());
                for (int y = 0; y < currentNard.hracideska.GetLength(1); y++)
                {
                    Console.Write("{0} ", IntToCharacter(currentNard.hracideska[x, y]));
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

        public int[][] Volba(bool tahneBily)
        {
            if(tahneBily)
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
            int a = 1;

            while (chyba > -1)
            {
                chyba = -1;
                //while (a > 1)
                //{
                    //a = 1;
               //}
                string zadani = Console.ReadLine();

                if(zadani.Length == 2)
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
    }
}
