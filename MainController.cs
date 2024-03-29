﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NARD_01
{
    class MainController
    {
        public Board Nard;                      // mám proměnnou Nard typu Board a ta ukazuje na tridu Board, kdyz vytvorim nejaky objekt te tridy, tak on bude umět vše, co je popsané ve třídě Board
        public UserCommunication usCom;
        public int hracNaTahu = 1;           // argument pro Game( ........ ), r39
        public int inteligenceBileho = 0;       // nastavení hráče -----> 0.....hraje člověk, > 0 hraje PC, podle toho jak bude číslo daleko od 0 se bude zvedat inteligence
        public int inteligenceCerneho = 0;      // nastavení hráče
        
        public Rules gameRules;
        public ArtificialIntelligence brain;

        public string vypisTahu = "";           // vypíše provedený tah;  "" jsou tam proto, aby se vypsal (Vytvoří se jen prázdné místo) hned při prvním průchodu, kdy ještě žádný tah není
        // private int hloubka;                    // bude potřeba nejdříve se dotázat na hloubku, aby se nevkládala na začátku 0

        private Files files = new Files();      // <- instance třídy "Files"

        private Thread bestMoveCountTask;       // <=  proměnná, do které se bude ukládat SEPARÁTNÍ VLÁKNO


        public MainController()
        {
            Nard = new Board();
            usCom = new UserCommunication();
            gameRules = new Rules();
            
            gameRules.NaplnBoard(Nard);
            this.Game();
        }



        public void Game()
        {
            // volba nastavení hráče na tahu
            usCom.VypisZpravu( "Nastavení hráčů. Zadej 0 pro hráče, nebo 1..4 jako hodnotu inteligence PC", false, ConsoleColor.Green );
            inteligenceBileho = -1;
            while ( inteligenceBileho == -1 )
            {
                inteligenceBileho = usCom.GetPlayerSettings( "Nastav bílého hráče \"x\": " );
                if ( inteligenceBileho == -1 )
                {
                    usCom.VypisZpravu( "Nesprávně nastavená hodnota!", false, ConsoleColor.Red );
                }
            }

            inteligenceCerneho = -1;
            while ( inteligenceCerneho == -1 )
            {
                inteligenceCerneho = usCom.GetPlayerSettings( "Nastav černého hráče \"o\": " );
                if ( inteligenceCerneho == -1 )
                {
                    usCom.VypisZpravu( "Nesprávně nastavená hodnota!", false, ConsoleColor.Red );
                }
            }



            while ( !gameRules.IsGameFinished(Nard) )
            {
                usCom.ConsoleClear();
                usCom.VypisBoard( Nard );
                usCom.VypisZpravu( vypisTahu, false, ConsoleColor.Yellow );        // "false" -> protože tady není žádoucí čekat na Enter (=UserCommunication-VypisZpravu-bool cekaNaVstup)  +  "vypisTahu -> MainController-ProvedTah-VypisTahu

                if ( (hracNaTahu == 1 && inteligenceBileho > 0) || (hracNaTahu == -1 && inteligenceCerneho > 0) )  // inteligence1,2 > 0 ....... hraje PC
                {
                    Thread.Sleep( 300 );                    // časová prodleva před tahem počítače, aby tahy nebyly moc rychlé

                    brain = new ArtificialIntelligence( Nard, hracNaTahu > 0 ? inteligenceBileho : inteligenceCerneho, hracNaTahu );    // <= pokud je na řade s tahem počítač, vytvoří se nová instance "brain", kde se nastaví inteligence podle toho, jak je daný hráč nastaven.....
                    bestMoveCountTask = new Thread( brain.VypocitejNejlepsiTah );                           // ..... spustí se výpočet a výsledek se provede a SEPARÁTNÍ VLÁKNO se uloží do proměnné  ->  "bestMoveCountTask"
                    bestMoveCountTask.Start();

                    Console.Write( "\n" );
                    while ( bestMoveCountTask.IsAlive )
                    {
                        Console.Write( "." );                                       // během trvání výpočtu se budou vypisovat tečky (do console)
                    }
                    Console.Write( "\n" );

                    int[] vybranyTah = brain.NejlepsiTah;

                    Nard.VykonejTah(vybranyTah);                                     // vykonam na šachovnici ten ------> vybrany tah    

                    vypisTahu = string.Format("{0}{1} → {2}{3}, počet tahů bez zajetí: {4}", (char)(vybranyTah[ 1 ] + 'A'), (char)(vybranyTah[ 0 ] + '1'), (char)(vybranyTah[ 5 ] + 'A'), (char)(vybranyTah[ 4 ] + '1'), Nard.PocetTahuBezZajeti());

                    //vypisTahu = Convert.ToChar(vybranyTah[1] + 65) + (vybranyTah[0] + 1).ToString() + " → " + Convert.ToChar(vybranyTah[5] + 65) + (vybranyTah[4] + 1).ToString(); // místo TahOdkud a TahKam musím zadávat indexy "vybranéhoTahu"
                    // tah se vypiše ...... souřadnice reprezentující písmena se převedou na písmena; čísla zůstanou jen se zvětší o +1, aby odpovídala hodnotám sloupců

                    hracNaTahu = -hracNaTahu;       // ....... VÝMĚNA HRÁČE NA TAHU ?????????????
                }
                else
                {
                    bool provedenTah;
                    while ( !UzivatelskeVolby( out provedenTah, out bool potlacitChybu ) )               // pokud plati NEGACE TRUE, tzn. nastává false, vypíše se zpráva    // ...... DOVYSVĚTLIT !!!!!!!!!!!!
                    {
                        if ( !potlacitChybu )
                        {
                            usCom.VypisZpravu( "  ------->   Error - Tah neni platny ", false, ConsoleColor.Red );      // "false" -> protože tady není žádoucí čekat na Enter (=UserCommunication-VypisZpravu-bool cekaNaVstup), .....
                                                                                                    // .....aby se provedl další krok, tady chci rovnou vypsat tu zprávu  
                        }
                    }                                                                           
                    if ( provedenTah )
                    {
                        hracNaTahu = -hracNaTahu;
                    }
                }
            }

            Nard.PocetFigur(out int pocetBilych, out int pocetCernych);
            if (pocetBilych > pocetCernych)
            {
                usCom.VypisZpravu( "Hra skončila. Vyhrál BÍLÝ hráč.", true, ConsoleColor.Green );
            }
            else if (pocetCernych > pocetBilych)
            {
                usCom.VypisZpravu( "Hra skončila. Vyhrál ČERNÝ hráč.", true, ConsoleColor.Green );
            }
            else
                usCom.VypisZpravu( "Hra skončila remízou.", true, ConsoleColor.Green );
        }



        // Volby: move, help, generalHelp, ondo, redo, load, save
        public bool UzivatelskeVolby( out bool provedenTah, out bool potlacitChybu )     // <----- pomocí výstupního bool parametru( "out bool provedenTah" ) si z metody UzivatelskeVolby vytáhneme informaci o tom, jestli byl proveden tah........
        {                                                       // ........ ta proměnná bude true pouze, pokud bude skutečně proveden tah  ==> pokud nebyl proveden tah, nezmění se hráč, který je na tahu
            provedenTah = false;
            potlacitChybu = false;       // ......... DOVYSVĚTLIT !!!!!!!!!!!!!!!
            int[] Tah;  // do promenne 'command' se vlozi "return Command.Move"(=táhni) nebo "return Command.GeneralHelp"  +  do promenne 'int[] Tah' se vlozi TAH z "out int[] Vlozeno_coords"  (<--- všechno viz UserCmmunication metoda Volba())
            UserCommunication.Command command = usCom.Volba(hracNaTahu == 1, out Tah);       // provede se to, co je v podmince metody Volba, ale hodnota(1  V -1) pro tu metodu se vezme tady z r11
            switch( command )
            {
                case UserCommunication.Command.Move:
                    provedenTah = ProvedTah(Tah);
                    return provedenTah;

                case UserCommunication.Command.GeneralHelp:
                    usCom.VypisZpravu( "\n Zadán požadavek o nápovědu všech tahů", false, ConsoleColor.Green );     // ....... DOVYSVĚTLIT CELÉ !!!!!!!!!!!!!!!!!

                    brain = new ArtificialIntelligence( Nard, 5, hracNaTahu );
                    brain.VypocitejNejlepsiTah();
                    int[] vybranyTah = brain.NejlepsiTah;
                    usCom.VypisZpravu(string.Format("    Nejlepší možný tah je: {0}{1} -> {2}{3}\n", (char)( vybranyTah[ 1 ] + 'A' ), (char)( vybranyTah[ 0 ] + '1' ), (char)( vybranyTah[ 5 ] + 'A' ), (char)( vybranyTah[ 4 ] + '1' ) ), false, ConsoleColor.Green );
                    potlacitChybu = true;
                    return false;

                case UserCommunication.Command.Undo:
                {
                    bool proveden = Nard.TahZpet();
                    potlacitChybu = true;
                    if ( proveden )
                    {
                        vypisTahu = "Proveden tah zpět";
                        provedenTah = true;
                    }
                    else
                        usCom.VypisZpravu( "Již nelze provádět tahy vpřed. Jsme na začátku hry.", false, ConsoleColor.Red );
                    return proveden;
                }

                case UserCommunication.Command.Redo:
                {
                    bool proveden = Nard.TahVpred();
                    potlacitChybu = true;
                    if (proveden)
                    {
                        vypisTahu = "Proveden tah vpřed";
                        provedenTah = true;
                    }
                    else
                        usCom.VypisZpravu( "Již nelze provádět tahy vpřed. Jsme na začátku hry.", false, ConsoleColor.Red );
                    return proveden;
                }

                case UserCommunication.Command.Load:
                    if ( files.LoadGame( "default.xml", out Board newBoard, out int newPlayerOnMove, out int player1, out int player2 ) )      // přidáno použití této metody, která je vytvořena ve Files.cs
                    {
                        Nard = newBoard;
                        hracNaTahu = newPlayerOnMove;
                        inteligenceBileho = player1;
                        inteligenceCerneho = player2;

                        usCom.VypisZpravu( "Hra byla načtena", true, ConsoleColor.Green );  // tato zpráva se vypíše po:  1.načtení Desky, 2.načtení Hráče na TAHU, 3.-4. načtení KDO hraje za Bílé a za Černé
                    }
                    else
                        usCom.VypisZpravu( "Uloženou hru se nepodařilo načíst", true, ConsoleColor.Red );
                    return true;
                     
                case UserCommunication.Command.Save:            // tady níž == proměnné, ve kterých je uchované to nastavení hráčů [ inteligenceBileho, inteligenceCerneho ]
                    if ( files.SaveGame( "default.xml", Nard, inteligenceBileho, inteligenceCerneho ) )           // přidáno použití této metody  
                    {
                        //inteligenceBileho = player1;
                        usCom.VypisZpravu("Hra byla uložena", true, ConsoleColor.Green);
                    }
                    else
                        usCom.VypisZpravu( "Hru se nepodařilo uložit", true, ConsoleColor.Red );
                    return true;

                case UserCommunication.Command.SetPlayer:                   // .......... DOVYSVĚTLIT !!!!!!!!!
                    usCom.VypisZpravu( string.Format( "Změna nastavení {0} hráče na hodnotu {1}", Tah[0] == 1 ? "bílého" : "černého", Tah[1] ), true, ConsoleColor.Green );
                    switch( Tah[ 0 ])
                    {
                        case 1:
                            inteligenceBileho = Tah[ 1 ];
                            break;
                        case 2:
                            inteligenceCerneho = Tah[ 1 ];
                            break;
                    }
                    return true;
            }
            return false;
        }



        // Provedeni tahu
        public bool ProvedTah(int[] Tah)    // tahy jsou na indexech -> Odkud[0 a 1], Kam[0 a 1]  =======> Tah(A2A3) = Tah[0123]
        {
            int[] TahOdkud = new int[2];    // pole se 2 prvky
            TahOdkud[0] = Tah[1];
            TahOdkud[1] = Tah[0];

            int[] TahKam = new int[2];
            TahKam[0] = Tah[3];
            TahKam[1] = Tah[2];


            //int kamen_cil = Nard.hracideska[TahKam[0], TahKam[1]];      // vraci hodnotu (figurky: 1 v -1 v 0), ktera je na policku kam kracim => abych nevstoupil nekam, kde uz nejaka figurka stoji
            //int kamen = Nard.hracideska[TahOdkud[0], TahOdkud[1]];      // vraci hodnotu, ktera je na policku odkud jdu => abych vedel jaka bude hodnota (figurka) tam kam jdu
            //int[] pohyb = { TahOdkud[0], TahOdkud[1], 0, TahKam[0], TahKam[1], kamen }; // A, 2, 0(=policko zustane prazdne] na A, 3, kamen(=jaka figurka se sem premistila: 1 v -1 v 0)


            List<int[]> platneTahy = gameRules.GenerujPlatneTahy(Nard, hracNaTahu);  // v Seznamu jsou platne tahy pro všechny figurky jedné (aktuální) barvy; před dalším tahem se vyčistí
            foreach (int[] aktualniTah in platneTahy)
            {
                if (aktualniTah[0] == TahOdkud[0] && aktualniTah[1] == TahOdkud[1] && aktualniTah[4] == TahKam[0] && aktualniTah[5] == TahKam[1]) // indexy viz -> Rules.cz -> ZkusZajmout Figurku -> ř95
                {                                                       // pokud jsou v 'platneTahy' ty zadané tahy(jsou definované souřadnicemi), tak se vykonají(VykonejTah)
                    Nard.VykonejTah(aktualniTah);

                    vypisTahu = string.Format("{0}{1} → {2}{3}, počet tahů bez zajetí: {4}", (char)(TahOdkud[1] + 'A'), (char)(TahOdkud[0] + '1'), (char)(TahKam[1] + 'A'), (char)(TahKam[0] + '1'), Nard.PocetTahuBezZajeti());

                    //vypisTahu = Convert.ToChar(TahOdkud[1] + 65) + (TahOdkud[0] + 1).ToString() + " → " + Convert.ToChar(TahKam[1] + 65) + (TahKam[0] + 1).ToString();
                    // tah se vypiše ...... souřadnice reprezentující písmena se převedou na písmena; čísla zůstanou jen se zvětší o +1, aby odpovídala hodnotám sloupců
                    return true;                // = 'true' pokud se tah provede
                }
            }
            return false;                       // = 'false' pokud se tah neprovede
        }
    }
}
