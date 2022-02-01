using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class ArtificialIntelligence
    {
        private bool vypocetDokoncenPriv = false;                                   // <----- privátní atribut; ke čtení jen z venku (princip zapouzdření)
        public bool VypocetDokoncen { get { return vypocetDokoncenPriv; } }

        private int[] nejlepsiTahPriv = null;                                       // <----- privátní atribut; ke čtení jen z venku (princip zapouzdření)
        public int[] NejlepsiTah { get { return nejlepsiTahPriv; } }                // do tohoto ATRIBUTU( = nejlepsiTah ) bude vždy uložený výsledek

        private Board nardAI;
        private Rules rules;                                                        // potřebuju pravidla, protože budu počítat platné tahy
        private int hloubkaAI;
        private int hracNaTahuAI;
        private Random nahodnyVyber = new Random();
        private List<int[]> seznamNejlepsichTahu;                                   // kolekce nejlepších tahů !!!!!!   <----- z nich se vybere jeden jako výsledek a ten se použije

        private const int MAX = 100;
        private const int MANY = 90;



        /// <summary>
        /// KONSTRUKTOR - vytvořím kopii herní desky, inicializuju rules, vytvořím seznam nej tahů
        /// </summary>
        /// <param name="deska"></param>
        /// <param name="hloubka"></param>
        /// <param name="kdoJeNaTahu"></param>
        public ArtificialIntelligence(Board deska, int hloubka, int kdoJeNaTahu)    // argumenty, do kterých se vloží patřičná data a ta se předají do proměnných -> NardAI, HloubkaAI, HracNaTahuAI
        {
            nardAI = new Board( deska );                // vytvořím si KOPII instance desky ( viz. druhý konstruktor ve třídě Board )
            hloubkaAI = hloubka;
            hracNaTahuAI = kdoJeNaTahu;
            rules = new Rules();                        // tvorba instance Pravidel (= rules)
            seznamNejlepsichTahu = new List<int[]>();   // tvorba instance nejlepších tahů (= seznamNejlepsichTahu)
        }



        public void VypocitejNejlepsiTah()
        {
            VypocitejNejlepsiTahy();
            nejlepsiTahPriv = VyberNahodnyTah( seznamNejlepsichTahu );
            vypocetDokoncenPriv = true;
        }



        private int[] VyberNahodnyTah( List<int[]> seznamTahu )                     // private ----> protože bude mít význam jen uvnitř třídy
        {
            //int nahodnyTah = nahodnyVyber.Next(seznamTahu.Length);
            //int vybranyTah = seznamTahu[nahodnyTah];
            //return vybranyTah;
            return seznamTahu[nahodnyVyber.Next(seznamTahu.Count)];
        }



        private void VypocitejNejlepsiTahy()
        {
            rules.GenerujPlatneTahy( nardAI, hracNaTahuAI );                        // pomocí pravidel vygeneruju seznam platných tahů a ten seznam si uložíme do proměnné "moves"
            List<int[]> moves = new List<int[]>();                  // <===== objekt TAHy tyou seznam, který je ve formě pole intů
            foreach (int[] oneMove in rules.SeznamPlatnychTahu)                     // každy jeden TAH ze seznamu platných tahů NAKLONUJ (jako Typ pole intů)....... 
                moves.Add(oneMove.Clone() as int[]);                                // ........ a vlož ho do proměnné "moves", která je právě Typu int[]
                                                                                    // důvodem této akce je: že v minimaxu se ten seznam tahů ve třídě rules bude měnit, ale já potřebuju uchované tyto TAHy, aby jsem je mohl, později, ve smyčce projít jeden po druhém...

            int nejlepsiDosavadniHodnota = -MAX;                    // <----- tuto proměnnou nastavím na nejhorší výsledek; pokud minimax najde lepší, tak tento tah uloží do seznamu a zvedne hodnotu nejlepsiDosavadniHodnota. Pokud najde horší, tak se nikam ukládat nebude

            foreach ( int[] move in moves )                         // smyčka, která projde všechny platné tahy ve výchozím postavení - to je to, které hráč vidí na monitoru (do hloubky jde až minimax)
            {
                nardAI.VykonejTah( move );                                          // provede se jeden z tahů, otestuje se, jak moc je dobrej ( = zavolá se Minimax ) a na konci se vrátí zpět, aby se mohl vyzkoušet jiný tah
                hracNaTahuAI *= -1;                                                 // při každém provedeném tahu se musí změnit hráč na tahu

                int vypocetMinimaxu = -Minimax( hloubkaAI - 1 );                    // zavolá se výpočet minimaxu, který od daného tahu bude provádět další tahy do hloubky (dle inteligence) a vrátí nějakou číselnou hodnotu, která tomtu tahu bude nějak odpovídat - "jak moc je to výhodný tah"

                // uložení kolekce nejlépe hodnocených tahů
                if ( vypocetMinimaxu >= nejlepsiDosavadniHodnota)                  // tady se porovná vypočítaná hodnota tahu. Pokud je stejná, jako ty předchozí, tak se přidá do seznamu. Pokud je lepší, tak se ty horší ze seznamu nejdřív vymažou a pak až se tam uloží tato lepší. Pokud je horší, než ty v seznamu, tak na ni zapomene
                {
                    if ( vypocetMinimaxu > nejlepsiDosavadniHodnota )
                    {
                        seznamNejlepsichTahu.Clear();
                        nejlepsiDosavadniHodnota = vypocetMinimaxu;
                    }
                    seznamNejlepsichTahu.Add( move.Clone() as int[] );
                }

                nardAI.TahZpet();
                hracNaTahuAI *= -1;
            }
        }




        /// <summary>
        /// metoda Minimax
        /// </summary>
        /// <param name="hloubka"></param>
        /// <returns> Bude se sama volat, dokud nedojde do konce hry, nebo do požadované hloubky</returns>
        private int Minimax( int hloubka)
        {
            // Ohodnocení, výhra, prohra, remíza
            if ( rules.IsGameFinished( nardAI ) )           // hra končí když: 1.počet tahů bez zajmutí figurky = 30  nebo  2.jeden z hráčů má 1 nebo žádnou figurku
            {
                SpocitejKameny( hracNaTahuAI, out int mojeKameny, out int jehoKameny );     

                if ( mojeKameny > jehoKameny )              // dokážu rozeznat vítězství od prohry
                    return MAX;
                if ( mojeKameny < jehoKameny )
                    return -MAX;
                if ( mojeKameny == jehoKameny )
                    return 0;
            }

            // Pokud bylo dosaženo maximální hloubky, provede se ohodnocení pozice              /* <------- počítání hodnoty aktuálního rozložení hrací desky */
            if ( hloubka == 0 )     
            {
                int value = 0;      //  value -> se mění podle toho, jestli je to pro hráče výhodné nebo nevýhodné

                SpocitejKameny( hracNaTahuAI, out int mojeKameny, out int jehoKameny );
                value += mojeKameny * 2;                                // pokud bude mít soupeř míň figurek, jsem na tom líp
                value -= jehoKameny * 5;                                // pokud nude mít soupeř víc figurek, jsem na tom hůř
                                                                        // .......... aby byl hráč agresivnější, tak nastavím větší hodnotu nepřátelským figurkám. Tím se bude snažit zabrat figurku i když bude více riskovat
                return value;
            }

            rules.GenerujPlatneTahy( nardAI, hracNaTahuAI );            // vypočítání platných tahů (protože byl proveden tah a změněn hráč na tahu -> ve VypocitejNejlepsiTahy [začne, třeba, BÍLÝ] a byl změněn hráč na tahu na druhou barvu [ČERNÝ])
            List<int[]> moves = new List<int[]>();                  // <===== seznam TAHů, který je ve formě pole intů
            foreach ( int[] oneMove in rules.SeznamPlatnychTahu )
            {
                moves.Add( oneMove.Clone() as int[] );                  // "moves" je seznam polí integerů( = int[] ); metoda Clone() vytváří objekt datového typu "object" => aby to šlo uložit do toho pole, je třeba to přetypovat na int[]
            }

            int oneMoveValue = -MAX;
            foreach ( int[] oneMove in moves )
            {
                nardAI.VykonejTah( oneMove );
                hracNaTahuAI *= -1;                                     // v minimaxu prováděny tahy tam a zpět, takže opět změna hráče na tahu
                oneMoveValue = Math.Max( oneMoveValue, -Minimax( hloubka - 1 ) );
                nardAI.TahZpet();
                hracNaTahuAI *= -1;                                     // v minimaxu prováděny tahy tam a zpět, takže opět změna hráče na tahu
            }
            if (oneMoveValue > MANY)
                oneMoveValue -= 1;
            if (oneMoveValue < MANY)
                oneMoveValue += 1;

            return oneMoveValue;
        }



        /// <summary>
        /// Počítání kamenů
        /// </summary>
        /// <param name="hracNaTahu"></param>
        /// <param name="mojeKameny"></param>
        /// <param name="jehoKameny"></param>
        /// <returns>rozlišuji na kameny hráče na tahu a kameny nepřítele</returns>
        private void SpocitejKameny( int hracNaTahu, out int  mojeKameny, out int jehoKameny )
        {
            nardAI.PocetFigur( out int pocetBilychKamenu, out int pocetCernychKamenu );     // rozliším podle hodnoty hráče na tahu. Pokud je < 0 , tak jsou "moje" kameny černé a "jeho" bílé...
            mojeKameny = hracNaTahu < 0 ? pocetCernychKamenu : pocetBilychKamenu;
            jehoKameny = hracNaTahu < 0 ? pocetCernychKamenu : pocetBilychKamenu;
        }
    }
}
