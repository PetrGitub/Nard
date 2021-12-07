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

            for (int x = favouriteNard.hracideska.GetLength( 0 ) - 1; x >= 0; x--)                // Ř Á D K Y
            {
                Console.Write("{0,3} ", (x + 1).ToString());                                                            // {0,3}  =>  {0}=vkládání čísel řádků -> (x + 1) = 7.........        3=odsazení prvního sloupce(7-1) od kraje
                for (int y = 0; y < favouriteNard.hracideska.GetLength( 1 ); y++)                 // S L O U P C E                   
                {
                    Console.Write( "{0} ", IntToCharacter( favouriteNard.hracideska[ x, y ] ) );                        // metoda IntToCharacter prochází souřadnice na "hracideska", dostává: 1   -1    0    a vkládá -> x    o    prázdno;                        x  =  ŘÁDKY       y  =  SLOUPCE
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
            GeneralHelp,
            Undo,
            Redo,
            Load,
            Save,
            SetPlayer
        }


        // Volba - vylepšená verze
        // VÝSTUP směřuje do: MainController metoda UzivatelskeVolby()
        // Kdo je na tahu a zadavani souradnic   .......   texty (uvnitr kódu řeším konkrétní tahy - ZadejTah)   VÝSTUP této metody: 1."return Command.Move;" (+souřadnice tahu se vloží do: "out int[] Vlozeno_coords"), 2.return "Command.GeneralHelp;" (+"null")
        public Command Volba(bool tahneBily, out int[] vlozeno_coords)          // argumenty     // pole intu 'Vlozeno_coords' => VYSTUPNI(= out) PARAMETR (tzn., vlozi se do nej souřadnice tahů => děje se to na r??)
        {
            if (tahneBily)
            {
                Console.WriteLine("Bily na tahu");
            }
            else
            {
                Console.WriteLine("Cerny na tahu");
            }

            vlozeno_coords = null;

            int[] TahOdkud;
            int[] TahKam;

            Console.Write("   Zadej souradnice pole ODKUD chces tahout: ");
            Command command = ZadejTah(false, out TahOdkud);                    //  TO SE Z TÉ KONZOLE JEN-TAK PŘEČTE???????,    Proč nechybí - console.readline????????,    "false" znamená, že NepřijímáJenSouřadnice na začátku????????
            if (command == Command.Move)
            {
                Console.Write("   Zadej souradnice pole KAM chces tahout: ");
                ZadejTah(true, out TahKam);                                     //  TO SE Z TÉ KONZOLE JEN-TAK PŘEČTE???????,    Proč nechybí - console.readline????????
                vlozeno_coords = new int[] { TahOdkud[0], TahOdkud[1], TahKam[0], TahKam[1] };
                return Command.Move;
            }

            if ( command == Command.SetPlayer )     // ....... DOVYSVĚTLIT !!!!!!!!!!!!!
            {
                vlozeno_coords = TahOdkud;
            }

            return command;
        }
        
        
        
        //Zadej tah
        public Command ZadejTah(bool prijimamJenSouradnice, out int[] souradnice)
        {
            souradnice = null;
            int x;
            int y;

            while (true)
            {
                string zadaneSouradnice = Console.ReadLine();


                if ( zadaneSouradnice.Length == 0 )
                {
                    Console.WriteLine("Souřadnice nebyly zadány");
                    continue;                                           // co je za "continue" se už nevykoná a všechno se vrací k "zadaneSouradnice"
                }

                if (zadaneSouradnice.Length >= 2)
                {
                    zadaneSouradnice = zadaneSouradnice.ToLower();

                    if (!prijimamJenSouradnice)           // jestliže nebyly vloženy Souřadnice, ale něco jiného (help, undo, redo, save, load)
                    {
                        if (zadaneSouradnice == "help")
                        {
                            return Command.GeneralHelp;
                        }
                        else if (zadaneSouradnice == "undo")
                        {
                            return Command.Undo;
                        }
                        else if (zadaneSouradnice == "redo")
                        {
                            return Command.Redo;
                        }
                        else if (zadaneSouradnice == "save")
                        {
                            return Command.Save;
                        }
                        else if (zadaneSouradnice == "load")
                        {
                            return Command.Load;
                        }
                        else if (zadaneSouradnice.Contains("player1"))     
                        {
                            try         /*        Try ----- Catch blok          */          // <------- pokud během vykonávání  "TRY BLOKU"  dojde k jakékoliv chybě, tak se vypíše chyba, že byl špatně použitý příkaz...
                            {                                                                           //  ...... Co může být za chyby?  ......  
                                int hodnota = int.Parse(zadaneSouradnice[8].ToString());    //   ----->  "nemusí tam být žádné číslo". Dotazuji se na zadaneSouradnice[ 8 ], protože na indexu 8 očekávám číslici. Ale pokud žádnou nezadám, tak se dotazuju mimo pole. Try/catch blok to ošetří za nás
                                                                                            //   ----->  "snažím se znak na pozici 8 parsovat na číslo int". Může tam být písmeno, mezera, nebo jiný znak - Try/catch opět vyřeší
                                if (hodnota < 0 || hodnota > 5)                             //   ----->  pokud to převedu na číslo, porovnám, jestli není menší, než 0, nebo větší než 5. Pokud ano, pomocí  "throw"  vyvolám výjimku, kterou opět try/catch blok zachytí jako chybu.
                                    throw new Exception();

                                souradnice = new int[] { 1, hodnota };  // pokud je vše v pořádku, mám číslo v odpovídajícím rozsahu, tak ho uložím do pole. Na nulté pozici je index o kterého hráče se jedná (1, nebo 2) a na první pozici je hodnota nové inteligence.
                                return Command.SetPlayer;               // ...... return - vrátíme příkaz SetPlayer
                            }
                            catch
                            {
                                Console.WriteLine("Chybně zadaný příkaz pro nastavení hráče. Použij \"player1 [hodnota]\" - např. \"player1 0\"");
                                continue;
                            }
                        }
                        else if (zadaneSouradnice.Contains("player2"))
                        {
                            try
                            {
                                int hodnota = int.Parse(zadaneSouradnice[8].ToString());
                                if (hodnota < 0 || hodnota > 5)
                                    throw new Exception();

                                souradnice = new int[] { 2, hodnota };
                                return Command.SetPlayer;
                            }
                            catch
                            {
                                Console.WriteLine("Chybně zadaný příkaz pro nastavení hráče. Použij \"player2 [hodnota]\" - např. \"player2 0\"");
                                continue;
                            }
                        }       
                    }
                }
     
                try     // vkládání souřadnic
                {
                    x = zadaneSouradnice[0];    // Do "x" se nevloží písmeno (např: "b"), ale číslo (např: 98), pokud v tom stringu na pozici 0 je písmeno b. String je totiž pole charů.
                    if (x < 'a' || 'h' < x)     // To srovnání je pak takto: x < 'a' srovnává hodnotu uloženou v x(např: b=98) s číslem 97(=a).                 
                    {
                        Console.WriteLine("Chybné zadání souřadnice!");
                        continue;
                    }
                    x = x - 'a';

                    y = zadaneSouradnice[1];
                    if ( y < '1' || '7' < y )
                    {
                        Console.WriteLine("Chybné zadání souřadnice!");
                        continue;
                    }
                    y = y - '1';

                    souradnice = new int[] { x, y };    // načtené hodnoty se vloží do "souradnice"   ve tvaru  =====>  { x=sloupec, y=řádek }    !!!!!!!!!!!!!!

                    return Command.Move;
                }

                catch
                {
                    Console.WriteLine("Chybné souřadnice nebo neplatný povel!");
                    continue;
                }
            }
        }



        public void VypisZpravu( string anyMessage, bool cekaNaVstup, ConsoleColor clr )
        {
            ConsoleColor tmp = Console.ForegroundColor;         // původní barva písma (=clr) se vloží do přechodné proměnné (=tmp)
            Console.ForegroundColor = clr;                      // nastaví se nová barva písma

            Console.WriteLine( anyMessage );
            if  ( cekaNaVstup )
            {
                Console.WriteLine( "Stiskni ENTER" );
                Console.ReadLine();                 // musí to tady být - jinak se hned v dalším kroku vymaže konzole a vykresli nový Board a vlastně není vidět žádná provedená akce
            }

            Console.ForegroundColor = tmp;                      // vrátí se původní barva písma
        }



        // nastavení hráče, PC
        public int GetPlayerSettings( string anyMessage )       //  <-  metoda, která čte vstup a vrací chyby, pokud přijde něco jiného, než 0..4
        {
            Console.Write ( anyMessage );                       // přečte se zpráva o tom, jakými hodnotami se nastavuje hráč a PC ......
            string input = Console.ReadLine();                  // ...... tady se načte číselná hodnota, která byla vložena (Pozor! je to číslo, ale bylo vloženo jako STRING)
            int value;
            try
            {
                value = int.Parse( input );                     // vložené číslo se "přeparsuje" na INTEGER
            }
            catch
            {
                return -1;
            }
            if ( value < 0 || value > 4 )
                return -1;

            return value;                                       // vrací správně vloženou hodnotu
        }











        /*
        // VÝSTUP směřuje do: MainController metoda UzivatelskeVolby()
        // Kdo je na tahu a zadavani souradnic   .......   texty (uvnitr kódu řeším konkrétní tahy - ZadejTah)   VÝSTUP této metody: 1."return Command.Move;" (+souřadnice tahu se vloží do: "out int[] Vlozeno_coords"), 2.return "Command.GeneralHelp;" (+"null")
        public Command Volba(bool tahneBily, out int[] Vlozeno_coords)      // argumenty     // pole intu 'Vlozeno_coords' => VYSTUPNI(= out) PARAMETR (tzn., vlozi se do nej souřadnice tahů => děje se to na r??)
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
            TahOdkud = this.ZadejTah();                                         // souradnice tahu ODKUD jsou v TahOdkud
            Console.Write("   Zadej souradnice pole KAM chces tahout: ");
            TahKam = this.ZadejTah();                                           // souradnice tahu KAM jsou v TahKam

            Vlozeno_coords = null;
            if(TahOdkud[0] == 91 || TahKam[0] == 91)                            // číslo 91 pochází z řádků 113-116 ZadejTah()
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
                
                string zadani = Console.ReadLine();     // ted mam v promenne "zadani" vložený tah (napr: A2)

                if (zadani.Length == 2 )
                {
                    zadani = zadani.ToLower();          // z VELKYCH - male

                    if(zadani[0] == '?')
                    {
                        return new int[] { 91 };
                    }

                    for (int i = 1; i >= 0; i--)
                    {
                        b = (int)zadani[i];             // postupně čtu vložený tah (jdu od ZADU, tzn. jako první načtu ZNAK na indexu 1 a převedu na KÓD, potom index 0)
                        if (!(b < 49 || (55 < b && b < 97) || b > 104 || (zadani[0] > 48 && zadani[0] < 56) || !(zadani[1] > 48 && zadani[1] < 56)))        // když to přepíšu v ASCII:  !(b<1 || (7<b && b<a) || b>h)
                        {                                                                                                                                   // na indexu 0 se NESMÍ objevit čísla 1 - 7, na indexu 1 se MUSÍ objevit čísla 1 - 7
                            if (b >= 49 && b <= 55)                             // 1 - 7
                            {
                                tah_hrace[1 - i] = b - 49;
                            }
                            //else if (b >= 65 && b <= 72)
                            //{
                            //tah[1 - i] = b - 65;
                            //}
                            else if (b >= 97 && b <= 104)                       // a - h
                            {
                                tah_hrace[1 - i] = b - 97;
                            }
                        }
                        else
                        {
                            chyba += 1;
                            if (! (48 < zadani[1] && zadani[1] < 56) )                  // pokud mám na indexu 1 něco špatně zadané, vypíše se mi první zpráva, pokud mám špatné zadání na indexu 0, vypíše se druhá zpráva
                            {
                                Console.WriteLine("V zadani tahu se objevila nespravna   index1    kombinace. Opakujte zadani.\n");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("V zadani tahu se objevila nespravna   index0    kombinace. Opakujte zadani.\n");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    chyba += 1;
                    Console.WriteLine("V zadani tahu se objevila nespravna kombinace 10001. Opakujte zadani.\n");
                }
            }
            Console.WriteLine();
            return tah_hrace;
        }
        */
    }
}
