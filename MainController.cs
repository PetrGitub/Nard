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

            int[] Odtud;
            Odtud = Tah[0];

            int[] Sem;
            Sem = Tah[1];

            Nard.hracideska[Sem[0], Sem[1]] = Nard.hracideska[Odtud[0], Odtud[1]];
            Nard.hracideska[Odtud[0], Odtud[1]] = 0;
        }
    }
}
