using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class MainController
    {
        public Board Nard;                      // mám proměnnou Nard typu Board a ta ukazuje na tridu Board, kdyz vytvorim nejaky objekt te tridy, tak on bude umět vše, co je popsané ve třídě Board
        public UserCommunication usCom;
        public int tahneBily = 1;           // argument pro Volba( ........ ), r35
        public Rules gameRules;

        public string vypisTahu = "";           // vypíše provedený tah;  "" jsou tam proto, aby se vypsal (Vytvoří se jen prázdné místo) hned při prvním průchodu, kdy ještě žádný tah není

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
            while (true)
            {
                usCom.VypisBoard(Nard);
                usCom.VypisZpravu(vypisTahu);
                while( !UzivatelskeVolby() )
                {
                    usCom.VypisZpravu("  ------->   Error - Tah neni platny ");
                }
                usCom.ConsoleClear();
                
                this.tahneBily = -tahneBily;    // !1   !-1    prepinani bily-cerny
            }
        }

        // Volby: move, help, generalHelp, ondo, redo, load, save
        public bool UzivatelskeVolby()
        {
            int[] Tah;            // do promenne 'command' se vlozi "return Command.Move"(=táhni)  +  do promenne 'int[][] Tah' se vlozi TAH z "out int[][] Vlozeno_coords"
            UserCommunication.Command command = usCom.Volba(tahneBily == 1, out Tah);       // provede se to, co je v podmince metody Volba, ale hodnota(1  V -1) pro tu metodu se vezme tady z r11
            switch( command )
            {
                case UserCommunication.Command.Move:
                    return ProvedTah(Tah);

                case UserCommunication.Command.GeneralHelp:
                    //usCom.VypisZpravu("Pokusna verze");
                    Console.WriteLine("hfvnhffi");
                    return true;
            }
            return false;
        }

        // Provedeni tahu
        public bool ProvedTah(int[] Tah)    // tahy jsou na indexech -> Odkud[0 a 1], Kam[0 a 1]  =======> Tah(A2A3) = Tah[0123]
        {
            int[] TahOdkud = new int[2];    // pole se 2 prvky
            TahOdkud[0] = Tah[0];
            TahOdkud[1] = Tah[1];

            int[] TahKam = new int[2];
            TahKam[0] = Tah[2];
            TahKam[1] = Tah[3];


            int kamen_cil = Nard.hracideska[TahKam[0], TahKam[1]];      // vraci hodnotu (figurky: 1 v 2 v 0), ktera je na policku kam kracim > abych nevstoupil nekam, kde uz nejaka figurka stoji
            int kamen = Nard.hracideska[TahOdkud[0], TahOdkud[1]];      // vraci hodnotu, ktera je na policku odkud jdu > abych vedel jaka bude hodnota (figurka) tam kam jdu
            int[] pohyb = { TahOdkud[0], TahOdkud[1], 0, TahKam[0], TahKam[1], kamen }; // A, 2, 0(=policko zustane prazdne] na A, 3, kamen(=jaka figurka se sem premistila: 1 v 2 v 0


            List<int[]> platneTahy = gameRules.GenerujPlatneTahy(Nard, tahneBily);  // v Seznamu jsou platne tahy pro všechny figurky jedné (aktuální) barvy; před dalším tahem se vyčistí
            foreach (int[] vypisPlatneTahy in platneTahy)
            {
                if (vypisPlatneTahy[0] == TahOdkud[0] && vypisPlatneTahy[1] == TahOdkud[1] && vypisPlatneTahy[4] == TahKam[0] && vypisPlatneTahy[5] == TahKam[1])
                {                                                       // pokud jsou v 'platneTahy' ty zadané tahy(jsou definované souřadnicemi), tak se vykonají(VykonejTah)
                    Nard.VykonejTah(pohyb);
                    vypisTahu = Convert.ToChar(TahOdkud[1] + 65) + (TahOdkud[0] + 1).ToString() + " → " + Convert.ToChar(TahKam[1] + 65) + (TahKam[0] + 1).ToString();
                                                // tah se vypiše ...... souřadnice reprezentující písmena se převedou na písmena; čísla zůstanou jen se zvětší o +1, aby odpovídala hodnotám sloupců
                    return true;                // = 'true' pokud se tah provede
                }
            }
            return false;                       // = 'false' pokud se tah neprovede
        }
    }
}
