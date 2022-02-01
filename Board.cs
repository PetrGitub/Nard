using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class Board
    {
        public int[,] hracideska = new int[ 7, 8 ];
        public List<int[]> SeznamTahu = new List<int[]>();
        private int indexSeznamuPriv = 0;                               // tzv. UKAZATEL ( privátní ), který ukazuje v historii na pozici, která se právě odehrála
        public int IndexSeznamu { get { return indexSeznamuPriv; } }    // UKAZATEL ( veřejný ), obsahuje public GETTER => tzn., že je k němu přístup ZVENKU, ale jen ke ČTENÍ;  vrací hodnotu privátní proměnné _ indexSeznamuPriv



        public Board()
        {
           
        }



        /// <summary>
        /// nový KONSTRUKTOR
        /// </summary>
        /// <param name="boardToClone"> konstruktor přijme v PARAMETRU jinou instanci třídy Board a naklonuje se z ní </param>
        public Board( Board boardToClone )                       //  ------> v atributech hraciDeska a SeznamTahu bude mít naprosto stejné hodnoty, jako ta původní deska
        {
            for ( int i = 0; i < hracideska.GetLength( 0 ); i++ )
            {
                for ( int J = 0; J < hracideska.GetLength( 1 ); J++ )
                {
                    hracideska[ i, J ] = boardToClone.hracideska[ i, J ];
                }
            }
            foreach ( int[] move in boardToClone.SeznamTahu )
            {
                SeznamTahu.Add( move.Clone() as int[] );        // SeznamTahu je pole integerů( = int[]) ; metoda Clone() vytváří objekt datového typu "object" => aby to šlo uložit do toho pole, je třeba to přetypovat na int[]
            }
        }



        /// <summary>
        /// Metoda je používaná(viditelná pro) v metodě Game ve třídě MainController
        /// </summary>
        /// <param name="nejakyPohyb"></param>
        public void VykonejTah( int[] nejakyPohyb )          // <----- z kódu mimo třídu Board(z venku) budu volat jen TUTO metodu( =VykonejTah ) a budu jí předávat jen TAH, který má udělat
        {                                                                                                                      //  =====> tzn. při TAHu hráče/PC se automaticky nastaví obě BOOL proměnné -> tahZpet=false, zapsatDoSeznamuTahu=true
            VykonejTahPriv( nejakyPohyb, false, true );     //  VykonejTahPriv se provede(zavolá) v rámci metody VykonejTah, ale nebude vidět z venku
        }



        /// <summary>
        /// Složitější metoda, která se volá v rámci VykonejTah
        /// </summary>
        /// <param name="nejakyPohyb"></param>
        /// <param name="tahZpet"></param>
        /// <param name="zapsatDoSeznamuTahu"></param>
        private void VykonejTahPriv( int[] nejakyPohyb, bool tahZpet, bool zapsatDoSeznamuTahu )
        {
            for (int index = 0; index < nejakyPohyb.Length; index += 4) // smyčka, která při každém průchodu zpracuje 4 prvky, provede změnu na desce a pak se posune o 4 prvky dál. Takhle projede celé pole, ať má délku 8, nebo 12 prvků...
            {                                                           // |A|1|1|0| - na souřadnici A1 je figurka 1 a to se změní na 0 (figurka zmizí)     |A|2|0|1| - na souřadnici A2 není nic (0) a objejví se tam figurka 1
                int x = nejakyPohyb[ index + 0 ];
                int y = nejakyPohyb[ index + 1 ];
                int novaHodnotaFigurky;

                if (tahZpet)
                {
                    novaHodnotaFigurky = nejakyPohyb[index + 2];        // číslo na INDEXu 2 značí hodnotu políčka na těchto souřadnicích "PŘED ZMĚNOU" 
                }
                else                                                        // obyčejný tah -> obsahuje 2 čtveřice: Index + 3 v první čtveřici znamená, že figurka zmizela, tedy skutečně 0 ..........
                {                                                                                       // ........ Index + 3 ve druhé čtveřici ale bude symbolizovat figurku, která se na daném políčku objevila - tedy třeba 1
                    novaHodnotaFigurky = nejakyPohyb[index + 3];        // číslo na INDEXu 3 značí hodnotu políčka na těchto souřadnicích "PO ZMĚNĚ" 
                }
                
                this.hracideska[ x, y ] = novaHodnotaFigurky;
            }

            if( zapsatDoSeznamuTahu )
            {
                if ( indexSeznamuPriv != SeznamTahu.Count )             // pokud UKAZATEL( = indexSeznamuPriv ) neukazuje na konec seznamu( = SeznamTahu.Count ) .........
                {
                    SeznamTahu.RemoveRange( indexSeznamuPriv, SeznamTahu.Count - indexSeznamuPriv );    // ......... tak se vše v SeznamTahu od ukazatele do konce zahodí
                }
                SeznamTahu.Add( nejakyPohyb );
                indexSeznamuPriv = SeznamTahu.Count;                    // UKAZATEL se nastaví na konec SeznamTahu
            }
        }



        /// <summary>
        /// Metoda pracuje jen s TAHy, které jsou v HISTORII
        /// </summary>
        /// <returns> Chci provést tah zpět. Ověřím, jestli neukazuji na 0. Pokud ano, vrátím false a konec. Pokud není 0, tak se ukazatel sníží o jedna a ten tah, na který nově ukazuje, tak se provede</returns>
        public bool TahZpet()       // návratová hodnota BOOL = je možné tah vykonat? => UKAZATEL ukazuje na 0 a zkusím udělat TahZpet (= vrátí se FALSE, nic se nestane)
        {                                                                           //=> UKAZATEL neukazuje na 0 a zkusím udělat TahZpet (= ukazatel se sníží o 1 a ten tah, na který NOVĚ ukazuje se provede)
            if ( indexSeznamuPriv == 0 )    // pokud UKAZATEL je 0
            {
                return false;
            }
            else                /*      ověřit, jestli tady bude fungovat ELSE !!!!!!!!!!!      */
            {
                indexSeznamuPriv--;                                                 // pokud UKAZATEL není 0, tak se sníží o jedna
                VykonejTahPriv( SeznamTahu[ indexSeznamuPriv ], true, false );      // a ten tah, na který UKAZATEL nově ukazuje, tak se provede;     tzn., ze SeznamTahu se vybere TAH, na který ukazuje UKAZATEL a provede se, tahZpet=true, zapsatDoSeznamuTahu=false
                return true;
            }
            
        }



        /// <summary>
        /// Metoda pracuje jen s TAHy, které jsou v HISTORII
        /// </summary>
        /// <returns></returns>
        public bool TahVpred()      // návratová hodnota BOOL = je možné tah vykonat? => UKAZATEL ukazuje, v historii TAHů, na konec(= poslední tah) a zkusím udělat TahVpred (= vrátí se FALSE, nic se nestane, není kam jít)
        {                                                                           //=> UKAZATEL ukazuje na nějaký TAH, ale v seznamTahu jsou po něm ještě další tahy, tak zkusím udělat TahVpred (= ukazatel se zvýší o 1 a ten tah se provede)
            if ( indexSeznamuPriv == SeznamTahu.Count )   // pokud UKAZATEL je na konci SeznamuTahu .........
            {
                return false;
            }
            else                /*      ověřit, jestli tady bude fungovat ELSE !!!!!!!!!!!      */
            {
                VykonejTahPriv( SeznamTahu[ indexSeznamuPriv ], false, false ); // ........ provede se TAH, který UKAZATEL právě ukazuje;     tzn., ze SeznamTahu se vybere TAH, na který ukazuje UKAZATEL a provede se, tahZpet=false, zapsatDoSeznamuTahu=false
                indexSeznamuPriv++;                                             // pokud UKAZATEL není na konci SeznamuTahu, tak se zvýší o jedna
                return true;
            }
        }



        public int GetValue( int coordX, int coordY )                 // metoda vrátí hodnotu (figurku: 1  v  -1  v  0), která stojí na daných souřadnicích
        {
               return hracideska[ coordX, coordY ];
        }
         

        public void SetValue( int coordX, int coordY, int value )     // metoda vloží hodnotu (figurku: 1  v  -1  v  0), na dané souřadnice
        {
            hracideska[ coordX, coordY ] = value;
        }



        /// <summary>
        /// Zjišťuje počet bílých a černých figur
        /// </summary>
        /// <param name="pocetBilychFigur"></param>
        /// <param name="pocetCernychFigur"></param>
        public void PocetFigur( out int pocetBilychFigur, out int pocetCernychFigur )
        {
            pocetBilychFigur = 0;
            pocetCernychFigur = 0;
            for ( int i = 0; i < hracideska.GetLength(0); i++ )
            {
                for ( int j = 0; j < hracideska.GetLength(1); j++ )
                { 
                    switch ( hracideska[i, j] )
                    {
                        case 1:
                            pocetBilychFigur++;
                            break;
                        case -1:
                            pocetCernychFigur++;
                            break;
                    }

                    /*if (deska.hracideska[i, j] == 1)
                    {
                        PocetBilychFigur++;
                    }
                    else if (deska.hracideska[i, j] == -1)
                    {
                        PocetCernychFigur++;
                    }*/
                }
            }
            //return pocetBilychFigur <= 1 || pocetCernychFigur <= 1;                 //TRUE -----> pokud bílých nebo černých figur je míň než 2
        }



        /// <summary>
        /// Tahle metoda projde od ukazatele historii tahů a vždy získá přesný výsledek
        /// </summary>
        /// <returns></returns>
        public int PocetTahuBezZajeti()
        {
            int count = 0;
            for ( int index = indexSeznamuPriv - 1; index >= 0; index-- )
            {
                if (SeznamTahu[index].Length == 8)
                {
                    count++;
                }
                else
                    break;
            }
            return count;
        }
    }
}
