using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class MainController
    {
        public Board Nard;                      // mám proměnnou Nard typu Board a ta ukazuje na tridu Board, kdyz vytvorim nejaky objekt te tridy, tak on bude umět vše, co je popsané ve třídě Board
        public UserCommunication usCom;
        public bool tahneBily = true;           // argument pro Volba( ........ ), r35
        public Rules gameRules;

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
                this.ProvedTah();
                usCom.ConsoleClear();
                this.tahneBily = !tahneBily;    // !true   !false    prepinani bily-cerny
            }
        }

        // Provedeni tahu
        public void ProvedTah()
        {
            int[][] Tah;
            Tah = usCom.Volba(tahneBily);       // provede se to, co je v podmince metody Volba, ale hodnota(true x false) pro tu metodu se vezme tady z r11

            int[] TahOdkud;
            TahOdkud = Tah[0];

            int[] TahKam;
            TahKam = Tah[1];

            Nard.hracideska[TahKam[0], TahKam[1]] = Nard.hracideska[TahOdkud[0], TahOdkud[1]];
            Nard.hracideska[TahOdkud[0], TahOdkud[1]] = 0;
        }
    }
}
