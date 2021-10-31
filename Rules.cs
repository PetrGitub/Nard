using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NARD_01
{
    class Rules
    {
        public List<int[]> SeznamPlatnychTahu = new List<int[]>();

        public Rules()
        {
           
        }


        // Naplneni matice   ----->    na indexy[i,j] se vloží: ( 1  -1   0 )  =====> a to se potom použije v UserCommunication při "VypisBoard"[x,y]
        public void NaplnBoard(Board deska)
        {
            for (int i = 0; i < deska.hracideska.GetLength(0); i++)       // this ....... ukazuje na konkretni hraciDesku, protoze jich muze existovat vic             !!!!!   GetLength(0) --> sloupce   a   GetLength(1) --> řádky   !!!!!
            {
                for (int j = 0; j < deska.hracideska.GetLength(1); j++)
                {
                    if (i <= 1)
                    {
                        deska.hracideska[i, j] = 1;
                    }
                    else if (i >= 5)
                    {
                        deska.hracideska[i, j] = -1;
                    }
                    /*else
                    {
                        deska.hracideska[i, j] = 0;
                    }*/
                }
            }
        }

        

        /// <summary>
        /// Generuje tahy pro všechny figurky, které mají barvu, která je na tahu
        /// </summary>
        /// <param name="deska"></param>
        /// <param name="kdoJeNaTahu"></param>
        /// <returns></returns>
        public List<int[]> GenerujPlatneTahy(Board deska, int kdoJeNaTahu)  // metoda vygeneruje všechny platné tahy pro aktuální barvu (tzn. pro "x" nebo "o")
        {
            SeznamPlatnychTahu.Clear();

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(deska.GetValue(i, j) == kdoJeNaTahu)     // pokud je na daných souřadnicích barva( 1  v -1 ), která je na tahu........
                    {
                        GenerujPlatneTahyProFigurku(i, j, deska, kdoJeNaTahu);      // .......vygeneruju do SeznamuPlatnýchTahů platné tahy figurky na těchto souřadnicích (týká se to všech figurek, které mají barvu, která je na tahu)
                    }
                }
            }
            return SeznamPlatnychTahu;
        }


        int[,] directions =
        {
            {  0,-1 },  // doleva ===== index[0]
            {  1, 0 },  // nahoru ===== index[1]
            {  0, 1 },  // doprava ===== index[2]
            { -1, 0 }   // dolu ===== index[3]
        };

        /// <summary>
        /// Generuje tahy pro figurku, která je na tahu
        /// </summary>
        /// <param name="coordX"></param>
        /// <param name="coordY"></param>
        /// <param name="deska"></param>
        /// <param name="kdoJeNaTahu"></param>
        public void GenerujPlatneTahyProFigurku(int coordX, int coordY, Board deska, int kdoJeNaTahu)
        {
            
            for (int dir = 0; dir < 4; dir++)   //procházení 4 směrů ve smyčce ----->   smyčka nastaví hodnoty "x" a "y" ve 4 směrech = doleva, nahoru, doprava, dolu
            {                   // toX = x-ová souřadnice pole kam kráčím
                int toX = coordX + directions[ dir, 1 ];   // v proměnné "coordX" je x-ová souřadnice figurky;  dir=index směru (např: dir=0 -> {-1,0});  0 a 1=indexy souřadnic v těch směrech, tzn. => directions[{-1,0},0] ta druhá 0 říká, ......
                int toY = coordY + directions[ dir, 0 ];   // v proměnné "coordY" je y-ová souřadnice figurk-1,0y,0                                                               .......... že si z té závorky vyberu čísli na indexu 0, což je -1

                if ( !IsValidCoords(toX, toY) )         // KONTROLA, jestli souřadnice, kam můžu jít nejsou mimo desku
                    continue;   // continue = pokud je podmínka true = zastaví se proces a "for" se posune o +1 (pokračuje další iterace)

                if(deska.GetValue(coordX, coordY) == kdoJeNaTahu && deska.GetValue(toX, toY) == 0)  // 1.figurka, kterou hýbu má barvu, která je na tahu,  2.na miste kam jdu nic nestoji
                {
                    ZkusZajmoutFigurku(new int[] { coordX, coordY, kdoJeNaTahu, 0, toX, toY, 0, kdoJeNaTahu });  // např.: |X|Y|1|0| |X+1|Y+0|0|1|
                }           
            }
        }


        /// <summary>
        /// Zajmutí figurky (pokud to je možné)
        /// </summary>
        /// <param name="prvniPohyb"></param>
        public void ZkusZajmoutFigurku( int[] prvniPohyb, Board deska, int kdoJeNaTahu )
        {
            int newCoordX = prvniPohyb[4];                      // <---- odkaz na ..... "toX"               do "newCoordX je vložena x-ová souřadnice políčka, na které figurka táhla (se přesunula)
            int newCoordY = prvniPohyb[5];                      // <---- odkaz na ..... "toY"               do "newCoordX je vložena y-ová souřadnice políčka, na které figurka táhla (se přesunula)

            for (int dir = 0; dir < 4; dir++)
            {
                int nextToX = newCoordX + directions[ dir, 1 ];         // nextToX ...... x-ová souřadnice políčka, kde by mohla být figurka soupeře
                int nextToY = newCoordY + directions[ dir, 0 ];         // nextToX ...... y-ová souřadnice políčka, kde by mohla být figurka soupeře

                if ( !IsValidCoords(nextToX, nextToY))           // KONTROLA, jestli souřadnice, kde by mohla stát soueřova figurka, nejsou mimo desku
                    continue;   // continue = pokud je podmínka true = zastaví se proces a "for" se posune o +1 (pokračuje další iterace)

                int possibleEnemy = deska.GetValue(nextToX, nextToY);   // possibleEnemy = hráč, který NENÍ na tahu  ---->  dostanu vrácenou figurku ( -1, 0, 1 ) stojící na SOUŘADNICÍCH KDE BY MOHL BÝT SOUPEŘ a tu hodnotu vložím do "possibleEnemy"
                if (possibleEnemy != (-kdoJeNaTahu))        // <----- pokud - kdo NENÍ na tahu(soupeř) NESTOJÍ na souřadnicích kam se koukám - pokračuju  !!!!! ještě jinak: pokud - se vedle mne nenachází nepřítel - jdu prohlédnout další směr !!!!!
                    continue;

                int behindNextToX = nextToX + directions[ dir, 1 ];     // v proměnné "behindNextToX" je x-ová souřadnice figurky, která je AŽ ZA POZICÍ kde by mohl být soupeř (=až za "nextToX")
                int behindNextToY = nextToY + directions[ dir, 0 ];     // v proměnné "behindNextToY" je y-ová souřadnice figurky, která je AŽ ZA POZICÍ kde by mohl být soupeř (=až za "nextToY")

                /* SEM VLOŽÍM ŘEŠENÍ SITUACE V ROHU */



                if (deska.GetValue( behindNextToX, behindNextToY ) != kdoJeNaTahu)      // pokud - ten kdo JE na tahu nestojí ob jedno pole(=nestojí tam další moje figurka) - pokračuju  ----> ALE pokud - ob jedno pole STOJÍ MOJE DALŠÍ FIGURKA - můžu zajmout soupeře
                    continue;

                prvniPohyb = prvniPohyb.Concat( new int[] { nextToX, nextToY, possibleEnemy, 0 } ).ToArray();     // přidám zajetí do celkového tahu (pokud byly splněny podmínky pro zajetí)
            }

            SeznamPlatnychTahu.Add( prvniPohyb );
        }


        public bool IsValidCoords(int souradniceX, int souradniceY)                                 // metoda vrátí všechny souřadnice na šachovnici => ty validní     ->   jen ty souřadnice, na kterých se hraje
        {
            return souradniceX >= 0 && souradniceX <= 6 && souradniceY >= 0 && souradniceY <= 7;    // použitím této metody se vyloučí možnost dostat se mimo šachovnici
        }
    }
}
