using System;
using System.Collections.Generic;
using System.Text;

namespace Nard
{
    class Board
    {
        private int m = 7, n = 8;
        private int[,] hracideska = new int[7, 8];
        public Board()
        {
            NaplnBoard();
            //ZadejTah();
            //ProvedTah();
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

        // Vypsani matice
        public void VypisBoard()
        {
            Console.Write("\n    A B C D E F G H\n");
            //for (int y = 1; y <= Board.GetLength(1); y++)
            //{
            //    Console.Write("{0} ", y.ToString());
            //}
            //Console.WriteLine();

            for (int x = this.hracideska.GetLength(0) - 1; x >= 0; x--)
            {
                Console.Write("{0,3} ", (x + 1).ToString());
                for (int y = 0; y < this.hracideska.GetLength(1); y++)
                {
                    Console.Write("{0} ", IntToCharacter(this.hracideska[x, y]));
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

                while (a > 1)
                {
                    Console.WriteLine("V zadani tahu se objevila nespravna kombinace. Opakujte zadani.\n");
                    a = 1;
                }

                Console.WriteLine("Zadej tah ve tvaru: A1 ");
                string zadani = Console.ReadLine();

                for (int i = 1; i >= 0; i--)
                {

                    b = (int)zadani[i];
                    if (!(b < 49 || (55 < b && b < 65) || (72 < b && b < 97) || b > 104 || (zadani[0] > 48 && zadani[0] < 56)
                        || (!((zadani[1] > 48 && zadani[1] < 56) && zadani.Length == 2))))
                    {
                        if (b >= 49 && b <= 55)
                        {
                            tah[1 - i] = b - 49;

                        }
                        else if (b >= 65 && b <= 72)
                        {
                            tah[1 - i] = b - 65;
                        }
                        else if (b >= 97 && b <= 104)
                        {
                            tah[1 - i] = b - 97;
                        }
                    }
                    else
                    {
                        chyba += 1;
                        a += 1;
                    }
                }
            }
            Console.WriteLine();
            return tah;
        }

        public void ProvedTah()
        {
            int[] Odtud;
            Odtud = ZadejTah();

            int[] Sem;
            Sem = ZadejTah();

            this.hracideska[Sem[0], Sem[1]] = this.hracideska[Odtud[0], Odtud[1]];
            this.hracideska[Odtud[0], Odtud[1]] = 0;
        }
    }
}
