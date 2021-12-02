using System;
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
        public int inteligenceBileho = 0;        // nastavení hráče -----> 0.....hraje člověk, > 0 hraje PC, podle toho jak bude číslo daleko od 0 se bude zvedat inteligence
        public int inteligenceCerneho = 1;        // nastavení hráče
        public bool PC = true;
        public Rules gameRules;
        public ArtificialIntelligence brain;

        public string vypisTahu = "";           // vypíše provedený tah;  "" jsou tam proto, aby se vypsal (Vytvoří se jen prázdné místo) hned při prvním průchodu, kdy ještě žádný tah není
        // private int hloubka;                    // bude potřeba nejdříve se dotázat na hloubku, aby se nevkládala na začátku 0


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
            while ( !gameRules.IsGameFinished(Nard) )
            {
                usCom.ConsoleClear();
                usCom.VypisBoard(Nard);
                usCom.VypisZpravu(vypisTahu, false);        // "false" -> protože tady není žádoucí čekat na Enter (=UserCommunication-VypisZpravu-bool cekaNaVstup)  +  "vypisTahu -> MainController-ProvedTah-VypisTahu

                if ( (hracNaTahu == 1 && inteligenceBileho > 0) || (hracNaTahu == -1 && inteligenceCerneho > 0) )  // inteligence1,2 > 0 ....... hraje PC
                {
                    Thread.Sleep( 300 );                    // časová prodleva před tahem počítače, aby tahy nebyly moc rychlé

                    brain = new ArtificialIntelligence( Nard, hracNaTahu > 0 ? inteligenceBileho : inteligenceCerneho, hracNaTahu );    // <= pokud je na řade s tahem počítač, vytvoří se nová instance "brain", kde se nastaví inteligence podle toho, jak je daný hráč nastaven.....
                    brain.VypocitejNejlepsiTah();                                                                                       // ..... spustí se výpočet a výsledek se provede
                    int[] vybranyTah = brain.nejlepsiTah;

                    Nard.VykonejTah(vybranyTah);                                     // vykonam na šachovnici ten ------> vybrany tah    

                    vypisTahu = string.Format("{0}{1} → {2}{3}, počet tahů bez zajetí: {4}", (char)(vybranyTah[1] + 'A'), (char)(vybranyTah[0] + '1'), (char)(vybranyTah[5] + 'A'), (char)(vybranyTah[4] + '1'), Nard.PocetTahuBezZajeti());

                    //vypisTahu = Convert.ToChar(vybranyTah[1] + 65) + (vybranyTah[0] + 1).ToString() + " → " + Convert.ToChar(vybranyTah[5] + 65) + (vybranyTah[4] + 1).ToString(); // místo TahOdkud a TahKam musím zadávat indexy "vybranéhoTahu"
                    // tah se vypiše ...... souřadnice reprezentující písmena se převedou na písmena; čísla zůstanou jen se zvětší o +1, aby odpovídala hodnotám sloupců

                    hracNaTahu = -hracNaTahu;
                }
                else
                {
                    bool provedenTah;
                    while (!UzivatelskeVolby(out provedenTah))               // pokud plati NEGACE TRUE, tzn. nastává false, vypíše se zpráva
                    {
                        usCom.VypisZpravu("  ------->   Error - Tah neni platny ", false);      // "false" -> protože tady není žádoucí čekat na Enter (=UserCommunication-VypisZpravu-bool cekaNaVstup), .....
                    }                                                                           // .....aby se provedl další krok, tady chci rovnou vypsat tu zprávu  
                }
                this.hracNaTahu = -hracNaTahu;    // !1   !-1    prepinani bily-cerny
            }

            Nard.PocetFigur(out int pocetBilych, out int pocetCernych);
            if (pocetBilych > pocetCernych)
            {
                usCom.VypisZpravu("Hra skončila. Vyhrál BÍLÝ hráč.", true);
            }
            else if (pocetCernych > pocetBilych)
            {
                usCom.VypisZpravu("Hra skončila. Vyhrál ČERNÝ hráč.", true);
            }
            else
                usCom.VypisZpravu( "Hra skončila remízou.", true );
        }



        // Volby: move, help, generalHelp, ondo, redo, load, save
        public bool UzivatelskeVolby( out bool provedenTah )     // <----- pomocí výstupního bool parametru( "out bool provedenTah" ) si z metody UzivatelskeVolby vytáhneme informaci o tom, jestli byl proveden tah........
        {                                                       // ........ ta proměnná bude true pouze, pokud bude skutečně proveden tah  ==> pokud nebyl proveden tah, nezmění se hráč, který je na tahu
            provedenTah = false;
            int[] Tah;  // do promenne 'command' se vlozi "return Command.Move"(=táhni) nebo "return Command.GeneralHelp"  +  do promenne 'int[] Tah' se vlozi TAH z "out int[] Vlozeno_coords"  (<--- všechno viz UserCmmunication metoda Volba())
            UserCommunication.Command command = usCom.Volba(hracNaTahu == 1, out Tah);       // provede se to, co je v podmince metody Volba, ale hodnota(1  V -1) pro tu metodu se vezme tady z r11
            switch( command )
            {
                case UserCommunication.Command.Move:
                    provedenTah = ProvedTah(Tah);
                    return provedenTah;

                case UserCommunication.Command.GeneralHelp:
                    usCom.VypisZpravu( "Zadán požadavek o nápovědu všech tahů", true );
                    return true;

                case UserCommunication.Command.Undo:
                    usCom.VypisZpravu( "Undo - vrátíte krok zpět", true );
                    return true;

                case UserCommunication.Command.Redo:
                    usCom.VypisZpravu( "Redo - Posunete se o krok vpřed", true );
                    return true;

                case UserCommunication.Command.Load:
                    usCom.VypisZpravu( "Nahrát uložený stav - LOAD", true );
                    return true;
                     
                case UserCommunication.Command.Save:
                    usCom.VypisZpravu( "Uložit aktuální stav - SAVE", true );
                    return true;

                case UserCommunication.Command.SetPlayer:                   // .......... DOVYSVĚTLIT !!!!!!!!!
                    usCom.VypisZpravu( string.Format( "Změna nastavení {0} hráče na hodnotu {1}", Tah[0] == 1 ? "bílého" : "černého", Tah[1] ), true );
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
