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
                case -1:
                    return "o";
                default:
                    return " ";
            }
        }

        public enum Command                 // výčtový Typ
        {
            Move,
            Help,
            GeneralHelp,
            Undo,
            Redo,
            Load,
            Save
        }

        // Kdo je na tahu a zadavani souradnic   .......   texty (uvnitr kódu řeším konkrétní tahy - ZadejTah)
        public Command Volba(bool tahneBily, out int[] Vlozeno_coords)        // argumenty     // pole intu 'Vlozeno_coords' = VYSTUPNI(= out) PARAMETR (tzn., vlozi se do nej souřadnice tahů)
        {
            if (tahneBily)  // podmínka
            {
                Console.WriteLine("Bily na tahu");
            }
            else
            {
                Console.WriteLine("Cerny na tahu");
            }


            int[] TahOdkud;                 // pole kam se zadaji souradnice ODKUD jde TAH
            int[] TahKam;                   // pole kam se zadaji souradnice KAM jde TAH
            Console.Write("   Zadej souradnice pole ODKUD chces tahout: ");
            TahOdkud = this.ZadejTah();                                           // souradnice tahu ODKUD jsou v TahOdkud
            Console.Write("   Zadej souradnice pole KAM chces tahout: ");
            TahKam = this.ZadejTah();                                           // souradnice tahu KAM jsou v TahKam

            Vlozeno_coords = null;
            if(TahOdkud[0] == 91 || TahKam[0] == 91)
            {
                return Command.GeneralHelp;
            }

            Vlozeno_coords = new int[] { TahOdkud[0], TahOdkud[1], TahKam[0], TahKam[1] };       // souradnice tahů ODKUD a KAM se vloží do "Vlozeno_coords

            return Command.Move;            // navratova hodnota je -----> Command
        }

        // Zadej tah
        public int[] ZadejTah()
        {
            int chyba = 0;
            int b;
            int[] tah_hrace = new int[2];

            while (chyba > -1)
            {
                chyba = -1;
                
                string zadani = Console.ReadLine();

                if (zadani.Length > 0)
                {
                    zadani = zadani.ToLower();          // z VELKYCH - male

                    if(zadani[0] == '?')
                    {
                        return new int[] { 91 };
                    }

                    for (int i = 1; i >= 0; i--)
                    {
                        b = (int)zadani[i];
                        if (!(b < 49 || (55 < b && b < 97) || b > 104 || (zadani[0] > 48 && zadani[0] < 56)
                            || (!((zadani[1] > 48 && zadani[1] < 56)))))
                        {
                            if (b >= 49 && b <= 55)
                            {
                                tah_hrace[1 - i] = b - 49;
                            }
                            //else if (b >= 65 && b <= 72)
                            //{
                            //tah[1 - i] = b - 65;
                            //}
                            else if (b >= 97 && b <= 104)
                            {
                                tah_hrace[1 - i] = b - 97;
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
            return tah_hrace;
        }

        public void VypisZpravu(string anyMessage)
        {
            Console.WriteLine(anyMessage);
        }
    }
}
