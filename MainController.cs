using System;
using System.Collections.Generic;
using System.Text;

namespace _07_B___poleCELL_piece_STRING
{
    class MainController
    {
        public Board Nard;
        public UserCommunication usCom;

        public MainController()
        {
            Nard = new Board();
            usCom = new UserCommunication();
            this.Game();
        }

        public void Game()
        {
            while(true)
            {
                usCom.VypisBoard(Nard);
                this.ProvedTah();
                usCom.ConsoleClear();
            }
        }

        // Provedeni tahu
        public void ProvedTah()
        {
            int[] Odtud;
            Odtud = usCom.ZadejTah();

            int[] Sem;
            Sem = usCom.ZadejTah();

            Nard.hracideska[Sem[0], Sem[1]] = Nard.hracideska[Odtud[0], Odtud[1]];
            Nard.hracideska[Odtud[0], Odtud[1]] = 0;
        }
    }
}

// namespace Pokus01_tvorba_sachovnice

