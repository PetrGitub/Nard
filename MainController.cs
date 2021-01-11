using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class MainController
    {
        public Board Nard;
        public UserCommunication usCom;
        public bool tahneBily = true;           // argument pro Volba( ........ ), r35

        public MainController()
        {
            Nard = new Board();
            usCom = new UserCommunication();
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
            Tah = usCom.Volba(tahneBily);

            int[] TahOdkud;
            TahOdkud = Tah[0];

            int[] TahKam;
            TahKam = Tah[1];

            Nard.hracideska[TahKam[0], TahKam[1]] = Nard.hracideska[TahOdkud[0], TahOdkud[1]];
            Nard.hracideska[TahOdkud[0], TahOdkud[1]] = 0;
        }
    }
}
